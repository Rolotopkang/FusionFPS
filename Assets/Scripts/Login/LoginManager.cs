using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField Username;
    public TMP_InputField Password;
    public Text WrongHint;
    public String DebugName;
    public String DebugPassword;
    private LoginUIManager loginUIManager;
    public ConnectServerManager ConnectServerManager;
    private void Awake()
    {
        loginUIManager = GetComponentInParent<LoginUIManager>();
    }

    private void OnEnable()
    {
        Username.text = String.Empty;
        Password.text = String.Empty;
        WrongHint.text = String.Empty;
    }
    
    public void BTNLogin()
    {
        switch (MysqlManager.GetInstance().Login(Username.text, Password.text))
        {
            case EnumTools.LoginState.Success:
                ConnectServerManager.SetMyNickName(Username.text);
                ConnectServerManager.ConnectToServer();
                loginUIManager.ChangeToUI(2);
                break;
            case EnumTools.LoginState.SearchNoUser:
                Username.text = String.Empty;
                Password.text = String.Empty;
                WrongHint.text = "Can't Find User";
                break;
            case EnumTools.LoginState.WrongPassword:
                Username.text = String.Empty;
                Password.text = String.Empty;
                WrongHint.text = "Wrong Password";
                break;
            case EnumTools.LoginState.Error:
                Username.text = String.Empty;
                Password.text = String.Empty;
                WrongHint.text = "Unknown Wrong";
                break;
        }
        
    }

    public void BTNLoginOffline()
    {
        if (Username.text != "")
        {
            ConnectServerManager.SetMyNickName(Username.text);
            ConnectServerManager.ConnectToServer();
            loginUIManager.ChangeToUI(2);
        }
        else
        {
            WrongHint.text = "Wrong Name Formal";
        }
    }

    //BTN引用
    public void BTNToRegister()
    {
        loginUIManager.ChangeToUI(1);
    }
    //debug用
    public void BTNDEBUG()
    {
        switch (MysqlManager.GetInstance().Login(DebugName, DebugPassword))
        {
            case EnumTools.LoginState.Success:
                ConnectServerManager.SetMyNickName(DebugName);
                ConnectServerManager.ConnectToServer();
                loginUIManager.ChangeToUI(2);
                break;
            case EnumTools.LoginState.SearchNoUser:
                Username.text = String.Empty;
                Password.text = String.Empty;
                WrongHint.text = "Can't Find User";
                break;
            case EnumTools.LoginState.WrongPassword:
                Username.text = String.Empty;
                Password.text = String.Empty;
                WrongHint.text = "Wrong Password";
                break;
            case EnumTools.LoginState.Error:
                Username.text = String.Empty;
                Password.text = String.Empty;
                WrongHint.text = "Unknown Wrong";
                break;
        }
    }
}
