using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseConnector : MonoBehaviour
{
    private void Awake()
    {
        MysqlManager mysqlManager = new MysqlManager();
        mysqlManager.SetInstance(mysqlManager);
        MysqlManager.Init();
    }

    public EnumTools.LoginState Login(String username,String password)
    {
        return MysqlManager.GetInstance().Login(username, password);
    }

    public EnumTools.RegisterState Register(String username,String password)
    {
        return MysqlManager.GetInstance().Register(username, password);
    }
    
}
