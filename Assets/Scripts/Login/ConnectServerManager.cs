using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;

public class ConnectServerManager : MonoBehaviourPunCallbacks
{
    public LoginUIManager loginUIManager;
    private bool connectToMaster = false;
    private String MyNickName="error";



    [SerializeField]
    private LoginUIAnimation LoginUIAnimation;

    [SerializeField]
    private MenuController MenuController;

    private void Awake()
    {
        // DontDestroyOnLoad(this);
    }
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
        LoginUIAnimation.UIFade();
        MenuController.gameObject.SetActive(true);
        MenuController.onUIshow();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void SetMyNickName(String set)
    {
        MyNickName = set;
    }
}
