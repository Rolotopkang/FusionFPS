using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using SickscoreGames.HUDNavigationSystem;
using UnityEngine;

public class HUDSynSystem : MonoBehaviour
{
    public bool isLocal;
    [SerializeField]
    private PhotonView PhotonView;
    public HUDNavigationElement _hudNavigationElement;


    //我是傻逼
    private void Awake()
    {
        _hudNavigationElement = GetComponent<HUDNavigationElement>();
        isLocal = PhotonView.Owner.Equals(PhotonNetwork.LocalPlayer);
    }

    public void OnElementReady(HUDNavigationElement element)
    {
        element.Indicator.GetComponent<HNS_PlayerIcon_Indicator_Manager>().
            Initialisation(StaticTools.IsEnemy
                    (PhotonNetwork.LocalPlayer,PhotonView), PhotonView.Owner.NickName);
    }
    
    
}
