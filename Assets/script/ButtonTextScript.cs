using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonTextScript : MonoBehaviour
{
    Text[] tex;
    Text[] StatusNowtex;
    int[] State = new int[] { 0, 0, 0 };
    SupportScript[] sup;
    SupportScript supcharascr0;
    SupportScript supcharascr1;
    SupportScript supcharascr2;
    public int StageStep;
    
    string[] Order = new string[] { "いどう！", "そこから！", "いっしょに！" ,"いない"};

    GameObject[] Chains;

    StageScript stageSp;
    bool CaputureFlag = false;
    float timer,Limtimer;
    Image Status_foot;
    Image Status_poison;



    void Start()
    {
        stageSp = GameObject.Find("GameMaster").GetComponent<StageScript>();
        tex = new Text[] {
            //いぬさるきじ
        transform.GetChild(0).GetChild(1).gameObject.GetComponent<Text>(),
        transform.GetChild(1).GetChild(1).gameObject.GetComponent<Text>(),
        transform.GetChild(2).GetChild(1).gameObject.GetComponent<Text>(),
        //ちから・すばやさ・はんい・おいかぜ
        transform.GetChild(3).GetChild(2).gameObject.GetComponent<Text>(),//3
        transform.GetChild(4).GetChild(2).gameObject.GetComponent<Text>(),
        transform.GetChild(5).GetChild(2).gameObject.GetComponent<Text>(),
        transform.GetChild(6).GetChild(2).gameObject.GetComponent<Text>(),
       // transform.GetChild(5).GetChild(1).gameObject.GetComponent<Text>(),
       //かいふく
        transform.GetChild(7).GetChild(3).gameObject.GetComponent<Text>()//5
        };
        StatusNowtex = new Text[]
        {
        transform.GetChild(3).GetChild(7).gameObject.GetComponent<Text>(),
        transform.GetChild(4).GetChild(7).gameObject.GetComponent<Text>(),
        transform.GetChild(5).GetChild(4).gameObject.GetComponent<Text>(),
        transform.GetChild(6).GetChild(4).gameObject.GetComponent<Text>()
        };
        for (int i = 0; i <= 2; i++)
        {
            tex[i].text = Order[3];
        }


        Chains = new GameObject[]
        {
            transform.GetChild(0).GetChild(4).gameObject,
            transform.GetChild(1).GetChild(4).gameObject,
            transform.GetChild(2).GetChild(4).gameObject,
            transform.GetChild(3).GetChild(8).gameObject,
            transform.GetChild(4).GetChild(8).gameObject,
            transform.GetChild(5).GetChild(5).gameObject,
            transform.GetChild(6).GetChild(5).gameObject,
            transform.GetChild(7).GetChild(4).gameObject
        };


        switch (StageStep){
            case 0:break;
            case 1:
                supcharascr0 = GameObject.Find("dog").GetComponent<SupportScript>();
                Chains[0].SetActive(false);
                break;
            case 2:
                supcharascr0 = GameObject.Find("dog").GetComponent<SupportScript>();
                Chains[0].SetActive(false);
                supcharascr1 = GameObject.Find("monkey").GetComponent<SupportScript>();
                Chains[1].SetActive(false);
                break;
            case 3:
                supcharascr0 = GameObject.Find("dog").GetComponent<SupportScript>();
                supcharascr1 = GameObject.Find("monkey").GetComponent<SupportScript>();
                supcharascr2 = GameObject.Find("bird2").GetComponent<SupportScript>();
                Chains[0].SetActive(false);
                Chains[1].SetActive(false);
                Chains[2].SetActive(false);
                break;
        }
        
        sup = new SupportScript[] 
        {
            supcharascr0,
            supcharascr1,
            supcharascr2
        };

        Status_foot = transform.GetChild(11).GetChild(1).GetComponent<Image>();
        Status_poison = transform.GetChild(11).GetChild(2).GetComponent<Image>();
        Status_foot.gameObject.SetActive(false);
        Status_poison.gameObject.SetActive(false);
    }

    void Update()
    {
        for (int i = 0; i < StageStep; i++)
            switch (sup[i].ReadMoveState())
            {
                case SupportScript.MoveMode.NoOrder:
                    tex[i].text = Order[0];
                    break;
                case SupportScript.MoveMode.Await:
                    tex[i].text = Order[1];
                    break;
                case SupportScript.MoveMode.Follow:
                    tex[i].text = Order[2];
                    break;
                case SupportScript.MoveMode.Delay:
                    break;
                default:
                    tex[i].text = "いない";
                    break;
            }
        TapButton3();
        TapButton4();
        TapButton5();
        TapButton6();
        TapButton7();
        if (CaputureFlag)
        {
            timer += Time.deltaTime;
            if (timer > Limtimer)
            {
                timer = 0;
                Status_foot.gameObject.SetActive(false);
                Status_poison.gameObject.SetActive(false);
                CaputureFlag = false;
            }
        }
    }

    public void TapButton3()//ちから3
    {
        if (stageSp.Power > 0)
            Chains[3].SetActive(false);
        else
            Chains[3].SetActive(true); 
        tex[3].text = (stageSp.Power).ToString("F0");
        StatusNowtex[0].text = (stageSp.UpGradePower).ToString("F0");
    }

    public void TapButton4()//素早さ4
    {
        if (stageSp.Faster > 0)
            Chains[4].SetActive(false);
        else
            Chains[4].SetActive(true); 
        tex[4].text = (stageSp.Faster).ToString("F0");
        StatusNowtex[1].text = (stageSp.UpGradeFaster).ToString("F0");
    }

    public void TapButton5()//はんい5
    {
        if (stageSp.Spread > 0)
            Chains[5].SetActive(false);
        else
            Chains[5].SetActive(true);
        tex[5].text = (stageSp.Spread).ToString("F0");
        StatusNowtex[2].text = (stageSp.UpGradeSpread).ToString("F0");
    }

    public void TapButton6()//おいかぜ6
    {
        if (stageSp.PlayerSpeed > 0)
            Chains[6].SetActive(false);
        else
            Chains[6].SetActive(true);
        tex[6].text = (stageSp.PlayerSpeed).ToString("F0");
        StatusNowtex[3].text = (stageSp.UpGradePlayerSpeed).ToString("F0");
    }

    public void TapButton7()//回復7
    {
        if (stageSp.Heal > 0)
            Chains[7].SetActive(false);
        else
            Chains[7].SetActive(true);
        tex[7].text = (stageSp.Heal).ToString("F0");
    }

    public void CurrentStatus(string type, int count)
    {
        switch (type)
        {
            case "Capture":
                CaputureFlag = true;
                Limtimer = count;
                Status_foot.gameObject.SetActive(true);
                Status_poison.gameObject.SetActive(true);
                break;
        }
    }
}
