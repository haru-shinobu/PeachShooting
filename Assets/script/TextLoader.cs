using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextLoader : MonoBehaviour
{
    //　読み込んだテキストを出力するUIテキスト
    [SerializeField]
    private Text dataText;
    //　読む込むテキストが書き込まれている.txtファイル
    [SerializeField]
    private TextAsset textAsset;
    //　Resourcesフォルダから直接テキストを読み込む
    private string loadText;
    //　改行で分割して配列に入れる
    private string[] splitText1;
    //　改行で分割して配列に入れる
    private string[] splitText;
    //　現在表示中テキスト1番号
    private int textNum1;
    //　現在表示中テキスト2番号
    private int textNum2;

    void Start()
    {
        loadText = (Resources.Load("story", typeof(TextAsset)) as TextAsset).text;
        splitText = loadText.Split(char.Parse("\n"));
        textNum1 = 0;
        textNum2 = 0;
        dataText.text = "マウスのクリックでテキストの読み込み、表示がされる";
    }

    void Update()
    {
        //ボタンを押したとき読み込んだ内容を表示
        if (Input.GetButtonDown("Fire2"))
        {
            if (splitText[textNum2] != "")
            {
                dataText.text = splitText[textNum2];
                textNum2++;
                if (textNum2 >= splitText.Length)
                {
                    textNum2 = 0;
                }
            }
            else
            {
                dataText.text = "";
                textNum2++;
            }
        }
    }
}