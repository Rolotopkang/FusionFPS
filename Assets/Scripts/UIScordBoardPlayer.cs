using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityTemplateProjects.Tools;

public class UIScordBoardPlayer : MonoBehaviour
{
    [Header("数据槽位")]
    [SerializeField] private TextMeshProUGUI Data_Num;
    [SerializeField] private TextMeshProUGUI Data_PlayerName;
    [SerializeField] private TextMeshProUGUI Data_KillNum;
    [SerializeField] private TextMeshProUGUI Data_DeathNum;
    [SerializeField] private TextMeshProUGUI Data_Score;
    [SerializeField] private TextMeshProUGUI Data_Ping;
    [SerializeField] private GameObject MineHint;
    [SerializeField] private GameObject DeathHint;

    private Player _player;
    public Player GetPlayer() => _player;
    public int GetKillNum() => tmp_killNum;

    private int tmp_killNum;
    private int tmp_DeathNum;
    private int tmp_Ping;
    private bool isDeath;
    
    #region Unity

    private void Update()
    {
        _player.CustomProperties.TryGetValue(EnumTools.PlayerProperties.Data_kill.ToString(), out tmp_killNum);
        _player.CustomProperties.TryGetValue(EnumTools.PlayerProperties.Data_Death.ToString(), out tmp_DeathNum);
        _player.CustomProperties.TryGetValue(EnumTools.PlayerProperties.Data_Ping.ToString(), out tmp_Ping);
        _player.CustomProperties.TryGetValue(EnumTools.PlayerProperties.IsDeath.ToString(), out isDeath);
        
        
        Data_KillNum.text = tmp_killNum.ToString();
        Data_DeathNum.text = tmp_DeathNum.ToString();
        Data_Ping.text = tmp_Ping.ToString();
        Data_Score.text= _player.GetScore().ToString();
        Data_PlayerName.text = _player.IsMasterClient ? "<#FF1B00>[房]<#FFFFFF>"+_player.NickName : _player.NickName;
        DeathHint.SetActive(isDeath);
        
        //在父类中的位置
        Data_Num.text = (transform.GetSiblingIndex()+1).ToString();
    }

    #endregion

    #region Funtions

    public void Register(Player player)
    {
        _player = player;
        MineHint.SetActive(player.Equals(PhotonNetwork.LocalPlayer));
    }

    // /// <summary>
    // /// 更新面板
    // /// </summary>
    // public void Refresh()
    // {
    //     
    // }

    #endregion
}
