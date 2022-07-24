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

/// <summary>
/// 游戏模式基类
/// </summary>
public abstract class GameModeManagerBehaviour : MonoBehaviourPun,IOnEventCallback,IInRoomCallbacks,IPunObservable
{
    [Header("游戏模式描述设置选项")] 
    [TextArea]
    [SerializeField] private String descriptions;
    [Header("游戏模式数据设置选项")]
    [Tooltip("普通击杀得分")] 
    [SerializeField] private int score_NormalKill;
    [Tooltip("爆头击杀分数")]
    [SerializeField] private int score_HeadShotKill;
    [Tooltip("部署CD")] 
    [SerializeField] private float deployWaitTime = 3f;
    [SerializeField] private int GameLoopSec = 1200;

    private List<PlayerManager> _playerManagers;
    protected bool isMaster = false;
    private float _preSecTimer = 0f;
    private int _gameLoopSec = 0;



    private void Awake()
    {
        _playerManagers = new List<PlayerManager>();
    }

    protected void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    protected void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    protected virtual void Start()
    {
        isMaster = PhotonNetwork.LocalPlayer.IsMasterClient;
        if (isMaster)
        {
            _gameLoopSec = GameLoopSec;
        }
    }

    private void Update()
    {
        //没帧执行
        TickAll();
        //每秒执行
        TickSec();
        //伪权威执行
        if(!isMaster) return;
        TickMaster();
    }


    #region Funtions

    protected virtual void TickSec()
    {
        _preSecTimer += Time.deltaTime;
        if (_preSecTimer >= 1f)//一秒
        {
            _preSecTimer = 0;
            //上传ping
            SetPlayerIntProperties(PhotonNetwork.LocalPlayer,EnumTools.PlayerProperties.Data_Ping,PhotonNetwork.GetPing(),false);
            if (isMaster)
            {
                _gameLoopSec--;
                if (_gameLoopSec <= 0)
                {
                    RaiseGameEndEvent();
                }
            }
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
    protected void SetPlayerIntProperties(Player player, EnumTools.PlayerProperties playerProperties ,int set ,bool isAdd)
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
    
    protected void SetPlayerBoolProperties(Player player, EnumTools.PlayerProperties playerProperties ,bool set)
    {
        Hashtable hash = new Hashtable();
        hash.Add(playerProperties.ToString(),set);
        player.SetCustomProperties(hash);
    }

    public void AddPlayerManagerMethod(PlayerManager playerManager)
    {
        _playerManagers.Add(playerManager);
    }

    public PlayerManager GetPlayerManager(Player player)
    {
        foreach (PlayerManager playerManager in _playerManagers)
        {
            if (playerManager.Owner.Equals(player))
            {
                return playerManager;
            }
        }

        return null;
    }

    /// <summary>
    /// 主客户端发起游戏结束事件
    /// TODO
    /// </summary>
    private void RaiseGameEndEvent()
    {
        
    }
    
    #endregion
    
    #region Events
    
    public virtual void OnEvent(EventData photonEvent)
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
    public virtual void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient) { isMaster = true; }
    }

    public virtual void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName+"加入了房间");
        //伪权威执行
        if (isMaster)
        {
            //初始化玩家属性
            SetPlayerIntProperties(newPlayer,EnumTools.PlayerProperties.Data_kill,0,true);
            SetPlayerIntProperties(newPlayer,EnumTools.PlayerProperties.Data_Death,0,true);
            SetPlayerIntProperties(newPlayer,EnumTools.PlayerProperties.Data_Ping,999,false);
            newPlayer.SetScore(0);
        }
    }


    public virtual void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName+"退出了房间");
    }

    public virtual void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
            Debug.Log("房间属性变化！");
    }

    public virtual void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
    }

    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            if (isMaster)
            {
                stream.SendNext(_gameLoopSec);
            }
        }
        else
        {
            if (!isMaster)
            {
                _gameLoopSec =(int)stream.ReceiveNext();
            }
        }
    }

    #endregion

    #region GetterSetter

    public string GetGameModeDiscription => descriptions;

    public int GetScore_NormalKill => score_NormalKill;

    public int GetScore_HeadShotKill => score_HeadShotKill;

    public float GetdeployWaitTime => deployWaitTime;

    public int GetGameLoopSec() => _gameLoopSec;
    #endregion
}
