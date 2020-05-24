using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StageScript : MonoBehaviour
{
    GameObject ServantDog, ServantMonkey, ServantBird;
    bool FriendFlag = false;
    int Friendmover = 0;
    float Frendmovetimer = 0;
    RectTransform[] ButtonTrs;
    Vector3[] ButtonPos;
    PlayerControllerScript Player;
    SupportScript sup1, sup2, sup3;
    List<GameObject> Button = new List<GameObject>();
    GameObject[] Effect;
    int ItemUse;

    public int StageEnemyItemRandomMax;
    public int
        Heal,
        Power,
        Faster,
        Spread,
        PlayerSpeed;
    public int
        UpGradePower = 0,
        UpGradeFaster = 0,
        UpGradeSpread = 0,
        UpGradePlayerSpeed = 0;
    void Awake()
    {
        Instantiate(Resources.Load<GameObject>("BGMObject"));
        ServantDog = GameObject.Find("dog");
        ServantMonkey = GameObject.Find("monkey");
        ServantBird = GameObject.Find("bird2");
        Player = GameObject.Find("Player").GetComponent<PlayerControllerScript>();

        sup1 = ServantDog.GetComponent<SupportScript>();
        sup2 = ServantMonkey.GetComponent<SupportScript>();
        sup3 = ServantBird.GetComponent<SupportScript>();

        GameObject GMCanvas = GameObject.Find("GMCanvas");
        for (int i = 0; i < 3; i++)
            Button.Add(GMCanvas.transform.GetChild(i).gameObject);
        Button.Add(GMCanvas.transform.GetChild(5).gameObject);
        Button.Add(GMCanvas.transform.GetChild(6).gameObject);
        ItemUse = 0;
        Heal = 0;
        Power = 0;
        Faster = 0;
        Spread = 0;
        PlayerSpeed = 0;
        UpGradePower = 0;
        UpGradeFaster = 0;
        UpGradeSpread = 0;
        UpGradePlayerSpeed = 0;
        GameObject EnemyGenerator = GameObject.FindWithTag("EnemyGenerator");
        switch (SceneManager.GetActiveScene().name)
        {
            case "Stage1":

                StageEnemyItemRandomMax = 3;
                ServantDog.gameObject.SetActive(false);
                ServantMonkey.gameObject.SetActive(false);
                ServantBird.gameObject.SetActive(false);
                GameObject.Find("GMCanvas").GetComponent<ButtonTextScript>().StageStep = 0;
                EnemyGenerator.AddComponent<StageStoryScript>();
                EnemyGenerator.AddComponent<EventScript1>();
                break;
            case "Stage2":
                StageEnemyItemRandomMax = 3;
                ServantDog.gameObject.SetActive(true);
                ServantMonkey.gameObject.SetActive(false);
                ServantBird.gameObject.SetActive(false);
                GameObject.Find("GMCanvas").GetComponent<ButtonTextScript>().StageStep = 1;
                EnemyGenerator.AddComponent<StageStoryScript>();
                EnemyGenerator.AddComponent<EventScript2>();
                break;
            case "Stage3":
                StageEnemyItemRandomMax = 4;
                ServantDog.gameObject.SetActive(true);
                ServantMonkey.gameObject.SetActive(true);
                ServantBird.gameObject.SetActive(false);
                GameObject.Find("GMCanvas").GetComponent<ButtonTextScript>().StageStep = 2;
                EnemyGenerator.AddComponent<StageStoryScript>();
                EnemyGenerator.AddComponent<EventScript3>();
                break;
            case "StageDeath":
                StageEnemyItemRandomMax = 6;
                ServantDog.gameObject.SetActive(true);
                ServantMonkey.gameObject.SetActive(true);
                ServantBird.gameObject.SetActive(true);
                GameObject.Find("GMCanvas").GetComponent<ButtonTextScript>().StageStep = 3;
                EnemyGenerator.AddComponent<StageStoryScript>();
                EnemyGenerator.AddComponent<EventScript4>();

                break;
        }
        Effect = new GameObject[]
        {
            Resources.Load<GameObject>("Effect_HPHeal"),
            Resources.Load<GameObject>("Effect_PowerUp"),
            Resources.Load<GameObject>("Effect_SpeedUp"),
            Resources.Load<GameObject>("Effect_SpreadUp"),
            Resources.Load<GameObject>("Effect_OikakeUp")
        };
        ButtonTrs = new RectTransform[]
        {
            Button[0].GetComponent<RectTransform>(),
            Button[1].GetComponent<RectTransform>(),
            Button[2].GetComponent<RectTransform>(),
            Button[3].GetComponent<RectTransform>(),
            Button[4].GetComponent<RectTransform>()
        };
        ButtonPos = new Vector3[]
        {
            Button[0].transform.localPosition,
            Button[1].transform.localPosition,
            Button[2].transform.localPosition,
            Button[3].transform.localPosition,
            Button[4].transform.localPosition
        };
    }
    
    public void GameEnd()
    {
        sup1.AttackOK = sup2.AttackOK = sup3.AttackOK = false;

        GameObject.Find("AllSceneManager").GetComponent<SceneManagerScript>().EndScene();
        GameObject.Find("AllCanvas").GetComponent<NextPagesScript>().GScreenCapture();
    }

    public void GameItemCount(string Itemname)
    {
        switch (Itemname)
        {
            case "DangoUp":
            case "DangoUp(Clone)":
                Heal++;
                break;
            case "PowerUp":
            case "PowerUp(Clone)":
                Power++;
                break;
            case "FasterUp":
            case "FasterUp(Clone)":
                Faster++;
                break;
            case "SpreadUp":
            case "SpreadUp(Clone)":
                Spread++;
                break;
            case "PlayerSpeedUp":
            case "PlayerSpeedUp(Clone)":
                PlayerSpeed++;
                break;
        }
    }

    public void TapOnDango()
    {
        if (0 < Heal)
        {
            Heal--;
            Instantiate(Effect[0], Player.transform.position+new Vector3(0,-0.3f),Quaternion.identity,Player.transform);
            Player.TapOnDango();
        }
    }

    public void TapOnPower()
    {
        if (0 < Power)
        {
            ItemUse++;
            Power--;
            Instantiate(Effect[1], Player.transform.position, Quaternion.identity, Player.transform);
            sup1.ItemState = SupportScript.ItemType.Power;
            sup2.ItemState = SupportScript.ItemType.Power;
            sup3.ItemState = SupportScript.ItemType.Power;
            Player.TapOnPower();
            UpGradePower++;
        }
    }
    
    public void TapOnFaster()
    {
        if (0 < Faster)
        {
            ItemUse++;
            Faster--;
            
            Instantiate(Effect[2], Player.transform.position, Quaternion.identity, Player.transform);
            sup1.ItemState = SupportScript.ItemType.Faster;
            sup2.ItemState = SupportScript.ItemType.Faster;
            sup3.ItemState = SupportScript.ItemType.Faster;
            Player.AttackSpeed += 0.1f;
            UpGradeFaster++;
        }
    }
    public void TapOnSpread()
    {
        if (0 < Spread)
        {
            ItemUse++;
            Spread--;
            Instantiate(Effect[3], ServantMonkey.transform.position, Quaternion.identity,ServantMonkey.transform);
            sup2.ItemState = SupportScript.ItemType.Spread;
            UpGradeSpread++;
        }
    }
    public void TapOnTracking()
    {
        if (0 < PlayerSpeed)
        {
            ItemUse++;
            PlayerSpeed--;
            Instantiate(Effect[4], Player.transform.position, Quaternion.identity,Player.transform);
            Player.MoveSpeed *= 1.2f;
            UpGradePlayerSpeed++;
        }
    }

    public void FrendOut(int val)
    {
        FriendFlag = true;
        Friendmover = val;
    }

    public void FrendAttackOk()
    {
        if (ServantDog)
            ServantDog.gameObject.GetComponent<SupportScript>().AttackOK = true;
        if (ServantMonkey)
            ServantMonkey.gameObject.GetComponent<SupportScript>().AttackOK = true;
        if (ServantBird)
            ServantBird.gameObject.GetComponent<SupportScript>().AttackOK = true;
    }
    public void FrendAttackNG()
    {
        if (ServantDog)
            ServantDog.gameObject.GetComponent<SupportScript>().AttackOK = false;
        if (ServantMonkey)
            ServantMonkey.gameObject.GetComponent<SupportScript>().AttackOK = false;
        if (ServantBird)
            ServantBird.gameObject.GetComponent<SupportScript>().AttackOK = false;
    }

    void Update()
    {
        if (FriendFlag)
        {
            Frendmovetimer += Time.deltaTime;
            for (int i = Friendmover; i < 3; i++)
            {
                ButtonTrs[i].transform.localPosition = Vector3.Lerp(ButtonPos[i], ButtonPos[i] + new Vector3(0, -110), Frendmovetimer);
                if (Friendmover <= 1) 
                {
                    ButtonTrs[3].transform.localPosition = Vector3.Lerp(ButtonPos[3], ButtonPos[3] + new Vector3(0, -110), Frendmovetimer);
                    ButtonTrs[4].transform.localPosition = Vector3.Lerp(ButtonPos[4], ButtonPos[4] + new Vector3(0, -110), Frendmovetimer);
                }
                else if(Friendmover == 2)
                {
                    ButtonTrs[4].transform.localPosition = Vector3.Lerp(ButtonPos[4], ButtonPos[4] + new Vector3(0, -110), Frendmovetimer);
                }
            }
        }
    }
}
