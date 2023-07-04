using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;using UnityTemplateProjects.Tools;

public class ConnectServerManager : SingletonPunCallbacks<ConnectServerManager>
{
    private bool connectToMaster = false;
    private String MyNickName="error";

    [SerializeField]
    private LoginUIAnimation LoginUIAnimation;

    [SerializeField]
    private GameObject LoginUI;

    [SerializeField]
    private MenuController MenuController;
    
    public void ConnectToServer()
    {
        if (!connectToMaster)
        {
            PhotonNetwork.ConnectUsingSettings();
            connectToMaster = true;
        }
        else
        {
            Debug.Log("重复连接到服务器!");
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("ConnectedToMaster");

        if (AccountManager.GetInstance().GetIsConnected())
        {
            return;
        }
        
        // foreach (Player player in PhotonNetwork.PlayerList)
        // {
        //     Debug.Log(player.NickName);
        //     Debug.Log(MyNickName);
        //     Debug.Log("&&&&&&&&&&&&&");
        //     if (player.NickName.Equals(MyNickName))
        //     {
        //         UI_Error.GetInstance().OpenUI_AccountWarning();
        //         PhotonNetwork.Disconnect();
        //         return;
        //     }
        // }
        if (PhotonNetwork.CountOfPlayers >= 40)
        {
            UI_Error.GetInstance().OpenErrorUI("服务器已满","退出",
                UI_Error.GetInstance().Exit
                );
            PhotonNetwork.Disconnect();
            connectToMaster = false;
            return;
        }
        PhotonNetwork.NickName = MyNickName;
        PhotonNetwork.AutomaticallySyncScene = true;
        AccountManager.GetInstance().setIsConnected(true);
        AccountManager.GetInstance().SetLocalUsername(MyNickName);

        
        LoginUIAnimation.UIFade();
        MenuController.gameObject.SetActive(true);
        MenuController.onUIshow();
        
        //转移
        //TODO
        AccountManager.GetInstance().setIsConnected(true);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        if(cause != DisconnectCause.DisconnectByClientLogic)
            UI_Error.GetInstance().OpenErrorUI("网络连接错误","退出",UI_Error.GetInstance().Exit);
    }

    public void SetMyNickName(String set)
    {
        MyNickName = set;
    }
}
