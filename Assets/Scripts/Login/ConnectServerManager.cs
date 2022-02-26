using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ConnectServerManager : MonoBehaviourPunCallbacks
{
    public LoginUIManager loginUIManager;
    private bool connectToMaster = false;
    private String MyNickName="error";

    public void ConnectToServer()
    {
        if (!connectToMaster)
        {
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log("connected to server");
            connectToMaster = true;
        }
        else
        {
            Debug.Log("重复连接到服务器!");
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.NickName = MyNickName;
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        loginUIManager.ChangeToUI(3);
    }

    public void SetMyNickName(String set)
    {
        MyNickName = set;
    }
}
