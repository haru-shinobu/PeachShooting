using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EventScript2 : MonoBehaviour
{
    Image Hander;
    GameObject StoryCanvas;
    StageScript stage;

    void Start()
    {
        Hander = Resources.Load<Image>("handImage");
        StoryCanvas = GameObject.Find("StoryCanvas");
        stage = GameObject.FindWithTag("GameController").GetComponent<StageScript>();
    }

    public void Coll(int num)
    {

        switch (num)
        {
            case 1:
                GameObject.FindWithTag("Player").GetComponent<PlayerControllerScript>().WorldPointUpdate();
                break;
            case 2:///
                stage.FrendOut(1);
                Hander = Instantiate(Hander, StoryCanvas.transform);
                Hander.rectTransform.localScale = new Vector3(1, 1, 1);
                Hander.rectTransform.localPosition = new Vector3(-310, -75, 0);
                break;
            case 3:
                Destroy(Hander);
                break;
            case 100:
                stage.GameItemCount("DangoUp");

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
