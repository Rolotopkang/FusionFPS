//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Grip Abstract Class.
    /// </summary>
    public abstract class GripBehaviour : MonoBehaviour
    {
        #region GETTERS

        /// <summary>
        /// 获取ID
        /// </summary>
        /// <returns></returns>
        public abstract int GetID();

        /// <summary>
        /// 获取后坐力减小系数
        /// </summary>
        /// <returns></returns>
        public abstract float GetRecoilCoefficient();
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