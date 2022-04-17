using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Cinemachine;
using ExitGames.Client.Photon;
using InfimaGames.LowPolyShooterPack;
using InfimaGames.LowPolyShooterPack.Interface;
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
    private GameObject DeathUI;
    
    
    private DeployManager DeployManager;

    private String DeployMainWeapon;
    private String DeploySecWeapon;

    private GameObject MainCM;

    private Transform tmp_Spawnpoint;

    private GameObject tmp_Player;

    private CinemachineVirtualCamera CinemachineVirtualCamera;
    
    protected IGameModeService gameModeService;



    private bool InstantiateDown = false;
    #region Unity
    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        DeployUI = transform.GetChild(0).gameObject;
        DeathUI = transform.GetChild(1).gameObject;
        DeployManager = DeployUI.GetComponent<DeployManager>();
        MainCM = GameObject.FindWithTag("MainCamera");
        CinemachineVirtualCamera = MainCM.GetComponent<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            DeployUI.SetActive(true);
        }
        gameModeService = ServiceLocator.Current.Get<IGameModeService>();
    }
    
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
        // if (photonView.IsMine)
        // {
        //     Debug.Log("测试生成！");
        //     tmp_Player =PhotonNetwork.Instantiate(
        //         Path.Combine("PhotonNetwork", "P_LPSP_FP_CH"), Vector3.zero
        //         , Quaternion.identity);
        //     PhotonNetwork.Destroy(tmp_Player.GetPhotonView());
        //     Debug.Log("测试摧毁完毕");
        // }
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
        String tmp_KillWeapon = (string)tmp_KillData[2];
        bool tmp_headShot = (bool)tmp_KillData[3];
        long tmp_time = (long)tmp_KillData[4];

        //如果死亡的是这个manager控制的玩家
        if (tmp_deathPlayer.Equals(photonView.Owner))
        {
            //如果死亡的是本地角色
            if (tmp_deathPlayer.Equals(PhotonNetwork.LocalPlayer))
            {
                Debug.Log("本地死亡！！！");
                //转换相机+
                //关闭第一人称脚本,第三人称开启
                gameModeService.GetPlayerGameObject(tmp_deathPlayer).GetComponent<LoacalChanger>().LocalDeath();
                
                
                //击杀者透视
                
                //显示死亡UI
                DeathUI.SetActive(true);
                //更新后处理效果
            }
        
            //双端执行:
            
            //开启布娃娃系统
            gameModeService.GetPlayerGameObject(tmp_deathPlayer).GetComponentInChildren<RagdollController>().Death(true);
            //枪械掉落
            //尸体消失倒计时
            //玩家列表删除玩家
        }
        
    }
    
    public void OnBTNDeploy()
    {
        DeployUI.SetActive(false);
        tmp_Spawnpoint = SpawnManager.GetInstance().GetSpawnPoint();
        
        
        DeployMainWeapon = DeployManager.getChosedMainWeapon();
        DeploySecWeapon = DeployManager.getChosedSecWeapon();

        // CinemachineVirtualCamera.Follow = tmp_Spawnpoint;
        // CinemachineVirtualCamera.LookAt = tmp_Spawnpoint.transform.GetChild(0);

        
        MainCM.GetComponent<TestTrack>().Dochange(tmp_Spawnpoint.gameObject);//tmp_Spawnpoint.gameObject
        StartCoroutine(InstantiatePlayer());
        StartCoroutine(Deploy());


    }


    
    private IEnumerator Deploy()
    {
        Debug.Log("等待相机");
        //等待相机到位
        yield return new WaitUntil(() => MainCM.GetComponent<TestTrack>().isLocated);
        Debug.Log("相机到位");
        yield return new WaitUntil(() => InstantiateDown);
        InstantiateDown = false;
        tmp_Player.SetActive(true);
    }

    private IEnumerator InstantiatePlayer()
    {
        Debug.Log("开始生成角色");
        tmp_Player =PhotonNetwork.Instantiate(
            Path.Combine("PhotonNetwork", "P_LPSP_FP_CH"), tmp_Spawnpoint.position
            , tmp_Spawnpoint.rotation,0,new object []{photonView.ViewID});
        tmp_Player.SetActive(false);
        InstantiateDown = true;
        Debug.Log("生成完毕");
        yield return null;
    }
    
    async void DepolyAsync()
    {
        await Task.Delay(1);
        Debug.Log("开始生成角色");
        tmp_Player =PhotonNetwork.Instantiate(
            Path.Combine("PhotonNetwork", "P_LPSP_FP_CH"), tmp_Spawnpoint.position
            , tmp_Spawnpoint.rotation,0,new object []{photonView.ViewID});
        tmp_Player.SetActive(false);
        Debug.Log("生成完毕");
    }
 
    public String GetDeployMainWeapon() => DeployMainWeapon;
    public String GetDeploySecWeapon() => DeploySecWeapon;
}
