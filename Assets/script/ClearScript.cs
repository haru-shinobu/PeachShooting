using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearScript : MonoBehaviour
{
    SceneManagerScript SMS;
    NextPagesScript next;
    bool b_find_scenemanager = false;
    bool flag = false;
    bool target_wayflag = true;
    private AudioSource audio;
    RectTransform BG
    , Title
    , Peach
    , TresureBase
    , Momotaro
    , servant1
    , servant2
    , servant3
    , wheel;
    RectTransform Target;
    Vector3 StartPos;
    float angle = 0;
    float audioLength = 0;


    void Start()
    {
        audio = gameObject.GetComponent<AudioSource>();
        if (GameObject.Find("AllSceneManager"))
        {
            audio.volume = GameObject.Find("AllSceneManager").GetComponent<InstantSaveScript>().SettingsRead("BGM");

            next = GameObject.Find("AllCanvas").GetComponent<NextPagesScript>();
            SMS = GameObject.Find("AllSceneManager").GetComponent<SceneManagerScript>();
            b_find_scenemanager = true;
        }
        var canvas = GameObject.Find("CCanvas");
        BG = canvas.transform.GetChild(0).GetComponent<RectTransform>();
        Title = canvas.transform.GetChild(2).GetComponent<RectTransform>();
        Title.gameObject.SetActive(false);
        Peach = canvas.transform.GetChild(3).GetComponent<RectTransform>();
        TresureBase = canvas.transform.GetChild(6).GetComponent<RectTransform>();
        Momotaro = TresureBase.transform.GetChild(1).GetComponent<RectTransform>();
        servant1 = TresureBase.transform.GetChild(4).GetComponent<RectTransform>();
        servant2 = TresureBase.transform.GetChild(5).GetComponent<RectTransform>();
        servant3 = TresureBase.transform.GetChild(6).GetComponent<RectTransform>();
        wheel = TresureBase.transform.GetChild(3).GetComponent<RectTransform>();
        var wheel_Under = wheel.localPosition.y - wheel.rect.height * 0.5f;
        TresureBase.localPosition = BG.localPosition - new Vector3(BG.rect.width * 0.54f, BG.rect.height * 0.4f);
        servant1.localPosition = new Vector3(Momotaro.localPosition.x, wheel_Under + servant1.rect.height * 0.5f) + new Vector3(Momotaro.rect.width * 0.6f, 0, 0);
        servant2.localPosition = new Vector3(servant1.localPosition.x, wheel_Under + servant2.rect.height * 0.5f) + new Vector3(servant1.rect.width * 2.5f, 0, 0);
        servant3.localPosition = new Vector3(servant1.localPosition.x, wheel_Under + servant3.rect.height * 0.5f) + new Vector3(servant1.rect.width * 1f, 0, 0);
        TresureBase.localScale = new Vector3(-1, 1, 1);
        StartPos = TresureBase.localPosition;
        Target = canvas.transform.GetChild(5).GetComponent<RectTransform>();
        audio.Play();
        Invoke("FlagChange",1f);
        audioLength = audio.clip.length;
        Instantiate(Resources.Load<ParticleSystem>("EffectBlosam"), TresureBase.transform);
    }
    void FlagChange()
    {
        flag = true;
    }

    void Update()
    {
        var TargetPos = Target.localPosition - new Vector3(Target.rect.width,0);
        var distance = Vector3.Distance(StartPos, TargetPos) / audioLength;
        angle = (180 * distance) / (50 * Mathf.PI);
        wheel.localRotation = Quaternion.Euler(0, 0, audio.time * angle);
        TresureBase.localPosition = Vector3.Lerp(StartPos, TargetPos, audio.time / audioLength);
        if (flag)
        {

            if (audio.time > 18)
            {
                if (target_wayflag)
                {
                    Target.transform.localScale = new Vector3(-1, 1, 1);
                    Target.transform.localRotation = Quaternion.Euler(0, 0, -Target.transform.localRotation.z);
                    Target.GetComponent<OldMScript>().endingflag = true;
                    Target.GetChild(0).gameObject.SetActive(false);
                    target_wayflag = false;
                    Invoke("EndTitle", 1);
                }
            }
            if (audio.time >= 21)
            {
                flag = false;
                Endingfin();
            }
        }
    }

    void EndTitle()
    {
        Title.gameObject.SetActive(true);
        Invoke("EndText",1);
    }
    void EndText()
    {
        Peach.gameObject.SetActive(true);
    }

    void Endingfin()
    {
        if (b_find_scenemanager)
        {
            SMS.EndScene();
        }
    }
}
