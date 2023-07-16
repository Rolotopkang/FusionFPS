using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountManager : Singleton<AccountManager>
{
    private bool isConnected = false;

    private string username;

    public string GetLocalUsername() => username;
    public void SetLocalUsername(string name) => username = name;

    public void setIsConnected(bool set)
    {
        isConnected = set;
    }

    public bool GetIsConnected() => isConnected;
}
