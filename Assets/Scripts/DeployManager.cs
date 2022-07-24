using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
using InfimaGames.LowPolyShooterPack;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityTemplateProjects.Tools;

public class DeployManager : Singleton<DeployManager>
{
    [SerializeField]
    private ItemButton currMainWeapon;
    [SerializeField]
    private ItemButton currSecWeapon;

    public Image MainWeaponImage;
    public Image SecWeaponImage;
    
    [SerializeField]
    private GameObject MainWeaponPanel;

    [SerializeField]
    private GameObject SecWeaponPanel;
    
    [SerializeField]
    private ItemButton[] mainWeaponBTNlist;
    [SerializeField]
    private ItemButton[] secWeaponBTNlist;

    [SerializeField]
    private TextMeshProUGUI MainWeaponName;
    
    [SerializeField]
    private TextMeshProUGUI SecWeaponName;

    [SerializeField]
    private TextMeshProUGUI CountDownText;

    [SerializeField]
    private GameObject CountDownBTN;
    [SerializeField]
    private GameObject DepolyBTN;

    [SerializeField] 
    private DeployUI_Top[] DeployUITops;
    
    [Header("部署倒计时时长")]
    [SerializeField]
    private float DePloyTimer;

    



    public CanvasFader canvasFader;
    
    
    private float timer;
    private float showTimer = 0;
    public float GetShowTimer => showTimer;
    private float maxTimer;
    public float GetMaxDePloyTimer => maxTimer;
    
    private void Awake()
    {
        base.Awake();
        canvasFader = GetComponent<CanvasFader>();
        mainWeaponBTNlist = MainWeaponPanel.GetComponentsInChildren<ItemButton>();
        secWeaponBTNlist = SecWeaponPanel.GetComponentsInChildren<ItemButton>();
        DePloyTimer = RoomManager.GetInstance().currentGamemodeManager.GetdeployWaitTime;
    }

    private void Start()
    {
        DeployUITops[ RoomManager.GetInstance().currentGamemode switch
        {
            MapTools.GameMode.DeathMatch => 0,
            MapTools.GameMode.TeamDeathMatch => 1,
            MapTools.GameMode.Conquest => 2,
            MapTools.GameMode.TeamAdversarial => 3,
            MapTools.GameMode.BombScenario => 4,

        }].gameObject.SetActive(true);
    }

    public void OnEnable()
    {
        DepolyBTN.SetActive(false);
        CountDownBTN.SetActive(true);

        //TODO 如果能部署再计时
        timer = DePloyTimer;
        maxTimer = timer;
        showTimer = timer;
        StartCoroutine(Time());
    }

    private void Update()
    {
        currMainWeapon = CheckActive(mainWeaponBTNlist);
        currSecWeapon = CheckActive(secWeaponBTNlist);
        MainWeaponImage.sprite = currMainWeapon.BTDS;
        SecWeaponImage.sprite = currSecWeapon.BTDS;
        MainWeaponName.text = currMainWeapon.ShowName;
        SecWeaponName.text = currSecWeapon.ShowName;
        if (timer<=0)
        {
            CountDownBTN.SetActive(false);
            DepolyBTN.SetActive(true);
            RoomManager.GetInstance().currentGamemodeManager.GetPlayerManager(PhotonNetwork.LocalPlayer).SetCanDeploy(true);
        }
        if (showTimer > 0)
        {
            showTimer -= UnityEngine.Time.deltaTime;
            if (showTimer < 0)
            {
                showTimer = 0;
            }
        }
    }

    private ItemButton CheckActive(ItemButton[] itemButtons)
    {
        foreach (ItemButton itemButton in itemButtons)
        {
            if (itemButton.isDown)
            {
                return itemButton;
            }
                
        }

        return null;
    }
    
    public void SetMainIsChecked()
    {
        foreach (ItemButton itemButton in mainWeaponBTNlist)
        {
            itemButton.isDown = false;
        }
    }
    
    public void SetSecIsChecked()
    {
        foreach (ItemButton itemButton in secWeaponBTNlist)
        {
            itemButton.isDown = false;
        }
    }

    public void OnDeployButton()
    {
        RoomManager.GetInstance().currentGamemodeManager.GetPlayerManager(PhotonNetwork.LocalPlayer).OnBTNDeploy();
    }


    #region Tools

    IEnumerator Time()
    {
        while (timer >= 0)
        {
            CountDownText.text = "部署在" + (int)timer + "秒之后";
            yield return new WaitForSeconds(1);
            timer--;
        }
    }

    #endregion
    
    #region GetterSetter

    public String getChosedMainWeapon() => currMainWeapon.GetItemName();
    public String getChosedSecWeapon() => currSecWeapon.GetItemName();

    #endregion
    
}
