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
    private CinemachineComponentBase ComponentBase;

    protected IGameModeService gameModeService;

    
    #region Unity
    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        DeployUI = transform.GetChild(0).gameObject;
        DeathUI = transform.GetChild(1).gameObject;
        DeployManager = DeployUI.GetComponent<DeployManager>();
        MainCM = GameObject.FindWithTag("MainCamera");
        CinemachineVirtualCamera = MainCM.GetComponent<CinemachineVirtualCamera>();
        ComponentBase = CinemachineVirtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
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
        String tmp_KillWeapon = (String)tmp_KillData[2];
        bool tmp_headShot = (bool)tmp_KillData[3];
        long tmp_time = (long)tmp_KillData[4];
        
        Debug.Log("角色死亡"+"死亡者"+tmp_deathPlayer+"被"+tmp_KillFrom+"用"+tmp_KillWeapon+"是否爆头"+tmp_headShot+"击杀");
        
        
        //如果死亡的是这个manager控制的玩家
        if (tmp_deathPlayer.Equals(photonView.Owner))
        {
            //如果死亡的是本地角色
            if (tmp_deathPlayer.Equals(PhotonNetwork.LocalPlayer))
            {
                Debug.Log("本地死亡！！！");

                
                //转换相机+
                // MainCM.transform.position = gameModeService.GetPlayerGameObject(tmp_deathPlayer).transform.position +new Vector3(0,4,0);
                GameObject tmp_TP = gameModeService.GetPlayerGameObject(tmp_deathPlayer).GetComponent<LoacalChanger>().TPBody;
                GameObject tmp_KillerTP = gameModeService.GetPlayerGameObject(tmp_KillFrom).GetComponent<LoacalChanger>().TPBody;
                CinemachineVirtualCamera.LookAt = tmp_TP.transform;
                StartCoroutine(LookAt(3, tmp_KillerTP.transform));
                
                //关闭第一人称脚本,第三人称开启
                gameModeService.GetPlayerGameObject(tmp_deathPlayer).GetComponent<LoacalChanger>().LocalDeath();


                


                //击杀者透视
                
                //显示死亡UI
                DeathUI.SetActive(true);
                //更新后处理效果
                
                //10秒后重新部署
                //TODO
                Invoke("OnBTNReborn",10);
            }
        
            //双端执行:
            
            //开启布娃娃系统
            gameModeService.GetPlayerGameObject(tmp_deathPlayer).GetComponentInChildren<RagdollController>().Death(true);
            //枪械掉落
            //尸体消失倒计时
            //玩家列表删除玩家
        }
        
    }

    
    private IEnumerator LookAt(int waitTime, Transform target)
    {
        yield return new WaitForSeconds(waitTime);
        CinemachineVirtualCamera.Follow = null;
        CinemachineVirtualCamera.LookAt = target.transform;
    }
    
    public void OnBTNDeploy()
    {
        DeployUI.SetActive(false);
        tmp_Spawnpoint = SpawnManager.GetInstance().GetSpawnPoint();
        
        
        DeployMainWeapon = DeployManager.getChosedMainWeapon();
        DeploySecWeapon = DeployManager.getChosedSecWeapon();

        // CinemachineVirtualCamera.Follow = tmp_Spawnpoint;
        // CinemachineVirtualCamera.LookAt = tmp_Spawnpoint.transform.GetChild(0);
        
        MainCM.GetComponent<MainCameraController>().Dochange(tmp_Spawnpoint.gameObject);//tmp_Spawnpoint.gameObject
        StartCoroutine(Deploy());
    }

    public void OnBTNReborn()
    {
        CinemachineVirtualCamera.Follow = null;
        CinemachineVirtualCamera.LookAt = null;
        if (ComponentBase is Cinemachine3rdPersonFollow)
        {
            (ComponentBase as Cinemachine3rdPersonFollow).CameraDistance = 0; // your value
            (ComponentBase as Cinemachine3rdPersonFollow).ShoulderOffset.y = 0;
        }
        //网络删除角色
        Debug.Log("网络删除角色!");
        PhotonNetwork.Destroy(tmp_Player.GetPhotonView());
        MainCM.GetComponent<MainCameraController>().Dochange();
        StartCoroutine(ReturnDeploy());
    }
    
    
    
    
    private IEnumerator ReturnDeploy()
    {
        DeathUI.SetActive(false);
        Debug.Log("等待相机");
        yield return new WaitUntil(() => MainCM.GetComponent<MainCameraController>().isLocated);
        Debug.Log("相机到位");
        
        DeployUI.SetActive(true);
    }

    private IEnumerator Deploy()
    {
        Debug.Log("等待相机");
        //等待相机到位
        yield return new WaitUntil(() => MainCM.GetComponent<MainCameraController>().isLocated);
        Debug.Log("相机到位");
        Debug.Log("开始生成角色");
        tmp_Player =PhotonNetwork.Instantiate(
            Path.Combine("PhotonNetwork", "P_LPSP_FP_CH"), tmp_Spawnpoint.position
            , tmp_Spawnpoint.rotation,0,new object []{photonView.ViewID});
        Debug.Log("生成完毕");
        
        if (ComponentBase is Cinemachine3rdPersonFollow)
        {
            (ComponentBase as Cinemachine3rdPersonFollow).CameraDistance = 5; // your value
            (ComponentBase as Cinemachine3rdPersonFollow).ShoulderOffset.y = 3;
        }
        CinemachineVirtualCamera.Follow = tmp_Player.transform;
        CinemachineVirtualCamera.LookAt = tmp_Player.transform;
    }

    public String GetDeployMainWeapon() => DeployMainWeapon;
    public String GetDeploySecWeapon() => DeploySecWeapon;
}
