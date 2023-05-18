using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using InfimaGames.LowPolyShooterPack;
using UnityEngine;
using UnityEngine.Events;
using UnityTemplateProjects.DBServer.NetMsg;
using UnityTemplateProjects.DBServer.NetWork;

public class DatabaseConnector : Singleton<DatabaseConnector>
{
    private void Start()
    {
        DotNettyClient.GetInstance().Connect();
        MessageDistributer messageDistributer = new MessageDistributer();
        StartCoroutine(VersionCheck());
    }

    IEnumerator VersionCheck()
    {
        yield return new WaitUntil(DotNettyClient.GetInstance().isConnected);
        VersionGet(()=> MessageDistributer.GetInstance().EconomicResponse += VersionCheck);
    }

    public void Add()
    {
        UserMoneyAdd("1",100,()=> Debug.Log("成功"));
    }
    
    public void VersionCheckTest()
    {
        VersionGet(()=> MessageDistributer.GetInstance().EconomicResponse += VersionCheck);
    }

    public void VersionCheck(EconomicMsg economicMsg)
    {
        Debug.Log("客户端版本:"+Application.version + "|||服务器版本:" + economicMsg.username);
        if (economicMsg.username.Equals(Application.version))
        {
            Debug.Log("版本无需更新");
        }
        else
        {
            Debug.Log("版本需要更新");
            
        }

        MessageDistributer.GetInstance().EconomicResponse -= VersionCheck;
    }

    public void Login(String username,String password , Action onResponse)
    {
        LoginMsg req = new LoginMsg();
        req.username = username;
        req.password = password;
        req.code = (int) NetCode.UserLogin;
        DotNettyClient.GetInstance().SendData(req , onResponse);
    }

    public void Register(String username,String password , Action onResponse)
    {
        RegisterMsg req = new RegisterMsg();
        req.username = username;
        req.password = password;
        req.code = (int) NetCode.UserRegister;
        DotNettyClient.GetInstance().SendData(req,onResponse);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void UserMoneyGet(String username , Action onResponse)
    {
        EconomicMsg req = new EconomicMsg();
        req.username = username;
        req.code = (int)NetCode.UserMoneyGet;
        DotNettyClient.GetInstance().SendData(req,onResponse);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void UserMoneyAdd(String username, int add, Action onResponse)
    {
        EconomicMsg req = new EconomicMsg();
        req.username = username;
        req.amount = add;
        req.code = (int)NetCode.UserMoneyAdd;
        DotNettyClient.GetInstance().SendData(req,onResponse);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void VersionGet(Action onResponse)
    {
        NetMessage req = new NetMessage();
        req.code = (int)NetCode.VersionControl;
        DotNettyClient.GetInstance().SendData(req,onResponse);
    }
}
