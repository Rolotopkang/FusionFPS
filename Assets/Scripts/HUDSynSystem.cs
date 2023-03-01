using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using SickscoreGames.HUDNavigationSystem;
using UnityEngine;
using UnityTemplateProjects.Tools;
using EventCode = Scripts.Weapon.EventCode;

public class HUDSynSystem : MonoBehaviour,IOnEventCallback
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
        switch (RoomManager.GetInstance().currentGamemode)
        {
            case MapTools.GameMode.Conquest:
                isEnemy = StaticTools.IsEnemy(PhotonNetwork.LocalPlayer, PhotonView);
                break;
            case MapTools.GameMode.DeathMatch:
                isEnemy = true;
                break;
            default:
                return;
                break;
        }
        
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

    public void OnEvent(EventData photonEvent)
    {
        switch ((EventCode) photonEvent.Code)
        {
            case EventCode.KillPlayer: 
                OnPlayerDeath(photonEvent); 
                break;
        }
    }

    private void OnPlayerDeath(EventData eventData)
    {
        Dictionary<byte, object> tmp_KillData = (Dictionary<byte, object>)eventData.CustomData;
        Player tmp_deathPlayer =(Player)tmp_KillData[0];
        if (tmp_deathPlayer.Equals(PhotonNetwork.LocalPlayer))
        {
            if (isEnemy)
            {
                OnClientGaze(false);
                OnClientAround(false);
            }
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
