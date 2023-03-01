using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using ExitGames.Client.Photon.StructWrapping;
using InfimaGames.LowPolyShooterPack;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;using UnityTemplateProjects.Tools;
using EventCode = Scripts.Weapon.EventCode;
using Hashtable = ExitGames.Client.Photon.Hashtable;

/// <summary>
/// 游戏模式基类
/// </summary>
public abstract class GameModeManagerBehaviour : SingletonPunCallbacks<GameModeManagerBehaviour>,IOnEventCallback,IInRoomCallbacks,IPunObservable
{
    [Header("游戏模式描述设置选项")] 
    [TextArea]
    [SerializeField] private String descriptions;

    [Header("游戏模式数据设置选项")] 
    [Tooltip("游戏至少需要人数")] 
    [SerializeField] protected int min_Player = 2;
    [Tooltip("普通击杀得分")] 
    [SerializeField] private int score_NormalKill;
    [Tooltip("爆头击杀分数")]
    [SerializeField] private int score_HeadShotKill;
    [Tooltip("部署CD")] 
    [SerializeField] private float deployWaitTime = 3f;
    [SerializeField] private int GameLoopSec = 1200;

    protected List<PlayerManager> _playerManagers;
    protected bool isMaster = false;
    private float _preSecTimer = 0f;
    private int _gameLoopSec = 0;
    protected bool GameRunning = false;
    protected bool CanGameStart = true;


    private void Awake()
    {
        _playerManagers = new List<PlayerManager>();
        base.Awake();
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
            SetPlayerBoolProperties(PhotonNetwork.LocalPlayer,EnumTools.PlayerProperties.IsDeath,false);
            SetPlayerIntProperties(PhotonNetwork.LocalPlayer,EnumTools.PlayerProperties.Data_kill,0,false);
            SetPlayerIntProperties(PhotonNetwork.LocalPlayer,EnumTools.PlayerProperties.Data_Death,0,false);
            SetPlayerIntProperties(PhotonNetwork.LocalPlayer,EnumTools.PlayerProperties.Data_Ping,999,false);
            PhotonNetwork.LocalPlayer.SetScore(0);
        }
        
        // 获取房间游戏状态信息
        PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("State", out GameRunning);
        
    }

    private void Update()
    {
        //每帧执行
        TickAll();
        //伪权威执行
        if(!isMaster) return;
        TickMaster();
    }


    #region Funtions

    protected virtual void TickSec()
    {
        //上传ping
        if (PhotonNetwork.InRoom)
        {
            SetPlayerIntProperties(PhotonNetwork.LocalPlayer,EnumTools.PlayerProperties.Data_Ping,PhotonNetwork.GetPing(),false);
        }
        if (isMaster)
        {
            if (GameRunning)
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
        _preSecTimer += Time.deltaTime;
        if (_preSecTimer >= 1f)//一秒
        {
            _preSecTimer = 0;
           TickSec();
        }
    }

    protected virtual void TickMaster()
    {
        //游戏开始判定
        if (!GameRunning &&  CanGameStart)
        {
            //判定人数
            if (_playerManagers.Count >= min_Player)
            {
                StartGame();
            }
            
        }
    }

    protected virtual void StartGame()
    {
        Dictionary<byte, object> tmp_GameStartData = new Dictionary<byte, object>();
        RaiseEventOptions tmp_RaiseEventOptions = new RaiseEventOptions() {Receivers = ReceiverGroup.All};
        SendOptions tmp_SendOptions = SendOptions.SendReliable;
        PhotonNetwork.RaiseEvent((byte)EventCode.GameStart,
            tmp_GameStartData,
            tmp_RaiseEventOptions,
            tmp_SendOptions);
        Debug.Log("发送游戏开始事件！");
        
        GameRunning = true;
        CanGameStart = false;
        Hashtable tmp_hash = new Hashtable();
        tmp_hash.Add("State",true);
        PhotonNetwork.CurrentRoom.SetCustomProperties(tmp_hash);
    }

    //重置游戏
    //!!!!master端
    public virtual void ResetGame()
    {
        if (isMaster)
        {
            //重置玩家得分
            foreach (var player in PhotonNetwork.CurrentRoom.Players)
            {
                Hashtable hash = new Hashtable();
                hash.Add("Data_kill",0);
                hash.Add("Data_Death",0);
                hash.Add("Data_Ping",0);
                hash.Add(EnumTools.PlayerProperties.IsDeath.ToString(), true);
                player.Value.SetCustomProperties(hash);
                player.Value.SetScore(0);
            }
            //重置游戏时间
            _gameLoopSec = GameLoopSec;
            Invoke("ResetGameInvoke" ,1f);
        }
    }

    public void ResetGameInvoke()
    {
        CanGameStart = true;
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
        Debug.Log("游戏模式控制器增加"+playerManager.Owner);
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

    #endregion
    
    #region Events
    
    public virtual void OnEvent(EventData photonEvent)
    { 
        switch ((EventCode) photonEvent.Code)
        {
            case EventCode.KillPlayer:
                if (isMaster)
                {
                    OnPlayerDeath(photonEvent);
                }
                break;
            case EventCode.GameStart:
                Debug.Log("游戏开始！");
                OnGameStart(photonEvent);
                break;
            case EventCode.GameEnd:
                Debug.Log("游戏结束！");
                OnGameEnd(photonEvent);
                break;
        }
        
    }
    
    protected virtual void OnGameStart(EventData eventData)
    {
        //权威端修改房间状态
        if (isMaster)
        {
            
        }
        
        
    }

    protected virtual void OnGameEnd(EventData eventData)
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
    /// 主客户端发起游戏结束事件
    /// TODO
    /// </summary>
    protected virtual void RaiseGameEndEvent()
    {
        GameRunning = false;
        Hashtable tmp_hash = new Hashtable();
        tmp_hash.Add("State",false);
        PhotonNetwork.CurrentRoom.SetCustomProperties(tmp_hash);
        
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
            SetPlayerIntProperties(newPlayer,EnumTools.PlayerProperties.Data_kill,0,false);
            SetPlayerIntProperties(newPlayer,EnumTools.PlayerProperties.Data_Death,0,false);
            SetPlayerIntProperties(newPlayer,EnumTools.PlayerProperties.Data_Ping,999,false);
            SetPlayerBoolProperties(newPlayer,EnumTools.PlayerProperties.IsDeath,false);
            newPlayer.SetScore(0);
        }
    }


    public virtual void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName+"退出了房间");
        for (int i = 0; i < _playerManagers.Count; i++)
        {
            if (_playerManagers[i].Owner.Equals(otherPlayer))
            {
                Debug.Log("删除玩家"+_playerManagers[i].Owner+"脚本");
                _playerManagers.Remove(_playerManagers[i]);
            }
        }
    }

    public virtual void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        
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
                stream.SendNext(CanGameStart);
                stream.SendNext(GameRunning);
            }
        }
        else
        {
            if (!isMaster)
            {
                _gameLoopSec =(int)stream.ReceiveNext();
                CanGameStart = (bool)stream.ReceiveNext();
                GameRunning = (bool)stream.ReceiveNext();
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

    public bool GetRoomState => GameRunning;

    public bool GetRoomStateEnd => CanGameStart;
    

    #endregion
}
