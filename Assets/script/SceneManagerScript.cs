using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagerScript : MonoBehaviour
{
    float LoadTime;
    float WaitTime;
    bool Flag;
    public bool GoNextSceneFlag;
    float AsyProg;
    public bool DontDestroyEnabled = true;
    NextPagesScript Nextpages;
    AudioSource audioSource;
    AudioClip soundPages;
    void Awake()
    {
        AsyProg = 0.0f;
        if (DontDestroyEnabled)
        {
            DontDestroyOnLoad(this);
        }

        LoadTime = 0;
        WaitTime = 0;
        Flag = false;
        GoNextSceneFlag = false;
        Nextpages = GameObject.Find("AllCanvas").GetComponent<NextPagesScript>();

        audioSource = GameObject.Find("AllCanvas").GetComponent<AudioSource>();
        soundPages = Resources.Load<AudioClip>("Book03-7(Flick_Through)");

    }

    void FixedUpdate()
    {
        if (Flag)
            LoadTime += Time.deltaTime;
    }

    public void SceneChange(string str, int waittime)
    {
        WaitTime = waittime;
        Flag = true;
        StartCoroutine("LoadScene", str);
    }
    
    public void EndScene()
    {
        Nextpages.GScreenCapture();
        StartCoroutine("LoadSceneGameOver");
    }
    public void ClearScene()
    {
        StartCoroutine("LoadSceneGameClear");
    }

    protected IEnumerator LoadScene(string str)
    {
        var async = SceneManager.LoadSceneAsync(str);

        async.allowSceneActivation = false;
        while (async.progress < 0.9f || LoadTime < WaitTime || !GoNextSceneFlag)
        {
            AsyProg = async.progress;
            yield return new WaitForEndOfFrame();
        }
        Flag = false;
        LoadTime = 0;
        GoNextSceneFlag = false;
        audioSource.PlayOneShot(soundPages);
        async.allowSceneActivation = true;
    }

    protected IEnumerator LoadSceneGameOver()
    {
        var async = SceneManager.LoadSceneAsync("TitleScene");
        
        async.allowSceneActivation = false;
        while (async.progress < 0.9f || !GoNextSceneFlag)
        {
            AsyProg = async.progress;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1.0f);
        audioSource.PlayOneShot(soundPages);
        Flag = false;
        GoNextSceneFlag = false;
        LoadTime = 0;
        async.allowSceneActivation = true;
    }
    protected IEnumerator LoadSceneGameClear()
    {
        var async = SceneManager.LoadSceneAsync("Clear");

        async.allowSceneActivation = false;
        while (async.progress < 0.9f || !GoNextSceneFlag)
        {
            AsyProg = async.progress;
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("GameOver");
        yield return new WaitForSeconds(1.0f);
        audioSource.PlayOneShot(soundPages);
        Flag = false;
        GoNextSceneFlag = false;
        LoadTime = 0;
        async.allowSceneActivation = true;

    }
    public float LoadRequest()
    {
        return AsyProg;
    }
}
