using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SasumataScript : MonoBehaviour
{
    public float AttackPower = 5;
    public int HoldCounter = 1;
    Transform PlayerTrs;
    GameObject Target;
    GameObject RootObj;
    bool AttackFlag = false;
    int count = 0;
    float timer = 0, ptimer = 0;
    ButtonTextScript buttontex;
    bool Flag = true;
    enum AttackState
    {
        Capture = 1,
        Capture2nd = 2,
        Throw = 3
    }
    [SerializeField]
    AttackState Status;
    void Awake()
    {
        buttontex = GameObject.Find("GMCanvas").GetComponent<ButtonTextScript>();
        Status = AttackState.Capture;
        PlayerTrs = GameObject.FindWithTag("Player").GetComponent<Transform>();
        RootObj = transform.root.gameObject;
        RootObj.SendMessage("AttackFlagChange");
    }

    void Update()
    {
        switch (Status)
        {
            case AttackState.Capture:
                transform.right = -(this.transform.position - PlayerTrs.position);
                TargetHit(AttackFlag);
                break;
            case AttackState.Capture2nd:
                transform.right = -(this.transform.position - PlayerTrs.position);
                ptimer += Time.deltaTime;
                transform.position = transform.root.position + transform.right * Mathf.PingPong(ptimer, 2);
                TargetHit(AttackFlag);
                break;
            case AttackState.Throw:
                if (Flag)
                {
                    transform.SetParent(null);
                    Flag = !Flag;
                }
                transform.position += transform.right * 0.1f;
                break;
        }
        
    }

    void TargetHit(bool Hit)
    {
        if (Hit)
        {
            timer += Time.deltaTime;
            if (timer > 1)
            {
                Target.SendMessage("Damage", AttackPower);
                count++;
                timer = 0;
            }
        }
        if (HoldCounter <= count)
            AttackFlag = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            if (Status == AttackState.Capture)
            {
                collision.SendMessage("Capture", HoldCounter);
                buttontex.CurrentStatus("Capture", HoldCounter);
                Target = collision.gameObject;
                AttackFlag = true;
            }
            else
            if (Status == AttackState.Capture2nd)
            {
                collision.SendMessage("Capture", HoldCounter - 1);
                buttontex.CurrentStatus("Capture", HoldCounter - 1);
                Target = collision.gameObject;
                AttackFlag = true;
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
            case AttackState.Capture:
                Status = AttackState.Capture2nd;
                break;
            case AttackState.Capture2nd:
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
                Status = AttackState.Capture;
                break;
            case 1:
                Status = AttackState.Capture2nd;
                break;
            case 2:
                Status = AttackState.Throw;
                transform.right = -(this.transform.position - PlayerTrs.position);
                transform.SetParent(null);
                break;
        }
    }
}
