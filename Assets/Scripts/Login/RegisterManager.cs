using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityTemplateProjects.DBServer.NetWork;

public class RegisterManager : MonoBehaviour
{
    public TMP_InputField Username;
    public TMP_InputField Password;
    public TMP_InputField VefiryPassword;
    public Text WrongHint;
    private UIBase _uiBase;
    private bool formatCheck = false;
    private bool RegisterAble = true;

    private void Awake()
    {
        _uiBase = GetComponentInParent<UIBase>();
    }

    private void OnEnable()
    {
        Username.text = String.Empty;
        Password.text = String.Empty;
        VefiryPassword.text = String.Empty;
        WrongHint.text = String.Empty;
    }

    private void Update()
    {
        if (!Password.text.Equals(VefiryPassword.text))
        {
            WrongHint.text = "两次密码不一致";
            formatCheck = false;
        }
        else if(Password.text.Equals(String.Empty) && VefiryPassword.text.Equals(String.Empty))
        {
            formatCheck = false;
        }
        else
        {
            WrongHint.text = String.Empty;
            formatCheck = true;
        }
    }

    public void BTNRegister()
    {
        if (!formatCheck)
        {
            return;
        }

        if (!RegisterAble)
        {
            return;
        }

        RegisterAble = false;
        string tmp_psw = MD5Encrypt.MD5Encrypt32(Password.text);
        DatabaseConnector.GetInstance().Register(Username.text,tmp_psw,() => MessageDistributer.GetInstance().RegisterResponse += OnRegisterResponse);
        Username.text = String.Empty;
        Password.text = String.Empty;
        VefiryPassword.text = String.Empty;
    }

    private void OnRegisterResponse(RegisterStatus registerStatus)
    {
        switch (registerStatus)
        {
            case RegisterStatus.SUCCESS:
                Debug.Log("注册成功");
                WrongHint.text = "注册成功";
                break;
            case RegisterStatus.FAILED_SAMENAME:
                Debug.Log("注册重名");
                WrongHint.text = "名字已被占用";
                break;
            case RegisterStatus.CONNECTION_FAILED:
                Debug.Log("注册连接失败");
                WrongHint.text = "未知错误";
                break;
        }
        MessageDistributer.GetInstance().RegisterResponse -= OnRegisterResponse;
        RegisterAble = true;
    }

    public void BTNReturnToLogin()
    {
        _uiBase.ChangeToUI(0);
    }
}
