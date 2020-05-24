using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeScript : MonoBehaviour
{
    Transform trs;
    float timer = 0;
    public float Range = 2;
    List<Vector3> AnglePos = new List<Vector3>();
    private bool SwingFlag = true;
    Transform Player;
    public int AttackPower = 20;
    private enum AttackState
    {
        Swing = 0,
        Spin = 1,
        Throw = 2
    }
    private AttackState Status;
    Renderer ren;
    GameObject Root;
    void Start()
    {
        Root = transform.parent.gameObject;
        Root.SendMessage("AttackFlagChange");
        Vector3 nowpos = transform.position;
        trs = gameObject.GetComponent<Transform>();
        Status = AttackState.Swing;
        Player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        ren = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.deltaTime;
        
        switch (Status)
        {
            case AttackState.Swing:
                if (SwingFlag)
                    trs.Rotate(Vector3.forward, 4);
                else
                    trs.Rotate(Vector3.forward, -4);
                if (timer > 1)
                {
                    SwingFlag = !SwingFlag;
                    timer = 0;
                }
                break;
            case AttackState.Spin:
                trs.Rotate(Vector3.forward, 15);
                if (timer > 5)
                {
                    Status = AttackState.Throw;
                    timer = 0;
                }
                break;
            case AttackState.Throw:
                trs.Rotate(Vector3.forward, 6);
                trs.position += Vector3.right * 0.1f;
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            if (Status != AttackState.Throw)
            {
                collision.SendMessage("Damage", AttackPower);
            }
            else
                collision.SendMessage("Damage", AttackPower * 0.4f);

        if (collision.tag == "Attack")
                Destroy(collision.gameObject);
    }

    void BounceBack()
    {
        switch (Status)
        {
            case AttackState.Swing:
                Status = AttackState.Spin;
                break;
            case AttackState.Spin:
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
                Status = AttackState.Swing;
                break;
            case 1:
                Status = AttackState.Spin;
                break;
            case 2:
                Status = AttackState.Throw;
                break;
        }
    }

    void OnBecameInvisible()
    {
        if(Root)
        Root.SendMessage("AttackDelete");
        Destroy(this.gameObject);
    }
}
