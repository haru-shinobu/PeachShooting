using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaginataScript : MonoBehaviour
{
    public float AttackPower = 5;
    public float timer = 0;
    bool CutFlag = true;
    GameObject RootObj;
    bool ThrowFlag = true;
    public bool MoveFlag = false;
    GameObject Player;
    enum AttackState
    {
        cut = 0,
        swing = 1,
        Throw = 2
    }
    AttackState Status;

    void Awake()
    {
        Player = GameObject.FindWithTag("Player");
        transform.RotateAround(transform.root.position, Vector3.forward, 45);
        Status = AttackState.cut;
        RootObj = transform.root.gameObject;
        RootObj.SendMessage("AttackFlagChange");
    }


    void FixedUpdate()
    {
        switch (Status)
        {
            case AttackState.cut:
                if (timer >= 1.8f)
                {
                    CutFlag = !CutFlag;
                    timer = 0;
                }
                timer += Time.deltaTime;
                if (CutFlag)
                    transform.RotateAround(transform.root.position, Vector3.forward, -5);
                else
                    transform.RotateAround(transform.root.position, Vector3.forward, 1);
                break;
            case AttackState.swing:
                transform.RotateAround(transform.root.position, Vector3.forward, -10);
                break;
            case AttackState.Throw:
                if (ThrowFlag)
                {
                    ThrowFlag = !ThrowFlag;
                    transform.right = Player.transform.position - this.transform.position;
                    transform.SetParent(null);
                }
                transform.position += transform.right * 0.1f;
                break;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            if (Status == AttackState.cut)
            {
                collision.SendMessage("Damage", AttackPower);
            }
            else
            if (Status == AttackState.swing)
            {
                collision.SendMessage("Damage", AttackPower);
            }
            else
                collision.SendMessage("Damage", AttackPower * 2);
    }

    void OnBecameInvisible()
    {
        if (RootObj)
            RootObj.SendMessage("AttackDelete");
        Destroy(this.gameObject);
    }


    void BounceBack()
    {
        switch (Status)
        {
            case AttackState.cut:
                Status = AttackState.swing;
                break;
            case AttackState.swing:
                Status = AttackState.Throw;
                break;
            case AttackState.Throw:
                break;
        }
    }

    void StartWeponState(int I)
    {
        switch (I)
        {
            case 0:
            case 1:
                Status = AttackState.cut;
                break;
                //Status = AttackState.swing;
                //break;
            case 2:
                Status = AttackState.Throw;
                break;
        }
    }
}
