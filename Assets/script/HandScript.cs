using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandScript : MonoBehaviour
{
    Vector3 pos;
    Vector3 worldPos;
    Vector2 StartPos;
    float Under, Over;
    float RightEnd, LeftEnd;
    void Start()
    {
        pos = new Vector3(0.3f, 0.3f);
        worldPos = transform.position;
        Under = -1.3f;
        Over = 3.5f;
        RightEnd = 8.62f;
        LeftEnd = -8.62f;
    }

    void Update()
    {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 InstantPos = worldPos;
                this.StartPos = Input.mousePosition;
                worldPos = Camera.main.ScreenToWorldPoint(StartPos);
                worldPos.z = 0;

                if (-2.0f <= worldPos.y && worldPos.y <= 4.0f)
                {
                    if (worldPos.y <= Under) worldPos.y = Under;
                    if (Over <= worldPos.y) worldPos.y = Over;

                    if (worldPos.y >= 4.0f && worldPos.x <= -7.4f)
                        worldPos = transform.position;
                }
                else
                {
                    worldPos = InstantPos;
                }
            }

            float X = worldPos.x, Y = worldPos.y;
            if (transform.position.x > RightEnd)
                X = RightEnd;
            else
                if (transform.position.x < LeftEnd)
                X = LeftEnd;

            if (transform.position.y > Over)
                Y = Over;
            else
                if (transform.position.y < Under)
                Y = Under;

            transform.position = new Vector3(X, Y) + pos;
    }
}
