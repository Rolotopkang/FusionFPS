//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Weapon Attachment Manager Behaviour.
    /// </summary>
    public abstract class WeaponAttachmentManagerBehaviour : MonoBehaviour
    {
        #region UNITY FUNCTIONS

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

        #region Funtions

        public abstract void Init(WeaponAttachmentManager.AttachmentIndexs attachmentIndexs);   

        public abstract void ScopeChangeTo(int id);
        public abstract void MuzzleChangeTo(int id);
        public abstract void MagazineChangeTo(int id);
        public abstract void GripChangeTo(int id);

        #endregion
        
        #region GETTERS

        /// <summary>
        /// Returns the equipped scope.
        /// </summary>
        public abstract ScopeBehaviour GetEquippedScope();
        /// <summary>
        /// Returns the equipped scope default.
        /// </summary>
        public abstract ScopeBehaviour GetEquippedScopeDefault();
        
        /// <summary>
        /// Returns the equipped magazine.
        /// </summary>
        public abstract MagazineBehaviour GetEquippedMagazine();
        /// <summary>
        /// Returns the equipped muzzle.
        /// </summary>
        public abstract MuzzleBehaviour GetEquippedMuzzle();
        /// <summary>
        /// Returns the equipped grip.
        /// </summary>
        public abstract GripBehaviour GetEquippedGrip();

        public abstract WeaponAttachmentManager.AttachmentIndexs GetCurrentAttachmentIndexs();

        public abstract float GetDamageAlpha();
        
        public abstract float GetShootSpeedAlpha();
        public abstract float GetFlySpeedAlpha();
        public abstract float GetVerticalRecoilAlpha();
        public abstract float GetHorizentalRecoilAlpha();
        public abstract float GetADSSpreadAlpha();
        public abstract float GetHipShotSpreadAlpha();
        public abstract float GetMovSpreadAlpha();
        public abstract float GetT_ADSTimeAlpha();
        public abstract float GetT_SwitchGunAlpha();
        public abstract float GetT_ReloadAlpha();


        #endregion
    }
}