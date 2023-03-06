using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityTemplateProjects.UI;

public class Game_Settings : UIBase_Setting
{
    [SerializeField]
    private Slider mouseSensibilitySlider;
    [SerializeField]
    private TMP_InputField CrosshairColor;

    [Header("游戏设置初始化参数")]
    [SerializeField]
    private float F_MouseSensitivity = 1;
    [SerializeField]
    private String F_CrosshairColor = "00FF0F";
    protected override void Initialized()
    {
        base.Initialized();
        if(!PlayerPrefs.HasKey("MouseSensitivity"))
        {
            PlayerPrefs.SetFloat("MouseSensitivity",F_MouseSensitivity);
            mouseSensibilitySlider.value = F_MouseSensitivity;
            PlayerPrefs.Save();
        }
        
        if(!PlayerPrefs.HasKey("CrosshairColor"))
        {
            PlayerPrefs.SetString("CrosshairColor",F_CrosshairColor);
            CrosshairColor.text= F_CrosshairColor;
            PlayerPrefs.Save();
        }
    }

    protected override void GetSetting()
    {
        base.GetSetting();
        mouseSensibilitySlider.value = PlayerPrefs.GetFloat("MouseSensitivity");
        CrosshairColor.text = PlayerPrefs.GetString("CrosshairColor");
        CrosshairColor.transform.GetComponent<HexColorField>().UpdateColor(CrosshairColor.text);
    }

    protected override void SaveSetting()
    {
        PlayerPrefs.SetFloat("MouseSensitivity", mouseSensibilitySlider.value);
        PlayerPrefs.SetString("CrosshairColor",CrosshairColor.text);
        base.SaveSetting();
    }
}
