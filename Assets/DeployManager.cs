using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityTemplateProjects.Tools;

public class DeployManager : MonoBehaviour
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
    

    [Header("部署倒计时时长")]
    [SerializeField]
    private float DePloyTimer;

    private float timer;
    
    private PlayerManager PlayerManager;

    private void Awake()
    {
        mainWeaponBTNlist = MainWeaponPanel.GetComponentsInChildren<ItemButton>();
        secWeaponBTNlist = SecWeaponPanel.GetComponentsInChildren<ItemButton>();
        PlayerManager = GetComponentInParent<PlayerManager>();
    }

    private void OnEnable()
    {
        DepolyBTN.SetActive(false);
        CountDownBTN.SetActive(true);
        timer = DePloyTimer;
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


    #region DataUpdate

    

    #endregion


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
