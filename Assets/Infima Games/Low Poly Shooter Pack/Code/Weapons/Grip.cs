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
        
        public override Sprite GetBTNSpriteB() => BTNspriteB;
        public override Sprite GetBTNSpriteD() => BTNspriteD;

        #endregion
    }
}