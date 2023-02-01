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
    
    
    private bool isElementReady = false;
    private HUDNavigationElement _element;
    [SerializeField] private Battle _battle;


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
        isElementReady = true;
        _element = element;
    }

    private void Update()
    {
        if(_element.Indicator)
            _element.Indicator.GetComponent<HNS_PlayerIcon_Indicator_Manager>().UpdateHPInfo(_battle.GetCurrentHealthP());
    }

    public void OnClientGaze(bool set)
    {
        if(_element.Indicator)
            _element.Indicator.GetComponent<HNS_PlayerIcon_Indicator_Manager>().SetGazed(set);
    }

    public void OnClientAround(bool set)
    {
        if(_element.Indicator)
            _element.Indicator.GetComponent<HNS_PlayerIcon_Indicator_Manager>().SetArounded(set);
    }


}
