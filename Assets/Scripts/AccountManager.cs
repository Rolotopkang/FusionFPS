using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountManager : Singleton<AccountManager>
{
    private bool isConnected = false;

    public void setIsConnected(bool set)
    {
        isConnected = set;
    }

    public bool GetIsConnected() => isConnected;
}
