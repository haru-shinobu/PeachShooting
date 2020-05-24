using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMove_Script : MonoBehaviour
{
    public bool MoveStartFlag = false;
    Vector3 BattleStartPos;
    float Under = -1.3f, Over = 3.5f, RightEnd = 8.62f, LeftEnd = -8.62f;
    Vector3 RandomWay;
    [SerializeField]
    private float DHP, HP = 1;//300;
    public int TouchDamage = 10;

    public List<float> MoveChangeHP;
    GameObject AttackPref;
    GameObject Spetial, Spetial2;
    GameObject EffectObj;
    float timer = 0;
    float timerA = 0;
    [SerializeField]
    private float attackinterval = 3;
    int HPFlag = 0;
    [SerializeField]
    float SetStrings;

    private enum BossName
    {
        BossDog = 0,
        BossMonkey = 1,
        BossBird = 2,
    }
    BossName boss;
    private enum AttackChangeState
    {
        FullHP = 0,//直進
        HP = 1,
        HalfHP = 2,
        LowHP = 3,        
        EndHP = 4
    }
    AttackChangeState Status;
    Vector3 Attackoffset;

    void Start()
    {
        SetStrings = 1;
        SpriteRenderer spr = gameObject.GetComponent<SpriteRenderer>();
        
        switch (gameObject.name)
        {
            case "Boss_Dog":
            case "Boss_Dog(Clone)":
                SetStrings += PlayerPrefs.GetFloat("DOG");//GameObject.Find("AllSceneManager").GetComponent<InstantSaveScript>().SettingsRead("DOG");
                AttackPref = Resources.Load<GameObject>("EDogAttack");
                Spetial = Resources.Load<GameObject>("dogcirle");
                boss = BossName.BossDog;
                spr.sprite = Resources.Load<Sprite>("dog");
                spr.color = new Color32(95, 95, 255, 255);
                break;
            case "Boss_Monkey":
            case "Boss_Monkey(Clone)":
                SetStrings += PlayerPrefs.GetFloat("MONKEY");//GameObject.Find("AllSceneManager").GetComponent<InstantSaveScript>().SettingsRead("MONKEY");
                AttackPref = Resources.Load<GameObject>("EMonAttack");
                Spetial = Resources.Load<GameObject>("EMonBomb");
                boss = BossName.BossMonkey;
                spr.sprite = Resources.Load<Sprite>("monkey");
                spr.color = new Color32(132, 132, 132, 255);
                HP += 100;
                break;
            case "Boss_Bird":
            case "Boss_Bird(Clone)":
                SetStrings += PlayerPrefs.GetFloat("BIRD");//GameObject.Find("AllSceneManager").GetComponent<InstantSaveScript>().SettingsRead("BIRD");
                AttackPref = Resources.Load<GameObject>("Efeather");
                Spetial = Resources.Load<GameObject>("EfeatherSpetial");
                boss = BossName.BossBird;
                spr.sprite = Resources.Load<Sprite>("bird2");
                spr.color = new Color32(255, 255, 0, 255);
                HP += 200;
                break;
        }        


        attackinterval = 3 - SetStrings;
        EffectObj = Resources.Load<GameObject>("DamageText");
        
        AttackPref.GetComponent<SupportAttack>().AttackPower *= SetStrings;
        
        RandomWay = new Vector3(0, 0);
        timer = timerA = 0;
        HP *= SetStrings;
        DHP = HP;
        transform.position = new Vector3(-10, 1, 0);
        BattleStartPos = new Vector3(-5, 1, 0);
        Status = AttackChangeState.FullHP;
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
            switch (boss)
            {
                case BossName.BossDog:
                    doga();
                    break;
                case BossName.BossMonkey:
                    mona();
                    break;
                case BossName.BossBird:
                    bira();
                    break;
            }
        }
    }

    void doga()
    {
        switch (Status)
        {
            case AttackChangeState.FullHP:
                Instantiate(AttackPref, transform.position, Quaternion.identity);
                break;
            case AttackChangeState.HP:
                Instantiate(AttackPref, transform.position + Attackoffset, new Quaternion(0, 0, 0.1f, 1.0f));
                Instantiate(AttackPref, transform.position + Attackoffset, new Quaternion(0, 0, 0.1f, -1.0f));
                break;
            case AttackChangeState.HalfHP:
                Instantiate(AttackPref, transform.position + Attackoffset, new Quaternion(0, 0, 0.2f, 1.0f));
                Instantiate(AttackPref, transform.position + Attackoffset, new Quaternion(0, 0, 0.1f, 1.0f));
                Instantiate(AttackPref, transform.position + Attackoffset, new Quaternion(0, 0, 0.1f, -1.0f));
                Instantiate(AttackPref, transform.position + Attackoffset, new Quaternion(0, 0, 0.2f, -1.0f));
                break;
            case AttackChangeState.LowHP:
                Instantiate(AttackPref, transform.position + new Vector3(1, 0.7f), Quaternion.identity);
                Instantiate(AttackPref, transform.position + new Vector3(1, -0.7f), Quaternion.identity);
                break;
            case AttackChangeState.EndHP:
                Instantiate(Spetial, transform.position + Attackoffset, Quaternion.identity);
                break;
        }
    }
    void mona()
    {
        switch (Status)
        {
            case AttackChangeState.FullHP:
                Instantiate(AttackPref, transform.position, Quaternion.identity);
                break;
            case AttackChangeState.HP:
                Instantiate(AttackPref, transform.position + new Vector3(1, 0.7f), Quaternion.identity);
                Instantiate(AttackPref, transform.position + new Vector3(1, -0.7f), Quaternion.identity);
                break;
            case AttackChangeState.HalfHP:
                Instantiate(Spetial, transform.position + Attackoffset, Quaternion.identity);
                break;
            case AttackChangeState.LowHP:
                Instantiate(AttackPref, transform.position + Attackoffset, new Quaternion(0, 0, 0.25f, 1.0f));
                Instantiate(AttackPref, transform.position + Attackoffset, new Quaternion(0, 0, 0.1f, 1.0f));
                Instantiate(AttackPref, transform.position + Attackoffset, new Quaternion(0, 0, 0.1f, -1.0f));
                Instantiate(AttackPref, transform.position + Attackoffset, new Quaternion(0, 0, 0.25f, -1.0f));
                break;
            case AttackChangeState.EndHP:
                Instantiate(Spetial, transform.position + Attackoffset, Quaternion.identity).SendMessage("FlagChange");
                break;
        }
    }
    void bira()
    {
        switch (Status)
        {
            case AttackChangeState.FullHP:
                Instantiate(AttackPref, transform.position, Quaternion.identity);
                break;
            case AttackChangeState.HP:
                Instantiate(Spetial, transform.position + Attackoffset, Quaternion.identity);
                break;
            case AttackChangeState.HalfHP:
                Instantiate(AttackPref, transform.position + new Vector3(1, 0.7f), Quaternion.identity);
                Instantiate(AttackPref, transform.position + new Vector3(1, -0.7f), Quaternion.identity);
                break;
            case AttackChangeState.LowHP:
                Instantiate(AttackPref, transform.position + Attackoffset, new Quaternion(0, 0, 0.2f, 1.0f));
                Instantiate(AttackPref, transform.position + Attackoffset, new Quaternion(0, 0, 0.1f, 1.0f));
                Instantiate(AttackPref, transform.position + Attackoffset, new Quaternion(0, 0, 0.1f, -1.0f));
                Instantiate(AttackPref, transform.position + Attackoffset, new Quaternion(0, 0, 0.2f, -1.0f));
                break;
            case AttackChangeState.EndHP:
                Instantiate(Spetial, transform.position + Attackoffset, Quaternion.identity).SendMessage("FlagChange");
                break;
        }
    }

    void Damage(int val)
    {
        if (MoveStartFlag)
        {
            Instantiate(EffectObj, transform.position, Quaternion.identity).SendMessage("NewBorn", val);
            HP -= val;

            if (HP > DHP * MoveChangeHP[1])
                Status = AttackChangeState.FullHP;
            else
            if (HP > DHP * MoveChangeHP[2])
                Status = AttackChangeState.HP;
            else
            if (HP > DHP * MoveChangeHP[3])
            {
                Status = AttackChangeState.HalfHP;
                if (HPFlag == 0)
                {
                    attackinterval *= 0.8f;
                    HPFlag++;
                }
            }
            else
            if (HP > DHP * MoveChangeHP[4])
            {
                Status = AttackChangeState.LowHP;
                if (HPFlag == 1)
                {
                    HPFlag++;
                    attackinterval *= 1.6f;
                }
            }
            else
            if (HP > 0)
            {
                Status = AttackChangeState.EndHP;
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