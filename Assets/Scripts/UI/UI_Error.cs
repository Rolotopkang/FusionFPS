using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Error : Singleton<UI_Error>
{

    [SerializeField] private UIBase uiBase;
    [SerializeField] private GameObject LoginPanel;
    
    [SerializeField] private GameObject UI_NetWorkWarning;
    [SerializeField] private GameObject UI_AccountWarning;
    [SerializeField] private GameObject UI_ServerFullWarning;
    [SerializeField] private GameObject UI_TestTimeOutWarning;

    protected override void Awake()
    {
        base.Awake();
        UI_NetWorkWarning.SetActive(false);
        UI_AccountWarning.SetActive(false);
        UI_ServerFullWarning.SetActive(false);
    }

    public void OnReconnect()
    {
        UI_NetWorkWarning.SetActive(false);
        //TODO
    }

    public void OnReturnLogin()
    {
        UI_AccountWarning.SetActive(false);
        UI_ServerFullWarning.SetActive(false);
        uiBase.ChangeToUI(0);
    }

    public void OnTestTimeOut()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    
    public void OpenUI_NetWorkWarning()
    {
        UI_NetWorkWarning.SetActive(true);
    }

    public void OpenUI_ServerFullWarning()
    {
        UI_ServerFullWarning.SetActive(true);
    }

    public void OpenUI_TestTimeOutWarning()
    {
        UI_TestTimeOutWarning.SetActive(true);
        LoginPanel.SetActive(false);
    }

    public void OpenUI_AccountWarning()
    {
        UI_AccountWarning.SetActive(true);
    }
}
