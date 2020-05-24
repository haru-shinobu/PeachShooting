using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickScript : MonoBehaviour
{
    private int Way = 1;
    [SerializeField]
    private bool Flag = true, RFlag = true;
    public int AttackPower = 20;
    Vector3 Vec;
    Transform trs;
    float LifeTime;
    public float Limittime;
    public float Speed = 2;
    GameObject Root;
    enum AttackState
    {
        Swing = 0,
        Scale = 1,
        Throw = 2
    }
    private AttackState Status;

    void Start()
    {
        Root = transform.root.gameObject;
        Root.SendMessage("AttackFlagChange");
        trs = transform.GetComponent<Transform>();
        trs.position += new Vector3(trs.localScale.x, 0);
        Vec = trs.position + new Vector3(-2.1f, 0);
        LifeTime = 0;
        if (GameObject.FindWithTag("Player").transform.position.y < transform.position.y)
        {
            Flag = !Flag;
            transform.RotateAround(Vec,trs.forward,-90);
        }
        else
        {
            transform.RotateAround(Vec, trs.forward, 90);
        }
    }

    
    void FixedUpdate()
    {
        LifeTime += Time.deltaTime;

        switch (Status)
        {
            case AttackState.Swing:
                if (Flag)
                    trs.RotateAround(Root.transform.position, Vector3.forward, -Speed);
                else
                    trs.RotateAround(Root.transform.position, Vector3.forward, Speed);
                if (LifeTime > Limittime)
                    Destroy(gameObject);
                break;
            case AttackState.Scale:
                trs.RotateAround(Root.transform.position, Vector3.forward, -Speed * 0.5f);
                if (LifeTime > Limittime * 2) 
                    Destroy(gameObject);
                break;
            case AttackState.Throw:
                trs.RotateAround(Root.transform.position, Vector3.forward, -Speed);
                if (LifeTime > Limittime)
                    Destroy(gameObject);
                break;
        }
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (RFlag)
        {
            if (collision.tag == "Attack")
                collision.SendMessage("BounceBack");
            if (collision.tag == "Player")
                collision.gameObject.SendMessage("Damage", AttackPower);
            if (collision.tag == "Dog" || collision.tag == "Monkey" || collision.tag == "Bird")
                collision.gameObject.SendMessage("EAttackHit");
        }
        else
        {
            if (collision.tag == "Attack")
                collision.SendMessage("BounceBack");
            if (collision.tag == "Enemy")
            {
                collision.gameObject.SendMessage("Damage", AttackPower * 0.5f);
                Flag = false;
                RFlag = true;
            }
        }
    }

    void BounceBack()
    {
        if (Status == AttackState.Swing)  {
            RFlag = false;
            LifeTime = 0;
        }
    }

    public void StartWeponState(int I)
    {
        switch (I)
        {
            case 0:
                Status = AttackState.Swing;
                break;
            case 1:
                Status = AttackState.Scale;
                break;
            case 2:
                Status = AttackState.Throw;
                break;
        }
    }

    void OnBecameInvisible()
    {
        if (Root)
            Root.SendMessage("AttackDelete");
        Destroy(this.gameObject);
    }
}
