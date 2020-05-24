using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EfeatherSpetial : MonoBehaviour
{
    float Under = -1.3f, Over = 3.5f, RightEnd = 8.62f, LeftEnd = -8.62f;
    public bool Flag = true;
    int num = 1;
    float timer = 0;
    float timer2 = 0;
    GameObject bird,Player;
    GameObject THISGO;
    void Start()
    {
        if (Flag == true)
            transform.rotation = Quaternion.Euler(0, 0, 90);
        bird = GameObject.Find("Boss_Bird");
        Player = GameObject.FindWithTag("Player");
        THISGO = Resources.Load<GameObject>("Efeather");
    }


    void Update()
    {
        timer += Time.deltaTime;
        if (num-- > 0)
        {
            Instantiate(THISGO, this.gameObject.transform.localPosition, Quaternion.identity, null);
        }            
        if (Flag)
        {
            if (Over * 0.9f > transform.localPosition.y && timer < 6)
                transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, Over + 0.5f), Time.deltaTime);
            else
                if (transform.localPosition.x <= Player.transform.localPosition.x)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(Player.transform.localPosition.x + 0.2f, transform.localPosition.y), Time.deltaTime);
            }
            else
            {
                transform.localPosition -= Vector3.up * 0.1f;
            }
        }
        else
        {
            if (timer < 4)
                transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, Player.transform.localPosition.y), Time.deltaTime);
            else
            {
                transform.localPosition += Vector3.right * 0.1f;
            }
        }
    }

    public void FlagChange()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        Flag = false;
    }
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
