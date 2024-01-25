using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
using InfimaGames.LowPolyShooterPack;
using InfimaGames.LowPolyShooterPack.Interface;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityTemplateProjects.Tools;

public class DeployManager : Singleton<DeployManager>
{
    private ItemButton currMainWeapon;
    private ItemButton currSecWeapon;
    private ItemButton currGrenade;
    private ItemButton currItem;
    
    public Image MainWeaponImage;
    public Image SecWeaponImage;
    public Image GrenadeImage;
    public Image ItemImage;
    
    [SerializeField]
    private GameObject MainWeaponPanel;

    [SerializeField]
    private GameObject SecWeaponPanel;
    
    [SerializeField]
    private GameObject GrenadePanel;

    [SerializeField]
    private GameObject ItemPanel;
    
    [SerializeField]
    private ItemButton[] mainWeaponBTNlist;
    [SerializeField]
    private ItemButton[] secWeaponBTNlist;
    [SerializeField]
    private ItemButton[] GrenadeBTNlist;
    [SerializeField]
    private ItemButton[] ItemBTNlist;

    [SerializeField]
    private TextMeshProUGUI MainWeaponName;
    
    [SerializeField]
    private TextMeshProUGUI SecWeaponName;
    
    [SerializeField]
    private TextMeshProUGUI GrenadeName;
    
    [SerializeField]
    private TextMeshProUGUI ItemName;

    [SerializeField]
    private TextMeshProUGUI CountDownText;

    [SerializeField]
    private GameObject CountDownBTN;
    [SerializeField]
    private GameObject DepolyBTN;

    [SerializeField] 
    private DeployUI_Top[] DeployUITops;

    [SerializeField] 
    private TextMeshProUGUI Hint;
    
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
        
        mainWeaponBTNlist = MainWeaponPanel.GetComponentsInChildren<ItemButton>();
        secWeaponBTNlist = SecWeaponPanel.GetComponentsInChildren<ItemButton>();
        GrenadeBTNlist = GrenadePanel.GetComponentsInChildren<ItemButton>();
        ItemBTNlist = ItemPanel.GetComponentsInChildren<ItemButton>();

        if (mainWeaponBTNlist.Length > 0)
        {
            mainWeaponBTNlist[0].isDown = true;
        }

        if (secWeaponBTNlist.Length > 0)
        {
            secWeaponBTNlist[0].isDown = true;
        }

        if (GrenadeBTNlist.Length > 0)
        {
            GrenadeBTNlist[0].isDown = true;
        }

        if (ItemBTNlist.Length > 0)
        {
            ItemBTNlist[0].isDown = true;
        }
    }

    public void OnEnable()
    {
        In_Game_SettingsMenu.GetInstance().setCursorLocked(false);
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
        //人数不足提示
        if (RoomManager.GetInstance().currentGamemodeManager.GetRoomState)
        {
            Hint.gameObject.SetActive(false);
        }
        else
        {
            Hint.gameObject.SetActive(true);
            if (RoomManager.GetInstance().currentGamemodeManager.GetRoomStateEnd)
            {
                Hint.text = "游戏人数不足";
            }
            else
            {
                Hint.text = "等待上一场游戏结算";
            }
        }
        
        
        currMainWeapon = CheckActive(mainWeaponBTNlist);
        currSecWeapon = CheckActive(secWeaponBTNlist);
        currGrenade = CheckActive(GrenadeBTNlist);
        currItem = CheckActive(ItemBTNlist);
        
        MainWeaponImage.sprite = currMainWeapon.BTDS;
        SecWeaponImage.sprite = currSecWeapon.BTDS;
        GrenadeImage.sprite = currGrenade.BTDS;
        ItemImage.sprite = currItem.BTDS;
        
        
        MainWeaponName.text = currMainWeapon.ShowName;
        SecWeaponName.text = currSecWeapon.ShowName;
        GrenadeName.text = currGrenade.ShowName; 
        ItemName.text = currItem.ShowName;
        
        CountDownBTN.SetActive(!(timer<=0));
        DepolyBTN.SetActive(timer<=0);
        RoomManager.GetInstance().currentGamemodeManager.GetPlayerManager(PhotonNetwork.LocalPlayer).SetCanDeploy(timer<=0);
        
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
    
    public void SetGrenadeChecked()
    {
        foreach (ItemButton itemButton in GrenadeBTNlist)
        {
            itemButton.isDown = false;
        }
    }
    
    public void SetItemChecked()
    {
        foreach (ItemButton itemButton in ItemBTNlist)
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

    public String getClosedItem() => currItem.GetItemName();

    #endregion

}
