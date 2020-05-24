using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HPvarScript : MonoBehaviour
{
    Slider slider;
    PlayerControllerScript Plays;
    void Start()
    {
        slider = GameObject.Find("HPSlider").GetComponent<Slider>();
        Plays = GameObject.Find("Player").GetComponent<PlayerControllerScript>();
    }

    void Update()
    {
        slider.value = Plays.GetHP();
    }
}
