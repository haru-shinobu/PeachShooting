using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DizableScript : MonoBehaviour
{
    [SerializeField]
    Material mat,mat2;
    private float clossover, clossover2;
    private bool Flag,Flag2;
    SceneManagerScript SMS;
    void Start()
    {
        Flag = false;
        Flag2 = false;
        clossover = clossover2 = 0.0f;
        SMS = GameObject.Find("AllSceneManager").GetComponent<SceneManagerScript>();
        mat.SetFloat("_Progress", clossover);
        mat2.SetFloat("_Progress", clossover2);
    }

    // Update is called once per frame
    public void TriggerOn()
    {
        Flag = true;
    }
    public void Trigger2On()
    {
        Flag2 = true;
    }

    void Update()
    {
        //TimeScaleが０の時に実行されるreturn
        if (Mathf.Approximately(Time.timeScale, 0f)) return;
        if (Flag && !Flag2)
        {
            clossover = SMS.LoadRequest();
            if (0.9f <= clossover)
            {
                clossover = 1.0f;
                Flag = false;
            }
            mat.SetFloat("_Progress", clossover);
        }
        if (!Flag && Flag2)
        {
            clossover2 += Time.deltaTime;
            if (1 <= clossover2)
                Flag2 = false;
            mat2.SetFloat("_Progress", clossover2);
        }
    }
}
