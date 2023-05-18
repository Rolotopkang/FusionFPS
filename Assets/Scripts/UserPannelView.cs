using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityTemplateProjects.DBServer.NetMsg;
using UnityTemplateProjects.DBServer.NetWork;

public class UserPannelView : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup Panel;
    [SerializeField]
    private TextMeshProUGUI Username;
    [SerializeField]
    private TextMeshProUGUI Money;

    private void Awake()
    {
        HideMe();
    }

    public void ShowME()
    {
        Panel.alpha = 1;
        if (AccountManager.GetInstance().GetIsConnected())
        {
            Username.text = AccountManager.GetInstance().GetLocalUsername();
            GetUserMoney();
        }
        else
        {
            Debug.LogWarning("没注册啊");
        }
    }

    public void HideMe()
    {
        Panel.alpha = 0;
    }

    private void GetUserMoney()
    {
        Debug.Log("show");
        DatabaseConnector.GetInstance().UserMoneyGet(AccountManager.GetInstance().GetLocalUsername(),
            () => MessageDistributer.GetInstance().EconomicResponse += UpdateMoneyUI);
    }


    private void UpdateMoneyUI(EconomicMsg economicMsg)
    {
        Money.text = economicMsg.amount.ToString();
        MessageDistributer.GetInstance().EconomicResponse -= UpdateMoneyUI;
    }
    
}
