using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour
{
    Transform Trs;
    Vector3 DelPos;
    float DPosF;
    public float MoveSpeed = 1.0f;
    public float AttackPower;
    bool Flag = false;
    void Start()
    {
        Trs = gameObject.GetComponent<Transform>();        
        DPosF = Trs.position.x - 8;
    }
    
    void FixedUpdate()
    {
        Trs.position -= new Vector3(0.1f, 0) * MoveSpeed;
        if (Trs.position.x < DPosF)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.gameObject.SendMessage("Damage", AttackPower);
            Destroy(gameObject);
        }
        if (collision.tag == "EAttack")
            if (collision.name == "axe(Clone)" || collision.name == "naginata(Clone)"
                || collision.name == "naginata(Clone)" || collision.name == "sasumata(Clone)" ||
                collision.name == "saw(Clone)" || collision.name == "Stick(Clone)")
                collision.gameObject.SendMessage("BounceBack");

        if (collision.tag == "Player")
        {
            collision.gameObject.SendMessage("Damage", AttackPower);
            Destroy(gameObject);
        }
    }

    void BounceBack()
    {
        Destroy(gameObject);
    }
}
