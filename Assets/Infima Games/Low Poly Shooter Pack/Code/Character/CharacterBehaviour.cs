﻿//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Character Abstract Behaviour.
    /// </summary>
    public abstract class CharacterBehaviour : MonoBehaviour
    {
        #region UNITY

        /// <summary>
        /// Awake.
        /// </summary>
        protected virtual void Awake(){}

        /// <summary>
        /// Start.
        /// </summary>
        protected virtual void Start(){}

        /// <summary>
        /// Update.
        /// </summary>
        protected virtual void Update(){}

        /// <summary>
        /// Late Update.
        /// </summary>
        protected virtual void LateUpdate(){}

        #endregion
        
        #region GETTERS

        /// <summary>
        /// Returns the player character's main camera.
        /// </summary>
        public abstract Camera GetCameraWorld();
        /// <summary>
        /// Returns the player character's weapon camera.
        /// </summary>
        /// <returns></returns>
        public abstract Camera GetCameraDepth();
        
        /// <summary>
        /// Returns a reference to the Inventory component.
        /// </summary>
        public abstract InventoryBehaviour GetInventory();

        /// <summary>
        /// Returns the current amount of grenades left.
        /// </summary>
        public abstract int GetGrenadesCurrent();
        /// <summary>
        /// Returns the total amount of grenades left.
        /// </summary>
        public abstract int GetGrenadesTotal();

        /// <summary>
        /// Returns true if the Crosshair should be visible.
        /// </summary>
        public abstract bool IsCrosshairVisible();
        /// <summary>
        /// Returns true if the character is running.
        /// </summary>
        public abstract bool IsRunning();

        /// <summary>
        /// Returns true if the character is crouching.
        /// </summary>
        public abstract bool IsCrouching();
        
        /// <summary>
        /// Returns true if the character is aiming.
        /// </summary>
        public abstract bool IsAiming();

        public abstract bool IsScopeChanging();

        /// <summary>
        /// Returns true if the tutorial text should be visible on the screen.
        /// </summary>
        public abstract bool IsTutorialTextVisible();

        public abstract bool IsMine();

        /// <summary>
        /// Returns the Movement Input.
        /// </summary>
        public abstract Vector2 GetInputMovement();

        /// <summary>
        /// 返回速度abs值
        /// </summary>
        /// <returns></returns>
        public abstract float GetSpeed();
        /// <summary>
        /// Returns the Look Input.
        /// </summary>
        public abstract Vector2 GetInputLook();

        /// <summary>
        /// Returns the audio clip played when the character throws a grenade.
        /// </summary>
        public abstract AudioClip[] GetAudioClipsGrenadeThrow();
        /// <summary>
        /// Returns the audio clip played when the character melees.
        /// </summary>
        public abstract AudioClip[] GetAudioClipsMelee();
        
        public abstract int GetShotfired();
        
        #endregion

        #region ANIMATION

        /// <summary>
        /// Ejects a casing from the equipped weapon.
        /// </summary>
        public abstract void EjectCasing();
        /// <summary>
        /// Fills the character's equipped weapon's ammunition by a certain amount, or fully if set to -1.
        /// </summary>
        public abstract void FillAmmunition(int amount);

        /// <summary>
        /// Throws a grenade.
        /// </summary>
        public abstract void Grenade();
        
        /// <summary>
        /// 换配件
        /// </summary>
        /// <param name="attachmentKind">配件类型</param>
        /// <param name="ID">配件ID</param>
        public abstract void ChangeAttachment(ScopeChangerBTN.AttachmentKind attachmentKind, int ID);
        /// <summary>
        /// Sets the equipped weapon's magazine to be active or inactive!
        /// </summary>
        public abstract void SetActiveMagazine(int active);
        
        /// <summary>
        /// Bolt Animation Ended.
        /// </summary>
        public abstract void AnimationEndedBolt();
        /// <summary>
        /// Reload Animation Ended.
        /// </summary>
        public abstract void AnimationEndedReload();

        /// <summary>
        /// Grenade Throw Animation Ended.
        /// </summary>
        public abstract void AnimationEndedGrenadeThrow();
        /// <summary>
        /// Melee Animation Ended.
        /// </summary>
        public abstract void AnimationEndedMelee();

        /// <summary>
        /// Inspect Animation Ended.
        /// </summary>
        public abstract void AnimationEndedInspect();
        /// <summary>
        /// Holster Animation Ended.
        /// </summary>
        public abstract void AnimationEndedHolster();

        /// <summary>
        /// Sets the equipped weapon's slide back pose.
        /// </summary>
        public abstract void SetSlideBack(int back);

        public abstract void SetActiveKnife(int active);



        #endregion
    }
}