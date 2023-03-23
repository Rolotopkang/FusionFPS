using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityTemplateProjects.Tools;

public class DetectTarget : MonoBehaviour
{
    [SerializeField]
    private HUDSynSystem _hudSynSystem;


    private PhotonView PhotonView;
    private bool isEnemy;
    private MapTools.GameMode currentGamemode;
    
    private void Awake()
    {
        PhotonView = GetComponentInParent<PhotonView>();
    }

    private void Start()
    {
        currentGamemode = RoomManager.GetInstance().currentGamemode;
        switch (currentGamemode)
        {
            case MapTools.GameMode.DeathMatch:
                isEnemy = !PhotonNetwork.LocalPlayer.Equals(PhotonView.Owner);
                break;
            case MapTools.GameMode.Conquest:
                isEnemy = StaticTools.IsEnemy(PhotonNetwork.LocalPlayer, PhotonView);
                break;
            case MapTools.GameMode.TeamDeathMatch:
                isEnemy = StaticTools.IsEnemy(PhotonNetwork.LocalPlayer, PhotonView);
                break;
            case MapTools.GameMode.BombScenario:
                isEnemy = StaticTools.IsEnemy(PhotonNetwork.LocalPlayer, PhotonView);
                break;
            case MapTools.GameMode.TeamAdversarial:
                isEnemy = StaticTools.IsEnemy(PhotonNetwork.LocalPlayer, PhotonView);
                break;
        }
    }

    private void Update()
    {
        if (isEnemy)
        {
            NetWorkSyn();
        }
    }

    public void RayCastHit(EnumTools.DetectTargetKind DetectTargetKind , bool set)
    {
        switch (currentGamemode)
        {
            case MapTools.GameMode.DeathMatch:
                switch (DetectTargetKind)
                {
                    case EnumTools.DetectTargetKind.Gaze :
                        _hudSynSystem.OnClientGaze(set); 
                        break;
                    case EnumTools.DetectTargetKind.Around :
                        _hudSynSystem.OnClientAround(set);
                        break;
                }
                break;
            default:
                switch (DetectTargetKind)
                {
                    case EnumTools.DetectTargetKind.Gaze :
                        if (isEnemy)
                        {
                            if (set)
                            {
                                Hashtable hash = new Hashtable();
                                hash.Add(EnumTools.PlayerProperties.IsDetected.ToString(),true);
                                PhotonView.Owner.SetCustomProperties(hash);
                            }
                            else
                            {
                                Invoke("CancelDetect",3f);
                            }
                        }
                        _hudSynSystem.OnClientGaze(set); 
                        break;
                    case EnumTools.DetectTargetKind.Around :
                        if(!isEnemy)
                            break;
                        if (set)
                        {
                            Hashtable hash = new Hashtable();
                            hash.Add(EnumTools.PlayerProperties.IsDetected.ToString(),true);
                            PhotonView.Owner.SetCustomProperties(hash);
                        }
                        else
                        {
                            Invoke("CancelDetect",3f);
                        }
                        break;
                }
                break;
        }
        
    }

    private void CancelDetect()
    {
        Hashtable hash = new Hashtable();
        hash.Add(EnumTools.PlayerProperties.IsDetected.ToString(),false);
        PhotonView.Owner.SetCustomProperties(hash);
    }
    
    private void NetWorkSyn()
    {
        if ((bool)PhotonView.Owner.CustomProperties[EnumTools.PlayerProperties.IsDetected.ToString()])
        { 
            _hudSynSystem.OnClientAround(true); 
        }
        else
        { 
            _hudSynSystem.OnClientAround(false); 
        }
    }
    
}
