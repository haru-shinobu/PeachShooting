using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantSaveScript : MonoBehaviour
{
    public bool DontDestroyEnabled = true;
    void Start()
    {
        if (DontDestroyEnabled)
        {
            DontDestroyOnLoad(this);
        }

        if (PlayerPrefs.HasKey("BGM"))
            PlayerPrefs.GetFloat("BGM");

        if (PlayerPrefs.HasKey("SE"))            
            PlayerPrefs.GetFloat("SE");

        if (PlayerPrefs.HasKey("DOG"))
            PlayerPrefs.GetFloat("DOG");

        if (PlayerPrefs.HasKey("MONKEY"))
            PlayerPrefs.GetFloat("MONKEY");

        if (PlayerPrefs.HasKey("BIRD"))
            PlayerPrefs.GetFloat("BIRD");

        if (PlayerPrefs.HasKey("ORG"))
            PlayerPrefs.GetFloat("ORG");
    }

    public float SettingsRead(string str)
    {
        if (str == "BGM")
                return PlayerPrefs.GetFloat("BGM");
        if (str == "SE")
                return PlayerPrefs.GetFloat("SE");
        if (str == "DOG")
                return PlayerPrefs.GetFloat("DOG");
        if (str == "MONKEY")
                return PlayerPrefs.GetFloat("MONKEY");
        if (str == "BIRD")
                return PlayerPrefs.GetFloat("BIRD");
        if (str == "ORG")
                return PlayerPrefs.GetFloat("ORG");
        return 0;
    }

    public void SettingsWrite(int val ,float fval)
    {
        switch (val)
        {
            case 0:
                PlayerPrefs.SetFloat("BGM", fval);
                break;
            case 1:
                PlayerPrefs.SetFloat("SE", fval);
                break;
            case 2:
                PlayerPrefs.SetFloat("DOG", fval);
                break;
            case 3:
                PlayerPrefs.SetFloat("MONKEY", fval);
                break;
            case 4:
                PlayerPrefs.SetFloat("BIRD", fval);
                break;
            case 5:
                PlayerPrefs.SetFloat("ORG", fval);
                break;
        }
    }
}
