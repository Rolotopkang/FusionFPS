using System;
using System.Collections;
using System.Collections.Generic;
using InfimaGames.LowPolyShooterPack;
using UnityEngine;

public class DeployUI_Weapon_BTNGenerator : MonoBehaviour
{
    [SerializeField]
    private EnumTools.WeaponKind weaponKind;

    [SerializeField]
    private GameObject WeaponBTNPrefab;

    private IWeaponInfoService _weaponInfoService;
    private void Awake()
    {
        _weaponInfoService = ServiceLocator.Current.Get<IWeaponInfoService>();
        List<WeaponData> weaponDatas = _weaponInfoService.GetWeaponInfosFromKinds(weaponKind);
        if (weaponDatas.Count > 0)
        { 
            foreach (WeaponData weaponData in weaponDatas)
            {
                GameObject tmp_BTN = Instantiate(WeaponBTNPrefab, Vector3.zero, Quaternion.identity, transform);
                tmp_BTN.GetComponent<ItemButton>().Init(weaponData.weaponName,weaponData.weaponShowName,weaponData.DeployB,weaponData.DeployD,weaponData.KillPannel);
            }
            
        }
       
    }
    
}
