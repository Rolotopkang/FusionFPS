﻿//Copyright 2022, Infima Games. All Rights Reserved.

using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    public class Inventory : InventoryBehaviour
    {
        #region FIELDS
        
        /// <summary>
        /// Array of all weapons. These are gotten in the order that they are parented to this object.
        /// </summary>
        private WeaponBehaviour[] weapons;
        
        /// <summary>
        /// Currently equipped WeaponBehaviour.
        /// </summary>
        private WeaponBehaviour equipped;
        /// <summary>
        /// Currently equipped index.
        /// </summary>
        private int equippedIndex = -1;

        private int currentSlotIndex = 1;


        /// <summary>
        /// 主武器
        /// </summary>
        private WeaponBehaviour MainWeapon;
        /// <summary>
        /// 副手武器
        /// </summary>
        private WeaponBehaviour SecWeapon;

        [SerializeField]
        private InventoryState inventoryState;

        [SerializeField] private int InventorySlotMaxNum = 2;
        enum InventoryState
        {
            MainWeapon,
            SecWeapon,
            item,
        }
        
        #endregion
        
        #region METHODS
        
        public override void Init(String DepolyMainWeapon ,String DepolySecWeapon)
        {
            weapons = GetComponentsInChildren<WeaponBehaviour>(true);
            foreach (WeaponBehaviour weapon in weapons)
                weapon.gameObject.SetActive(false);
            if (photonView.IsMine)
            {
                //设置初始主武器和副武器
                foreach (WeaponBehaviour weapon in weapons)
                {
                    if (weapon.GetWeaponName().Equals(DepolyMainWeapon))
                    {
                        MainWeapon = weapon;
                        equipped = MainWeapon;
                    }else if (weapon.GetWeaponName().Equals(DepolySecWeapon))
                    {
                        SecWeapon = weapon;
                    }
                }

                inventoryState = InventoryState.MainWeapon;
                currentSlotIndex = 1;
                Equip(GetIndexByWeaponBehaviour(MainWeapon));
            }
        }

        public override WeaponBehaviour Equip(int index)
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
            //更新玩家拥有武器属性
            UpdatePlayerProperties(index , equipped);
            //Return.
            return equipped;
        }
        
        #endregion

        #region Getters

        
        public override int ChangeWeapon(int index)
        {
            if (index == -1)
            {
                currentSlotIndex--;
                if (currentSlotIndex <= 0)
                {
                    currentSlotIndex = InventorySlotMaxNum;
                }
                index = currentSlotIndex;
            }
            
            if (index == 0)
            {
                currentSlotIndex++;
                if (currentSlotIndex > InventorySlotMaxNum)
                {
                    currentSlotIndex = 1;
                }
                index = currentSlotIndex;
            }

            switch (index)
            {
                case 1:
                    currentSlotIndex = 1;
                    inventoryState = InventoryState.MainWeapon;
                    return GetIndexByWeaponBehaviour(MainWeapon);
                case 2:
                    currentSlotIndex = 2;
                    inventoryState = InventoryState.SecWeapon;
                    return GetIndexByWeaponBehaviour(SecWeapon);
                case 3:
                    currentSlotIndex = 3;
                    inventoryState = InventoryState.item;
                    
                    break;
                case 4:
                    currentSlotIndex = 4;
                    break;
            }
            return GetIndexByWeaponBehaviour(MainWeapon);
        }


        public override WeaponBehaviour GetEquipped() => equipped;
        public override WeaponBehaviour GetMainWeapon() => MainWeapon;
        public override WeaponBehaviour GetSecWeapon() => SecWeapon;

        public override int GetEquippedIndex() => equippedIndex;

        #endregion

        #region Tools

        private void UpdatePlayerProperties(int index , WeaponBehaviour weaponBehaviour)
        {
            if (photonView.IsMine)
            {
                Hashtable hash = new Hashtable();
                hash.Add("EquipWeaponIndex",index);
                hash.Add(EnumTools.PlayerProperties.CurrentWeaponID.ToString(),weaponBehaviour.GetWeaponID());
                PhotonNetwork.SetPlayerCustomProperties(hash);
            }
        }
        
        private int GetIndexByWeaponBehaviour(WeaponBehaviour weaponBehaviour)
        {
            int i = 0;
            foreach (WeaponBehaviour weapon in weapons)
            {
                if (weapon.GetWeaponName().Equals(weaponBehaviour.GetWeaponName()))
                {
                    return i;
                }

                i++;
            }

            return -1;
        }

        #endregion
    }
}