using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NumUpdate : MonoBehaviour
{
    public Slider Slider;
    private TextMeshProUGUI Text;
    public String Format = "C0";


    private void Awake()
    {
        Text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        Text.text = Slider.value.ToString(Format);
    }
}
