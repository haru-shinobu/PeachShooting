using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DogBossTextScript : MonoBehaviour
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
    private bool StageNextTextFlag = true;
    DogBossEventScript EventManual;
    float timer = 0;

    float Under;
    Vector3 worldPos;
    Vector2 StartPos;
    public int Stagenum;
    void Start()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        Under = -1.3f;
        switch (Stagenum)
        {
            case 1:
                loadText2 = (Resources.Load("Stage1Enemy", typeof(TextAsset)) as TextAsset).text;
                break;
            case 2:
                loadText2 = (Resources.Load("Stage2Enemy", typeof(TextAsset)) as TextAsset).text;
                break;
            case 3:
                loadText2 = (Resources.Load("Stage3Enemy", typeof(TextAsset)) as TextAsset).text;
                break;
            case 4:
                loadText2 = (Resources.Load("Stage4Enemy", typeof(TextAsset)) as TextAsset).text;
                break;
            case 5:
                loadText2 = (Resources.Load("Stage5Enemy", typeof(TextAsset)) as TextAsset).text;
                break;
            case 6:
                loadText2 = (Resources.Load("Stage6Enemy", typeof(TextAsset)) as TextAsset).text;
                break;
            case 7:
                loadText2 = (Resources.Load("Stage7Enemy", typeof(TextAsset)) as TextAsset).text;
                break;
            case 8:
                loadText2 = (Resources.Load("Stage8Enemy", typeof(TextAsset)) as TextAsset).text;
                break;
        }
        OneFlag = true;
        NameText = transform.GetChild(0).GetChild(1).GetComponent<Text>();
        StoryText = transform.GetChild(0).GetChild(2).GetComponent<Text>();

        transform.GetChild(0).gameObject.SetActive(true);
        splitText2 = loadText2.Split(char.Parse("▼"));

        textNum2 = 0;
        NameText.text = "";
        StoryText.text = "";

        EventState = 0;
        EventManual = GetComponent<DogBossEventScript>();
    }

    void Update()
    {
        if (StageNextTextFlag)
        {
            timer += Time.deltaTime;
            if (splitText2[textNum2] != "@")
            {
                if (Input.GetMouseButtonDown(0) || OneFlag)
                {
                    Vector3 InstantPos = worldPos;
                    this.StartPos = Input.mousePosition;
                    worldPos = Camera.main.ScreenToWorldPoint(StartPos);

                    if (worldPos.y <= Under || OneFlag)
                    {
                        if (timer > 1)
                        {
                            timer = 0;
                            OneFlag = false;

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

                EventManual.Coll(EventState);
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
                Destroy(EventManual);
                EventManual.Coll(100);
                Destroy(this);
            }
        }
    }
    void TextEnd()
    {
        if (StageNextTextFlag)
        {
            StageNextTextFlag = false;
        }
    }
}