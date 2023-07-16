using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UI_Error : Singleton<UI_Error>
{

    [SerializeField] private UIBase uiBase;
    [SerializeField] private GameObject LoginPanel;
    [SerializeField] private UI_Error_panel UI_Error_Panel;

    protected override void Awake()
    {
        base.Awake();
        UI_Error_Panel.gameObject.SetActive(false);
    }

    public void OpenErrorUI(String wrongHint,String ButtonHint, UnityAction OnResponse)
    {
        UI_Error_Panel.gameObject.SetActive(true);
        UI_Error_Panel.Init(wrongHint,ButtonHint,OnResponse);
    }

    public void CloseUI()
    {
        UI_Error_Panel.gameObject.SetActive(false);
    }

    
    
    public void OnReconnect()
    {
        // UI_NetWorkWarning.SetActive(false);
        // //TODO
    }

    public void OnReturnLogin()
    {
        // UI_AccountWarning.SetActive(false);
        // UI_ServerFullWarning.SetActive(false);
        uiBase.ChangeToUI(0);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    
    public void OpenUI_NetWorkWarning()
    {
        // UI_NetWorkWarning.SetActive(true);
    }

    public void OpenUI_ServerFullWarning()
    {
        // UI_ServerFullWarning.SetActive(true);
    }

    public void OpenUI_TestTimeOutWarning()
    {
        // UI_TestTimeOutWarning.SetActive(true);
        LoginPanel.SetActive(false);
    }

    public void OpenUI_AccountWarning()
    {
        // UI_AccountWarning.SetActive(true);
    }
}
