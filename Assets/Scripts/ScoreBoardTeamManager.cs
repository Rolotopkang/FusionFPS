using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityTemplateProjects.Tools;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ScoreBoardTeamManager : MonoBehaviour,IInRoomCallbacks
{
    
    #region SerializeField

    [Header("房间")] 
    [SerializeField] private TextMeshProUGUI Data_RoomModeName;
    [SerializeField] private TextMeshProUGUI Data_RoomMapName;
    [SerializeField] private TextMeshProUGUI Data_RoomRule;
    
    [Header("个人战绩")]
    [SerializeField] private TextMeshProUGUI Data_LocalPlayerName;
    [SerializeField] private TextMeshProUGUI Data_LocalPlayerKill;
    [SerializeField] private TextMeshProUGUI Data_LocalPlayerDeath;
    [SerializeField] private TextMeshProUGUI Data_LocalPlayerKD;

    [Header("设置")]
    [SerializeField] private GameObject scoreboard;
    [SerializeField] private Transform scoreBoardRoot_Blue;
    [SerializeField] private Transform scoreBoardRoot_Red;


    [Header("预制体")] 
    [SerializeField] private GameObject UI_ScoreBoardPlayerPrefab;

    [SerializeField] private bool EndingMode = false;

    #endregion

    #region Private

    private Player LocalPlayer;
    private bool scoreBoardVisible;
    private Player[] roomPlayers;
    private List<UIScordBoardPlayer> UI_ScoreBoardPlayerList_Red;
    private List<UIScordBoardPlayer> UI_ScoreBoardPlayerList_Blue;

    #endregion

    #region Unity

    private void Awake()
    {
        UI_ScoreBoardPlayerList_Red = new List<UIScordBoardPlayer>();
        UI_ScoreBoardPlayerList_Blue = new List<UIScordBoardPlayer>();
        LocalPlayer = PhotonNetwork.LocalPlayer;
        scoreboard.SetActive(false);
        // updatePlayerList();
        // Instantiate_UI_ScoreBoardPlayerList();
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void Start()
    {
        Data_RoomMapName.text = RoomManager.GetInstance().currentGamemode switch
        {
            MapTools.GameMode.Conquest => "征服",
            MapTools.GameMode.DeathMatch => "个人自由混战",
            MapTools.GameMode.TeamDeathMatch => "团队自由混战",
            MapTools.GameMode.BombScenario => "爆破模式",
            MapTools.GameMode.TeamAdversarial => "团队竞技",
            _ => "错误模式（上报作者）"
        };
        Data_RoomMapName.text = PhotonNetwork.CurrentRoom.CustomProperties["mapDiscripName"].ToString();
        
        //TODO 更新房间规则
        Data_RoomRule.text = RoomManager.GetInstance().currentGamemodeManager.GetGameModeDiscription;
        Data_LocalPlayerName.text = LocalPlayer.NickName;
    }

    private void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    private void OnEnable()
    {
        if (EndingMode)
        {
            scoreboard.SetActive(true);
            updatePlayerList();
        }
    }

    private void OnDisable()
    {
        if (EndingMode)
        {
            scoreboard.SetActive(false);
        }
    }

    private void Update()
    {
        if (!EndingMode)
        {
            scoreboard.SetActive(scoreBoardVisible);
        }
    }
    
    #endregion

    #region Funtions

    /// <summary>
    /// 强制性刷新整个计分板
    /// </summary>
    private void updatePlayerList()
    {
        roomPlayers = PhotonNetwork.PlayerList;
        ClearLists();
        Instantiate_UI_ScoreBoardPlayerList();
        ScoreBoardSort();
        // //更新自己信息
        UpdateMineData();
    }

    private void Instantiate_UI_ScoreBoardPlayerList()
    {
        foreach (Player player in roomPlayers)
        {
            Transform tmp_parent = null;
            if (player.GetPhotonTeam() == null)
            {
                Debug.Log("计分板找不到队伍");
                continue;
            }
            if(player.GetPhotonTeam().Name.Equals("Red"))
            {
                tmp_parent = scoreBoardRoot_Red;
                UIScordBoardPlayer tmp_UIScordBoardPlayer = Instantiate(UI_ScoreBoardPlayerPrefab, tmp_parent).GetComponent<UIScordBoardPlayer>();
                tmp_UIScordBoardPlayer.Register(player);
                UI_ScoreBoardPlayerList_Red.Add(tmp_UIScordBoardPlayer);
            }else if (player.GetPhotonTeam().Name.Equals("Blue"))
            {
                tmp_parent = scoreBoardRoot_Blue;
                UIScordBoardPlayer tmp_UIScordBoardPlayer = Instantiate(UI_ScoreBoardPlayerPrefab, tmp_parent).GetComponent<UIScordBoardPlayer>();
                tmp_UIScordBoardPlayer.Register(player);
                UI_ScoreBoardPlayerList_Blue.Add(tmp_UIScordBoardPlayer);
            }
        }
    }

    /// <summary>
    /// tick按照击杀数给计分板排序
    /// </summary>
    private void ScoreBoardSort()
    {
        UI_ScoreBoardPlayerList_Red.Sort((x,y) => -x.GetKillNum().CompareTo(y.GetKillNum()));
        foreach (UIScordBoardPlayer uiScordBoardPlayer in UI_ScoreBoardPlayerList_Red)
        {
            uiScordBoardPlayer.transform.SetSiblingIndex(UI_ScoreBoardPlayerList_Red.IndexOf(uiScordBoardPlayer));
        }
        UI_ScoreBoardPlayerList_Blue.Sort((x,y) => -x.GetKillNum().CompareTo(y.GetKillNum()));
        foreach (UIScordBoardPlayer uiScordBoardPlayer in UI_ScoreBoardPlayerList_Blue)
        {
            uiScordBoardPlayer.transform.SetSiblingIndex(UI_ScoreBoardPlayerList_Blue.IndexOf(uiScordBoardPlayer));
        }
    }

    
    /// <summary>
    /// 更新个人积分信息
    /// </summary>
    private void UpdateMineData()
    {
        int tmp_killNum;
        int tmp_DeathNum;
        LocalPlayer.CustomProperties.TryGetValue(EnumTools.PlayerProperties.Data_kill.ToString(), out tmp_killNum);
        LocalPlayer.CustomProperties.TryGetValue(EnumTools.PlayerProperties.Data_Death.ToString(), out tmp_DeathNum);
        Data_LocalPlayerKill.text = tmp_killNum.ToString();
        Data_LocalPlayerDeath.text = tmp_DeathNum.ToString();
        Data_LocalPlayerKD.text =tmp_DeathNum==0? tmp_killNum.ToString("F1") : (tmp_killNum / (float)tmp_DeathNum).ToString("F1");
    }
    
    
    /// <summary>
    /// 当玩家退出房间把它从计分板中剔除
    /// </summary>
    /// <param name="player"></param>
    private void RemovePlayerFromList(Player player)
    {
        if(player.GetPhotonTeam().Name.Equals("Red"))
        {
            for (int i = 0; i < UI_ScoreBoardPlayerList_Red.Count; i++)
            {
                if (UI_ScoreBoardPlayerList_Red[i].GetPlayer().Equals(player))
                {
                    Destroy(UI_ScoreBoardPlayerList_Red[i].transform.gameObject);
                    UI_ScoreBoardPlayerList_Red.Remove(UI_ScoreBoardPlayerList_Red[i]);
                }
            }
        }else if (player.GetPhotonTeam().Name.Equals("Blue"))
        {
            for (int i = 0; i < UI_ScoreBoardPlayerList_Blue.Count; i++)
            {
                if (UI_ScoreBoardPlayerList_Blue[i].GetPlayer().Equals(player))
                {
                    Destroy(UI_ScoreBoardPlayerList_Blue[i].transform.gameObject);
                    UI_ScoreBoardPlayerList_Blue.Remove(UI_ScoreBoardPlayerList_Blue[i]);
                }
            }
        }

        
    }

    /// <summary>
    /// 当玩家加入房间把它加入计分板
    /// </summary>
    /// <param name="player"></param>
    private void AddPlayerToList(Player player)
    {
        Transform tmp_parent = null;
        if(player.GetPhotonTeam().Name.Equals("Red"))
        {
            tmp_parent = scoreBoardRoot_Red;
            UIScordBoardPlayer tmp_UIScordBoardPlayer = Instantiate(UI_ScoreBoardPlayerPrefab, tmp_parent).GetComponent<UIScordBoardPlayer>();
            tmp_UIScordBoardPlayer.Register(player);
            UI_ScoreBoardPlayerList_Red.Add(tmp_UIScordBoardPlayer);
        }else if (player.GetPhotonTeam().Name.Equals("Blue"))
        {
            tmp_parent = scoreBoardRoot_Blue;
            UIScordBoardPlayer tmp_UIScordBoardPlayer = Instantiate(UI_ScoreBoardPlayerPrefab, tmp_parent).GetComponent<UIScordBoardPlayer>();
            tmp_UIScordBoardPlayer.Register(player);
            UI_ScoreBoardPlayerList_Blue.Add(tmp_UIScordBoardPlayer);
        }
    }

    private void ClearLists()
    {
        foreach (UIScordBoardPlayer scordBoardPlayer in UI_ScoreBoardPlayerList_Blue)
        {
            Destroy(scordBoardPlayer.gameObject);
        }
        UI_ScoreBoardPlayerList_Blue.Clear();
        foreach (UIScordBoardPlayer scordBoardPlayer in UI_ScoreBoardPlayerList_Red)
        {
            Destroy(scordBoardPlayer.gameObject);
        }
        UI_ScoreBoardPlayerList_Red.Clear();
    }
    
    #endregion
    
    #region INPUT

    public void OnScoreBoard(InputAction.CallbackContext context)
    {
        updatePlayerList();
        scoreBoardVisible = context switch
        {
            //Started. Show the tutorial.
            {phase: InputActionPhase.Started} => true,
            //Canceled. Hide the tutorial.
            {phase: InputActionPhase.Canceled} => false,
            //Default.
            _ => scoreBoardVisible
        };
    }

    #endregion

    #region Events

    public void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (EndingMode)
        {
            return;
        }
        updatePlayerList();
    }
    
    public void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (EndingMode)
        {
            return;
        }
        updatePlayerList();
    }
    
    
    public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey("Data_kill"))
        {
            Invoke("ScoreBoardSort",0.02f);
        }

        //如果是队伍信息改变强制刷新整个计分板
        if (changedProps.ContainsKey(PhotonTeamsManager.TeamPlayerProp))
        {
            if (EndingMode)
            {
                return;
            }
            updatePlayerList();
        }
    }

    public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        
    }

    public void OnMasterClientSwitched(Player newMasterClient)
    {
        if (EndingMode)
        {
            return;
        }
        updatePlayerList();
    }


    #endregion

}
