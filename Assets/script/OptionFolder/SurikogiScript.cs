using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SurikogiScript : MonoBehaviour
{
    RectTransform RecT;
    float upLimit;
    float time;
    Vector3 Pos;
    void Start()
    {
        time = 0;
        upLimit = 14.0f;
        RecT = gameObject.GetComponent<RectTransform>();
        Pos = gameObject.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime * 10;
        RecT.localPosition = (new Vector3(Pos.x + Mathf.PingPong(time, upLimit), Pos.y + Mathf.PingPong(time, upLimit * 0.5f), 0));
    }
}
