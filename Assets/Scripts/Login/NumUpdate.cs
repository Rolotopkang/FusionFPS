using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumUpdate : MonoBehaviour
{
    public Slider Slider;
    private Text Text;


    private void Awake()
    {
        Text = GetComponent<Text>();
    }

    private void Update()
    {
        Text.text = Slider.value.ToString();
    }
}
