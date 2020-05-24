using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    public float MoveSpeed = 1.0f;
    float moveDist = 0.05f;
    Vector3 worldPos;
    Transform trs;
    float Under, Over;
    float RightEnd, LeftEnd;
    [SerializeField]
    private float HP = 100;
    private bool CaptureFlag = false;
    [SerializeField]
    int CaptureTime = 0;
    private float timer = 0, oldtimer = 0;
    GameObject TapEffect;
    int ContinuosAttack = 0;
    public enum SarvantMode
    {
        NoSarvant = 0,
        DogActive = 1,
        MonkeyActive = 2,
        Bird2Active = 3
    }
    protected SarvantMode SarvantState;
    public int GameState;


    public GameObject SwordPrefab;
    SwordScript SwordSPr;
    public float AttackSpeed = 1.0f;
    Vector3 offset;
    public float AttackPower;
    StageScript PlayersStrength;

    Vector2 StartPos;

    AudioSource audioSource;
    public AudioClip soundsord;
    private float maxVolume = 1;
    bool DeathFlag = true;
    void Start()
    {
        DeathFlag = true;
        audioSource = gameObject.AddComponent<AudioSource>();
        TapEffect = Resources.Load<GameObject>("TapEffect");
        worldPos = transform.position;
        PlayersStrength = GameObject.Find("GameMaster").GetComponent<StageScript>();
        trs = GetComponent<Transform>();
        Under = -1.3f;
        Over = 3.5f;
        RightEnd = 8.62f;
        LeftEnd = -8.62f;

        SwordPrefab = (GameObject)Resources.Load("SwordAttack");
        SwordSPr = SwordPrefab.GetComponent<SwordScript>();
        SwordSPr.MoveSpeed = AttackSpeed;
        SwordSPr.AttackPower = AttackPower;

        offset = new Vector3(-0.5f, 0);
        if (PlayerPrefs.HasKey("SE"))
            maxVolume = PlayerPrefs.GetFloat("SE") * 0.5f;
        audioSource.volume = maxVolume;
    }

    void Update()
    {
        if (Mathf.Approximately(Time.timeScale, 0f))
        {
            return;
        }
        timer += Time.deltaTime;
        if (!CaptureFlag)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 InstantPos = worldPos;
                this.StartPos = Input.mousePosition;
                worldPos = Camera.main.ScreenToWorldPoint(StartPos);
                Object.Instantiate(TapEffect, new Vector3(worldPos.x, worldPos.y, 0), Quaternion.identity);
                worldPos.z = 0;

                if (-2.0f <= worldPos.y && worldPos.y <= 4.0f)
                {
                    if (worldPos.y <= Under) worldPos.y = Under;
                    if (Over <= worldPos.y) worldPos.y = Over;

                    if (worldPos.y >= 4.0f && worldPos.x <= -7.4f)
                        worldPos = transform.position;
                }
                else
                {
                    worldPos = InstantPos;
                }
            }
        }
        else
        {
            if (timer > CaptureTime)
                CaptureFlag = false;
        }

        if (timer > oldtimer + 0.3f)
        {
            oldtimer = timer;
            if (ContinuosAttack-- > 0)
            {
                Instantiate(SwordPrefab, trs.position + offset, Quaternion.identity);
                audioSource.PlayOneShot(soundsord);
            }
        }
    }

    void FixedUpdate()
    {
        if (!CaptureFlag)
        {
            float X = worldPos.x, Y = worldPos.y;
            if (transform.position.x > RightEnd)
                X = RightEnd;
            else
                if (transform.position.x < LeftEnd)
                X = LeftEnd;

            if (transform.position.y > Over)
                Y = Over;
            else
                if (transform.position.y < Under)
                Y = Under;
            
            transform.position = Vector3.MoveTowards(transform.position, new Vector2(X, Y), MoveSpeed * moveDist);
        }
    }

    public void GoAttack()
    {
        ContinuosAttack = 5;
        audioSource.PlayOneShot(soundsord);
        Instantiate(SwordPrefab, trs.position + offset, Quaternion.identity);
    }

    public void Damage(float Damage)
    {
        HP -= Damage;
        if (HP <= 0 && DeathFlag)
        {
            var objs= GameObject.FindGameObjectsWithTag("Attack");
            foreach(GameObject obj in objs)
            {
                Destroy(obj);
            }
            objs = GameObject.FindGameObjectsWithTag("EAttack");
            foreach (GameObject obj in objs)
            {
                Destroy(obj);
            }
            DeathFlag = false;
            GameObject.Find("GameMaster").GetComponent<StageScript>().GameEnd();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Item")
        {
            PlayersStrength.GameItemCount(collision.name);
            Destroy(collision.gameObject);
        }
    }
    public void TapOnDango()
    {
        HP += 50;
        if (100 < HP)
            HP = 100;
    }
    public void TapOnPower()
    {
        SwordSPr.AttackPower++;
    }

    public float GetHP()
    {
        return HP;
    }

    void Capture(int val)
    {
        CaptureFlag = true;
        CaptureTime = val;
        timer = 0;
    }

    public void WorldPointUpdate()
    {
        worldPos = new Vector3(0, 0, 0);
        Object.Instantiate(TapEffect, new Vector3(worldPos.x, worldPos.y, 0), Quaternion.identity);
    }
    public void WorldPointSet()
    {
        worldPos = new Vector3(5, 1, 0);
        Object.Instantiate(TapEffect, new Vector3(worldPos.x, worldPos.y, 0), Quaternion.identity);
    }
}
