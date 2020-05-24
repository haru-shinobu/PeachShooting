using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageStoryScript : MonoBehaviour
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
    EventScript1 EventManual;
    EventScript2 EventManual2;
    EventScript3 EventManual3;
    EventScript4 EventManual4;
    int StageType;
    float Under = -2.0f;
    Vector2 StartPos;
    void Start()
    {
        Under = -2.0f;
        switch (SceneManager.GetActiveScene().name)
        {
            case "Stage1":
                loadText2 = (Resources.Load("Story", typeof(TextAsset)) as TextAsset).text;
                EventManual = GetComponent<EventScript1>();
                StageType = 1;
                break;
            case "Stage2":
                loadText2 = (Resources.Load("Story2", typeof(TextAsset)) as TextAsset).text;
                EventManual2 = GetComponent<EventScript2>();
                StageType = 2;
                break;
            case "Stage3":
                loadText2 = (Resources.Load("Story3", typeof(TextAsset)) as TextAsset).text;
                EventManual3 = GetComponent<EventScript3>();
                StageType = 3;
                break;
            case "StageDeath":
                loadText2 = (Resources.Load("Story4", typeof(TextAsset)) as TextAsset).text;
                EventManual4 = GetComponent<EventScript4>();
                StageType = 4;
                break;
        }
        
        NameText = transform.GetChild(0).GetChild(1).GetComponent<Text>();
        StoryText = transform.GetChild(0).GetChild(2).GetComponent<Text>();
        

        splitText2 = loadText2.Split(char.Parse("▼"));

        textNum2 = 0;
        NameText.text = "表示域";
        StoryText.text = "表示域";

        EventState = 0;
        Stage1NextTextFlag = true;
    }

    void Update()
    {
        if (Stage1NextTextFlag)
        {
            if (splitText2[textNum2] != "@")
            {
                if (Input.GetMouseButtonDown(0) || OneFlag)
                {   
                    this.StartPos = Input.mousePosition;
                    Vector3 worldPos = Camera.main.ScreenToWorldPoint(StartPos);

                    if (worldPos.y <= Under || OneFlag)
                    {
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
            else
            {

                EventState++;
                textNum2++;
                switch (StageType)
                {
                    case 1: EventManual.Coll(EventState);break;
                    case 2: EventManual2.Coll(EventState);break;
                    case 3: EventManual3.Coll(EventState);break;
                    case 4: EventManual4.Coll(EventState); break;
                }
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
                switch (StageType)
                {
                    case 1: EventManual.Coll(100) ;Destroy(EventManual); break;
                    case 2: EventManual2.Coll(100);Destroy(EventManual2); break;
                    case 3: EventManual3.Coll(100);Destroy(EventManual3); break;
                    case 4: EventManual4.Coll(100); Destroy(EventManual4); break;
                }
                Destroy(this);
            }
        }
    }
    void TextEnd()
    {
        if (Stage1NextTextFlag)
        {
            Stage1NextTextFlag = false;
            switch (SceneManager.GetActiveScene().name)
            {
                case "Stage1":
                    EventManual.Dest();
                    break;
                case "Stage2":
                    EventManual2.Dest();
                    break;
                case "Stage3":
                    EventManual3.Dest();
                    break;
                case "StageDeath":
                    break;
            }
        }
    }
}