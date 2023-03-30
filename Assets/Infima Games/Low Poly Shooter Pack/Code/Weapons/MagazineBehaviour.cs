//Copyright 2022, Infima Games. All Rights Reserved.

using System;
using UnityEngine;
using UnityTemplateProjects.Weapon;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Magazine Behaviour.
    /// </summary>
    public abstract class MagazineBehaviour : MonoBehaviour
    {
        #region GETTERS
        
        
        
        public abstract WeaponAttachmentData GetWeaponAttachmentData();
        /// <summary>
        /// 获取名字
        /// </summary>
        /// <returns></returns>
        public abstract String GetName();
        /// <summary>
        /// 获取ID
        /// </summary>
        /// <returns></returns>
        public abstract int GetID();
        /// <summary>
        /// Returns The Total Ammunition.
        /// </summary>
        public abstract int GetAmmunitionTotal();
        /// <summary>
        /// Returns the Sprite used on the Character's Interface.
        /// </summary>
        public abstract Sprite GetSprite();
        /// <summary>
        /// Returns the BTNSprite used on the Character's Interface.
        /// </summary>
        public abstract Sprite GetBTNSpriteB();
        
        /// <summary>
        /// Returns the BTNSprite used on the Character's Interface.
        /// </summary>
        public abstract Sprite GetBTNSpriteD();

        #endregion
    }
}