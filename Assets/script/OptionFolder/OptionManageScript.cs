using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OptionManageScript : MonoBehaviour
{
    SceneManagerScript SMS;
    InstantSaveScript saver;
    NextPagesScript next;
    public
    Slider[] slidergroup;

    [SerializeField]
    float max, min;
    [SerializeField]
    float EnemyMax, EnemyMin;
    AudioSource audioSource;
    bool flag = false;
    void Start()
    {
        SMS = GameObject.Find("AllSceneManager").GetComponent<SceneManagerScript>();
        next = GameObject.Find("AllCanvas").GetComponent<NextPagesScript>();
        saver = GameObject.Find("AllSceneManager").GetComponent<InstantSaveScript>();
        Transform canv = GameObject.Find("Canvas").transform;
        audioSource = gameObject.GetComponent<AudioSource>();
        slidergroup = new Slider[]
        {
            canv.GetChild(6).GetComponent<Slider>(),//BGM
            canv.GetChild(7).GetComponent<Slider>(),//SE
            canv.GetChild(8).GetComponent<Slider>(), //dog
            canv.GetChild(9).GetComponent<Slider>(),//monkey
            canv.GetChild(10).GetComponent<Slider>(),//bird
            canv.GetChild(11).GetComponent<Slider>()//org
        };
        for (int i = 0; i < 2; i++)
        {
            slidergroup[i].maxValue = max;
            slidergroup[i].minValue = min;
        }
        for (int i = 2; i < 6; i++)
        {
            slidergroup[i].maxValue = EnemyMax;
            slidergroup[i].minValue = EnemyMin;
        }

        ReadSave();
        audioSource.volume = saver.SettingsRead("BGM") * 0.5f;
        flag = true;
    }

    void ReadSave()
    {
        slidergroup[0].value = saver.SettingsRead("BGM");     
        slidergroup[1].value = saver.SettingsRead("SE");
        slidergroup[2].value = saver.SettingsRead("DOG");
        slidergroup[3].value = saver.SettingsRead("MONKEY");
        slidergroup[4].value = saver.SettingsRead("BIRD");
        slidergroup[5].value = saver.SettingsRead("ORG");
    }

    public void SliderChange()
    {
        if (flag)
            for (int i = 0; i < 6; i++)
            {
                saver.SettingsWrite(i, slidergroup[i].value);
                if (i == 0)
                    audioSource.volume = saver.SettingsRead("BGM") * 0.5f;
            }
    }

    public void ReturnButton()
    {
        for (int i = 0; i < 6; i++)
        {
            saver.SettingsWrite(i, slidergroup[i].value);
        }
        PlayerPrefs.Save();
        next.GScreenCapture();
        string p = "TitleScene";
        SMS.SceneChange(p, 1);
    }
}