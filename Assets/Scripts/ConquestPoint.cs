using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using SickscoreGames.HUDNavigationSystem;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(PhotonView))]
public class ConquestPoint : MonoBehaviourPun,IPunObservable
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

    #region Unity

    private void Awake()
    {
        redTeamsInPointList = new List<Player>();
        blueTeamsInPointList = new List<Player>();
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
                            pointOwnerTeam = occupyProgress>=1? EnumTools.Teams.Blue :EnumTools.Teams.Red;
                            isOccupying = false;
                            //化整
                            occupyProgress = occupyProgress >= 1 ? 1 : -1;
                            //加分
                            //TODO
                        }
                        
                    }
                    
                    break;
                }
                case EnumTools.Teams.Red:
                {
                    //点内一方人多
                    if (GetInPointBlueTeamsNum() != GetInPointRedTeamsNum())
                    {
                        if (GetInPointBlueTeamsNum()>GetInPointRedTeamsNum())
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
                            isScrambling = GetInPointRedTeamsNum() != 0;
                        }
                    }

                    break;
                }
                case EnumTools.Teams.Blue:
                {
                    //点内一方人多
                    if (GetInPointBlueTeamsNum() != GetInPointRedTeamsNum())
                    {
                        if (GetInPointBlueTeamsNum()<GetInPointRedTeamsNum())
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
                            isScrambling = GetInPointBlueTeamsNum() != 0;
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
            //如果是本地进入
            if (tmp_player.Equals(PhotonNetwork.LocalPlayer))
            {
                //TODO
                //添加屏显UI
            }

            //如果没死做加入
            if (!(bool) tmp_player.CustomProperties[EnumTools.PlayerProperties.IsDeath.ToString()])
            {
                if (tmp_player.GetPhotonTeam().Name.Equals("Blue"))
                {
                    blueTeamsInPointList.Add(tmp_player);
                }else if (tmp_player.GetPhotonTeam().Name.Equals("Red"))
                {
                    redTeamsInPointList.Add(tmp_player);
                }
                else
                {
                    Debug.LogError("此角色没有设置阵营！");
                }
            }
            


            //房主端执行
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                
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
                //TODO
                //关闭屏显UI
            }
            RemovePlayer(tmp_player);
        }
    }

    #region Funtions

    private void RemovePlayer(Player player)
    {
        if (player.GetPhotonTeam().Name.Equals("Blue"))
        {
            blueTeamsInPointList.Remove(player);
        }else if (player.GetPhotonTeam().Name.Equals("Red"))
        {
            redTeamsInPointList.Remove(player);
        }
        else
        {
            Debug.LogError("此角色没有设置阵营！");
        }
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
                isOccupying = (bool)stream.ReceiveNext();
                isScrambling = (bool)stream.ReceiveNext();
                occupyProgress = (float)stream.ReceiveNext();
                pointOwnerTeam = (EnumTools.Teams)Enum.Parse(typeof(EnumTools.Teams),(string)stream.ReceiveNext());
            }
        }
    }

    #endregion
    
    #region Getter

    public int GetInPointRedTeamsNum() => redTeamsInPointList.Count;
    
    public int GetInPointBlueTeamsNum() => blueTeamsInPointList.Count;

    #endregion
}
