using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityTemplateProjects.DBServer.NetWork;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField Username;
    public TMP_InputField Password;
    public Text WrongHint;
    public String DebugName;
    public String DebugPassword;

    private String Tmp_Name;
    private bool LoginAble =  true;
    private UIBase _uiBase;
    public ConnectServerManager ConnectServerManager;
    private void Awake()
    {
        _uiBase = GetComponentInParent<UIBase>();
    }

    private void OnEnable()
    {
        Username.text = String.Empty;
        Password.text = String.Empty;
        WrongHint.text = String.Empty;
    }
    
    public void BTNLogin()
    {
        if (!LoginAble)
        {
            return;
        }

        if (Username.text.Equals(String.Empty) || Password.text.Equals(String.Empty))
        {
            WrongHint.text = "请输入用户名和密码";
            return;
        }
        LoginAble = false;
        string tmp_psw = MD5Encrypt.MD5Encrypt32(Password.text);
        DatabaseConnector.GetInstance().Login(Username.text, tmp_psw , () => MessageDistributer.GetInstance().LoginResponse += OnLoginResponse);
        Tmp_Name = Username.text;
        Username.text = String.Empty;
        Password.text = String.Empty;
    }
    

    private void OnLoginResponse(LoginStatus loginStatus)
    {
        switch (loginStatus)
        {
            case LoginStatus.SUCCESS:
                Debug.Log("登陆成功");
                ConnectServerManager.SetMyNickName(Tmp_Name);
                ConnectServerManager.ConnectToServer();
                _uiBase.ChangeToUI(2);
                break;
            case LoginStatus.ID_NOT_EXIST:
                Debug.Log("用户不存在");
                WrongHint.text = "用户不存在";
                break;
            case LoginStatus.PASSWORD_ERROR:
                Debug.Log("密码错误");
                WrongHint.text = "密码错误";
                break;
            case LoginStatus.ACCOUNT_ONLINE:
                Debug.Log("账户已经在线！");
                WrongHint.text = "账户已经在线！";
                break;
            case LoginStatus.UNKNOWN_ERROR:
                Debug.Log("未知错误");
                WrongHint.text = "未知错误";
                break;
        }
        MessageDistributer.GetInstance().LoginResponse -= OnLoginResponse;
        LoginAble = true;
    }

    public void BTNLoginOffline()
    {
        if (Username.text != "")
        {
            ConnectServerManager.SetMyNickName(Username.text);
            ConnectServerManager.ConnectToServer();
            _uiBase.ChangeToUI(2);
        }
        else
        {
            WrongHint.text = "Wrong Name Formal";
        }
    }

    private void ShowWrongHint(String hint)
    {
        
    }

    //BTN引用
    public void BTNToRegister()
    {
        _uiBase.ChangeToUI(1);
    }
    
    
    
    // //debug用
    // public void BTNDEBUG()
    // {
    //     switch (MysqlManager.GetInstance().Login(DebugName, DebugPassword))
    //     {
    //         case EnumTools.LoginState.Success:
    //             ConnectServerManager.SetMyNickName(DebugName);
    //             ConnectServerManager.ConnectToServer();
    //             _uiBase.ChangeToUI(2);
    //             break;
    //         case EnumTools.LoginState.SearchNoUser:
    //             Username.text = String.Empty;
    //             Password.text = String.Empty;
    //             WrongHint.text = "Can't Find User";
    //             break;
    //         case EnumTools.LoginState.WrongPassword:
    //             Username.text = String.Empty;
    //             Password.text = String.Empty;
    //             WrongHint.text = "Wrong Password";
    //             break;
    //         case EnumTools.LoginState.Error:
    //             Username.text = String.Empty;
    //             Password.text = String.Empty;
    //             WrongHint.text = "Unknown Wrong";
    //             break;
    //     }
    // }
}
