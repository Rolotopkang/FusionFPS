using System;
using System.Collections;
using System.Collections.Generic;
using DotNetty.Common.Internal.Logging;
using DotNetty.Unity;
using UnityEngine;
using UnityEngine.UI;
using UnityTemplateProjects.DBServer.NetMsg;
using UnityTemplateProjects.DBServer.NetWork;

public class DBTest : MonoBehaviour
{

    void Start()
    {
        ConnectToServer();
    }


    void Update()
    {
        
    }

    private void ConnectToServer()
    {
        Debug.Log("开始连接服务器");
        // NetClient.GetInstance().Init("127.0.0.1",16888);
        // NetClient.GetInstance().Connect();
        DotNettyClient.GetInstance().Connect();
    }

    // public void TestSend()
    // {
    //     LoginMsg req = new LoginMsg();
    //     req.username = "逆天";
    //     req.password = "?????";
    //     req.code = 1001;
    //     // NetClient.GetInstance().SendMessage(req);
    //     
    //     DotNettyClient.GetInstance().SendData(req);
    // }
    //
    // public void TestR()
    // {
    //     RegisterMsg req = new RegisterMsg();
    //     req.username = "逆天";
    //     req.password = "?????";
    //     req.time = "testtime";
    //     req.code = 1002;
    //     // NetClient.GetInstance().SendMessage(req);
    //     
    //     DotNettyClient.GetInstance().SendData(req);
    // }
}
