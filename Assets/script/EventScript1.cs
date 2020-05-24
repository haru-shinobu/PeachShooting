using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EventScript1 : MonoBehaviour
{

    bool Flag = false;
    float timer = 0;
    GameObject PointHand;
    Image Hander;
    GameObject StoryCanvas;
    GameObject OptionBG;
    GameObject OldMa;
    GameObject dango;
    GameObject Player;
    StageScript stage;
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        Player.GetComponent<SpriteRenderer>().sortingOrder = 10;
        Hander = Resources.Load<Image>("handImage");
        StoryCanvas = GameObject.Find("StoryCanvas");

        OptionBG = Instantiate(Resources.Load<GameObject>("OptionBG"));

        var bg = OptionBG.GetComponent<SpriteRenderer>();
        float worldScreenHeight = Camera.main.orthographicSize * 2f;
        float worldScreenWight = worldScreenHeight / Screen.height * Screen.width;
        float width = bg.sprite.bounds.size.x;
        float height = bg.sprite.bounds.size.y;
        OptionBG.transform.localScale = new Vector3(worldScreenWight / width, worldScreenHeight / height);
        Vector3 camPos = Camera.main.transform.position;
        camPos.z = 0;
        OptionBG.transform.position = camPos;

        OptionBG.GetComponent<SpriteRenderer>().sortingOrder = 8;
        PointHand = Instantiate(Resources.Load<GameObject>("hand"));

        OldMa = Resources.Load<GameObject>("OldMa");
        dango = Resources.Load<GameObject>("dango2");
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
                Destroy(PointHand);
                stage.FrendOut(0);
                Hander = Instantiate(Hander, StoryCanvas.transform);
                break;
            case 3://攻撃
                Hander.rectTransform.localScale = new Vector3(-1, 1, 1);
                Hander.rectTransform.localPosition = new Vector3(300, -75, 0);
                break;
            case 4://かいふく
                Hander.rectTransform.localScale = new Vector3(1, 1, 1);
                Hander.rectTransform.localPosition = new Vector3(250, -75, 0);
                break;
            case 5:
                Hander.rectTransform.localPosition += new Vector3(-240, 0, 0);
                break;
            case 6:
                Hander.rectTransform.localPosition += new Vector3(-80, 0, 0);
                break;
            case 7:
                Destroy(Hander);
                OldMa = Instantiate(OldMa, Player.transform.position + new Vector3(-4, 0), Quaternion.identity);
                dango = Instantiate(dango, OldMa.transform.position + new Vector3(1, 0), Quaternion.identity);
                break;
            case 8:
                Flag = true;
                break;
            case 100:
                Player.GetComponent<SpriteRenderer>().sortingOrder = 4;
                gameObject.GetComponent<EnemyGaneratorScript>().StartFlag = true;
                transform.GetChild(0).gameObject.SetActive(false);
                break;
        }
    }
    void Update()
    {
        if (Flag)
        {
            timer += Time.deltaTime;
            if (timer < 0.8f)
            {
                if (dango)
                {
                    dango.transform.position = Vector3.Lerp(OldMa.transform.position, Player.transform.position, timer);
                    dango.transform.SetParent(Player.transform);
                }
            }
            else
                Flag = !Flag;
        }
    }
    public void Dest()
    {
        Destroy(OptionBG);
        Destroy(OldMa);
        Destroy(dango);
    }
}
