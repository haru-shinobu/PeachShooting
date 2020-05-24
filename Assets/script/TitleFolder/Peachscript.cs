using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peachscript : MonoBehaviour
{
    RectTransform peach;
    RectTransform Lake;
    RectTransform Liver;

    Vector2 PeachStartPos;
    Vector2 TargetPos;
    
    bool Flag;
    OldMScript OldM;

    int Phase = 0;
    float timer = 0;

    void Start()
    {
        peach = gameObject.GetComponent<RectTransform>();
        Lake = GameObject.Find("Button1").GetComponent<RectTransform>();
        Liver = GameObject.Find("LiverImage").GetComponent<RectTransform>();

        PeachStartPos = peach.localPosition;
        TargetPos = Lake.localPosition;
        
        
        Flag = false;
        OldM = GameObject.Find("OldMImage").GetComponent<OldMScript>();
    }

    void FixedUpdate()
    {
        if (Flag)
        {
            switch (Phase)
            {
                case 0:
                    timer += Time.deltaTime;
                    peach.localPosition = Vector2.Lerp(PeachStartPos, TargetPos, timer);
                    peach.localScale *= 0.95f;
                    if (timer >= 1)
                    {
                        timer = 0;
                        Phase++;
                        PeachStartPos = peach.localPosition;
                        TargetPos = Lake.localPosition - new Vector3(0, Lake.rect.height * 0.5f);
                    }
                    break;
                case 1:
                    timer += Time.deltaTime;
                    peach.localPosition = Vector2.Lerp(PeachStartPos, TargetPos, timer);
                    if (timer >= 1)
                    {
                        timer = 0;
                        Phase++;
                        PeachStartPos = peach.localPosition;
                        TargetPos = Liver.localPosition + new Vector3(Liver.rect.width * 0.35f, Liver.rect.height * 0.35f);
                    }
                    break;
                case 2:
                    timer += Time.deltaTime;
                    peach.localPosition = Vector2.Lerp(PeachStartPos, TargetPos, timer);
                    peach.localScale *= 1.006f;
                    if (timer >= 1)
                    {
                        timer = 0;
                        Phase++;
                        PeachStartPos = peach.localPosition;
                        TargetPos = Liver.localPosition + new Vector3(Liver.rect.width * 0.45f, Liver.rect.height * 0.1f);
                    }
                    break;
                case 3:
                    timer += Time.deltaTime;
                    peach.localScale *= 1.004f;
                    peach.localPosition = Vector2.Lerp(PeachStartPos, TargetPos, timer);
                    if (timer >= 1)
                    {
                        timer = 0;
                        Phase++;
                        PeachStartPos = peach.localPosition;

                        var oldm = OldM.transform.GetComponent<RectTransform>();
                        TargetPos = oldm.localPosition
                        + new Vector3(oldm.rect.width * 0.5f, -oldm.rect.height * 0.4f);
                    }
                    break;
                case 4:
                    timer += Time.deltaTime;
                    peach.localScale *= 1.02f;
                    peach.localPosition = Vector2.Lerp(PeachStartPos, TargetPos, timer);
                    if (timer >= 1)
                        Phase++;
                    break;
                case 5:
                    Flag = false;
                    OldM.Phase++;
                    gameObject.transform.SetParent(OldM.transform);
                    break;
            }
        }
    }
    public void OnTap1()
    {
        Flag = true;
    }
}