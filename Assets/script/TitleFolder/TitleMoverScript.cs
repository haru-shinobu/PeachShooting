using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMoverScript : MonoBehaviour
{
    DizableScript Diz;
    SceneManagerScript SMS;
    int timer;
    NextPagesScript next;
    int Tapnum = 0;

    bool ButtonFlag;

    void Start()
    {
        ButtonFlag = true;
        next = GameObject.Find("AllCanvas").GetComponent<NextPagesScript>();
        Diz = gameObject.GetComponent<DizableScript>();
        SMS = GameObject.Find("AllSceneManager").GetComponent<SceneManagerScript>();
        //次シーンの読み込み待ち時間(秒)
        timer = 0;
        Tapnum = 0;
        gameObject.GetComponent<AudioSource>().volume = GameObject.Find("AllSceneManager").GetComponent<InstantSaveScript>().SettingsRead("BGM");
    }

    //TAP TO STARTボタン
    public void OnTap1()
    {
        if (ButtonFlag)
        {
            timer = 7;
            //LoadScene (ゲーム本編)
            string p = "Stage1";
            SMS.SceneChange(p, timer);
            ButtonFlag = false;
        }
        else
        {
            if (Tapnum++ == 1)
                SMS.GoNextSceneFlag = true;
        }
    }
    //せっていボタン
    public void OnTap2()
    {
        if (ButtonFlag)
        {
            string p = "Option";
            SMS.SceneChange(p, 0);
            next.GScreenCapture();
            ButtonFlag = false;
        }
    }
}
