//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;
using UnityTemplateProjects.Weapon;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Magazine.
    /// </summary>
    public class Magazine : MagazineBehaviour
    {
        #region FIELDS SERIALIZED

        [Header("Settings")]
        
        [Header("ID")] [SerializeField] 
        private WeaponAttachmentData WeaponAttachmentData;
        
        [Tooltip("Total Ammunition.")]
        [SerializeField]
        private int ammunitionTotal = 10;
        

        [Header("Interface")]

        [Tooltip("Interface Sprite.")]
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
        public override string GetName() => WeaponAttachmentData.AttachmentName;
        public override int GetID() => WeaponAttachmentData.AttachmentID;
        
        public override int GetAmmunitionTotal() => ammunitionTotal;
        
        public override Sprite GetSprite() => WeaponAttachmentData.sprite;

        public override Sprite GetBTNSpriteB() => WeaponAttachmentData.BTNspriteB;
        public override Sprite GetBTNSpriteD() => WeaponAttachmentData.BTNspriteD;


        
        #endregion
    }
}