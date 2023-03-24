//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;
using UnityTemplateProjects.Weapon;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Grip.
    /// </summary>
    public class Grip : GripBehaviour
    {
        #region FIELDS SERIALIZED

        
        [Header("ID")] [SerializeField] 
        private WeaponAttachmentData WeaponAttachmentData;

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

        public override WeaponAttachmentData GetWeaponAttachmentData() => WeaponAttachmentData;


        public override int GetID() => WeaponAttachmentData.AttachmentID;

        public override Sprite GetSprite() => WeaponAttachmentData.sprite;

        public override Sprite GetBTNSpriteB() => WeaponAttachmentData.BTNspriteB;
        public override Sprite GetBTNSpriteD() => WeaponAttachmentData.BTNspriteD;

        #endregion
    }
}