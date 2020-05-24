using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NextPagesScript : MonoBehaviour
{
    public Image Draw_tex;
    public bool DontDestroyEnabled = true;
    float crossover;
    bool Flag;
    bool NextStep;
    SceneManagerScript SceneScript;
    void Awake()
    {
        if (DontDestroyEnabled)
        {
            DontDestroyOnLoad(transform.root.gameObject);
            DontDestroyOnLoad(this);
        }
        SceneManager.activeSceneChanged += ActiveSceneChanged;
        SceneScript = GameObject.Find("AllSceneManager").GetComponent<SceneManagerScript>();
        Flag = true;
        crossover = 1;
        NextStep = false;
    }
    public void GScreenCapture()
    {
        StartCoroutine("Capture");
        crossover = 1;        
    }
    void Update()
    {
        if (Flag)
        {
            crossover -= Time.deltaTime;
            if (crossover <= -1) {
                Flag = false;
                Draw_tex.enabled = false;
            }
            Draw_tex.material.SetFloat("_Flip", crossover);
        }

        if (NextStep) {
            SceneScript.GoNextSceneFlag = true;
            NextStep = false;
        }
    }

    IEnumerator Capture()
    {
        //ReadPicxelsがこの後でないと使えないので必ず書く
        yield return new WaitForEndOfFrame();

        //スクリーンの大きさのSpriteを作る
        var texture = new Texture2D(Screen.width, Screen.height);

        //スクリーンを取得する
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        //適応する
        texture.Apply();

        //取得した画像をSpriteに入るように変換する
        byte[] pngdata = texture.EncodeToPNG();
        texture.LoadImage(pngdata);

        //先ほど作ったSpriteに画像をいれる
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        //Spriteを使用するオブジェクトに指定する
        //     今回はUIのImage
        Draw_tex.material.SetTexture("_MainTex", texture);
        //Imageをアクティブにする
        Draw_tex.enabled = true;
        crossover = 1;
        NextStep = true;
    }
    void ActiveSceneChanged(Scene prevScene, Scene nextScene)
    {
        Flag = true;
    }
}
