using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using ExitGames.Client.Photon.StructWrapping;
using InfimaGames.LowPolyShooterPack;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public abstract class GameModeManagerBehaviour : MonoBehaviour,IOnEventCallback,IInRoomCallbacks
{
    [Header("游戏模式数据设置选项")]
    [Tooltip("普通击杀得分")] 
    [SerializeField] private int score_NormalKill;
    [Tooltip("爆头击杀分数")]
    [SerializeField] private int score_HeadShotKill;

    private bool isMaster = false;
    private float time;

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    private void Start()
    {
        isMaster = PhotonNetwork.LocalPlayer.IsMasterClient;
    }

    private void Update()
    {
        //所有都执行
        TickAll();
        TickSec();
        //伪权威执行
        if(!isMaster) return;
        TickMaster();
    }


    #region Funtions

    protected virtual void TickSec()
    {
        time += Time.deltaTime;
        if (time >= 1f)//一秒
        {
            time = 0;
            //上传ping
            SetPlayerIntProperties(PhotonNetwork.LocalPlayer,EnumTools.PlayerProperties.Data_Ping,PhotonNetwork.GetPing(),false);
        }
    }
    
    protected virtual void TickAll()
    {
        
    }

    protected virtual void TickMaster()
    {
        
    }
    

    protected virtual void OnPlayerDeath(EventData eventData)
    {
        Dictionary<byte, object> tmp_KillData = (Dictionary<byte, object>)eventData.CustomData;
        Player tmp_deathPlayer =(Player)tmp_KillData[0];
        Player tmp_KillFrom =(Player)tmp_KillData[1];
        String tmp_KillWeapon = (String)tmp_KillData[2];
        bool tmp_headShot = (bool)tmp_KillData[3];
        long tmp_time = (long)tmp_KillData[4];

        if (tmp_deathPlayer.Equals(tmp_KillFrom))
        {
            SetPlayerIntProperties(tmp_deathPlayer,EnumTools.PlayerProperties.Data_Death,1,true);
            SetPlayerBoolProperties(tmp_deathPlayer,EnumTools.PlayerProperties.IsDeath,true);
            return;
        }
        
        //死亡角色死亡数加一
        SetPlayerIntProperties(tmp_deathPlayer,EnumTools.PlayerProperties.Data_Death,1,true);
        SetPlayerBoolProperties(tmp_deathPlayer,EnumTools.PlayerProperties.IsDeath,true);
        //击杀者击杀数加一
        SetPlayerIntProperties(tmp_KillFrom,EnumTools.PlayerProperties.Data_kill,1,true);
        //加分数
        tmp_KillFrom.AddScore(tmp_headShot? score_HeadShotKill : score_NormalKill);
    }

    /// <summary>
    /// 设置个人int类型属性
    /// </summary>
    /// <param name="player">需要更改属性的玩家</param>
    /// <param name="playerProperties">更改的类型</param>
    /// <param name="set">设置值（int）</param>
    /// <param name="isAdd">是否是在原属性上修改</param>
    private void SetPlayerIntProperties(Player player, EnumTools.PlayerProperties playerProperties ,int set ,bool isAdd)
    {
        int oldSet;
        
        String tmp_Properties = playerProperties switch
        {
            EnumTools.PlayerProperties.Data_kill => "Data_kill",
            EnumTools.PlayerProperties.Data_Death => "Data_Death",
            EnumTools.PlayerProperties.Data_Ping => "Data_Ping"
        };

        //如果是在原属性上增加
        if (isAdd)
        {
            //如果有属性就基于原属性
            if (player.CustomProperties.TryGetValue(tmp_Properties,out oldSet))
            {
                set = oldSet + set;
            }
        }
        
        Hashtable hash = new Hashtable();
        hash.Add(tmp_Properties,set);
        player.SetCustomProperties(hash);
        // Debug.Log(player.NickName+"角色的"+playerProperties+"设置为"+set);
    }
    
    private void SetPlayerBoolProperties(Player player, EnumTools.PlayerProperties playerProperties ,bool set)
    {
        Hashtable hash = new Hashtable();
        hash.Add(playerProperties.ToString(),set);
        player.SetCustomProperties(hash);
    }
    
    #endregion
    
    #region Events
    
    public void OnEvent(EventData photonEvent)
    {
        switch ((Scripts.Weapon.EventCode) photonEvent.Code)
        {
            case Scripts.Weapon.EventCode.KillPlayer:
                if (isMaster)
                {
                    OnPlayerDeath(photonEvent);
                }
                break;
        }
    }
    public void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient) { isMaster = true; }
    }

    public void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName+"加入了房间");
        //初始化玩家属性
        SetPlayerIntProperties(newPlayer,EnumTools.PlayerProperties.Data_kill,0,true);
        SetPlayerIntProperties(newPlayer,EnumTools.PlayerProperties.Data_Death,0,true);
        SetPlayerIntProperties(newPlayer,EnumTools.PlayerProperties.Data_Ping,999,false);
        newPlayer.SetScore(0);
    }

    public void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName+"退出了房间");
    }

    public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        Debug.Log("房间属性变化！");
    }

    public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
    }

    #endregion
}
