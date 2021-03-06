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

    private GameObject MainCM;
    private GameObject CMBrain;

    private Transform tmp_Spawnpoint;

    private GameObject tmp_Player;
    private Outline playerFrom_outline = null;

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
                Instantiate(ScordBoardTeamPrefab, transform);
            }
            else
            {
                Instantiate(ScordBoardPrefab, transform);
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
        
        Debug.Log("????????????"+"?????????"+tmp_deathPlayer+"???"+tmp_KillFrom+"???"+tmp_KillWeapon+"????????????"+tmp_headShot+"??????");

        //????????????????????????manager???????????????
        if (tmp_deathPlayer.Equals(photonView.Owner))
        {
            //??????????????????????????????
            if (tmp_deathPlayer.Equals(PhotonNetwork.LocalPlayer))
            {
                Debug.Log("?????????????????????");
                
                //????????????+
                // MainCM.transform.position = gameModeService.GetPlayerGameObject(tmp_deathPlayer).transform.position +new Vector3(0,4,0);
                GameObject tmp_TP = gameModeService.GetPlayerGameObject(tmp_deathPlayer).GetComponent<LoacalChanger>().TPBody;
                GameObject tmp_KillerTP = gameModeService.GetPlayerGameObject(tmp_KillFrom).GetComponent<LoacalChanger>().TPBody;
                playerFrom_outline = gameModeService.GetPlayerGameObject(tmp_KillFrom).GetComponent<LoacalChanger>().OutlineScript;
                
                //????????????
                _HUDNavigationSystem.PlayerCamera = null;
                _HUDNavigationSystem.PlayerController = null;
                HUDNavigationCanvas.EnableCanvas(false);

                CMBrain.SetActive(true);
                MainCM.SetActive(true);
                CinemachineVirtualCamera.LookAt = tmp_TP.transform;
                StartCoroutine(LookAt(3, tmp_KillerTP.transform));
                
                //????????????????????????,??????????????????
                gameModeService.GetPlayerGameObject(tmp_deathPlayer).GetComponent<LoacalChanger>().LocalDeath();


                
                
                //???????????????
                if (tmp_deathPlayer.Equals(tmp_KillFrom))
                {
                    tmp_KillWeapon = "suicide";
                }
                //????????????UI
                DeathUI.GetComponent<DeathUIManager>().Set(
                    tmp_KillFrom.NickName,
                    weaponInfoService.GetWeaponInfoFromName(tmp_KillWeapon).KillPannel ,
                    tmp_KillWeapon,
                    gameModeService.GetPlayerGameObject(tmp_KillFrom).GetComponent<Battle>().GetCurrentHealth()
                );
                DeathUI.SetActive(true);
                //?????????????????????
                
                //5??????????????????
                //TODO
                Invoke("OnBTNReborn",5);
            }
        
            //????????????:
            
            //?????????????????????
            gameModeService.GetPlayerGameObject(tmp_deathPlayer).GetComponentInChildren<RagdollController>().Death(true);
            gameModeService.GetPlayerGameObject(tmp_deathPlayer).GetComponent<CharacterController>().enabled = false;
            //????????????
            //?????????????????????
            //????????????????????????
        }
        
        //??????????????????
        if (photonView.Owner.Equals(PhotonNetwork.LocalPlayer))
        {
            // Debug.Log("????????????????????????");
            UIKillFeedBackRoomManager.CreateKillFeedbackRoom(
                new UIKillFeedBackRoom.RoomKillMes(
                    tmp_deathPlayer.NickName,
                    tmp_deathPlayer.Equals(PhotonNetwork.LocalPlayer)? Color.green : Color.red,
                    tmp_KillWeapon,
                    tmp_headShot,
                    tmp_KillFrom.NickName,
                    tmp_KillFrom.Equals(PhotonNetwork.LocalPlayer)? Color.green : Color.red));
        }
    }

    
    private IEnumerator LookAt(int waitTime, Transform target)
    {
        yield return new WaitForSeconds(waitTime);
        CinemachineVirtualCamera.Follow = null;
        CinemachineVirtualCamera.LookAt = target.transform;
        //???????????????
        if (playerFrom_outline)
        {
            playerFrom_outline.enabled = true;
        }
    }
    
    public void OnBTNDeploy()
    {
        if(!canDeploy)
            return;
        
        tmp_Spawnpoint = SpawnManager.GetInstance().GetSpawnPoint();
        if (tmp_Spawnpoint == null)
        {
            Debug.LogWarning("?????????????????????");
            return;
        }
        
        //???????????????
        canDeploy = false;
        
        
        //??????UI
        DeployManager.canvasFader.FadeOut(1f);
        //?????????????????????
        if (RoomManager.GetInstance().currentGamemode.Equals(MapTools.GameMode.Conquest))
        {
            DeployChoiceManager.GetInstance().CanvasFader.FadeOut(1f);
        }

        //??????????????????
        DeployMainWeapon = DeployManager.getChosedMainWeapon();
        DeploySecWeapon = DeployManager.getChosedSecWeapon();

        // CinemachineVirtualCamera.Follow = tmp_Spawnpoint;
        // CinemachineVirtualCamera.LookAt = tmp_Spawnpoint.transform.GetChild(0);
        
        MainCM.GetComponent<MainCameraController>().Dochange(tmp_Spawnpoint.gameObject);//tmp_Spawnpoint.gameObject
        StartCoroutine(Deploy());
    }

    public void OnBTNReborn()
    {
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
        //??????????????????
        Debug.Log("??????????????????!");
        PhotonNetwork.Destroy(tmp_Player.GetPhotonView());
        MainCM.GetComponent<MainCameraController>().Dochange();
        StartCoroutine(ReturnDeploy());
    }
    
    
    
    
    private IEnumerator ReturnDeploy()
    {
        DeathUI.SetActive(false);
        Debug.Log("????????????");
        yield return new WaitUntil(() => MainCM.GetComponent<MainCameraController>().isLocated);
        Debug.Log("????????????");
        DeployManager.canvasFader.FadeIn(0.5f);
        //?????????????????????
        if (RoomManager.GetInstance().currentGamemode.Equals(MapTools.GameMode.Conquest))
        {
            DeployChoiceManager.GetInstance().CanvasFader.FadeIn(0.5f,DeployManager.GetInstance().OnEnable);
        }
    }

    private IEnumerator Deploy()
    {
        Debug.Log("????????????");
        //??????????????????
        yield return new WaitUntil(() => MainCM.GetComponent<MainCameraController>().isLocated);
        Debug.Log("????????????");
        Debug.Log("??????????????????");
        tmp_Player =PhotonNetwork.Instantiate(
            Path.Combine("PhotonNetwork", "P_LPSP_FP_CH"), tmp_Spawnpoint.position
            , tmp_Spawnpoint.rotation,0,new object []{photonView.ViewID});
        Debug.Log("????????????");
        
        //??????????????????
        Hashtable hash = new Hashtable();
        hash.Add(EnumTools.PlayerProperties.IsDeath.ToString(),false);
        photonView.Owner.SetCustomProperties(hash);
        
        //??????????????????
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

    public String GetDeployMainWeapon() => DeployMainWeapon;
    public String GetDeploySecWeapon() => DeploySecWeapon;

    public void SetCanDeploy(bool set)
    {
        canDeploy = set;
    } 

    #region Events



    #endregion
}
