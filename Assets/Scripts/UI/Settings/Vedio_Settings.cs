using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityTemplateProjects.UI;
using Debug = UnityEngine.Debug;

public class Vedio_Settings : UIBase_Setting
{
    [SerializeField] 
    private TMP_Dropdown dropdown_Quality;

    [SerializeField] 
    private Toggle toggle_PP;

    [SerializeField]
    private TMP_Dropdown dropdown_DisplayMode;
    
    [SerializeField] 
    private TMP_Dropdown dropdown_Resolutions;

    private FullScreenMode _fullScreenMode;

    private int _currentResolution;
    private int _currentDisplayMode;

    protected override void Initialized()
    {
        base.Initialized();
        if(!PlayerPrefs.HasKey("PostProcessingIntensity"))
        {
            PlayerPrefs.SetInt("PostProcessingIntensity",1);
            PlayerPrefs.Save();
        }
        if(!PlayerPrefs.HasKey("VideoQuality"))
        {
            PlayerPrefs.SetInt("VideoQuality",2);
            PlayerPrefs.Save();
        }

        if (!PlayerPrefs.HasKey("DisplayMode"))
        {
            PlayerPrefs.SetInt("DisplayMode",0);
        }
        
        if (!PlayerPrefs.HasKey("Resolution"))
        {
            PlayerPrefs.SetInt("Resolution",1);
        }
    }

    protected override void GetSetting()
    {
        base.GetSetting();
        dropdown_Quality.value = PlayerPrefs.GetInt("VideoQuality");
        toggle_PP.isOn = PlayerPrefs.GetInt("PostProcessingIntensity")==1;
        _currentResolution = PlayerPrefs.GetInt("DisplayMode");
        _currentResolution =PlayerPrefs.GetInt("Resolution");
        dropdown_Resolutions.value = _currentResolution;
        dropdown_DisplayMode.value = _currentDisplayMode;
    }

    protected override void SaveSetting()
    {
        PlayerPrefs.SetInt("PostProcessingIntensity", toggle_PP.isOn? 1:0 );
        PlayerPrefs.SetInt("VideoQuality", dropdown_Quality.value);
        PlayerPrefs.SetInt("Resolution", dropdown_Resolutions.value);
        PlayerPrefs.SetInt("DisplayMode", dropdown_DisplayMode.value);
        
        base.SaveSetting();
    }

    public void OnQualityDropDownUpdate()
    {
        QualitySettings.SetQualityLevel(dropdown_Quality.value);
    }

    public void OnPostProcessingUpdate()
    {
        SettingManager.GetInstance().UpdateSettings();
    }

    public void OnScreenResolutionUpdate()
    {
        switch (dropdown_Resolutions.value)
        {
            case 0 :
                Screen.SetResolution(2560,1440,indexToFullScreenMode(_currentDisplayMode));
                break;
            case 1:
                Screen.SetResolution(1920,1080,indexToFullScreenMode(_currentDisplayMode));
                break;
            case 2:
                Screen.SetResolution(1280,720,indexToFullScreenMode(_currentDisplayMode));
                break;
            case 3:
                Screen.SetResolution(800,600,indexToFullScreenMode(_currentDisplayMode));
                break;
            default:
                Debug.LogError("分辨率设置异常！");
                break;
        }
        _currentResolution = dropdown_Resolutions.value;
    }

    public void OnDisplayModeUpedate()
    {
        switch (dropdown_DisplayMode.value)
        {
            //全屏
            case 0 :
                Screen.SetResolution(
                    (int)indexToResolution(_currentResolution).x,
                    (int)indexToResolution(_currentResolution).y,
                    FullScreenMode.MaximizedWindow);

                break;
            case 1:
                Screen.SetResolution(
                    (int)indexToResolution(_currentResolution).x,
                    (int)indexToResolution(_currentResolution).y,
                    FullScreenMode.ExclusiveFullScreen);

                break;
            case 2:
                Screen.SetResolution(
                    (int)indexToResolution(_currentResolution).x,
                    (int)indexToResolution(_currentResolution).y,
                    FullScreenMode.Windowed);
                break;
            case 3:
                Screen.SetResolution(
                    (int)indexToResolution(_currentResolution).x,
                    (int)indexToResolution(_currentResolution).y,
                    FullScreenMode.FullScreenWindow);

                break;
            default:
                break;
        }

        _currentDisplayMode = dropdown_DisplayMode.value;
    }


    private FullScreenMode indexToFullScreenMode(int index)
    {
        return index switch
        {
            0 => FullScreenMode.MaximizedWindow,
            1 => FullScreenMode.ExclusiveFullScreen,
            2 => FullScreenMode.Windowed,
            3 => FullScreenMode.FullScreenWindow,
            _ => FullScreenMode.Windowed
        };
    }

    private Vector2 indexToResolution(int index)
    {
        return index switch
        {
            0 => new Vector2(2560, 1440),
            1 => new Vector2(1920, 1080),
            2 => new Vector2(1280, 720),
            3 => new Vector2(800, 600),
            _ => new Vector2(1920, 1080),
        };
    }
    
}
