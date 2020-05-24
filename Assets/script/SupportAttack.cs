using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportAttack : MonoBehaviour
{
    Transform Trs;
    Vector3 pos;
    public float AttackSpeed;
    public float LifeTime;
    public float LimitTime;
    public float AttackPower;
    [SerializeField]
    private int Way = 1;
    Vector3 rot;

    float movers = 0;
    private bool FeatherFlag = false;
    GameObject Enemy;
    GameObject player;
    void Start()
    {
        Trs = gameObject.GetComponent<Transform>();
        pos = -transform.right * AttackSpeed;
        rot = Trs.rotation.eulerAngles;
        Enemy = GameObject.FindWithTag("Enemy");
        player = GameObject.FindWithTag("Player");
    }

    void FixedUpdate()
    {
        if (FeatherFlag && Enemy)
        {
            movers += AttackSpeed * 0.01f;
            if (Way > 0)
                Trs.position = Vector3.Lerp(Trs.position, Enemy.transform.position, movers);
            else
                Trs.position = Vector3.Lerp(Trs.position, player.transform.position, movers);
        }
        else
        {
            Trs.position += pos * Way;
            LifeTime += Time.deltaTime;
            if (LimitTime <= LifeTime)
                Destroy(gameObject);
        }        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (Way > 0)
            if (collision.tag == "Enemy")
            {
                collision.gameObject.SendMessage("Damage", AttackPower);
                Destroy(gameObject);
            }
        if (Way < 0)
            if (collision.tag == "Player")
            {
                collision.gameObject.SendMessage("Damage", AttackPower);
                Destroy(gameObject);
            }
    }

    void BounceBack()
    {
        if (Way > 0)  {
            Way = -1;
            if (gameObject)
            {   
                Trs.Rotate(-rot);
                pos = -transform.right * AttackSpeed;
                LifeTime = 0;
                Trs.localScale = new Vector3(-Trs.localScale.x, Trs.localScale.y);
                tag = "EAttack";
                gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                
            }
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    void FeatherFlagChange()
    {
        FeatherFlag = true;
    }
}