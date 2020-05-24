using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogBossScript : MonoBehaviour
{
    public bool MoveStartFlag = false;
    Vector3 BattleStartPos;
    float Under = -1.3f, Over = 3.5f, RightEnd = 8.62f, LeftEnd = -8.62f;
    Vector3 RandomWay;
    [SerializeField]
    private float HP = 300,DHP;
    public int TouchDamage = 10;

    public List<float> MoveChangeHP;
    GameObject AttackPref;
    GameObject Dogcircle;
    GameObject EffectObj;
    float timer = 0;
    float timerA = 0;
    [SerializeField]
    private float attackinterval = 3;
    int HPFlag = 0;
    [SerializeField]
    float SetStrings;

    private enum AttackChangeState
    {
        PadStraight = 0,//直進
        PadDouble = 1,
        PadCircle = 2,
        PadFan = 3,
        PadFlatShot = 4
    }
    AttackChangeState Status;
    Vector3 Attackoffset;
    void Start()
    {
        SetStrings = GameObject.Find("AllSceneManager").GetComponent<InstantSaveScript>().SettingsRead("DOG") + 1;
        attackinterval = 3 - SetStrings;
        EffectObj = Resources.Load<GameObject>("DamageText");
        AttackPref = Resources.Load<GameObject>("EDogAttack");
        AttackPref.GetComponent<SupportAttack>().AttackPower *= SetStrings;
        Dogcircle = Resources.Load<GameObject>("dogcirle");
        RandomWay = new Vector3(0, 0);
        timer = timerA = 0;
        HP *= SetStrings;
        DHP = HP;
        transform.position = new Vector3(-10, 1, 0);
        BattleStartPos = new Vector3(-5, 1, 0);
        Status = AttackChangeState.PadStraight;
        Attackoffset = new Vector3(1, 0);
        HPFlag = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (MoveStartFlag)
        {
            
            DogMoving(MovePosCheck());
            DogFight();
        }
        else
        {
            transform.position = Vector3.Slerp(transform.position, BattleStartPos, Time.deltaTime*0.5f);
            
        }
    }

    int MovePosCheck()//移動制限
    {
        int state = 0;
        float Ypos = transform.position.y;
        float Xpos = transform.position.x;
        if (Under > Ypos)
        {
            transform.position = new Vector3(Xpos, Under); state += 1;
        }
        if (Over < Ypos)
        {
            transform.position = new Vector3(Xpos, Over); state += 2; 
        }
        if (RightEnd * 0.2f < Xpos)
        {
            transform.position = new Vector3(RightEnd * 0.2f, Ypos); state += 4;
        }
        if (LeftEnd > Xpos)
        {
            transform.position = new Vector3(LeftEnd, Ypos); state += 8;
        }
        return state;
    }

    void DogMoving(int val)//移動
    {
        timer += Time.deltaTime;
        if (timer > 0.5f)
        {
            timer -= 0.5f;
            float x = Random.Range(-1f, 1f);
            float y = Random.Range(-1f, 1f);
            switch (val)
            {
                case 1: if (y <= 0) y = -y; break;
                case 2: if (y >= 0) y = -y; break;
                case 4: if (x >= 0) x = -x; break;
                case 5: if (y <= 0) y = -y; if (x >= 0) x = -x; break;
                case 6: if (y >= 0) y = -y; if (x >= 0) x = -x; break;
                case 8: if (x <= 0) x = -x; break;
                case 9: if (y <= 0) y = -y; if (x <= 0) x = -x; break;
                case 10: if (y >= 0) y = -y; if (x <= 0) x = -x; break;
            }
            RandomWay = new Vector3(x, y);
        }
        else
        {
            transform.position += RandomWay * 0.1f;
        }
    }

    void DogFight()//攻撃
    {
        timerA += Time.deltaTime;
        if (timerA > attackinterval)
        {
            timerA -= attackinterval;
            switch (Status)
            {
                case AttackChangeState.PadStraight:
                    Instantiate(AttackPref, transform.position, Quaternion.identity);
                    break;
                case AttackChangeState.PadDouble:

                    Instantiate(AttackPref, transform.position + Attackoffset, new Quaternion(0, 0, 0.1f, 1.0f));
                    Instantiate(AttackPref, transform.position + Attackoffset, new Quaternion(0, 0, 0.1f, -1.0f));
                    break;
                case AttackChangeState.PadCircle:
                    Instantiate(Dogcircle,transform.position + Attackoffset,Quaternion.identity);
                    break;
                case AttackChangeState.PadFan:
                    Instantiate(AttackPref, transform.position + Attackoffset, new Quaternion(0, 0, 0.2f, 1.0f));
                    Instantiate(AttackPref, transform.position + Attackoffset, new Quaternion(0, 0, 0.1f, 1.0f));
                    Instantiate(AttackPref, transform.position + Attackoffset, new Quaternion(0, 0, 0.1f, -1.0f));
                    Instantiate(AttackPref, transform.position + Attackoffset, new Quaternion(0, 0, 0.2f, -1.0f));
                    break;
                case AttackChangeState.PadFlatShot:
                    Instantiate(AttackPref, transform.position + new Vector3(1, 0.7f), Quaternion.identity);
                    Instantiate(AttackPref, transform.position + new Vector3(1, -0.7f), Quaternion.identity);
                    break;
            }
        }
    }
    void Damage(int val)
    {
        if (MoveStartFlag)
        {

            Instantiate(EffectObj, transform.position, Quaternion.identity).SendMessage("NewBorn", val);
            HP -= val;

            if (HP > DHP * MoveChangeHP[1])
                Status = AttackChangeState.PadStraight;
            else
            if (HP > DHP * MoveChangeHP[2])
                Status = AttackChangeState.PadDouble;
            else
            if (HP > DHP * MoveChangeHP[3])
            {
                Status = AttackChangeState.PadFlatShot;
                if (HPFlag == 0)
                {
                    attackinterval *= 0.8f;
                    HPFlag++;
                }
            }
            else
            if (HP > DHP * MoveChangeHP[4])
            {
                Status = AttackChangeState.PadFan;
                if (HPFlag == 1)
                {
                    HPFlag++;
                    attackinterval *= 1.6f;
                }
            }
            else
            if (HP > 0)
            {
                Status = AttackChangeState.PadCircle;
                if (HPFlag == 2)
                {
                    HPFlag++;
                    attackinterval *= 1.3f;
                }
            }
            else
            if (HP < 0)
            {
                GameObject.Find("GameObject_EnemyGanerator").GetComponent<EnemyGaneratorScript>().BossHPLessFlag = true;
                Destroy(gameObject.GetComponent<Rigidbody2D>());
                MoveStartFlag = false;
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
}