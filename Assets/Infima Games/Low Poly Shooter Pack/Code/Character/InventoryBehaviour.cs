//Copyright 2022, Infima Games. All Rights Reserved.

using System;
using Photon.Pun;
using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Abstract Inventory Class. Helpful so you can implement your own inventory system!
    /// </summary>
    public abstract class InventoryBehaviour : MonoBehaviourPun
    {
        #region GETTERS

        /// <summary>
        /// 切换主副手武器
        /// </summary>
        /// <returns></returns>
        public abstract int ChangeWeapon();
        
        /// <summary>
        /// Returns the currently equipped WeaponBehaviour.
        /// </summary>
        public abstract WeaponBehaviour GetEquipped();

        public abstract WeaponBehaviour GetMainWeapon();
        
        public abstract WeaponBehaviour GetSecWeapon();

        /// <summary>
        /// Returns the currently equipped index. Meaning the index in the weapon array of the equipped weapon.
        /// </summary>
        public abstract int GetEquippedIndex();
        
        #endregion
        
        #region METHODS

        /// <summary>
        /// Init. This function is called when the game starts. We don't use Awake or Start because we need the
        /// PlayerCharacter component to run this with the index it wants to equip!
        /// </summary>
        /// <param name="equippedAtStart">Inventory index of the weapon we want to equip when the game starts.</param>
        public abstract void Init(String DepolyMainWeapon ,String DepolySecWeapon);
        
        /// <summary>
        /// Equips a Weapon.
        /// </summary>
        /// <param name="index">Index of the weapon to equip.</param>
        /// <returns>Weapon that was just equipped.</returns>
        public abstract WeaponBehaviour Equip(int index);

        #endregion
    }
}