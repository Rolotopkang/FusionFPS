//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// ItemAnimationDataBehaviour. Used as an abstract class to contain all definitions for the Recoil class.
    /// </summary>
    public abstract class ItemAnimationDataBehaviour : MonoBehaviour
    {
        #region GETTERS
        
        /// <summary>
        /// This function should return the RecoilData used for the camera.
        /// </summary>
        public abstract RecoilData GetCameraRecoilData();
        /// <summary>
        /// Returns the LeaningData needed to apply to the equipped weapon while the character is leaning.
        /// </summary>
        public abstract LeaningData GetLeaningData();



        #endregion
    }
}