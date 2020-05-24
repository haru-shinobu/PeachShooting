using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyBossScript : MonoBehaviour
{
    public bool MoveStartFlag = false;
    Vector3 BattleStartPos;
    float Under = -1.3f, Over = 3.5f, RightEnd = 8.62f, LeftEnd = -8.62f;
    Vector3 RandomWay;
    [SerializeField]
    private float HP = 300, DHP;
    public int TouchDamage = 10;

    public List<float> MoveChangeHP;
    GameObject AttackPref;
    GameObject Moncircle;
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
        ThrowStraight = 0,//直進
        ThrowDouble = 1,
        ThrowCircle = 2,
        ThrowFan = 3,
        ThrowFlatShot = 4
    }
    AttackChangeState Status;
    Vector3 Attackoffset;
    void Start()
    {
        SetStrings = 1;//GameObject.Find("AllSceneManager").GetComponent<InstantSaveScript>().SettingsRead("MONKEY") + 1.1f;
        attackinterval = 3 - SetStrings;
        EffectObj = Resources.Load<GameObject>("DamageText");
        AttackPref = Resources.Load<GameObject>("EMonAttack");
        AttackPref.GetComponent<SupportAttack>().AttackPower *= SetStrings;
        Moncircle = Resources.Load<GameObject>("EMonBomb");
        RandomWay = new Vector3(0, 0);
        timer = timerA = 0;
        HP *= SetStrings;
        DHP = HP;
        transform.position = new Vector3(-10, 1, 0);
        BattleStartPos = new Vector3(-5, 1, 0);
        Status = AttackChangeState.ThrowStraight;
        Attackoffset = new Vector3(1, 0);
        HPFlag = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!MoveStartFlag)
        {

            DogMoving(MovePosCheck());
            DogFight();
        }
        else
        {
            transform.position = Vector3.Slerp(transform.position, BattleStartPos, Time.deltaTime * 0.5f);

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
                case AttackChangeState.ThrowStraight:
                    Instantiate(AttackPref, transform.position, Quaternion.identity);
                    break;
                case AttackChangeState.ThrowDouble:
                    Instantiate(AttackPref, transform.position + Attackoffset, new Quaternion(0, 0, 0.1f, 1.0f));
                    Instantiate(AttackPref, transform.position + Attackoffset, new Quaternion(0, 0, 0.1f, -1.0f));
                    break;
                case AttackChangeState.ThrowCircle:
                    Instantiate(Moncircle, transform.position + Attackoffset, Quaternion.identity);
                    break;
                case AttackChangeState.ThrowFan:
                    Instantiate(AttackPref, transform.position + Attackoffset, new Quaternion(0, 0, 0.2f, 1.0f));
                    Instantiate(AttackPref, transform.position + Attackoffset, new Quaternion(0, 0, 0.1f, 1.0f));
                    Instantiate(AttackPref, transform.position + Attackoffset, new Quaternion(0, 0, 0.1f, -1.0f));
                    Instantiate(AttackPref, transform.position + Attackoffset, new Quaternion(0, 0, 0.2f, -1.0f));
                    break;
                case AttackChangeState.ThrowFlatShot:
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
                Status = AttackChangeState.ThrowStraight;
            else
            if (HP > DHP * MoveChangeHP[2])
                Status = AttackChangeState.ThrowDouble;
            else
            if (HP > DHP * MoveChangeHP[3])
            {
                Status = AttackChangeState.ThrowFlatShot;
                if (HPFlag == 0)
                {
                    attackinterval *= 0.8f;
                    HPFlag++;
                }
            }
            else
            if (HP > DHP * MoveChangeHP[4])
            {
                Status = AttackChangeState.ThrowFan;
                if (HPFlag == 1)
                {
                    HPFlag++;
                    attackinterval *= 1.6f;
                }
            }
            else
            if (HP > 0)
            {
                Status = AttackChangeState.ThrowCircle;
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