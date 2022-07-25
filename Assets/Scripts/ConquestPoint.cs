using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using SickscoreGames.HUDNavigationSystem;
using UnityEngine;
using UnityEngine.Rendering;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Random = UnityEngine.Random;
using EventCode = Scripts.Weapon.EventCode;

[RequireComponent(typeof(PhotonView))]
public class ConquestPoint : MonoBehaviourPun, IPunObservable, IInRoomCallbacks
{
    [SerializeField] public EnumTools.ConquestPoints pointName;

    /// <summary>
    /// 据点是否正在占领
    /// </summary>
    public bool isOccupying;

    /// <summary>
    /// 据点是否正在争夺
    /// </summary>
    public bool isScrambling;

    /// <summary>
    /// 争夺进度（-1~1）
    /// -1 red
    /// 1 blue
    /// </summary>
    public float occupyProgress;


    /// <summary>
    /// 据点所属阵营
    /// </summary>
    public EnumTools.Teams pointOwnerTeam = EnumTools.Teams.None;

    public List<Player> redTeamsInPointList;
    public List<Player> blueTeamsInPointList;
    private SpawnPoint[] SpawnPoints;

    #region Unity

    private void Awake()
    {
        redTeamsInPointList = new List<Player>();
        blueTeamsInPointList = new List<Player>();
        SpawnPoints = GetComponentsInChildren<SpawnPoint>();
    }

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
    }

    private void Update()
    {
        CheckDeath();
        if (PhotonNetwork.IsMasterClient)
        {
            //点内无人
            if (GetInPointBlueTeamsNum() == 0 && GetInPointRedTeamsNum() == 0)
            {
                isOccupying = false;
                isScrambling = false;
                if (pointOwnerTeam == EnumTools.Teams.None)
                {
                    if (occupyProgress == 0)
                    {
                        return;
                    }

                    if (Mathf.Abs(occupyProgress) < ConquestPointManager.GetInstance().occupySpeed *
                        Time.deltaTime)
                    {
                        occupyProgress = 0;
                        return;
                    }
                    
                    occupyProgress += 
                        (occupyProgress > 0 ? -1 : 1) * 
                        ConquestPointManager.GetInstance().occupySpeed *
                        Time.deltaTime;
            
                    return;
                }
                
                if (Mathf.Abs(occupyProgress) >= 1)
                {
                    return;
                }
                
                if (1-Mathf.Abs(occupyProgress) < ConquestPointManager.GetInstance().occupySpeed *
                    Time.deltaTime)
                {
                    occupyProgress = pointOwnerTeam == EnumTools.Teams.Blue? 1 : -1;
                    return;
                }
                
                occupyProgress += (pointOwnerTeam == EnumTools.Teams.Blue? 1: -1)* 
                    ConquestPointManager.GetInstance().occupySpeed *
                    Time.deltaTime;
                
                return;
            }

            //点内人数一样多
            if (GetInPointBlueTeamsNum() == GetInPointRedTeamsNum())
            {
                isScrambling = true;
                isOccupying = false;
                return;
            }

            switch (pointOwnerTeam)
            {
                case EnumTools.Teams.None:
                {
                    //点内一方人多
                    if (GetInPointBlueTeamsNum() != GetInPointRedTeamsNum())
                    {
                        int tmp_gip = GetInPointBlueTeamsNum() - GetInPointRedTeamsNum();
                        isScrambling = false;
                        isOccupying = true;
                        occupyProgress += tmp_gip * ConquestPointManager.GetInstance().occupySpeed * Time.deltaTime;
                        if (Mathf.Abs(occupyProgress) >= 1)
                        {
                            pointOwnerTeam = occupyProgress >= 1 ? EnumTools.Teams.Blue : EnumTools.Teams.Red;
                            isOccupying = false;
                            //化整
                            occupyProgress = occupyProgress >= 1 ? 1 : -1;
                            //发送占领事件
                            RaiseOccupiedEvent();
                        }
                    }
                    break;
                }
                case EnumTools.Teams.Red:
                {
                    //点内一方人多
                    if (GetInPointBlueTeamsNum() != GetInPointRedTeamsNum())
                    {
                        if (GetInPointBlueTeamsNum() > GetInPointRedTeamsNum())
                        {
                            isOccupying = true;
                            isScrambling = false;
                            int tmp_gip = GetInPointBlueTeamsNum() - GetInPointRedTeamsNum();
                            occupyProgress += tmp_gip * ConquestPointManager.GetInstance().occupySpeed * Time.deltaTime;
                            if (occupyProgress >= 0)
                            {
                                pointOwnerTeam = EnumTools.Teams.None;
                            }
                        }
                        else
                        {
                            isOccupying = false;
                            isScrambling = GetInPointBlueTeamsNum() != 0;
                        }
                    }

                    break;
                }
                case EnumTools.Teams.Blue:
                {
                    //点内一方人多
                    if (GetInPointBlueTeamsNum() != GetInPointRedTeamsNum())
                    {
                        if (GetInPointBlueTeamsNum() < GetInPointRedTeamsNum())
                        {
                            isOccupying = true;
                            isScrambling = false;
                            int tmp_gip = GetInPointBlueTeamsNum() - GetInPointRedTeamsNum();
                            occupyProgress += tmp_gip * ConquestPointManager.GetInstance().occupySpeed * Time.deltaTime;
                            if (occupyProgress <= 0)
                            {
                                pointOwnerTeam = EnumTools.Teams.None;
                            }
                        }
                        else
                        {
                            isOccupying = false;
                            isScrambling = GetInPointRedTeamsNum() != 0;
                        }
                    }

                    break;
                }
            }
        }
    }

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals("Player"))
        {
            PhotonView tmp_PhotonView = other.transform.GetComponent<PhotonView>();
            Player tmp_player = tmp_PhotonView.Owner;

            //如果没死做加入
            if (!(bool) tmp_player.CustomProperties[EnumTools.PlayerProperties.IsDeath.ToString()])
            {
                AddPlayer(tmp_player);
            }

            //房主端执行
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag.Equals("Player"))
        {
            PhotonView tmp_PhotonView = other.transform.GetComponent<PhotonView>();
            Player tmp_player = tmp_PhotonView.Owner;
            //如果是本地进入
            if (tmp_player.Equals(PhotonNetwork.LocalPlayer))
            {
                ConquestPoint_UI_HUD.GetInstance().OpenConquestPoint_UI_HUD(pointName);
            }

            //如果没死做加入
            if (!(bool) tmp_player.CustomProperties[EnumTools.PlayerProperties.IsDeath.ToString()])
            {
                if (!isPlayerIn(tmp_player))
                {
                    AddPlayer(tmp_player);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag.Equals("Player"))
        {
            PhotonView tmp_PhotonView = other.transform.GetComponent<PhotonView>();
            Player tmp_player = tmp_PhotonView.Owner;
            //如果是本地离开
            if (tmp_player.Equals(PhotonNetwork.LocalPlayer))
            {
                ConquestPoint_UI_HUD.GetInstance().CloseConquestPoint_UI_HUD();
            }

            RemovePlayer(tmp_player);
        }
    }

    #region Funtions

    private void AddPlayer(Player player)
    {
        if (player.GetPhotonTeam().Name.Equals("Blue"))
        {
            blueTeamsInPointList.Add(player);
        }
        else if (player.GetPhotonTeam().Name.Equals("Red"))
        {
            redTeamsInPointList.Add(player);
        }
        else
        {
            Debug.LogError("此角色没有设置阵营！");
        }
    }

    private void RemovePlayer(Player player)
    {
        if (player.GetPhotonTeam().Name.Equals("Blue"))
        {
            blueTeamsInPointList.Remove(player);
        }
        else if (player.GetPhotonTeam().Name.Equals("Red"))
        {
            redTeamsInPointList.Remove(player);
        }
        else
        {
            Debug.LogError("此角色没有设置阵营！");
        }
    }

    public bool isPlayerIn(Player checkPlayer)
    {
        foreach (Player player in blueTeamsInPointList)
        {
            if (checkPlayer.Equals(player))
            {
                return true;
            }
        }

        foreach (Player player in redTeamsInPointList)
        {
            if (checkPlayer.Equals(player))
            {
                return true;
            }
        }

        return false;
    }

    private void CheckDeath()
    {
        for (int i = 0; i < redTeamsInPointList.Count; i++)
        {
            if ((bool) redTeamsInPointList[i].CustomProperties[EnumTools.PlayerProperties.IsDeath.ToString()])
            {
                RemovePlayer(redTeamsInPointList[i]);
            }
        }

        for (int i = 0; i < blueTeamsInPointList.Count; i++)
        {
            if ((bool) blueTeamsInPointList[i].CustomProperties[EnumTools.PlayerProperties.IsDeath.ToString()])
            {
                RemovePlayer(blueTeamsInPointList[i]);
            }
        }
    }

    public void OnElementReady(HUDNavigationElement element)
    {
        if (element.Indicator != null)
        {
            Transform tmp_transform = element.Indicator.GetCustomTransform("Point");
            if (tmp_transform != null)
            {
                tmp_transform.GetComponent<ConquestPointIndicator>().ConquestPoint = this;
            }
        }

        if (element.Minimap != null)
        {
            Transform tmp_transform = element.Minimap.GetCustomTransform("Point");
            if (tmp_transform != null)
            {
                tmp_transform.GetComponent<ConquestPointIndicator>().ConquestPoint = this;
            }
        }
    }

    public Transform GetBirthPoint()
    {
        List<SpawnPoint> tmp_spawnPoints = new List<SpawnPoint>();
        foreach (SpawnPoint spawnPoint in SpawnPoints)
        {
            if (spawnPoint.GetIsOpen())
            {
                tmp_spawnPoints.Add(spawnPoint);
            }
        }

        if (tmp_spawnPoints.Count.Equals(0))
        {
            Debug.Log("所有重生点均不可用");
            return SpawnPoints[0].transform;
        }

        return tmp_spawnPoints[Random.Range(0, tmp_spawnPoints.Count)].transform;
    }

    private void RaiseOccupiedEvent()
    {
        if (pointOwnerTeam == EnumTools.Teams.None)
        {
            return;
        }
        Dictionary<byte, object> tmp_OccupiedData = new Dictionary<byte, object>();
        tmp_OccupiedData.Add(0,pointName.ToString());
        tmp_OccupiedData.Add(1,pointOwnerTeam.ToString());

        RaiseEventOptions tmp_RaiseEventOptions = new RaiseEventOptions() {Receivers = ReceiverGroup.All};
        SendOptions tmp_SendOptions = SendOptions.SendReliable;
        PhotonNetwork.RaiseEvent(
            (byte)EventCode.ConquestPointOccupied,
            tmp_OccupiedData,
            tmp_RaiseEventOptions,
            tmp_SendOptions);
        Debug.Log("发送占领事件");
    }
    
    #endregion

    #region Photon

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                stream.SendNext(isOccupying);
                stream.SendNext(isScrambling);
                stream.SendNext(occupyProgress);
                stream.SendNext(pointOwnerTeam.ToString());
            }
        }
        else
        {
            if (!PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                isOccupying = (bool) stream.ReceiveNext();
                isScrambling = (bool) stream.ReceiveNext();
                occupyProgress = (float) stream.ReceiveNext();
                pointOwnerTeam = (EnumTools.Teams) Enum.Parse(typeof(EnumTools.Teams), (string) stream.ReceiveNext());
            }
        }
    }

    public void OnPlayerEnteredRoom(Player newPlayer)
    {
    }

    public void OnPlayerLeftRoom(Player otherPlayer)
    {
        for (int i = 0; i < redTeamsInPointList.Count; i++)
        {
            if (otherPlayer.Equals(redTeamsInPointList[i]))
            {
                RemovePlayer(redTeamsInPointList[i]);
            }
        }

        for (int i = 0; i < blueTeamsInPointList.Count; i++)
        {
            if (otherPlayer.Equals(blueTeamsInPointList[i]))
            {
                RemovePlayer(blueTeamsInPointList[i]);
            }
        }
    }

    public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
    }

    public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
    }

    public void OnMasterClientSwitched(Player newMasterClient)
    {
    }

    #endregion

    #region Getter

    public List<Player> GetPlayerList(EnumTools.Teams teams)
    {
        if (teams == EnumTools.Teams.None)
            return null;
        return teams == EnumTools.Teams.Blue ? blueTeamsInPointList : redTeamsInPointList;
    }
    
    public int GetInPointRedTeamsNum() => redTeamsInPointList.Count;

    public int GetInPointBlueTeamsNum() => blueTeamsInPointList.Count;

    #endregion
}