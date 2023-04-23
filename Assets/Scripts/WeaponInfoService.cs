
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
        // string[] dirs = Directory.GetFiles("Assets/SO");
        var _getSources = Resources.LoadAll<WeaponData>("SO");
        foreach (var sources in _getSources) 
        { 
            weaponDataList.Add(sources as WeaponData); 
        }
        
        // for (int i = 0; i < dirs.Length; i+=2)
        // {
        //     weaponDataList.();
        //     // weaponDataList.Add(AssetDatabase.LoadAssetAtPath<WeaponData>(dirs[i])); 
        // }

        // foreach (WeaponData weaponData in weaponDataList)
        // {
        //     Debug.Log(weaponData.weaponName);
        // }
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
}
