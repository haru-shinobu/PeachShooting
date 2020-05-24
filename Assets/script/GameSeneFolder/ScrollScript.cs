using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScrollScript : MonoBehaviour
{
    RectTransform BS0;
    RectTransform BS1;
    RectTransform BG0;
    RectTransform BG1;
    RectTransform BG2;
    RectTransform BG3;
    Vector3 Pos,mPos;
    bool flag = true;
    float Wide,mWide;
    
    void Awake()
    {
        BS0 = gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        BS1 = gameObject.transform.GetChild(1).gameObject.GetComponent<RectTransform>();
        BG0 = gameObject.transform.GetChild(2).gameObject.GetComponent<RectTransform>();
        BG1 = gameObject.transform.GetChild(3).gameObject.GetComponent<RectTransform>();
        BG2 = gameObject.transform.GetChild(4).gameObject.GetComponent<RectTransform>();
        BG3 = gameObject.transform.GetChild(5).gameObject.GetComponent<RectTransform>();
        //100
        Pos = BG3.localPosition - BG2.localPosition;
        mPos = BG1.localPosition - BG0.localPosition;

        Wide = BG2.localPosition.x - (BG3.localPosition.x - BG2.localPosition.x);
        mWide = BG0.localPosition.x - (BG1.localPosition.x - BG0.localPosition.x);
    }


    void Update()//1700
    {
        if (flag)
        {
            BS0.localPosition += new Vector3(0.1f, 0);
            BS1.localPosition += new Vector3(0.1f, 0);
            BG0.localPosition += new Vector3(0.3f, 0);
            BG1.localPosition += new Vector3(0.3f, 0);
            BG2.localPosition += new Vector3(1, 0);
            BG3.localPosition += new Vector3(1, 0);

            if (BS0.localPosition.x >= mWide)
                BS0.localPosition = mPos + BS1.localPosition;
            if (BS1.localPosition.x >= mWide)
                BS1.localPosition = mPos + BS0.localPosition;
            if (BG0.localPosition.x >= mWide)
                BG0.localPosition = mPos + BG1.localPosition;
            if (BG1.localPosition.x >= mWide)
                BG1.localPosition = mPos + BG0.localPosition;
            if (BG2.localPosition.x >= Wide)
                BG2.localPosition = Pos + BG3.localPosition;
            if (BG3.localPosition.x >= Wide)
                BG3.localPosition = Pos + BG2.localPosition;
        }
    }
    public void Stop()
    {
        flag = false;
    }
}