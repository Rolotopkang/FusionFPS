//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;
using UnityTemplateProjects.Weapon;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Grip Abstract Class.
    /// </summary>
    public abstract class GripBehaviour : MonoBehaviour
    {
        #region GETTERS

        public abstract WeaponAttachmentData GetWeaponAttachmentData();
        /// <summary>
        /// 获取ID
        /// </summary>
        /// <returns></returns>
        public abstract int GetID();
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