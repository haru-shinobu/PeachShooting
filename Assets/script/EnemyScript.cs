using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyScript : MonoBehaviour
{
    Transform trs;
    Vector3 vec;
    float timer, attacktimer;
    public float AttackInterval;
    public float HP = 1000;
    private float DHP;
    public float movewide;
    public int TouchDamage = 10;
    [SerializeField]
    private bool AttackFlag = true;
    public List<float> movetime;

    public List<float> MoveChangeHP;

    float Under = -1.3f, Over = 3.5f, LeftEnd = -9f;

    GameObject EffectObj;
    GameObject EffectACObj;
    GameObject EffectDeadObj;

    public enum EnemyType
    {
        Mob = 0,
        Red = 1,
        Blue = 2,
        Yellow = 3,
        Green = 4,
        Black = 5
    }
    public EnemyType EnemyColor;
    enum MoveType
    {
        Loop = 0,
        BeforeNear = 1,
        Near = 2,
        Far = 3
    }
    private MoveType MoveStatus;

    Vector3 NearPos;
    Vector3 StartPos;
    Vector3 UnderPos;
    Transform Player;
    Vector3 TargetPos;
    float Instime = 0;
    GameObject AttackWepon;
    GameObject InstantObj;
    GameObject InstantObj2;
    GameObject InstantObj3;
    public bool MoveStartFlag = true;
    
    void Start()
    {
        Player = GameObject.Find("Player").GetComponent<Transform>();
        
        if (EnemyColor != EnemyType.Mob)
        {
            float Stat = PlayerPrefs.GetFloat("ORG")+1.0f;
            HP *= Stat;
            Player.SendMessage("WorldPointSet");
        }
        
        TargetPos = Player.position;
        trs = gameObject.GetComponent<Transform>();
        vec = new Vector3(1, 0, 0);
        timer = 0;
        StartPos = trs.position;
        UnderPos = new Vector3(LeftEnd+1, Under);
        
        MoveStatus = MoveType.Loop;
        DHP = HP;
        EffectObj = Resources.Load<GameObject>("DamageText");
        EffectACObj = Resources.Load<GameObject>("Effectred");
        EffectDeadObj = Resources.Load<GameObject>("Effect_Death");
        
        switch (EnemyColor)
        {
            case EnemyType.Mob:
                AttackWepon = (GameObject)Resources.Load("windbrow");
                break;
            case EnemyType.Red://金棒
                AttackWepon = (GameObject)Resources.Load("Stick");
                MoveStartFlag = false;
                break;
            case EnemyType.Blue://刺股
                AttackWepon = (GameObject)Resources.Load("sasumata");
                MoveStartFlag = false;
                break;
            case EnemyType.Yellow://両刃のこぎり
                AttackWepon = (GameObject)Resources.Load("saw");
                MoveStartFlag = false;
                break;
            case EnemyType.Green://薙刀
                AttackWepon = (GameObject)Resources.Load("naginata");
                MoveStartFlag = false;
                break;
            case EnemyType.Black://斧
                AttackWepon = (GameObject)Resources.Load("axe");
                MoveStartFlag = false;
                break;
        }
    }

    void FixedUpdate()
    {
        if (MoveStartFlag)
        {
            timer += Time.deltaTime;
            if (EnemyColor == EnemyType.Red || EnemyColor == EnemyType.Black || EnemyColor == EnemyType.Mob)
            {
                switch (MoveStatus)
                {
                    case MoveType.Loop:
                        if (timer <= movewide * 2)
                        {
                            if (EnemyColor == EnemyType.Mob)
                                StartPos += new Vector3(0.01f, 0, 0);
                            trs.position = StartPos + new Vector3(Mathf.PingPong(timer, movewide), Mathf.PingPong(timer, movewide * 0.5f));
                            Attack();
                        }
                        else
                            timer = 0;

                        break;

                    case MoveType.BeforeNear:
                        if (EnemyColor == EnemyType.Mob)
                            NearPos = trs.position + (Player.position - trs.position) * 0.5f - new Vector3(transform.lossyScale.x, 0);
                        else
                            NearPos = trs.position + (Player.position - trs.position) * 0.3f - new Vector3(transform.lossyScale.x, 0);
                        timer = 0;
                        MoveStatus = MoveType.Near;

                        break;

                    case MoveType.Near:
                        Attack();
                        trs.position = Vector3.Lerp(trs.position, NearPos, Time.deltaTime);

                        break;

                    case MoveType.Far:

                        Attack();
                        trs.position = Vector3.Lerp(trs.position, new Vector3(UnderPos.x, Mathf.PingPong(timer, movewide)), Time.deltaTime);
                        if (trs.position.x < UnderPos.x)
                            trs.position = new Vector3(UnderPos.x, trs.position.y);
                        if (trs.position.y > Over)
                            trs.position = new Vector3(trs.position.x, Over);
                        break;
                }
            }
            else
            {
                switch (MoveStatus)
                {
                    case MoveType.Loop:
                        if (timer * 3 <= movewide * 8)
                        {
                            trs.position = new Vector3(StartPos.x + Mathf.PingPong(timer * 3, movewide * 4), TargetPos.y);
                            Attack();
                        }
                        else
                        {
                            if (Instime == 0)
                                TargetPos = Player.position + new Vector3(5, 0, 0);
                            Instime += Time.deltaTime;
                            if (Instime <= 1)
                                trs.position = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime);
                            else
                                trs.position = Vector3.Lerp(transform.position, new Vector3(StartPos.x, TargetPos.y), Time.deltaTime);
                            if (Instime >= 2)
                            {
                                timer = 0;
                                Instime = 0;
                            }
                        }
                        break;

                    case MoveType.BeforeNear:
                        NearPos =(Player.position - transform.position) * 0.5f;
                        timer = 0;
                        MoveStatus = MoveType.Near;
                        break;

                    case MoveType.Near:
                        Attack();
                        trs.position = Vector3.Lerp(trs.position, NearPos, Time.deltaTime);
                        break;

                    case MoveType.Far:
                        Attack();
                        trs.position = Vector3.Lerp(trs.position, UnderPos + new Vector3(0, Mathf.PingPong(timer, Over - Under)), Time.deltaTime);
                        break;
                }
            }
        }
    }


    void Damage(int val)
    {
        if (MoveStartFlag)
            if (LeftEnd < transform.position.x)
            {
                //ダメージエフェクト
                Instantiate(EffectObj, transform.position, Quaternion.identity).SendMessage("NewBorn", val);
                HP -= val;
                if (HP < DHP * MoveChangeHP[0])
                {
                    if (HP > DHP * MoveChangeHP[1])
                    {
                        MoveStatus = MoveType.BeforeNear;
                        Instantiate(EffectACObj, transform.position, Quaternion.identity);
                        
                    }
                    else
                    if (HP > 0)
                    {
                        MoveStatus = MoveType.Far;
                        Instantiate(EffectACObj, transform.position, Quaternion.identity);
                        if (InstantObj2)
                            InstantObj2.SendMessage("StartWeponState", 2);                        
                    }
                    else
                    if (HP <= 0)
                    {
                        if (EnemyColor != EnemyType.Mob)
                            GameObject.Find("GameObject_EnemyGanerator").GetComponent<EnemyGaneratorScript>().BossHPLessFlag = true;
                        Instantiate(EffectDeadObj, transform.position, Quaternion.identity);
                        Destroy(gameObject.GetComponent<Rigidbody2D>());
                        MoveStartFlag = false;
                        if (EnemyColor != EnemyType.Mob)
                        {
                            if (InstantObj) Destroy(InstantObj);
                            if (InstantObj2) Destroy(InstantObj2);
                            if (InstantObj3) Destroy(InstantObj3);
                        }
                        else
                        {
                            GetComponent<MobItemScript>().ItemSpown();
                            Destroy(gameObject);
                        }
                    }
                }
            }
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.SendMessage("Damage", TouchDamage);
        }
    }

    //敵の攻撃
    void Attack()
    {
        if (AttackFlag)
        {
            attacktimer += Time.deltaTime;
            if (AttackInterval < attacktimer)
            {
                attacktimer -= AttackInterval;
                switch (MoveStatus)
                {
                    case MoveType.Loop:
                        if (!InstantObj)
                        {
                            AttackInterval = 4;
                            InstantObj = Instantiate(AttackWepon, trs.position, Quaternion.identity, transform);
                            InstantObj.SendMessage("StartWeponState", 3);
                        }
                        break;

                    case MoveType.Near:
                        AttackInterval = 10;
                        if (!InstantObj)
                        {
                            InstantObj2 = Instantiate(AttackWepon, trs.position, Quaternion.identity, transform);
                            InstantObj2.SendMessage("StartWeponState", 1);
                        }
                        else
                        {
                            InstantObj.SendMessage("StartWeponState", 1);
                        }
                        break;
                    case MoveType.Far:
                        if (!InstantObj2 && !InstantObj)
                        {
                            AttackInterval = 2;
                            InstantObj3 = Instantiate(AttackWepon, trs.position, Quaternion.identity, transform);
                            InstantObj3.SendMessage("StartWeponState", 2);
                        }
                        break;
                }
            }
        }
    }
    //Sendmessage
    void AttackDelete()
    {
        AttackFlag = true;
    }
    //Sendmessage
    void AttackFlagChange()
    {
        AttackFlag = false;
    }
    //Sendmessage
    void MoveStart()
    {
        MoveStartFlag = true;

        InstantObj = Instantiate(AttackWepon, trs.position, Quaternion.identity, transform);
    }
    void OnBecameInvisible()
    {
        if (transform.position.x > 0)
            Destroy(this.gameObject);
        if (EnemyColor != EnemyType.Mob)
        {
            transform.position = Vector3.Lerp(trs.position, UnderPos, 1);
        }
    }
}