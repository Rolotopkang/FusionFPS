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


    private bool isEnemy = false;

    //我是傻逼
    private void Awake()
    {
        _hudNavigationElement = GetComponent<HUDNavigationElement>();
        isLocal = PhotonView.Owner.Equals(PhotonNetwork.LocalPlayer);
    }

    public void OnElementReady(HUDNavigationElement element)
    {
        isEnemy = StaticTools.IsEnemy(PhotonNetwork.LocalPlayer, PhotonView);
        
        
        //Indicator 初始化
        element.Indicator.GetComponent<HNS_PlayerIcon_Indicator_Manager>().
            Initialisation(isEnemy, PhotonView.Owner.NickName);
        //Minimap 初始化
        element.Minimap.GetComponent<HNS_PlayerIcon_Minimap_Manager>().Initialisation(!isEnemy);
        
        isElementReady = true;
        _element = element;
        
    }

    private void Update()
    {
        if(_element.Indicator)
            _element.Indicator.GetComponent<HNS_PlayerIcon_Indicator_Manager>().UpdateHPInfo(_battle.GetCurrentHealthP());

        if (_element.Minimap)
        {
            //先判断是否被本地玩家观察
            //循环判断房内玩家属性有无被观察
            //TODO 接入团队标记系统
            // _element.Minimap.GetComponent<HNS_PlayerIcon_Minimap_Manager>().SetVisiable(
            //     
            // );
            
            
        }


    }

    public void OnClientGaze(bool set)
    {
        if(_element.Indicator)
            _element.Indicator.GetComponent<HNS_PlayerIcon_Indicator_Manager>().SetGazed(set);
        
        if(_element.Minimap)
            _element.Minimap.GetComponent<HNS_PlayerIcon_Minimap_Manager>().SetVisiable(set);
        
    }

    public void OnClientAround(bool set)
    {
        if(_element.Indicator)
            _element.Indicator.GetComponent<HNS_PlayerIcon_Indicator_Manager>().SetArounded(set);
        if(_element.Minimap)
            _element.Minimap.GetComponent<HNS_PlayerIcon_Minimap_Manager>().SetVisiable(set);
    }


}
