using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using WebSocketSharp;
using Random = UnityEngine.Random;

/// <summary>
/// 征服模式逻辑管理类
/// </summary>
public class ConquestManager : GameModeManagerBehaviour
{
    [Header("征服模式数据设置")] 
    [SerializeField] public float occupySpeed;
    
    private PhotonTeamsManager PhotonTeamsManager;
    private PhotonTeam[] teams;
    protected override void Start()
    {
        base.Start();
        PhotonTeamsManager = GetComponent<PhotonTeamsManager>();
        teams = PhotonTeamsManager.GetAvailableTeams();
        if (isMaster)
        {
            PhotonNetwork.LocalPlayer.JoinTeam(GetSmallTeamName());
            //初始化点位
            ConquestPointManager.GetInstance().InitConquestPoints();
        }
    }

    protected override void TickSec()
    {
        base.TickSec();
    }

    protected override void TickAll()
    {
        base.TickAll();
    }

    protected override void TickMaster()
    {
        base.TickMaster();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        //伪权威执行
        if (isMaster)
        {
            //加入人少的一队
            Debug.Log(newPlayer.NickName+"加入队伍！"+GetSmallTeamName());
            newPlayer.JoinTeam(GetSmallTeamName());

        }
        
    }
    
    protected override void OnPlayerDeath(EventData eventData)
    {
        base.OnPlayerDeath(eventData);
        Debug.Log("征服模式人物死亡");
        
    }


    #region Fuctions

    protected virtual string GetSmallTeamName()
    {
        Debug.Log("蓝队"+PhotonTeamsManager.GetTeamMembersCount("Blue"));
        Debug.Log("红队"+PhotonTeamsManager.GetTeamMembersCount("Red"));
        if (PhotonTeamsManager.GetTeamMembersCount("Blue") < PhotonTeamsManager.GetTeamMembersCount("Red"))
        {
            return "Blue";
        }
        else if (PhotonTeamsManager.GetTeamMembersCount("Blue") > PhotonTeamsManager.GetTeamMembersCount("Red"))
        {
            return "Red";
        }
        else
        {
            switch (Random.Range(1,2))
            {
                case 1:
                    return "Blue";
                case 2:
                    return "Red";
            }
            return "Blue";
        }
    }

    #endregion
}
