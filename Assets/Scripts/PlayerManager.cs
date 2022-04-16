using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Unity.Mathematics;
using UnityEngine;
using UnityTemplateProjects.Tools;
using EventCode = Scripts.Weapon.EventCode;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerManager : MonoBehaviour,IOnEventCallback
{
    
    private PhotonView photonView;
    private GameObject DeployUI;
    private DeployManager DeployManager;

    private String DeployMainWeapon;
    private String DeploySecWeapon;

    private GameObject MainCM;

    private Transform tmp_Spawnpoint;

    private GameObject tmp_Player;
    #region Unity
    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        DeployUI = transform.GetChild(0).gameObject;
        DeployManager = DeployUI.GetComponent<DeployManager>();
        MainCM = GameObject.FindWithTag("MainCamera");
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            DeployUI.SetActive(true);
        }
    }
    
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    #endregion


    public void OnEvent(EventData photonEvent)
    {
        switch ((Scripts.Weapon.EventCode) photonEvent.Code)
        {
            case EventCode.KillPlayer:
                OnPlayerDeath(photonEvent);
                break;
        }
    }


    public void OnPlayerDeath(EventData eventData)
    {
        Dictionary<byte, object> tmp_KillData = (Dictionary<byte, object>)eventData.CustomData;
        Player tmp_deathPlayer =(Player)tmp_KillData[0];
        Player tmp_KillFrom =(Player)tmp_KillData[1];
        String tmp_KillWeapon = (String)tmp_KillData[2];
        bool tmp_headShot = (bool)tmp_KillData[3];
        long tmp_time = (long)tmp_KillData[4];

        //如果死亡的是这个manager控制的玩家
        if (tmp_deathPlayer.Equals(photonView.Owner))
        {
            //如果死亡的是本地角色
            if (tmp_deathPlayer.Equals(PhotonNetwork.LocalPlayer))
            {
                //转换相机
                //击杀者透视
                //关闭第一人称脚本
                //显示死亡UI
                //更新后处理效果
            }
        
            //双端执行:
        
            //开启布娃娃系统
            //枪械掉落
            //尸体消失倒计时
            //玩家列表删除玩家
        }
        
    }
    
    public void OnBTNDeploy()
    {
        DeployUI.SetActive(false);
        tmp_Spawnpoint = SpawnManager.GetInstance().GetSpawnPoint();
        Debug.Log(tmp_Spawnpoint+"555555555555555555555555555555");
        DeployMainWeapon = DeployManager.getChosedMainWeapon();
        DeploySecWeapon = DeployManager.getChosedSecWeapon();
        
        MainCM.GetComponent<TestTrack>().Dochange(tmp_Spawnpoint.gameObject,this);//tmp_Spawnpoint.gameObject

        Task.Run(async () => {
            // Example of long running code.
            await Task.Delay(1);
            Debug.Log("开始加载人物");
            tmp_Player =PhotonNetwork.Instantiate(
                Path.Combine("PhotonNetwork", "P_LPSP_FP_CH"), tmp_Spawnpoint.position
                , tmp_Spawnpoint.rotation,0,new object []{photonView.ViewID});
            tmp_Player.SetActive(false);
        });
        
        StartCoroutine(Deploy());


    }


    
    private IEnumerator Deploy()
    {
        //等待相机到位
        yield return new WaitUntil(() => MainCM.GetComponent<TestTrack>().isLocated);
        Debug.Log("相机到位");
        tmp_Player.SetActive(true);
    }
    
    // async void DepolyAsync()
    // {
    //     tmp_Player =PhotonNetwork.Instantiate(
    //         Path.Combine("PhotonNetwork", "P_LPSP_FP_CH"), tmp_Spawnpoint.position
    //         , tmp_Spawnpoint.rotation,0,new object []{photonView.ViewID});
    //     tmp_Player.SetActive(false);
    // }
 
    public String GetDeployMainWeapon() => DeployMainWeapon;
    public String GetDeploySecWeapon() => DeploySecWeapon;
}
