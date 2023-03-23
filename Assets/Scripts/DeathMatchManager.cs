using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using EventCode = Scripts.Weapon.EventCode;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class DeathMatchManager : GameModeManagerBehaviour
{
    #region SerializedField

    [Header("死斗模式目标击杀数")]
    [SerializeField] public int WinRequestNum = 30;

    #endregion
    
    
    #region PrivateField
    private DeathMatchSetttlementUIController _deathMatchSetttlementUIController;
    private Player topPlayer;

    #endregion
    
    protected override void OnPlayerDeath(EventData eventData)
    {
        base.OnPlayerDeath(eventData);
        Debug.Log("死斗模式人物死亡");
    }

    protected override void Start()
    {
        base.Start();
        _deathMatchSetttlementUIController = GetComponentInChildren<DeathMatchSetttlementUIController>();
    }

    protected override void TickMaster()
    {
        base.TickMaster();
        if (GameRunning)
        {
            int tmp_MaxKill = -1;
            PlayerManager topPlayerManager = null;
            foreach (PlayerManager playerManager in _playerManagers)
            {
                if (playerManager.Owner.CustomProperties.TryGetValue(EnumTools.PlayerProperties.Data_kill.ToString(), out object x) && x is int tmp_playerKill)
                {
                    if (tmp_playerKill > tmp_MaxKill)
                    {
                        tmp_MaxKill = tmp_playerKill;
                        topPlayerManager = playerManager;
                    }
                }
            }

            if (tmp_MaxKill >= WinRequestNum)
            {
                topPlayer = topPlayerManager.Owner;
                RaiseGameEndEvent();
            }
        }

        
    }

    protected override void RaiseGameEndEvent()
    {
        base.RaiseGameEndEvent();
        if (topPlayer == null)
        {
            topPlayer = PhotonNetwork.LocalPlayer;
        }
        int tmp_kill =
            topPlayer.CustomProperties.TryGetValue(EnumTools.PlayerProperties.Data_kill.ToString(), out object x) && x is int tmp_playerKill ?
                tmp_playerKill : 0;
        int tmp_death = 
            topPlayer.CustomProperties.TryGetValue(EnumTools.PlayerProperties.Data_Death.ToString(), out object y) && y is int tmp_playerDeath ?
                tmp_playerDeath : 0;
        
        //游戏结束事件
        Dictionary<byte, object> tmp_GameEndData = new Dictionary<byte, object>();
        //胜利玩家
        tmp_GameEndData.Add(0,topPlayer);
        //剩余比分数B
        tmp_GameEndData.Add(1,tmp_kill);
        //剩余比分数R
        tmp_GameEndData.Add(2,tmp_death);
        //游戏结束时间戳
        tmp_GameEndData.Add(3,DateTime.Now.ToUniversalTime().Ticks);
        
        RaiseEventOptions tmp_RaiseEventOptions = new RaiseEventOptions() {Receivers = ReceiverGroup.All};
        SendOptions tmp_SendOptions = SendOptions.SendReliable;
        PhotonNetwork.RaiseEvent(
            (byte)EventCode.GameEnd,
            tmp_GameEndData,
            tmp_RaiseEventOptions,
            tmp_SendOptions);
        Debug.Log("发送游戏结束事件！");
    }

    protected override void OnGameEnd(EventData eventData)
    {
        base.OnGameEnd(eventData);
        _deathMatchSetttlementUIController.Initialized(eventData);
    }

    public override void ResetGame()
    {
        base.ResetGame();
        //TODO 
        //重置地图
        if (isMaster)
        {
            topPlayer = null;

        }
    }
}
