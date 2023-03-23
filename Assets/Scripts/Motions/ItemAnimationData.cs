//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// ItemAnimationData. Stores all information related to the weapon-specific procedural data.
    /// </summary>
    public class ItemAnimationData : ItemAnimationDataBehaviour
    {
        #region FIELDS SERIALIZED

        [Title(label: "Leaning Data")]

        [Tooltip("LeaningData. Contains all the information on what this weapon should do while the character is leaning.")]
        [SerializeField, InLineEditor]
        private LeaningData leaningData;
        
        [Title(label: "Camera Recoil Data")]

        [Tooltip("Weapon Recoil Data Asset. Used to get some camera recoil values, usually for weapons.")]
        [SerializeField, InLineEditor]
        private RecoilData cameraRecoilData;

        #endregion
        
        #region GETTERS

        /// <summary>
        /// GetCameraRecoilData.
        /// </summary>
        public override RecoilData GetCameraRecoilData() => cameraRecoilData;


        /// <summary>
        /// GetLeaningData.
        /// </summary>
        public override LeaningData GetLeaningData() => leaningData;

        #endregion
    }   
}