//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Laser Abstract Class.
    /// </summary>
    public abstract class LaserBehaviour : MonoBehaviour
    {
        #region GETTERS

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