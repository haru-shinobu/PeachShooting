using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterScript : MonoBehaviour
{
    static bool Flag = true;
    void Awake()
    {
        if (Flag)
        {
            gameObject.GetComponent<SceneManagerScript>().SceneChange("TitleScene", 0);
            GameObject.Find("AllCanvas").gameObject.GetComponent<NextPagesScript>().GScreenCapture();
        }
        Flag = false;
        Destroy(this);
    }
}
