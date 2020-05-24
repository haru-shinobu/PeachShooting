using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonBomScript : MonoBehaviour
{
    GameObject Pref,Player;
    Transform trs;
    Vector3 way, Target;
    float speed = 1f;
    float timer = 0;
    int count = 0;
    SpriteRenderer colors;
    byte colord = 255;
    public int Anum = 0;
    [SerializeField]
    bool Flag = true;
    void Start()
    {
        colors = gameObject.GetComponent<SpriteRenderer>();
        Pref = Resources.Load<GameObject>("EMonAttack");
        if (!Flag)
        {
            Pref.GetComponent<SupportAttack>().AttackSpeed = 0.05f;
            colors.color = new Color32(255, 255, 0, 255);
        }
        trs = gameObject.GetComponent<Transform>();
        Player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (Flag)
        {
            if (colord > 55)
            {
                colord -= 2;
                colors.color = new Color32(255, 0, colord, 255);
            }
            else
            {
                int ran = 180;
                int oldran = 0;
                int num = Random.Range(4, 10) + Anum;
                for (int i = 0; i < num; i++)
                {
                    do
                    {
                        ran = Random.Range(0, 36);
                        if (oldran - 2 <= ran || ran <= oldran + 2)
                            ran = Random.Range(0, 36);
                    } while (16 <= ran && ran <= 20);
                    oldran = ran;
                    Instantiate(Pref, transform.position, Quaternion.Euler(0, 0, ran * 10));
                }
                Destroy(gameObject);
            }
        }
        else
        if (timer > 0.5f)
        {
            timer -= 0.5f;
            float axis = -Quaternion.LookRotation(Player.transform.position).x;
            Instantiate(Pref, transform.position, new Quaternion(0, 0, axis, 1));
            int num = Random.Range(0, 3);
            if (++count >= num + 4)
            {
                int ran = 180;
                int oldran = 0;
                for (int i = 0; i < 4; i++)
                {
                    do
                    {
                        ran = Random.Range(0, 36);
                        if (oldran - 2 <= ran || ran <= oldran + 2)
                            ran = Random.Range(0, 36);
                    } while (16 <= ran && ran <= 20);
                    oldran = ran;
                    Instantiate(Pref, transform.position, Quaternion.Euler(0, 0, ran * 10));
                }
                Destroy(gameObject);
            }
        }
    }
        

    public void FlagChange()
    {
        Flag = false;
    }
}