using System;
using System.Collections;
using System.Collections.Generic;
using InfimaGames.LowPolyShooterPack;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class TPInventory : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Animator TPAnimator;
    
    private TPWeaponController[] weapons;
    
    private TPWeaponController equipped;

    private int equippedIndex = -1;

    private PhotonView PhotonView;
    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
        weapons = GetComponentsInChildren<TPWeaponController>(true);
        foreach (TPWeaponController weapon in weapons)
            weapon.gameObject.SetActive(false);
        if (!PhotonView.IsMine)
        {
            Invoke(nameof(chushihua),1f);
        }
    }

    public TPWeaponController Equip(int index)
    {
        //If we have no weapons, we can't really equip anything.
        if (weapons == null)
            return equipped;
        //index工具查找不到对应武器
        if (index.Equals(-1))
        {
            Debug.Log("武器不存在");
            return equipped;
        }
        //The index needs to be within the array's bounds.
        if (index > weapons.Length - 1)
            return equipped;
        //No point in allowing equipping the already-equipped weapon.
        if (equippedIndex == index)
            return equipped;
        //Disable the currently equipped weapon, if we have one.
        if (equipped != null)
            equipped.gameObject.SetActive(false);
        //Update index.
        equippedIndex = index;
        //Update equipped.
        equipped = weapons[equippedIndex];
        //Activate the newly-equipped weapon.
        equipped.gameObject.SetActive(true);
        //切换第三人称动画机
        //TODO 换枪动画携程
        TPAnimator.runtimeAnimatorController = equipped.RuntimeAnimatorController;

        //Return.
        return equipped;
    }

    private void chushihua()
    {
        Equip((int)PhotonView.Owner.CustomProperties["EquipWeaponIndex"]);
    }
    
    private int GetIndexByWeaponBehaviour(String name)
    {
        int i = 0;
        foreach (TPWeaponController weapon in weapons)
        {
            if (weapon.GetWeaponName().Equals(name))
            {
                return i;
            }

            i++;
        }

        return -1;
    }
    
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (targetPlayer.Equals(PhotonView.Owner)&& changedProps.ContainsKey("EquipWeaponIndex"))
        {
            Equip((int)PhotonView.Owner.CustomProperties["EquipWeaponIndex"]);
        }
    }

    public TPWeaponController GetEquipped() => equipped;
}