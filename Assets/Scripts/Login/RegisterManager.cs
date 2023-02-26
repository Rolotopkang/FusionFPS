using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegisterManager : MonoBehaviour
{
    public TMP_InputField Username;
    public TMP_InputField Password;
    public TMP_InputField VefiryPassword;
    public Text WrongHint;
    private UIBase _uiBase;
    private bool registerable = false;

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
            registerable = false;
        }
        else if(Password.text.Equals(String.Empty) && VefiryPassword.text.Equals(String.Empty))
        {
            registerable = false;
        }
        else
        {
            WrongHint.text = String.Empty;
            registerable = true;
        }
    }

    public void BTNRegister()
    {
        if (!registerable)
        {
            return;
        }
        switch (MysqlManager.GetInstance().Register(Username.text,Password.text))
        {
            case EnumTools.RegisterState.Success:
                Username.text = String.Empty;
                Password.text = String.Empty;
                VefiryPassword.text = String.Empty;
                WrongHint.text = "Register Successful";
                break;
            case EnumTools.RegisterState.RepeatName:
                Username.text = String.Empty;
                Password.text = String.Empty;
                VefiryPassword.text = String.Empty;
                WrongHint.text = "Name has already be registered";
                break;
            case EnumTools.RegisterState.HasNoInput:
                Username.text = String.Empty;
                Password.text = String.Empty;
                VefiryPassword.text = String.Empty;
                WrongHint.text = "Check Input";
                break;
            case EnumTools.RegisterState.Error:
                Username.text = String.Empty;
                Password.text = String.Empty;
                VefiryPassword.text = String.Empty;
                WrongHint.text = "Unknown Wrong";
                break;
        }
    }

    public void BTNReturnToLogin()
    {
        _uiBase.ChangeToUI(0);
    }
}
