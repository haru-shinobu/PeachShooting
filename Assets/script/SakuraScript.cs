using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SakuraScript : MonoBehaviour
{
    public int Sakura_Amount = 1;
    float R = 4;
    private GameObject[] sakura_groups;
    float xpos,ypos;
    void Awake()
    {
        GameObject Sakura = (GameObject)Resources.Load("Blosam");
        for (int i = 0; i < Sakura_Amount; i++) 
        {
            xpos = GetRandomPosX();
            ypos = GetRandomPosY(xpos);
            GameObject obj= Instantiate(Sakura, transform.position + new Vector3(xpos, ypos), Quaternion.identity,transform);
        }
    }

    float GetRandomPosX()
    {
        var x = UnityEngine.Random.Range(-R, R);
        return x;
    }

    float  GetRandomPosY(float pos)
    {
        float Und = Mathf.Sqrt(R * R - pos * pos);
        var y = UnityEngine.Random.Range(-Und * 0.4f, Und * 0.4f);
        return y;
    }

    void Start()
    {
        sakura_groups = GameObject.FindGameObjectsWithTag("Blosam");
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < Sakura_Amount; i++)
            sakura_groups[i].transform.Rotate(0, 0, -2);
    }
}
