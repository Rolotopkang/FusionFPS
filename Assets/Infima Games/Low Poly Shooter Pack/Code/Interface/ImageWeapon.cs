﻿//Copyright 2022, Infima Games. All Rights Reserved.

using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InfimaGames.LowPolyShooterPack.Interface
{
    /// <summary>
    /// Weapon Image. Handles assigning the proper sprites to the weapon images.
    /// </summary>
    public class ImageWeapon : Element
    {
        #region FIELDS SERIALIZED

        [Header("Settings")] 
        [SerializeField] 
        private EnumTools.InventoryKind InventoryKind;
        
        [Tooltip("Weapon Body Image.")]
        [SerializeField]
        private Image imageWeaponBody;

        [Tooltip("Weapon Grip Image.")]
        [SerializeField]
        private Image imageWeaponGrip;

        [Tooltip("Weapon Silencer Image.")]
        [SerializeField]
        private Image imageWeaponMuzzle;
        
        [Tooltip("Weapon Magazine Image.")]
        [SerializeField]
        private Image imageWeaponMagazine;
        
        [Tooltip("Weapon Scope Image.")]
        [SerializeField]
        private Image imageWeaponScope;
        
        [Tooltip("Weapon Scope Default Image.")]
        [SerializeField]
        private Image imageWeaponScopeDefault;

        [SerializeField]
        private TextMeshProUGUI bulletNum;

        [SerializeField]
        private TextMeshProUGUI GunName;

        [SerializeField]
        private CanvasGroup[] CanvasGroups;

        #endregion

        #region FIELDS

        /// <summary>
        /// Weapon Attachment Manager.
        /// </summary>
        private WeaponAttachmentManagerBehaviour attachmentManagerBehaviour;

        private WeaponBehaviour MainWeapon;

        private WeaponBehaviour SecWeapon;

        private WeaponBehaviour Item;
        
        private WeaponBehaviour CurrentWeapon;
        
        
        #endregion

        #region METHODS
        

        protected override void Tick()
        {
            MainWeapon = playerCharacterInventory.GetMainWeapon();
            SecWeapon = playerCharacterInventory.GetSecWeapon();
            Item = playerCharacterInventory.GetItem();
            CurrentWeapon = playerCharacterInventory.GetEquipped();

            switch (InventoryKind)
            {
                case EnumTools.InventoryKind.Main:
                    if (CurrentWeapon.Equals(MainWeapon))
                    {
                        SetCurrentItem(0);
                    }

                    attachmentManagerBehaviour = MainWeapon.GetAttachmentManager();
                    imageWeaponBody.sprite = MainWeapon.GetSpriteBody();
                    if (CurrentWeapon.Equals(MainWeapon))
                    {
                        UpdateSprite();
                    }
                    break;
                case EnumTools.InventoryKind.Sec:
                    if (CurrentWeapon.Equals(SecWeapon))
                    {
                        SetCurrentItem(1);
                    }
                    attachmentManagerBehaviour = SecWeapon.GetAttachmentManager();
                    imageWeaponBody.sprite = SecWeapon.GetSpriteBody();
                    if (CurrentWeapon.Equals(SecWeapon))
                    {
                        UpdateSprite();
                    }
                    break;
                case EnumTools.InventoryKind.Item:
                    if (CurrentWeapon.Equals(Item))
                    {
                        SetCurrentItem(2);
                    }
                    attachmentManagerBehaviour = Item.GetAttachmentManager();
                    imageWeaponBody.sprite = Item.GetSpriteBody();
                    if (CurrentWeapon.Equals(Item))
                    {
                        UpdateSprite();
                        bulletNum.text = playerCharacter.GetGrenadesCurrent().ToString();
                    }
                    break;
            }
        }

        private void UpdateSprite()
        {
            GunName.text = CurrentWeapon.GetWeaponName();
            //Sprite.
            Sprite sprite = default;
                            //Scope Default.
            ScopeBehaviour scopeDefaultBehaviour = attachmentManagerBehaviour.GetEquippedScopeDefault();
            //Get Sprite.
            if (scopeDefaultBehaviour != null)
                sprite = scopeDefaultBehaviour.GetSprite();
            //Assign Sprite!
            AssignSprite(imageWeaponScopeDefault, sprite, scopeDefaultBehaviour == null);
            
            //Scope.
            ScopeBehaviour scopeBehaviour = attachmentManagerBehaviour.GetEquippedScope();
            //Get Sprite.
            if (scopeBehaviour != null)
                sprite = scopeBehaviour.GetSprite();
            //Assign Sprite!
            AssignSprite(imageWeaponScope, sprite, scopeBehaviour == null || scopeBehaviour == scopeDefaultBehaviour);

            //Magazine.
            MagazineBehaviour magazineBehaviour = attachmentManagerBehaviour.GetEquippedMagazine();
            //Get Sprite.
            if (magazineBehaviour != null)
                sprite = magazineBehaviour.GetSprite();
            //Assign Sprite!
            AssignSprite(imageWeaponMagazine, sprite, magazineBehaviour == null);

            //Grip.
            GripBehaviour gripBehaviour = attachmentManagerBehaviour.GetEquippedGrip();
            //Get Sprite.
            if (gripBehaviour != null)
                sprite = gripBehaviour.GetSprite();
            //Assign Sprite!
            AssignSprite(imageWeaponGrip, sprite, gripBehaviour == null);
            
            //Muzzle.
            MuzzleBehaviour muzzleBehaviour = attachmentManagerBehaviour.GetEquippedMuzzle();
            //Get Sprite.
            if (muzzleBehaviour != null)
                sprite = muzzleBehaviour.GetSprite();
            //Assign Sprite!
            AssignSprite(imageWeaponMuzzle, sprite, muzzleBehaviour == null);
            
        }
        
        /// <summary>
        /// Assigns a sprite to an image.
        /// </summary>
        private static void AssignSprite(Image image, Sprite sprite, bool forceHide = false)
        {
            //Update.
            image.sprite = sprite;
            //Disable image if needed.
            image.enabled = sprite != null && !forceHide;
        }

        private void SetCurrentItem(int index)
        {
            foreach (CanvasGroup canvasGroup in CanvasGroups)
            {
                canvasGroup.alpha = 0.4f;
            }

            CanvasGroups[index].alpha = 0.8f;
        }
        #endregion
    }
}