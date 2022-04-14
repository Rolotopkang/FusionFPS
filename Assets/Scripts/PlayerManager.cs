using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using UnityTemplateProjects.Tools;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerManager : MonoBehaviour
{
    
    private PhotonView photonView;
    private GameObject DeployUI;
    private DeployManager DeployManager;

    private String DeployMainWeapon;
    private String DeploySecWeapon;

    private GameObject MainCM;
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

    #endregion
   

    public void OnBTNDeploy()
    {
        DeployUI.SetActive(false);
        Transform tmp_Spawnpoint = SpawnManager.GetInstance().GetSpawnPoint();
        DeployMainWeapon = DeployManager.getChosedMainWeapon();
        DeploySecWeapon = DeployManager.getChosedSecWeapon();
        
        
        // MainCM.GetComponent<TestTrack>().Dochange(tmp_Spawnpoint.gameObject);
        
        GameObject tmp_Player =PhotonNetwork.Instantiate(
            Path.Combine("PhotonNetwork", "P_LPSP_FP_CH"), tmp_Spawnpoint.position
            , tmp_Spawnpoint.rotation,0,new object []{photonView.ViewID});
    }
    

    public String GetDeployMainWeapon() => DeployMainWeapon;
    public String GetDeploySecWeapon() => DeploySecWeapon;
}
