using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EventScript4 : MonoBehaviour
{
    bool Flag = false;
    float timer = 0;
    GameObject PointHand;
    Image Hander;
    GameObject StoryCanvas;
    GameObject Player;
    StageScript stage;
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        Hander = Resources.Load<Image>("handImage");
        StoryCanvas = GameObject.Find("StoryCanvas");
        stage = GameObject.FindWithTag("GameController").GetComponent<StageScript>();
    }

    public void Coll(int num)
    {
        switch (num)
        {
            case 1:
                Player.GetComponent<PlayerControllerScript>().WorldPointUpdate();
                break;
            case 2:
                stage.FrendOut(3);
                Hander = Instantiate(Hander, StoryCanvas.transform);
                Hander.rectTransform.localScale = new Vector3(1, 1, 1);
                Hander.rectTransform.localPosition = new Vector3(170, -75, 0);
                break;
            case 3:
                Hander.rectTransform.localPosition = new Vector3(-150, -75, 0);
                break;
            case 4:
                Destroy(Hander);
                break;
            case 100:
                //0.1～2.0
                var state = (int)GameObject.FindWithTag("AllSceneManager").GetComponent<InstantSaveScript>().SettingsRead("ORG");
                //1のとき３、20の時、5
                for(int i = 0; i < 3 + state; i++)
                {
                    if (0 == (int)(i / 2))
                        stage.GameItemCount("DangoUp");
                    stage.GameItemCount("DangoUp");
                    stage.GameItemCount("PowerUp");
                    stage.GameItemCount("FasterUp");
                    stage.GameItemCount("SpreadUp");
                    if (i < 3)
                        stage.GameItemCount("PlayerSpeedUp");
                }

                gameObject.GetComponent<EnemyGaneratorScript>().StartFlag = true;
                transform.GetChild(0).gameObject.SetActive(false);
                stage.FrendAttackOk();
                break;
        }
    }

    public void Dest()
    {
    }
}
