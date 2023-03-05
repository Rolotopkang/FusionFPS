using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Audio_Settings : UIBase_Setting
{
    [SerializeField]
    private Slider VoiceSlider;
    
    [Header("默认音量大小")]
    [SerializeField]
    private int F_VoiceIntensity = 80;

    protected override void Initialized()
    {
        base.Initialized();
        if(!PlayerPrefs.HasKey("VoiceIntensity"))
        {
            PlayerPrefs.SetInt("VoiceIntensity",F_VoiceIntensity);
            VoiceSlider.value = F_VoiceIntensity;
            //TODO 加入改变音量
            PlayerPrefs.Save();
        }
    }

    protected override void GetSetting()
    {
        base.GetSetting();
        VoiceSlider.value = PlayerPrefs.GetInt("VoiceIntensity");
        //TODO 加入改变音量
    }

    protected override void SaveSetting()
    {
        PlayerPrefs.SetInt("VoiceIntensity", Int32.Parse(VoiceSlider.value.ToString()));
        base.SaveSetting();
    }
}
