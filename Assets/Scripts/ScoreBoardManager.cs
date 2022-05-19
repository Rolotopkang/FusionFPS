using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityTemplateProjects.Tools;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ScoreBoardManager : MonoBehaviour,IInRoomCallbacks
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
    
    [Header("计分板")]
    [SerializeField] private TextMeshProUGUI Data_TopPlayerName;
    [SerializeField] private TextMeshProUGUI Data_TopPlayerKill;

    [Header("设置")]
    [SerializeField] private GameObject scoreboard;
    [SerializeField] private Transform scoreBoardRoot;
    [SerializeField] private ScrollRect scoreBoardScrollRect;

    [Header("预制体")] 
    [SerializeField] private GameObject UI_ScoreBoardPlayerPrefab;

    #endregion

    #region Private

    private Player LocalPlayer;
    private bool scoreBoardVisible;
    private Player[] roomPlayers;
    private List<UIScordBoardPlayer> UI_ScoreBoardPlayerList;

    #endregion

    #region Unity

    private void Awake()
    {
        UI_ScoreBoardPlayerList = new List<UIScordBoardPlayer>();
    }

    private void Start()
    {
        LocalPlayer = PhotonNetwork.LocalPlayer;
        scoreboard.SetActive(false);
        updatePlayerList();
        Instantiate_UI_ScoreBoardPlayerList();
        PhotonNetwork.AddCallbackTarget(this);
        Data_RoomMapName.text = PhotonNetwork.CurrentRoom.CustomProperties["GameMode"] switch
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
        Data_RoomRule.text = "击杀30个获得胜利";


        Data_LocalPlayerName.text = LocalPlayer.NickName;
        


    }

    private void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    private void Update()
    {
        scoreboard.SetActive(scoreBoardVisible);
        //计分板排序
        UpdateMineData();
    }
    
    #endregion

    #region Funtions

    private void updatePlayerList()
    {
        roomPlayers = PhotonNetwork.PlayerList;
    }

    private void Instantiate_UI_ScoreBoardPlayerList()
    {
        foreach (Player player in roomPlayers)
        {
            UIScordBoardPlayer tmp_UIScordBoardPlayer = Instantiate(UI_ScoreBoardPlayerPrefab, scoreBoardRoot).GetComponent<UIScordBoardPlayer>();
            tmp_UIScordBoardPlayer.Register(player);
            UI_ScoreBoardPlayerList.Add(tmp_UIScordBoardPlayer);
        }
    }

    /// <summary>
    /// tick按照击杀数给计分板排序
    /// </summary>
    private void ScoreBoardSort()
    {
        Debug.Log("计分板重新排序！");
        UI_ScoreBoardPlayerList.Sort((x,y) => -x.GetKillNum().CompareTo(y.GetKillNum()));
        foreach (UIScordBoardPlayer uiScordBoardPlayer in UI_ScoreBoardPlayerList)
        {
            uiScordBoardPlayer.transform.SetSiblingIndex(UI_ScoreBoardPlayerList.IndexOf(uiScordBoardPlayer));
        }
        
    }

    private void UpdateMineData()
    {
        int tmp_TopkillNum;
        Data_TopPlayerName.text = UI_ScoreBoardPlayerList[0].GetPlayer().NickName;
        UI_ScoreBoardPlayerList[0].GetPlayer().CustomProperties.TryGetValue(EnumTools.PlayerProperties.Data_kill.ToString(), out tmp_TopkillNum);
        Data_TopPlayerKill.text = tmp_TopkillNum.ToString();
        
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
        for (int i = 0; i < UI_ScoreBoardPlayerList.Count; i++)
        {
            if (UI_ScoreBoardPlayerList[i].GetPlayer().Equals(player))
            {
                Destroy(UI_ScoreBoardPlayerList[i].transform.gameObject);
                UI_ScoreBoardPlayerList.Remove(UI_ScoreBoardPlayerList[i]);
            }
        }
    }

    /// <summary>
    /// 当玩家加入房间把它加入计分板
    /// </summary>
    /// <param name="player"></param>
    private void AddPlayerToList(Player player)
    {
        UIScordBoardPlayer tmp_UIScordBoardPlayer = Instantiate(UI_ScoreBoardPlayerPrefab, scoreBoardRoot).GetComponent<UIScordBoardPlayer>();
        tmp_UIScordBoardPlayer.Register(player);
        UI_ScoreBoardPlayerList.Add(tmp_UIScordBoardPlayer);
    }
    
    #endregion
    
    #region INPUT

    public void OnScoreBoard(InputAction.CallbackContext context)
    {
        scoreBoardVisible = context switch
        {
            //Started. Show the tutorial.
            {phase: InputActionPhase.Started} => true,
            //Canceled. Hide the tutorial.
            {phase: InputActionPhase.Canceled} => false,
            //Default.
            _ => scoreBoardVisible
        };
        ScoreBoardSort();
    }

    #endregion

    #region Events

    public void OnPlayerEnteredRoom(Player newPlayer)
    {
        updatePlayerList();
        AddPlayerToList(newPlayer);
    }

    public void OnPlayerLeftRoom(Player otherPlayer)
    {
        updatePlayerList();
        RemovePlayerFromList(otherPlayer);
    }









    public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey("Data_kill"))
        {
            Invoke("ScoreBoardSort",0.02f);
        }
    }
    public void OnMasterClientSwitched(Player newMasterClient) { }
    public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged) { }

    #endregion

}
