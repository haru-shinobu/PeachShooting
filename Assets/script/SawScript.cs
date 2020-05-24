using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawScript : MonoBehaviour
{
    enum AttackState{
        Swing = 0,
        Scale = 1,
        Throw = 2
    }
    private AttackState Status;
    Transform Player;
    Vector3 StartPos;
    Renderer ren;
    private float timer=0;
    public float AttackPower = 30;
    bool ScaleFlag = true;
    bool PlayerTargetRotFlag = true;
    GameObject RootObj;
    void Start()
    {
        RootObj = transform.root.gameObject;
        RootObj.SendMessage("AttackFlagChange");
        Status = AttackState.Swing;
        Player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        transform.position += new Vector3(transform.localScale.x, 0);
        StartPos = transform.position;
        ren = GetComponent<Renderer>();
    }


    void Update()
    {
        timer += Time.deltaTime;
        switch (Status)
        {
            case AttackState.Swing:
                if (ScaleFlag)
                {
                    ScaleFlag = !ScaleFlag;
                    transform.localScale = new Vector3(transform.localScale.x * 2, 1, 1);
                }
                transform.position = new Vector3(transform.root.position.x+Mathf.PingPong(timer, 2),transform.position.y);
                break;
            case AttackState.Scale:
                if (ScaleFlag)
                {
                    transform.localScale = new Vector3(transform.localScale.x * 3, 2, 1);
                }
                transform.position = new Vector3(transform.root.position.x + Mathf.PingPong(timer, 2), transform.position.y);
                break;
            case AttackState.Throw:
                if(PlayerTargetRotFlag)
                {
                    PlayerTargetRotFlag = !PlayerTargetRotFlag;
                    transform.SetParent(null);
                    this.transform.right = Player.position - this.transform.position;
                }
                transform.position += transform.right * 0.1f;
                break;
        }
    }

    void OnBecameInvisible()
    {
        if (RootObj)
            RootObj.SendMessage("AttackDelete");
        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            if (Status != AttackState.Throw)
            {
                collision.SendMessage("Damage", AttackPower);
            }
            else
                collision.SendMessage("Damage", AttackPower*0.5f);
    }

    void BounceBack()
    {
        switch (Status)
        {
            case AttackState.Swing:
                Status = AttackState.Scale;
                break;
            case AttackState.Scale:
                Status = AttackState.Throw;
                break;
            case AttackState.Throw:
                break;
        }
    }
    //StartWeponState
    public void StartWeponState(int val)
    {
        switch (val)
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
}
