//Copyright 2022, Infima Games. All Rights Reserved.

using System;
using Photon.Pun;
using UnityEngine;

namespace InfimaGames.LowPolyShooterPack.Interface
{
    /// <summary>
    /// Interface Element.
    /// </summary>
    public abstract class Element : MonoBehaviour
    {
        #region FIELDS


        protected GameObject playerGameObject;
        /// <summary>
        /// Game Mode Service.
        /// </summary>
        protected IGameModeService gameModeService;
        
        /// <summary>
        /// Player Character.
        /// </summary>
        protected CharacterBehaviour playerCharacter;
        /// <summary>
        /// Player Character Inventory.
        /// </summary>
        protected InventoryBehaviour playerCharacterInventory;

        /// <summary>
        /// Equipped Weapon.
        /// </summary>
        protected WeaponBehaviour equippedWeapon;


        protected Battle Battle;

        protected EventManager playerEventManager;
        
        
        #endregion

        #region UNITY

        /// <summary>
        /// Awake.
        /// </summary>
        protected virtual void Awake()
        {
            
        }

        protected virtual void Start()
        {
            //Get Game Mode Service. Very useful to get Game Mode references.
            gameModeService = ServiceLocator.Current.Get<IGameModeService>();
            playerGameObject ??=gameModeService.GetPlayerGameObject(PhotonNetwork.LocalPlayer);
            
            //获取角色战斗数据
            Battle ??= playerGameObject.GetComponent<Battle>();
            
            //获取角色事件管理器
            playerEventManager ??= playerGameObject.GetComponent<EventManager>();
            
            //Get Player Character.
            playerCharacter ??= gameModeService.GetPlayerCharacter(PhotonNetwork.LocalPlayer);
            //Get Player Character Inventory.
            playerCharacterInventory = playerCharacter.GetInventory();
        }

        /// <summary>
        /// Update.
        /// </summary>
        private void Update()
        {
            //Ignore if we don't have an Inventory.
            if (Equals(playerCharacterInventory, null))
                return;

            //Get Equipped Weapon.
            equippedWeapon = playerCharacterInventory.GetEquipped();

            //Tick.
            Tick();
        }

        #endregion

        #region METHODS

        /// <summary>
        /// Tick.
        /// </summary>
        protected virtual void Tick() {}

        #endregion
    }
}