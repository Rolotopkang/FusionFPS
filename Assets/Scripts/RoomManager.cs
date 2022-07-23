using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using Photon.Realtime;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityTemplateProjects.Tools;

public class RoomManager : SingletonPunCallbacks<RoomManager>
{
    [Header("游戏模式")] 
    [SerializeField] private GameObject DeathMatchModePrefab;
    [SerializeField] private GameObject ConquestModePrefab;


    public GameModeManagerBehaviour currentGamemodeManager = null;
    public MapTools.GameMode currentGamemode;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // public override void OnDisable()
    // {
    //     base.OnDisable();
    //     SceneManager.sceneLoaded -= OnSceneLoaded;
    // }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        Debug.Log("SceneLoaded!");
        CheckScene(scene);
    }

    public override void OnLeftRoom()
    {
        Destroy(currentGamemodeManager.transform.gameObject);
        currentGamemodeManager = null;
    }

    private void CheckScene(Scene scene)
    {
        if (!PhotonNetwork.InRoom)
        {
            Debug.Log("不在房间内");
            return;
        }


        Debug.Log("筛选游戏类型！");
        if (scene.buildIndex.Equals(int.Parse(PhotonNetwork.CurrentRoom.CustomProperties["mapIndex"].ToString())))
        {
            //预载
            PhotonNetwork.PrefabPool = new MyPhotonNetwrokPool(Path.Combine("PhotonNetwork", "P_LPSP_FP_CH"));
            
            switch (PhotonNetwork.CurrentRoom.CustomProperties["GameMode"])
            {
                case "Conquest":
                {
                    Debug.Log("征服模式！");
                    currentGamemodeManager = Instantiate(ConquestModePrefab, transform).GetComponent<GameModeManagerBehaviour>();
                    Debug.Log("生成征服模式游戏逻辑控制脚本");
                    currentGamemode = MapTools.GameMode.Conquest;
                    PhotonNetwork.Instantiate(Path.Combine("PhotonNetwork", "PlayerManager"),
                        Vector3.zero, quaternion.identity);
                    break;
                }
                case "BombScenario":
                {
                    Debug.Log("游戏模式未实装");
                    break;
                }
                case "TeamAdversarial":
                {
                    Debug.Log("游戏模式未实装");
                    break;
                }
                case "DeathMatch":
                {
                    Debug.Log("死斗模式！");
                    currentGamemodeManager = Instantiate(DeathMatchModePrefab, transform).GetComponent<GameModeManagerBehaviour>();
                    Debug.Log("生成死斗模式游戏逻辑控制脚本");
                    currentGamemode = MapTools.GameMode.DeathMatch;
                    PhotonNetwork.Instantiate(Path.Combine("PhotonNetwork", "PlayerManager"),
                        Vector3.zero, quaternion.identity);
                    break;
                }
                case "TeamDeathMatch":
                {
                    Debug.Log("团队死斗！");
                    PhotonNetwork.Instantiate(Path.Combine("PhotonNetwork", "PlayerManager"), 
                        Vector3.zero, quaternion.identity);
                    currentGamemode = MapTools.GameMode.TeamDeathMatch;
                    break;
                }
                default:
                {
                    Debug.Log(PhotonNetwork.CurrentRoom.CustomProperties["GameMode"]+"找不到");
                    break;
                }
            }
        }
        
    }


    public bool isFriendlyFire()
    {
        //TODO 如果房间允许队伤则返回true
        
        switch (currentGamemode)
        {
            case MapTools.GameMode.Conquest:
            case MapTools.GameMode.TeamDeathMatch: 
            case MapTools.GameMode.BombScenario:
            case MapTools.GameMode.TeamAdversarial:
                return false;
            case MapTools.GameMode.DeathMatch:
                return true;
        }

        return true;
    }

    public bool isTeamMatch()
    {
        switch (currentGamemode)
        {
            case MapTools.GameMode.Conquest:
            case MapTools.GameMode.TeamDeathMatch: 
            case MapTools.GameMode.BombScenario:
            case MapTools.GameMode.TeamAdversarial:
                return true;
            case MapTools.GameMode.DeathMatch:
                return false;
        }

        return false;
    }
}
