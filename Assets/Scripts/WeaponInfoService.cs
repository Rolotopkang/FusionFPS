
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class WeaponInfoService : MonoBehaviour,IWeaponInfoService
{
    private List<WeaponData> weaponDataList = new List<WeaponData>();

    private void Awake()
    {
        var _getSources = Resources.LoadAll<WeaponData>("SO");
        foreach (var sources in _getSources) 
        { 
            weaponDataList.Add(sources as WeaponData); 
        }

        foreach (WeaponData weaponData in weaponDataList)
        {
            Debug.Log(weaponData.weaponName);
        }
        Debug.Log("SO注册完毕");
    }

    public WeaponData GetWeaponInfoFromID(int id)
    {
        foreach (WeaponData weaponData in weaponDataList)
        {
            if (weaponData.ID.Equals(id))
            {
                return weaponData;
            }
        }

        Debug.LogError("没有此对应ID的武器");
        return null;
    }

    public WeaponData GetWeaponInfoFromName(string weaponName)
    {
        foreach (WeaponData weaponData in weaponDataList)
        {
            if (weaponData.weaponName.Equals(weaponName))
            {
                return weaponData;
            }
        }

        Debug.LogError("没有此对应注册名的武器");
        return null;
    }

    public List<WeaponData> GetWeaponInfosFromKinds(EnumTools.WeaponKind weaponKind)
    {
        List<WeaponData> temp = new List<WeaponData>();
        foreach (WeaponData weaponData in weaponDataList)
        {
            if (weaponData.WeaponKind.Equals(weaponKind))
            {
                temp.Add(weaponData);
            }
        }
        return temp;
    }
}
