using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MobItemScript : MonoBehaviour
{
    public float EHP = 0;
    EnemyScript ESp;
    GameObject Obj;
    public int RangeMax = 0;
    void Start()
    {
        ESp = GetComponent<EnemyScript>();
        RangeMax = GameObject.FindWithTag("GameController").GetComponent<StageScript>().StageEnemyItemRandomMax;

        int num = Random.Range(0, RangeMax + 5);
        switch (num)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
                Obj = null;
                break;
            case 5:
                Obj = (GameObject)Resources.Load("DangoUp");
                break;
            case 6:
                Obj = (GameObject)Resources.Load("PowerUp");
                break;
            case 7:
                Obj = (GameObject)Resources.Load("FasterUp");
                break;
            case 8:
                Obj = (GameObject)Resources.Load("SpreadUp");
                break;
            case 9:
                Obj = (GameObject)Resources.Load("PlayerSpeedUp");
                break;
            case 10:
                break;
        }
    }

    public void ItemSpown()
    {
        EHP = ESp.HP;
        if (EHP <= 0 && Obj != null)
        {
            Instantiate(Obj, transform.position, Quaternion.identity);
            Destroy(this);
        }
    }
}
