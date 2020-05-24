using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldMScript : MonoBehaviour
{
    public int Phase;
    RectTransform HandRec;
    RectTransform Rec;
    Vector3 BGPos;
    Vector2 Pos;
    float upLimit;
    float time;
    GameObject Peach;
    GameObject Canvas;
    NextPagesScript NextPages;
    bool b_Test_Find = false;
    public bool endingflag = false;
    void Start()
    {
        Phase = 0;
        Peach = GameObject.Find("PeachImage");
        Rec = gameObject.transform.GetComponent<RectTransform>();
        HandRec = gameObject.transform.GetChild(0).GetComponent<RectTransform>();
        RectTransform BG = GameObject.Find("BackGround").GetComponent<RectTransform>();
        BGPos = BG.localPosition - new Vector3(BG.rect.width * 0.5f, BG.rect.height * 0.45f, 0);
        Pos = HandRec.localPosition + new Vector3(HandRec.localScale.x, HandRec.localScale.y, 0);
        Rec.localRotation = new Quaternion(0, 0, -0.02f, 0.1f);
        upLimit = 6.0f;
        time = 0;
        Canvas = gameObject.transform.root.gameObject;
        if (GameObject.Find("AllCanvas"))
        {
            NextPages = GameObject.Find("AllCanvas").GetComponent<NextPagesScript>();
            b_Test_Find = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!endingflag)
            switch (Phase)
            {
                case 0:
                    time += Time.deltaTime * 10;
                    HandRec.localPosition = (new Vector3(Pos.x + Mathf.PingPong(time, upLimit * 0.5f), Pos.y + Mathf.PingPong(time, upLimit), 0));
                    break;
                case 1://向き反転
                    Rec.localRotation = new Quaternion(0, 0, 0, 0);
                    Rec.localScale = new Vector3(-Rec.localScale.x, Rec.localScale.y, Rec.localScale.z);
                    time = 0;
                    Phase++;
                    break;
                case 2:
                    Rec.localPosition = Vector2.Lerp(Rec.localPosition, BGPos, Time.deltaTime);
                    time += Time.deltaTime;
                    if (1.6f <= time)
                        Phase++;
                    break;
                case 3:
                    Peach.transform.SetParent(Canvas.transform);
                    Rec.localScale = new Vector3(-Rec.localScale.x, Rec.localScale.y, Rec.localScale.z);
                    Phase++;
                    break;
                case 4:
                    Rec.localPosition = Rec.localPosition + new Vector3(0.2f, 0);
                    time += Time.deltaTime;
                    if (2.0f <= time)
                        Phase++;
                    break;
                case 5:
                    gameObject.SetActive(false);
                    if (b_Test_Find)
                        NextPages.GScreenCapture();
                    break;
            }
    }
}