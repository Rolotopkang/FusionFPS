using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityTemplateProjects.UI;

public class UIBase_Setting : MonoBehaviour
{
    [SerializeField]
    protected string SettingName;

    [SerializeField] private TextMeshProUGUI _settingName;

    private void Awake()
    {
        
        Initialized();
    }

    private void OnEnable()
    {
        _settingName.text??= SettingName;
        GetSetting();
    }

    private void OnDisable()
    {
        SaveSetting();
        _settingName.text??= String.Empty;
    }

    protected virtual void Initialized()
    {
        
    }

    protected virtual void GetSetting()
    {
        
    }

    protected virtual void SaveSetting()
    {
        PlayerPrefs.Save();
        SettingManager.GetInstance()?.UpdateSettings();
    }
    
    
    
}
