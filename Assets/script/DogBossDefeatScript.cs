using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DogBossDefeatScript : MonoBehaviour
{
    //　読み込んだテキストを出力するUIテキスト
    [SerializeField]
    private Text NameText;
    private Text StoryText;
    //　Resourcesフォルダから直接テキストを読み込む
    private string loadText2;
    //　改行で分割して配列に入れる
    private string[] splitText2;
    //　現在表示中テキスト2番号
    private int textNum2;
    private bool OneFlag = true;
    int EventState;
    private bool Stage1NextTextFlag = true;
    GameObject Boss;
    GameObject Effect_curse, Effect_Blosam;
    float timer = 0;

    float Under;
    Vector2 StartPos;

    public int Stagenum;


    //audio
    AudioSource audioSource;
    public AudioClip se1;
    public AudioClip se2;
    public AudioClip se3;

    public bool bStopTresure = false;
    void Start()
    {
        GameObject.FindWithTag("GameController").GetComponent<StageScript>().FrendAttackNG();
        OneFlag = true;
        Under = -1.3f;
        switch (Stagenum)
        {
            case 1:
                Boss = GameObject.Find("Boss_Dog(Clone)");
                loadText2 = (Resources.Load("StageBossDestroy", typeof(TextAsset)) as TextAsset).text;
                break;
            case 2:
                Boss = GameObject.Find("Boss_Monkey(Clone)");
                loadText2 = (Resources.Load("StageBossDestroy2", typeof(TextAsset)) as TextAsset).text;
                break;
            case 3:
                Boss = GameObject.Find("Boss_Bird(Clone)");
                loadText2 = (Resources.Load("StageBossDestroy3", typeof(TextAsset)) as TextAsset).text;
                break;
            case 4:
                loadText2 = (Resources.Load("StageBossDestroy4", typeof(TextAsset)) as TextAsset).text;
                break;
        }

        Effect_curse = Resources.Load<GameObject>("Effect_curse");
        Effect_Blosam = Resources.Load<GameObject>("EffectBlosam");

        NameText = transform.GetChild(0).GetChild(1).GetComponent<Text>();
        StoryText = transform.GetChild(0).GetChild(2).GetComponent<Text>();

        transform.GetChild(0).gameObject.SetActive(true);
        splitText2 = loadText2.Split(char.Parse("▼"));

        textNum2 = 0;
        NameText.text = "";
        StoryText.text = "";

        EventState = 0;
        timer = 0;
        Input.GetMouseButtonDown(0);


        //audio
        audioSource = GetComponent<AudioSource>();
        se1 = Resources.Load<AudioClip>("taiko03");
        se2 = Resources.Load<AudioClip>("scape");
        se3 = Resources.Load<AudioClip>("shine6");

        GameObject.FindGameObjectWithTag("Dog").GetComponent<SupportScript>().MoveStateSet();
        GameObject.FindGameObjectWithTag("Monkey").GetComponent<SupportScript>().MoveStateSet();
        GameObject.FindGameObjectWithTag("Bird").GetComponent<SupportScript>().MoveStateSet();

    }

    void Update()
    {
        if (Stage1NextTextFlag)
        {
            timer += Time.deltaTime;
            if (splitText2[textNum2] != "@")
            {
                if (Input.GetMouseButtonDown(0) || OneFlag)
                {
                    this.StartPos = Input.mousePosition;
                    Vector3 worldPos = Camera.main.ScreenToWorldPoint(StartPos);

                    if (worldPos.y <= Under || OneFlag)
                    {

                        OneFlag = false;
                        if (timer > 1)
                        {

                            if (splitText2[textNum2] != "" || splitText2[textNum2] != "\n")
                            {
                                NameText.text = splitText2[textNum2];
                                textNum2++;
                                if (textNum2 >= splitText2.Length)
                                {
                                    TextEnd();
                                }
                                if (splitText2[textNum2] == "\n" || splitText2[textNum2] == "")
                                    textNum2++;
                                StoryText.text = splitText2[textNum2];
                                textNum2++;
                                if (textNum2 >= splitText2.Length)
                                {
                                    TextEnd();
                                }
                            }
                            else
                            {
                                textNum2++;
                            }
                        }
                    }
                }
            }
            else
            {

                EventState++;
                textNum2++;

                Coll(EventState);
                if (textNum2 >= splitText2.Length)
                {
                    TextEnd();
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Coll(5);
                Destroy(this);
            }
        }
    }
    void TextEnd()
    {
        if (Stage1NextTextFlag)
        {
            Stage1NextTextFlag = false;
        }
    }

    void Coll(int num)
    {
        if (Stagenum != 4)
        {
            switch (num)
            {
                case 1://のろい
                    Effect_curse = Instantiate(Effect_curse, Boss.transform.position, Quaternion.identity, transform);
                    Boss.GetComponent<SpriteRenderer>().color = new Color32(95, 95, 150, 255);
                    audioSource.PlayOneShot(se1);
                    break;
                case 2://鬼化解除
                    Boss.GetComponent<SpriteRenderer>().color = new Color32(150, 150, 150, 255);
                    Boss.transform.localScale = new Vector3(-0.4f, 0.4f);
                    audioSource.PlayOneShot(se2);
                    break;
                case 3://治療
                    Destroy(Effect_curse);
                    Instantiate(Effect_Blosam, Boss.transform.position, Quaternion.identity, transform);
                    Boss.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
                    audioSource.volume *= 0.4f;
                    audioSource.PlayOneShot(se3);
                    break;
                case 5:
                    transform.GetChild(0).gameObject.SetActive(false);

                    Destroy(Boss);
                    switch (Stagenum)
                    {
                        case 1:
                            GameObject.Find("AllSceneManager").GetComponent<SceneManagerScript>().SceneChange("Stage2", 1);
                            //GameObject.Find("AllSceneManager").GetComponent<SceneManagerScript>().SceneChange("Clear", 1);
                            break;
                        case 2:
                            GameObject.Find("AllSceneManager").GetComponent<SceneManagerScript>().SceneChange("Stage3", 1);
                            break;
                        case 3:
                            GameObject.Find("AllSceneManager").GetComponent<SceneManagerScript>().SceneChange("StageDeath", 1);
                            break;
                    }

                    GameObject.Find("AllCanvas").GetComponent<NextPagesScript>().GScreenCapture();
                    Destroy(this);
                    break;

            }
        }
        else
        {
            switch (num)
            {
                /*
                case 1:
                    GameObject.FindWithTag("Bird");
                    GameObject.FindWithTag("Dog");
                    GameObject.FindWithTag("Monkey");
                    break;
                case 2:
                    break;
                case 3:
                    break;
                    */
                case 4:
                    GetComponent<EnemyGaneratorScript>().Tresure();
                    break;
                case 5:
                    GameObject.Find("Canvas").GetComponent<ScrollScript>().Stop();
                    GameObject.Find("RockPoint").GetComponent<RockControllerScript>().Stop();

                    bStopTresure = true;
                    transform.GetChild(0).gameObject.SetActive(false);
                    GameObject.Find("AllSceneManager").GetComponent<SceneManagerScript>().ClearScene();
                    GameObject.Find("AllCanvas").GetComponent<NextPagesScript>().GScreenCapture();
                    Destroy(this);
                    break;
            }
        }
    }
}
