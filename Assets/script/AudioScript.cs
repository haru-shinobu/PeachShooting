using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class AudioScript : MonoBehaviour
{

    AudioSource BGMaudio;
    public AudioClip sound1;
    public AudioClip sound2;
    public AudioClip sound3;
    public AudioClip sound4;

    AudioClip sound;

    public bool IsFadeIn = true;
    public bool IsFadeOut = false;
    public float FadeSecond = 1.0f;
    float FadeDeltaTime = 0;
    private float MaxSound;
    
    void Start()
    {
        BGMaudio = GetComponent<AudioSource>();
        switch (SceneManager.GetActiveScene().name)
        {
            case "Stage1":
            case "Stage2":
            case "Stage3":
                sound = sound1;
                break;
            case "StageDeath":
                sound = sound2;
                break;
        }
        BGMaudio.clip = sound;
        BGMaudio.Play();
        if (PlayerPrefs.HasKey("BGM"))
        {
            MaxSound = PlayerPrefs.GetFloat("BGM") * 0.5f;
        }

    }

    public void BossBattle()
    {
        sound = sound3;
        BGMReset();
    }
    public void Ending()
    {
        sound = sound4;
        BGMReset();
    }
    void BGMReset()
    {
        IsFadeIn = false;
        IsFadeOut = true;
        BGMaudio.loop = false;
        FadeDeltaTime = 1.0f;
    }
    void Update()
    {
        if (IsFadeIn)
        {
            FadeDeltaTime += Time.deltaTime * 0.5f;
            if (FadeSecond < FadeDeltaTime)
            {
                FadeDeltaTime = FadeSecond;
                IsFadeIn = false;
                BGMaudio.loop = true;
            }
            BGMaudio.volume = ((float)(FadeDeltaTime / FadeSecond))*MaxSound;
        }

        if (IsFadeOut)
        {
            FadeDeltaTime -= Time.deltaTime;
            if (FadeDeltaTime < 0)
            {
                FadeDeltaTime = 0;
                IsFadeOut = false;
                BGMaudio.clip = sound;
                BGMaudio.Play();
                IsFadeIn = true;
            }
            BGMaudio.volume = ((float)(FadeDeltaTime / FadeSecond))*MaxSound;
        }
    }
}
