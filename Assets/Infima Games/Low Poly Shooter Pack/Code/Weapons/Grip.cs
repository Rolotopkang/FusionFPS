//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Grip.
    /// </summary>
    public class Grip : GripBehaviour
    {
        #region FIELDS SERIALIZED

        [Header("ID")]
        [SerializeField]
        private int GripID;

        [Header("后坐力减小系数")] 
        [SerializeField]
        [Range(0, 1)]
        private float RecoilCoefficient = 1;
        
        [Header("Settings")]

        [Tooltip("Sprite. Displayed on the player's interface.")]
        [SerializeField]
        private Sprite sprite;
        
        [Tooltip("Sprite. Displayed on the player's interface.")]
        [SerializeField]
        private Sprite BTNspriteB;
        
        [Tooltip("Sprite. Displayed on the player's interface.")]
        [SerializeField]
        private Sprite BTNspriteD;

        #endregion

        #region GETTERS

        public override Sprite GetSprite() => sprite;
        public override int GetID() => GripID;
        public override float GetRecoilCoefficient() => RecoilCoefficient;
        public override Sprite GetBTNSpriteB() => BTNspriteB;
        public override Sprite GetBTNSpriteD() => BTNspriteD;

        #endregion
    }
}