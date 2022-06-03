using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Random = System.Random;

public class DeathUIManager : MonoBehaviour
{
    [SerializeField] 
    private GameObject MaskIMG;

    [Header("击杀者")]
    [SerializeField] 
    private TextMeshProUGUI killbyName;
    [SerializeField] private Image KillerGunIMG;
    [SerializeField] private TextMeshProUGUI KillerWeaponName;
    [SerializeField] private TextMeshProUGUI KillerLeftHealth;
    [SerializeField] private Image KillerLeftHealthIMG;


    private Image[] maskImgList;
    private int maskImgNum;

    private void Awake()
    {
        maskImgList = MaskIMG.GetComponentsInChildren<Image>();
        foreach (Image VARIABLE in maskImgList)
        {
            VARIABLE.gameObject.SetActive(false);
        }
        maskImgList[0].gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        maskImgNum = UnityEngine.Random.Range(1, MaskIMG.transform.childCount);
        Debug.Log(MaskIMG.transform.childCount);
        maskImgList[maskImgNum].gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        maskImgList[maskImgNum].gameObject.SetActive(false);
    }

    public void Set(String killbyName, Sprite killerGunImg, String killerWeaponName, float killerLeftHealth)
    {
        this.killbyName.text = killbyName;
        KillerGunIMG.sprite= killerGunImg;
        KillerWeaponName.text = killerWeaponName;
        KillerLeftHealth.text = killerLeftHealth.ToString("f0");
        KillerLeftHealthIMG.fillAmount = killerLeftHealth / 100;
    }
}
