using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockControllerScript : MonoBehaviour
{
    private Transform[] trans = new Transform[11];
    Vector3 Pos,PosE;
    bool flag = true;
    void Start()
    {
        for (int i = 0; i <= 10; i++)
        {
            trans[i] = transform.GetChild(i).GetComponent<Transform>();
        }
        Pos = transform.GetChild(0).GetComponent<Transform>().position -
            (transform.GetChild(1).GetComponent<Transform>().position -
            transform.GetChild(0).GetComponent<Transform>().position);
        PosE = transform.GetChild(10).GetComponent<Transform>().position;
    }

    void Update()
    {
        if (flag)
        {
            for (int i = 0; i <= 10; i++)
            {
                trans[i].position += new Vector3(0.025f, 0, 0);
                if (trans[i].position.x >= Pos.x)
                    trans[i].position = PosE + new Vector3(0.025f, 0, 0);
            }
        }
    }

    public void Stop()
    {
        flag = false;
    }
}
