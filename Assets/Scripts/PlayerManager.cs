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
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using SickscoreGames.HUDNavigationSystem;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;
using UnityTemplateProjects.Tools;
using EventCode = Scripts.Weapon.EventCode;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerManager : MonoBehaviour,IOnEventCallback
{
    [SerializeField] 
    private GameObject DeployUIPrefab;
    [SerializeField]
    private GameObject KillhintUIPrefab;
    [SerializeField]
    private GameObject DeathUIPrefab;
    [SerializeField]
    private GameObject KillFeedBackRoomPrefab;
    [SerializeField]
    private GameObject ScordBoardPrefab;
    [SerializeField]
    private GameObject ScordBoardTeamPrefab;
    [SerializeField]
    private GameObject HUDNavigationCanvasPrefab;
    [SerializeField]
    private GameObject OutOfBoundWarningUIManagerPrefab;
    [Tooltip("Quality settings menu prefab spawned at start. Used for switching between different quality settings in-game.")]
    [SerializeField]
    private GameObject qualitySettingsPrefab;


    public Player Owner;
    private PhotonView photonView;
    private GameObject DeployUI;
    private GameObject DeathUI;
    private GameObject KillhintUI;
    private GameObject KillFeedBackRoom;
    private bool canDeploy = true;




    private DeployManager DeployManager;

    private String DeployMainWeapon;
    private String DeploySecWeapon;
    private String DeployItem;

    private GameObject MainCM;
    private GameObject CMBrain;

    private Transform tmp_Spawnpoint;

    private GameObject tmp_Player;
    private Outline playerFrom_outline = null;
    private GameObject Scoreboard;
    
    
    private CinemachineVirtualCamera CinemachineVirtualCamera;
    private CinemachineComponentBase ComponentBase;
    private HUDNavigationSystem _HUDNavigationSystem;
    private HUDNavigationCanvas HUDNavigationCanvas;

    protected IGameModeService gameModeService;
    protected IWeaponInfoService weaponInfoService;

    
    #region Unity
    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        Owner = photonView.Owner;
    }

    private void Start()
    {
        RoomManager.GetInstance().currentGamemodeManager.AddPlayerManagerMethod(this);
        if (photonView.IsMine)
        {
            MainCM = GameObject.FindWithTag("DeployCM");
            CMBrain = GameObject.FindWithTag("CMBrain");
            CinemachineVirtualCamera = MainCM.GetComponent<CinemachineVirtualCamera>();
            ComponentBase = CinemachineVirtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
            Instantiate(qualitySettingsPrefab,transform);
            DeployUI = Instantiate(DeployUIPrefab);
            DeployManager = DeployUI.GetComponent<DeployManager>();
            DeployUI.SetActive(true);
            
            DeathUI = Instantiate(DeathUIPrefab, transform);
            DeathUI.SetActive(false);
            
            KillhintUI = Instantiate(KillhintUIPrefab, transform);
            KillFeedBackRoom = Instantiate(KillFeedBackRoomPrefab, transform);
            Instantiate(OutOfBoundWarningUIManagerPrefab, transform);
            

            if (RoomManager.GetInstance().isTeamMatch())
            {
                Scoreboard = Instantiate(ScordBoardTeamPrefab, transform);
            }
            else
            {
                Scoreboard = Instantiate(ScordBoardPrefab, transform);
            }

            _HUDNavigationSystem = HUDNavigationSystem.Instance;
            HUDNavigationCanvas = HUDNavigationCanvas.Instance;
            HUDNavigationCanvas.EnableCanvas(false);
        }
        gameModeService = ServiceLocator.Current.Get<IGameModeService>();
        weaponInfoService = ServiceLocator.Current.Get<IWeaponInfoService>();
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
            case EventCode.GameEnd:
                if (photonView.IsMine)
                {
                    StartCoroutine(GameEnd());
                }
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
        
        Debug.Log("死亡者"+tmp_deathPlayer+"被"+tmp_KillFrom+"用"+tmp_KillWeapon+"是否爆头"+tmp_headShot+"击杀");

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
                playerFrom_outline = gameModeService.GetPlayerGameObject(tmp_KillFrom).GetComponent<LoacalChanger>().OutlineScript;
                
                //关闭导航
                _HUDNavigationSystem.PlayerCamera = null;
                _HUDNavigationSystem.PlayerController = null;
                HUDNavigationCanvas.EnableCanvas(false);

                CMBrain.SetActive(true);
                MainCM.SetActive(true);
                CinemachineVirtualCamera.LookAt = tmp_TP.transform;
                StartCoroutine(LookAt(3, tmp_KillerTP.transform));
                
                //关闭第一人称脚本,第三人称开启
                gameModeService.GetPlayerGameObject(tmp_deathPlayer).GetComponent<LoacalChanger>().LocalDeath();


                
                
                //如果是自杀
                if (tmp_deathPlayer.Equals(tmp_KillFrom))
                {
                    tmp_KillWeapon = "suicide";
                }
                //显示死亡UI
                DeathUI.GetComponent<DeathUIManager>().Set(
                    tmp_KillFrom.NickName,
                    weaponInfoService.GetWeaponInfoFromName(tmp_KillWeapon).KillPannel ,
                    tmp_KillWeapon,
                    gameModeService.GetPlayerGameObject(tmp_KillFrom).GetComponent<Battle>().GetCurrentHealth()
                );
                DeathUI.SetActive(true);
                //更新后处理效果
                
                //5秒后重新部署
                //TODO
                Invoke("OnBTNReborn",5);
            }
            //不是本地死亡
            else
            {
                gameModeService.GetPlayerGameObject(tmp_deathPlayer).GetComponent<LoacalChanger>().RemoteDeath();
            }
        
            //双端执行:
            
            //开启布娃娃系统
            gameModeService.GetPlayerGameObject(tmp_deathPlayer).GetComponentInChildren<RagdollController>().Death(true);
            gameModeService.GetPlayerGameObject(tmp_deathPlayer).GetComponent<CharacterController>().enabled = false;
            //枪械掉落
            //尸体消失倒计时
            //玩家列表删除玩家
        }
        
        //仅在本地执行
        if (photonView.Owner.Equals(PhotonNetwork.LocalPlayer))
        {
            // Debug.Log("显示房间死亡讯息");
            UIKillFeedBackRoomManager.CreateKillFeedbackRoom(
                new UIKillFeedBackRoom.RoomKillMes(
                    tmp_deathPlayer.NickName,
                    tmp_deathPlayer.Equals(PhotonNetwork.LocalPlayer)? Color.green : 
                    StaticTools.IsEnemy(PhotonNetwork.LocalPlayer,tmp_deathPlayer)? Color.red : 
                    new Color(0,1,1),
                    tmp_KillWeapon,
                    tmp_headShot,
                    tmp_KillFrom.NickName,
                     tmp_KillFrom.Equals(PhotonNetwork.LocalPlayer)? Color.green : 
                     StaticTools.IsEnemy(PhotonNetwork.LocalPlayer,tmp_KillFrom)? Color.red : 
                     new Color(0,1,1)));
            Debug.Log( tmp_deathPlayer.GetPhotonTeam()+ "队伍的" +tmp_deathPlayer.NickName +"被" + tmp_KillFrom.GetPhotonTeam()+"队伍的"+ tmp_KillFrom.NickName +"击杀");
        }
    }

    
    private IEnumerator LookAt(int waitTime, Transform target)
    {
        yield return new WaitForSeconds(waitTime);
        if (!RoomManager.GetInstance().currentGamemodeManager.GetRoomState)
        {
            yield break;
        }
        CinemachineVirtualCamera.Follow = null;
        CinemachineVirtualCamera.LookAt = target.transform;
        //击杀者透视
        if (playerFrom_outline)
        {
            playerFrom_outline.enabled = true;
        }
    }
    
    public void OnBTNDeploy()
    {
        //游戏没开始不允许部署
        if (!RoomManager.GetInstance().currentGamemodeManager.GetRoomState)
        {
            return;
        }

        if(!canDeploy)
            return;
        
        tmp_Spawnpoint = SpawnManager.GetInstance().GetSpawnPoint();
        if (tmp_Spawnpoint == null)
        {
            Debug.LogWarning("没有选择点位！");
            return;
        }
        
        //部署锁打开
        canDeploy = false;
        
        
        //关闭UI
        DeployManager.canvasFader.FadeOut(1f);
        //如果是征服模式
        if (RoomManager.GetInstance().currentGamemode.Equals(MapTools.GameMode.Conquest))
        {
            DeployChoiceManager.GetInstance().CanvasFader.FadeOut(1f);
        }

        //获取部署武器
        DeployMainWeapon = DeployManager.getChosedMainWeapon();
        DeploySecWeapon = DeployManager.getChosedSecWeapon();
        DeployItem = DeployManager.getClosedItem();
        
        
        //TODO
        //添加本局武器插槽记录


        // CinemachineVirtualCamera.Follow = tmp_Spawnpoint;
        // CinemachineVirtualCamera.LookAt = tmp_Spawnpoint.transform.GetChild(0);
        
        MainCM.GetComponent<MainCameraController>().Dochange(tmp_Spawnpoint.gameObject);//tmp_Spawnpoint.gameObject
        StartCoroutine(Deploy());
    }

    public void OnBTNReborn()
    {
        if (!RoomManager.GetInstance().currentGamemodeManager.GetRoomState)
        {
            return;
        }
        if (playerFrom_outline)
        {
            playerFrom_outline.enabled = false;
        }
        playerFrom_outline = null;
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
        if (!RoomManager.GetInstance().currentGamemodeManager.GetRoomState)
        {
            MainCM.GetComponent<MainCameraController>().ResetPos();
            return;
        }
        MainCM.GetComponent<MainCameraController>().Dochange();
        StartCoroutine(ReturnDeploy());
    }
    
    
    
    
    private IEnumerator ReturnDeploy()
    {
        DeathUI.SetActive(false);
        Debug.Log("等待相机");
        yield return new WaitUntil(() => MainCM.GetComponent<MainCameraController>().isLocated);
        Debug.Log("相机到位");
        DeployManager.canvasFader.FadeIn(0.5f);
        //如果是征服模式
        if (RoomManager.GetInstance().currentGamemode.Equals(MapTools.GameMode.Conquest))
        {
            DeployChoiceManager.GetInstance().CanvasFader.FadeIn(0.5f,DeployManager.GetInstance().OnEnable);
        }
    }


    private IEnumerator GameEnd()
    {
        //关闭导航
        _HUDNavigationSystem.PlayerCamera = null;
        _HUDNavigationSystem.PlayerController = null;
        HUDNavigationCanvas.EnableCanvas(false);
        DeployUI.SetActive(false);
        DeathUI.SetActive(false);
        Destroy(Scoreboard);

        yield return new WaitForSeconds(0.75f);
        
        //重置相机位置
        MainCM.GetComponent<MainCameraController>().ResetPos();
        //没死的话删除角色
        Debug.Log("网络删除角色!");
        if (tmp_Player!=null)
        {
            gameModeService.GetPlayerGameObject(PhotonNetwork.LocalPlayer)?.GetComponent<LoacalChanger>().LocalDeath();
            if (tmp_Player.GetPhotonView())
            {
                PhotonNetwork.Destroy(tmp_Player.GetPhotonView());
            }
        }
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
        
        //设置死亡状态
        Hashtable hash = new Hashtable();
        hash.Add(EnumTools.PlayerProperties.IsDeath.ToString(),false);
        photonView.Owner.SetCustomProperties(hash);
        
        //导航系统开启
        _HUDNavigationSystem.PlayerCamera = Camera.main;
        _HUDNavigationSystem.PlayerController = tmp_Player.transform;
        HUDNavigationCanvas.EnableCanvas(true);

        
        if (ComponentBase is Cinemachine3rdPersonFollow)
        {
            (ComponentBase as Cinemachine3rdPersonFollow).CameraDistance = 5; // your value
            (ComponentBase as Cinemachine3rdPersonFollow).ShoulderOffset.y = 3;
        }
        CinemachineVirtualCamera.Follow = tmp_Player.transform;
        CinemachineVirtualCamera.LookAt = tmp_Player.transform;
        MainCM.SetActive(false);
        CMBrain.SetActive(false);
    }

    public void GameReset()
    {
        CMBrain.SetActive(true);
        MainCM.SetActive(true);
        DeployUI.SetActive(true);
        DeployManager.canvasFader.FadeIn(0.5f);
        //如果是征服模式
        if (RoomManager.GetInstance().currentGamemode.Equals(MapTools.GameMode.Conquest))
        {
            DeployChoiceManager.GetInstance().CanvasFader.FadeIn(0.5f,DeployManager.GetInstance().OnEnable);
        }
        
        if (RoomManager.GetInstance().isTeamMatch())
        {
            Scoreboard= Instantiate(ScordBoardTeamPrefab, transform);
        }
        else
        {
            Scoreboard= Instantiate(ScordBoardPrefab, transform);
        }
    }

    public String GetDeployMainWeapon() => DeployMainWeapon;
    public String GetDeploySecWeapon() => DeploySecWeapon;

    public String GetDeployItem() => DeployItem;

    public GameObject GetPlayerOBJ() => tmp_Player;

    public void SetCanDeploy(bool set)
    {
        canDeploy = set;
    } 

    #region Events



    #endregion
}
