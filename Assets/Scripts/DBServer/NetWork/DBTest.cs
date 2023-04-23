using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityTemplateProjects.DBServer.NetMsg;
using UnityTemplateProjects.DBServer.NetWork;

public class DBTest : MonoBehaviour
{

    void Start()
    {
        MessageDistributer.GetInstance().MsgUpdate = Show;
        ConnectToServer();
    }


    void Update()
    {
        
    }

    private void ConnectToServer()
    {
        Debug.Log("开始连接服务器");
        NetClient.GetInstance().Init("127.0.0.1",16888);
        NetClient.GetInstance().Connect();
    }

    public void TestSend()
    {
        LoginMsg req = new LoginMsg();
        req.username = "test";
        req.password = "?????";
        NetClient.GetInstance().SendMessage(req);
    }
    
    public void TestR()
    {
        RegisterMsg req = new RegisterMsg();
        req.username = "test";
        req.password = "?????";
        req.time = "testtime";
        NetClient.GetInstance().SendMessage(req);
    }

    private void Show(string str)
    {
        Debug.Log(str);
    }
}
