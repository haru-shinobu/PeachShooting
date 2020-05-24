using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class EnemyGaneratorScript : MonoBehaviour
{
    
    public bool StartFlag = false;
    GameObject obj;
    public int HPmagnification = 1;
    float timer,oldtimer;
    int EnemiesNum = 0;
    private int StageEnemyAllNum;
    float Under,Over;
    GameObject Player;
    GameObject Boss;
    GameObject InstantBoss;
    GameObject[] Enem;
    bool bossflag = false;
    public bool BossHPLessFlag = false;
    bool Stage4Flag = false;
    int StageBossNum = 0;
    Text EnemyCountText;
    AudioScript audioScript;
    void Start()
    {

        gameObject.AddComponent<AudioSource>();
        audioScript = GameObject.FindWithTag("BGM").GetComponent<AudioScript>();
        obj = Resources.Load<GameObject>("Mob");
        obj.GetComponent<EnemyScript>().HP *= HPmagnification;
        timer = oldtimer = 0;
        EnemiesNum = 0;
        Under = -1.3f;
        Over = 3.5f;
        Player = GameObject.FindWithTag("Player");
        Boss = Resources.Load<GameObject>("Boss_");
        EnemyCountText = GameObject.Find("GameMaster").transform.GetChild(0).GetChild(12).GetChild(0).GetComponent<Text>();
        StageEnemyAllNum = 10;
        var setting = GameObject.FindGameObjectWithTag("AllSceneManager").GetComponent<InstantSaveScript>();
        switch (SceneManager.GetActiveScene().name)
        {
            case "Stage1":
                Boss.name = "Boss_Dog";
                StageEnemyAllNum += (int)(StageEnemyAllNum * setting.SettingsRead("DOG"));
                EnemyCountText.text = "敵:" + "0 / " + StageEnemyAllNum.ToString("F0");
                break;
            case "Stage2":
                Boss.name = "Boss_Monkey";
                StageEnemyAllNum += (int)(StageEnemyAllNum * setting.SettingsRead("MONKEY"));
                EnemyCountText.text = "敵:" + "0 / " + StageEnemyAllNum.ToString("F0");
                break;
            case "Stage3":
                Boss.name = "Boss_Bird";
                StageEnemyAllNum += (int)(StageEnemyAllNum * setting.SettingsRead("BIRD"));
                EnemyCountText.text = "敵:" + "0 / " + StageEnemyAllNum.ToString("F0");
                break;
            case "StageDeath":
                Stage4Flag = true;
                Boss = Resources.Load<GameObject>("Egreen");
                StageEnemyAllNum = 0;
                EnemyCountText.text = "ボス:" + "0 / 5";
                break;
        }
        

        GameObject.FindWithTag("GameController").GetComponent<StageScript>().GameItemCount("DangoUp");
    }

    // Update is called once per frame
    void Update()
    {
        if (StartFlag)
        {
            if (EnemiesNum < StageEnemyAllNum)
            {
                timer += Time.deltaTime;
                if (timer >= oldtimer + 1) 
                {
                    oldtimer = timer;
                    int randtimer = Random.Range(2, 6);
                    if (timer >= randtimer)
                    {
                        EnemiesNum++;
                        timer -= randtimer;
                        Instantiate(obj, new Vector3(-10, Random.Range(Under, Over), 0), Quaternion.identity);
                        EnemyCountText.text = "敵:" + EnemiesNum.ToString("F0") + " / " + StageEnemyAllNum.ToString("F0");
                        if (EnemiesNum >= StageEnemyAllNum) timer = 0;
                    }
                }
                
            }
            else
            {
                timer += Time.deltaTime;
                if (timer > 1)
                {
                    timer -= 1;
                    Enem = GameObject.FindGameObjectsWithTag("Enemy");
                    if (Enem.Length == 0)
                    {
                        BossCraft();
                        StartFlag = false;
                    }
                }
            }
        }
        else
        if(bossflag)
        {
            if (BossHPLessFlag)
            {
                BossHPLessFlag = false;
                if (!Stage4Flag)
                    BossDefeat();
                else
                    NextBossCraft();
            }
        }
    }

    void BossCraft()
    {
        EnemyCountText.text = "ボス:" + "1 / 1";
        bossflag = true;
        InstantBoss = Instantiate(Boss);

        switch (SceneManager.GetActiveScene().name)
        {
            case "Stage1":
                gameObject.AddComponent<DogBossTextScript>().Stagenum = 1;
                gameObject.AddComponent<DogBossEventScript>().type = 1;
                break;
            case "Stage2":
                gameObject.AddComponent<DogBossTextScript>().Stagenum = 2;
                gameObject.AddComponent<DogBossEventScript>().type = 2;
                break;
            case "Stage3":
                gameObject.AddComponent<DogBossTextScript>().Stagenum = 3;
                gameObject.AddComponent<DogBossEventScript>().type = 3;
                break;
            case "StageDeath":
                EnemyCountText.text = "ボス:" + "1 / 5";
                gameObject.AddComponent<DogBossTextScript>().Stagenum = 4;
                gameObject.AddComponent<DogBossEventScript>().type = 4;
                break;
        }
        //BGM Change
        audioScript.BossBattle();
    }
    void NextBossCraft()
    {
        if (InstantBoss)
        {
            Destroy(InstantBoss);
        }

        if (StageBossNum < 1)
        {
            gameObject.AddComponent<DogBossTextScript>().Stagenum = 5;
            gameObject.AddComponent<DogBossEventScript>().type = 5;
            Boss = Resources.Load<GameObject>("Eblue");
            InstantBoss = Instantiate(Boss);
        }
        else
            if (StageBossNum < 2)
        {
            gameObject.AddComponent<DogBossTextScript>().Stagenum = 6;
            gameObject.AddComponent<DogBossEventScript>().type = 6;
            Boss = Resources.Load<GameObject>("Egray");
            InstantBoss = Instantiate(Boss);
        }
        else
            if (StageBossNum < 3)
        {
            gameObject.AddComponent<DogBossTextScript>().Stagenum = 7;
            gameObject.AddComponent<DogBossEventScript>().type = 7;
            Boss = Resources.Load<GameObject>("Eyellow");
            InstantBoss = Instantiate(Boss);
        }
        else
            if (StageBossNum < 4)
        {
            gameObject.AddComponent<DogBossTextScript>().Stagenum = 8;
            gameObject.AddComponent<DogBossEventScript>().type = 8;
            Boss = Resources.Load<GameObject>("Ered");
            InstantBoss = Instantiate(Boss);
        }
        else
            BossDefeat();
        StageBossNum++;
        EnemyCountText.text = "ボス:" + (StageBossNum + 1).ToString("F0") + " / 5";
    }

    void BossDefeat()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Stage1":
                gameObject.AddComponent<DogBossDefeatScript>().Stagenum = 1;
                break;
            case "Stage2":
                gameObject.AddComponent<DogBossDefeatScript>().Stagenum = 2;
                break;
            case "Stage3":
                gameObject.AddComponent<DogBossDefeatScript>().Stagenum = 3;
                break;
            case "StageDeath":
                gameObject.AddComponent<DogBossDefeatScript>().Stagenum = 4;
                break;
        }
        //BGM Change
        audioScript.Ending();
    }
    public void Tresure()
    {
        StartCoroutine("Tresuremove");
    }
    IEnumerator Tresuremove()
    {
        var Tresure = Instantiate(Resources.Load<GameObject>("TresureBox"), new Vector3(-10, 0, 0), Quaternion.identity);
        var targetPosition = new Vector3(0, 0);
        float count = 0;
        while (!GetComponent<DogBossDefeatScript>().bStopTresure && count <= 1)
        {
            count += Time.deltaTime * 0.5f;
            Vector3.Lerp(Tresure.transform.position, targetPosition, count);
            yield return new WaitForEndOfFrame();
        }
        yield break;
    }
}
