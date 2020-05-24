using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DogBossEventScript : MonoBehaviour
{
    GameObject boss;
    bool Flag = false;
    bool moveFlag = false;
    
    float timer = 0;
    float timer2 = 0;
    public int type;
    StageScript stage;
    void Start()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        stage = GameObject.FindWithTag("GameController").GetComponent<StageScript>();
        moveFlag = false;
        stage.FrendAttackNG();
        switch (type)
        {
            case 1:
                boss = GameObject.Find("Boss_Dog(Clone)");
                break;
            case 2:
                boss = GameObject.Find("Boss_Monkey(Clone)");
                break;
            case 3:
                boss = GameObject.Find("Boss_Bird(Clone)");
                break;
            case 4:
                boss = GameObject.Find("Egreen(Clone)");
                break;
            case 5:
                boss = GameObject.Find("Eblue(Clone)");
                break;
            case 6:
                boss = GameObject.Find("Egray(Clone)");
                break;
            case 7:
                boss = GameObject.Find("Eyellow(Clone)");
                break;
            case 8:
                boss = GameObject.Find("Ered(Clone)");
                break;
        }
    }

    public void Coll(int num)
    {
        switch (num)
        {
            case 1:
                GameObject[] Effecters = GameObject.FindGameObjectsWithTag("Effect");
                foreach (GameObject effobj in Effecters)
                {
                    Destroy(effobj);
                }
                moveFlag = true;
                break;
            case 2:
                moveFlag=false;
                break;
            case 100:
                {
                    transform.GetChild(0).gameObject.SetActive(false);
                    if (type < 4)
                        boss.GetComponent<BossMove_Script>().MoveStartFlag = true;
                    else
                        boss.GetComponent<EnemyScript>().SendMessage("MoveStart");
                    stage.FrendAttackOk();
                }
                break;
        }
    }
    void Update()
    {
        if (moveFlag)
        {
            timer += Time.deltaTime;
            if (type > 3)
            {
                if (timer2 < 5)
                {
                    timer2 += Time.deltaTime;
                    boss.transform.position += new Vector3(0.01f, 0);
                }
            }
            if (timer > 1)
            {
                timer -= 1;
                if (Flag)
                {
                    Flag = !Flag;
                    boss.transform.position += new Vector3(0, 0.1f);
                }
                else
                {
                    Flag = !Flag;
                    boss.transform.position -= new Vector3(0, 0.1f);
                }
            }
        }
    }
}
