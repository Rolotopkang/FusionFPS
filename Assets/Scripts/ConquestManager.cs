using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// 征服模式逻辑管理类
/// </summary>
public class ConquestManager : GameModeManagerBehaviour
{
    #region SerializedField

    [Header("征服模式数据设置")] 
    [SerializeField] public float occupySpeed;

    [Tooltip("征服模式逻辑间隔")]
    [SerializeField] public float conquestLogicRate = 5f;

    [SerializeField] public int maxRebornCount = 50;
    [SerializeField] public int logicPrePointRebornCount = 1;
    [SerializeField] public int prePlayerDeathRebornCount = 1;
    [SerializeField] public int occupiedPrePointRebornCount = 10;

    [Header("分数设定")]
    [SerializeField] public int occupiedPrePointPlayerScore = 50;

    #endregion

    #region PrivateField

    private PhotonTeamsManager PhotonTeamsManager;
    private PhotonTeam[] teams;
    private float timer = 0f;
    private int teamBlueRebornCount;
    private int teamRedRebornCount;

    #endregion
    

    #region Unity

    protected override void Start()
    {
        base.Start();
        PhotonTeamsManager = GetComponent<PhotonTeamsManager>();
        teams = PhotonTeamsManager.GetAvailableTeams();
        teamBlueRebornCount = maxRebornCount;
        teamRedRebornCount = maxRebornCount;

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
        //游戏模式逻辑帧
        timer += Time.deltaTime;
        if (timer >= conquestLogicRate)//游戏模式判定秒
        {
            timer = 0;
            TickLogicSec();
        }
    }

    /// <summary>
    /// 游戏模式逻辑帧
    /// </summary>
    protected void TickLogicSec()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            TickLogicMaster();
        }
        
        
    }

    /// <summary>
    /// 权威逻辑帧
    /// </summary>
    protected void TickLogicMaster()
    {
        RebornCountTeamUpdate();
    }

    #endregion
    
    #region Photon

    public override void OnEvent(EventData photonEvent)
    {
        base.OnEvent(photonEvent);
        switch ((Scripts.Weapon.EventCode) photonEvent.Code)
        {
            case Scripts.Weapon.EventCode.ConquestPointOccupied:
                OnConquestPointOccupied(photonEvent);
                break;
        }
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
        Dictionary<byte, object> tmp_KillData = (Dictionary<byte, object>)eventData.CustomData;
        Player tmp_deathPlayer =(Player)tmp_KillData[0];
        Player tmp_KillFrom =(Player)tmp_KillData[1];
        String tmp_KillWeapon = (String)tmp_KillData[2];
        bool tmp_headShot = (bool)tmp_KillData[3];
        long tmp_time = (long)tmp_KillData[4];

        if (isMaster)
        {
            RebornCountPlayerUpdate(tmp_deathPlayer);
        }
    }

    private void OnConquestPointOccupied(EventData eventData)
    {
        Dictionary<byte, object> tmp_OccupiedData =(Dictionary<byte, object>)eventData.CustomData;
        EnumTools.ConquestPoints tmp_Point = (EnumTools.ConquestPoints)Enum.Parse(typeof(EnumTools.ConquestPoints), (string) tmp_OccupiedData[0]);
        EnumTools.Teams tmp_OccupiedTeam = (EnumTools.Teams)Enum.Parse(typeof(EnumTools.Teams), (string) tmp_OccupiedData[1]);
        ConquestPoint tmp_ConquestPoint = ConquestPointManager.GetInstance().GetConquestPointThroughName(tmp_Point.ToString());
        //权威执行
        if (isMaster)
        {
            //所有点内角色加分
            foreach (Player player in tmp_ConquestPoint.GetPlayerList(tmp_OccupiedTeam))
            {
                player.AddScore(occupiedPrePointPlayerScore);
            }

            //扣除点数
            if (tmp_OccupiedTeam == EnumTools.Teams.Blue)
            {
                teamRedRebornCount -= occupiedPrePointRebornCount;
            }
            else if (tmp_OccupiedTeam == EnumTools.Teams.Red)
            {
                teamBlueRebornCount -= occupiedPrePointRebornCount;
            }
        }
        
        //全客户端执行
        if (tmp_ConquestPoint.isPlayerIn(PhotonNetwork.LocalPlayer))
        {
            //加分UI
            UIKillFeedBackTextManager.CreateGetScoreFeedbackText("Point[ " + tmp_Point + " ]Occupied",
                occupiedPrePointPlayerScore);
        }
    }

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        base.OnPhotonSerializeView(stream, info);
        if (stream.IsWriting)
        {
            if (isMaster)
            {
                stream.SendNext(teamBlueRebornCount);
                stream.SendNext(teamRedRebornCount);
            }
        }
        else
        {
            if (!isMaster)
            {
                teamBlueRebornCount=(int) stream.ReceiveNext();
                teamRedRebornCount=(int) stream.ReceiveNext();
            }
        }
    }


    #endregion
    
    #region Fuctions
    
    private void RebornCountTeamUpdate()
    {
        if (ConquestPointManager.GetInstance().ReturnTeamDifferenceCount() == 0) { return; }

        if (ConquestPointManager.GetInstance().ReturnTeamDifferenceCount()<0)
        {
            teamBlueRebornCount += ConquestPointManager.GetInstance().ReturnTeamDifferenceCount()*logicPrePointRebornCount;
        }
        else
        {
            teamRedRebornCount += -ConquestPointManager.GetInstance().ReturnTeamDifferenceCount()*logicPrePointRebornCount;
        }
        Debug.Log("逻辑扣除重生点！");
    }

    private void RebornCountPlayerUpdate(Player player)
    {
        Debug.Log("队伍"+player.GetPhotonTeam().Name+"重生点数减少");
        if (player.GetPhotonTeam() == null)
            return;
        if (player.GetPhotonTeam().Name.Equals(EnumTools.Teams.Blue.ToString()))
        {
            teamBlueRebornCount -= prePlayerDeathRebornCount;
        }
        else
        {
            teamRedRebornCount -= prePlayerDeathRebornCount;
        }
    }
    
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

    #region GetterSetter

    public int GetRebornCount(Player player, bool isMineTeam)
    {
        if (player.GetPhotonTeam() == null)
            return 0;
        if (isMineTeam)
        {
            if (player.GetPhotonTeam().Name.Equals(EnumTools.Teams.Blue.ToString()))
            {
                return teamBlueRebornCount;
            }
            //else
            return teamRedRebornCount;
        }
        else
        {
            if (player.GetPhotonTeam().Name.Equals(EnumTools.Teams.Blue.ToString()))
            {
                return teamRedRebornCount;
            }
            //else
            return teamBlueRebornCount;
        }
    }
    

    #endregion

    
    
}
