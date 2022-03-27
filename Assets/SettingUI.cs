using System;
using System.Collections;
using System.Collections.Generic;
using InfimaGames.LowPolyShooterPack;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityTemplateProjects.Tools;
using UnityTemplateProjects.UI;

public class SettingUI : MonoBehaviour
{
    [SerializeField]
    private Slider VoiceSlider;
    [SerializeField]
    private Slider mouseSensibilitySlider;
    [SerializeField]
    private TMP_InputField CrosshairColor;

    [SerializeField]
    private int F_VoiceIntensity = 80;
    [SerializeField]
    private float F_MouseSensitivity = 1;
    [SerializeField]
    private String F_CrosshairColor = "00FF0F";

    private void Awake()
    {
        if(!PlayerPrefs.HasKey("VoiceIntensity"))
        {
            PlayerPrefs.SetInt("VoiceIntensity",F_VoiceIntensity);
            VoiceSlider.value = F_VoiceIntensity;
            //TODO 加入改变音量
            PlayerPrefs.Save();
        }

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

    
    private void OnDisable()
    {
        AudioSettings(true);
        GameSettings(true);
    }

    public void AudioSettings(bool isOpen)
    {
        if (isOpen)
        {
            PlayerPrefs.SetInt("VoiceIntensity", Int32.Parse(VoiceSlider.value.ToString()));
            PlayerPrefs.Save();
        }
        else
        {
            VoiceSlider.value = PlayerPrefs.GetInt("VoiceIntensity");
            //TODO 加入改变音量
        }
    }

    public void GameSettings(bool isOpen)
    {
        if (isOpen)
        {
            PlayerPrefs.SetFloat("MouseSensitivity", mouseSensibilitySlider.value);
            PlayerPrefs.SetString("CrosshairColor",CrosshairColor.text);
            PlayerPrefs.Save();
            SettingManager.GetInstance().UpdateSettings();
        }
        else
        {
            mouseSensibilitySlider.value = PlayerPrefs.GetFloat("MouseSensitivity");
            CrosshairColor.text = PlayerPrefs.GetString("CrosshairColor");
            CrosshairColor.transform.GetComponent<HexColorField>().UpdateColor(CrosshairColor.text);
        }
    }
}
