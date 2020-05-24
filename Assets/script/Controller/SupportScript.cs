using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportScript : MonoBehaviour
{
    public enum MoveMode
    {
        NoOrder = 0,
        Await = 1,
        Delay = 2,
        Follow = 3,
    }
    public enum ItemType
    {
        nothing = 0,
        Heal = 1,//団子
        Power = 2,
        Faster = 3,
        Spread = 4,
        Tracking = 5
    }
    public string tagname;
    public string AttackObjname;
    public float moveSpeed = 5f;
    public float DelayMove = 0.5f;
    public float AttackSpeed = 0.1f;
    public float AttackPower = 1f;
    public float Attacktime;
    private int SplitSpece = 5;
    public bool diffusion = false;
    public bool chase = false;
    public int Split = 1;
    public bool AttackOK = false;

    protected GameObject AttackPrefab;
    Vector3 offset;
    public GameObject Target;
    public GameObject AttackTarget;
    protected Transform FollowTarget;
    protected MoveMode MoveState;
    public ItemType ItemState;
    Transform MyTrans;
    float timer;
    float InstantTime;
    private bool EAttackFlag = false;
    float Under = -1.3f, Over = 3.5f, RightEnd = 8.62f, LeftEnd = -8.62f;
    private SupportAttack SAttack;
    void Start()
    {
        SplitSpece = 5;
        offset = new Vector3(-0.5f, 0);
        AttackPrefab = (GameObject)Resources.Load(AttackObjname);
        SAttack = AttackPrefab.gameObject.GetComponent<SupportAttack>();
        SAttack.AttackSpeed = AttackSpeed;
        SAttack.AttackPower = AttackPower;
        timer = InstantTime = 0;
        FollowTarget = Target.GetComponent<Transform>();
        MyTrans = gameObject.GetComponent<Transform>();
        MoveState = MoveMode.Follow;
        ItemState = ItemType.nothing;
    }

    public void UpGradePerformance()
    {
        switch (ItemState)
        {
            case ItemType.nothing:break;
            case ItemType.Heal:
                ItemState = ItemType.nothing;
                break;

            case ItemType.Power:
                SAttack.AttackPower = AttackPower++;
                ItemState = ItemType.nothing;
                break;

            case ItemType.Faster:
                SAttack.AttackSpeed = AttackSpeed + 0.1f;
                ItemState = ItemType.nothing;
                break;

            case ItemType.Spread:
                SAttack.AttackPower = AttackPower;
                ItemState = ItemType.nothing;
                break;

            case ItemType.Tracking:
                ItemState = ItemType.nothing;
                break;
        }
    }
    void FixedUpdate()
    {
        UpGradePerformance();
        if (AttackOK)
            timer += Time.deltaTime;
        switch (MoveState)
        {
            case MoveMode.NoOrder:
                timer = 0;
                Move();
                break;
            case MoveMode.Await:
                Attack();
                break;
            case MoveMode.Delay:
                Attack();
                if (timer >= InstantTime)
                    MoveState = MoveMode.Follow;
                break;
            case MoveMode.Follow:
                Move();
                Attack();
                break;
        }
    }
    void Move()
    {
        if (!EAttackFlag)
            transform.position = Vector3.Lerp(transform.position, FollowTarget.position, Time.deltaTime * moveSpeed);
        else
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(RightEnd - transform.localScale.x, transform.position.y), Time.deltaTime * moveSpeed * 5);
            if (RightEnd - transform.position.x <= transform.localScale.x)
                EAttackFlag = !EAttackFlag;
            if (transform.position.x > RightEnd)
                transform.position = new Vector3(RightEnd,transform.position.y);
            if (transform.position.y > Over)
                transform.position = new Vector3(transform.position.x, Over);
            if (transform.position.y < Under)
                transform.position = new Vector3(transform.position.x, Under);
        }
    }
    void Attack()
    {
        if (AttackOK)
            if (timer >= Attacktime)
            {
                timer -= Attacktime;
                InstantTime = timer + DelayMove;
                //通常弾
                if (!diffusion && !chase)
                    Instantiate(AttackPrefab, transform.position + offset, Quaternion.identity);
                //拡散弾
                if (diffusion && !chase)
                    SplitBullet();
                //追尾弾
                if (!diffusion && chase)
                    ChaseBullet();
            }
    }

    //拡散弾処理
    void SplitBullet()
    {
        if (Split == 1)//拡散弾の数
            Instantiate(AttackPrefab, transform.position + offset, Quaternion.identity);
        else
        {
            int num = Split;
            //拡散弾の数が偶数のとき
            if (num % 2 == 0)
            {
                Quaternion rot = Quaternion.Euler(0, 0, Split / 2 * -SplitSpece - SplitSpece * 0.5f);
                while (num > 0)
                {
                    Instantiate(AttackPrefab, transform.position + offset, rot * Quaternion.Euler(0, 0, num * SplitSpece));
                    num--;
                }
            }
            else
            {
                Quaternion rot = Quaternion.Euler(0, 0, Split / 2 * -SplitSpece);
                while (num > 0)
                {
                    Instantiate(AttackPrefab, transform.position + offset, rot * Quaternion.Euler(0, 0, (num - 1) * SplitSpece));
                    num--;
                }
            }
        }
    }

    //ターゲットから距離を保ってついてくる用に使用
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == tagname)
        {
            MoveState = MoveMode.Await;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == tagname)
        {
            MoveState = MoveMode.Delay;
            InstantTime = timer + DelayMove;
        }
    }

    //追尾弾
    void ChaseBullet()
    {
        Instantiate(AttackPrefab, transform.position + offset, Quaternion.identity).SendMessage("FeatherFlagChange");
    }

    //状態遷移
    public void ModeChange()
    {
        switch (MoveState)
        {
            case MoveMode.NoOrder:
                MoveState = MoveMode.Follow;
                break;
            case MoveMode.Await:
                MoveState = MoveMode.NoOrder;
                break;
            case MoveMode.Delay:
                MoveState = MoveMode.NoOrder;
                break;
            case MoveMode.Follow:
                MoveState = MoveMode.Await;
                break;
        }
    }

    public MoveMode ReadMoveState()
    {
        return MoveState;
    }
    public void MoveStateSet()
    {
        MoveState = MoveMode.Follow;
    }

    void EAttackHit()
    {
        EAttackFlag = true;
    }
}
