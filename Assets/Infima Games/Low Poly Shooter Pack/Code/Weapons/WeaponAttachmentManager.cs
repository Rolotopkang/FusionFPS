//Copyright 2022, Infima Games. All Rights Reserved.

using ExitGames.Client.Photon;
using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Weapon Attachment Manager. Handles equipping and storing a Weapon's Attachments.
    /// </summary>
    public class WeaponAttachmentManager : WeaponAttachmentManagerBehaviour
    {
        #region Struct

        public struct AttachmentIndexs
        {
            public int scopeIndex;
            public int muzzleIndex;
            public int gripIndex;
            public int magazineIndex;

            public AttachmentIndexs(int scopeIndex, int muzzleIndex, int gripIndex, int magazineIndex)
            {
                this.scopeIndex = scopeIndex;
                this.muzzleIndex = muzzleIndex;
                this.gripIndex = gripIndex;
                this.magazineIndex = magazineIndex;
            }
        }

        #endregion
        
        #region FIELDS SERIALIZED

        [SerializeField]
        private GameObject GO_SOCKET_Scope;
        [SerializeField]
        private GameObject GO_SOCKET_Muzzle;
        [SerializeField]
        private GameObject GO_SOCKET_Grip;
        [SerializeField]
        private GameObject GO_SOCKET_Magazine;
        
        [Header("Scope")]

        [Tooltip("Determines if the ironsights should be shown on the weapon model.")]
        [SerializeField]
        private bool scopeDefaultShow = true;
        
        [Tooltip("Default Scope!")]
        [SerializeField]
        private ScopeBehaviour scopeDefaultBehaviour;

        [Tooltip("Selected Scope Index. If you set this to a negative number, ironsights will be selected as the enabled scope.")]
        [SerializeField]
        private int scopeIndex = -1;

        [Tooltip("All possible Scope Attachments that this Weapon can use!")]
        [SerializeField]
        private ScopeBehaviour[] scopeArray;
        
        [Header("Muzzle")]

        [Tooltip("Selected Muzzle Index.")]
        [SerializeField]
        private int muzzleIndex;

        [Tooltip("All possible Muzzle Attachments that this Weapon can use!")]
        [SerializeField]
        private MuzzleBehaviour[] muzzleArray;
        
        [Header("Grip")]

        [Tooltip("Selected Grip Index.")]
        [SerializeField]
        private int gripIndex = -1;

        [Tooltip("All possible Grip Attachments that this Weapon can use!")]
        [SerializeField]
        private GripBehaviour[] gripArray;
        
        [Header("Magazine")]

        [Tooltip("Selected Magazine Index.")]
        [SerializeField]
        private int magazineIndex;
        
        [Tooltip("All possible Magazine Attachments that this Weapon can use!")]
        [SerializeField]
        private MagazineBehaviour[] magazineArray;

        [SerializeField]
        private GameObject ChangerUI;

        [SerializeField]
        private Vector3 changerUITransform;

        [SerializeField]
        private Quaternion changerUIQuaternion;
        #endregion

        #region FIELDS

        /// <summary>
        /// Equipped Scope.
        /// </summary>
        private ScopeBehaviour scopeBehaviour;
        /// <summary>
        /// Equipped Muzzle.
        /// </summary>
        private MuzzleBehaviour muzzleBehaviour;
        /// <summary>
        /// Equipped Grip.
        /// </summary>
        private GripBehaviour gripBehaviour;
        /// <summary>
        /// Equipped Magazine.
        /// </summary>
        private MagazineBehaviour magazineBehaviour;

        #endregion

        #region UNITY FUNCTIONS

        /// <summary>
        /// Awake.
        /// </summary>
        protected override void Awake()
        {
            scopeArray = GO_SOCKET_Scope.GetComponentsInChildren<ScopeBehaviour>(); 
            muzzleArray = GO_SOCKET_Muzzle.GetComponentsInChildren<MuzzleBehaviour>(); 
            gripArray = GO_SOCKET_Grip.GetComponentsInChildren<GripBehaviour>(); 
            magazineArray = GO_SOCKET_Magazine.GetComponentsInChildren<MagazineBehaviour>(); 
            
            GameObject tmp_ChangerUI = Instantiate(ChangerUI,changerUITransform,changerUIQuaternion,transform.GetChild(0).GetChild(0));
            tmp_ChangerUI.transform.localPosition = changerUITransform;
            tmp_ChangerUI.transform.localRotation = changerUIQuaternion;
            scopeBehaviour = scopeArray.SelectAndSetActive(scopeIndex);
            if (scopeBehaviour == null)
            {
                //Select Default Scope.
                scopeBehaviour = scopeDefaultBehaviour;
                //Set Active.
                scopeBehaviour.gameObject.SetActive(scopeDefaultShow);
            }
            muzzleBehaviour = muzzleArray.SelectAndSetActive(muzzleIndex);
            gripBehaviour = gripArray.SelectAndSetActive(gripIndex);
            magazineBehaviour = magazineArray.SelectAndSetActive(magazineIndex);
        }        

        #endregion

        #region Funtions

        public override void Init(AttachmentIndexs attachmentIndexs)
        {
            ScopeChangeTo(attachmentIndexs.scopeIndex);
            MuzzleChangeTo(attachmentIndexs.muzzleIndex);
            GripChangeTo(attachmentIndexs.gripIndex);
            MagazineChangeTo(attachmentIndexs.magazineIndex);
        }

        public override void ScopeChangeTo(int id)
        {
            int tmp_index = 0;
            foreach (ScopeBehaviour scope in scopeArray)
            {
                if (scope.GetID().Equals(id))
                {
                    scopeBehaviour = scope;
                    break;
                }
                tmp_index++;
            }
            scopeArray.SelectAndSetActive(tmp_index);
        }

        public override void MuzzleChangeTo(int id)
        {
            int tmp_index = 0;
            foreach (MuzzleBehaviour muzzle in muzzleArray)
            {
                if (muzzle.GetID().Equals(id))
                {
                    muzzleBehaviour = muzzle;
                    break;
                }
                tmp_index++;
            }
            muzzleArray.SelectAndSetActive(tmp_index);
        }

        public override void MagazineChangeTo(int id)
        {
            int tmp_index = 0;
            foreach (MagazineBehaviour magazine in magazineArray)
            {
                if (magazine.GetID().Equals(id))
                {
                    magazineBehaviour = magazine;
                    break;
                }
                tmp_index++;
            }
            magazineArray.SelectAndSetActive(tmp_index);
        }

        public override void GripChangeTo(int id)
        {
            int tmp_index = 0;
            foreach (GripBehaviour grip in gripArray)
            {
                if (grip.GetID().Equals(id))
                {
                    gripBehaviour = grip;
                    break;
                }
                tmp_index++;
            }
            gripArray.SelectAndSetActive(tmp_index);
        }

        #endregion
        
        #region GETTERS

        public ScopeBehaviour[] GetScopeBehaviours() => scopeArray;

        public MuzzleBehaviour[] GetMuzzleBehaviours() => muzzleArray;

        public MagazineBehaviour[] GetMagazineBehaviours() => magazineArray;

        public GripBehaviour[] GetGripBehaviours() => gripArray;
        
        
        
        public override ScopeBehaviour GetEquippedScope() => scopeBehaviour;
        public override ScopeBehaviour GetEquippedScopeDefault() => scopeDefaultBehaviour;

        public override MagazineBehaviour GetEquippedMagazine() => magazineBehaviour;
        public override MuzzleBehaviour GetEquippedMuzzle() => muzzleBehaviour;
        public override GripBehaviour GetEquippedGrip() => gripBehaviour;
        public override AttachmentIndexs GetCurrentAttachmentIndexs() =>
            new AttachmentIndexs(scopeIndex, muzzleIndex, gripIndex, magazineIndex);


        public override float GetDamageAlpha()
        {
            return scopeBehaviour.GetWeaponAttachmentData().DamageAlpha *
                   muzzleBehaviour.GetWeaponAttachmentData().DamageAlpha *
                   gripBehaviour.GetWeaponAttachmentData().DamageAlpha *
                   magazineBehaviour.GetWeaponAttachmentData().DamageAlpha;
        }

        public override float GetShootSpeedAlpha()
        {
            return scopeBehaviour.GetWeaponAttachmentData().ShootSpeedAlpha *
                   muzzleBehaviour.GetWeaponAttachmentData().ShootSpeedAlpha *
                   gripBehaviour.GetWeaponAttachmentData().ShootSpeedAlpha *
                   magazineBehaviour.GetWeaponAttachmentData().ShootSpeedAlpha;
        }

        public override float GetFlySpeedAlpha()
        {
            return scopeBehaviour.GetWeaponAttachmentData().FlySpeedAlpha *
                   muzzleBehaviour.GetWeaponAttachmentData().FlySpeedAlpha *
                   gripBehaviour.GetWeaponAttachmentData().FlySpeedAlpha *
                   magazineBehaviour.GetWeaponAttachmentData().FlySpeedAlpha;
        }

        public override float GetVerticalRecoilAlpha()
        {
            return scopeBehaviour.GetWeaponAttachmentData().VerticalRecoilAlpha *
                   muzzleBehaviour.GetWeaponAttachmentData().VerticalRecoilAlpha *
                   gripBehaviour.GetWeaponAttachmentData().VerticalRecoilAlpha *
                   magazineBehaviour.GetWeaponAttachmentData().VerticalRecoilAlpha;
        }

        public override float GetHorizentalRecoilAlpha()
        {
            return scopeBehaviour.GetWeaponAttachmentData().HorizentalRecoilAlpha *
                   muzzleBehaviour.GetWeaponAttachmentData().HorizentalRecoilAlpha *
                   gripBehaviour.GetWeaponAttachmentData().HorizentalRecoilAlpha *
                   magazineBehaviour.GetWeaponAttachmentData().HorizentalRecoilAlpha;
        }

        public override float GetADSSpreadAlpha()
        {
            return scopeBehaviour.GetWeaponAttachmentData().ADSSpreadAlpha *
                   muzzleBehaviour.GetWeaponAttachmentData().ADSSpreadAlpha *
                   gripBehaviour.GetWeaponAttachmentData().ADSSpreadAlpha *
                   magazineBehaviour.GetWeaponAttachmentData().ADSSpreadAlpha;
        }

        public override float GetHipShotSpreadAlpha()
        {
            return scopeBehaviour.GetWeaponAttachmentData().HipShotSpreadAlpha *
                   muzzleBehaviour.GetWeaponAttachmentData().HipShotSpreadAlpha *
                   gripBehaviour.GetWeaponAttachmentData().HipShotSpreadAlpha *
                   magazineBehaviour.GetWeaponAttachmentData().HipShotSpreadAlpha;
        }

        public override float GetMovSpreadAlpha()
        {
            return scopeBehaviour.GetWeaponAttachmentData().MovSpreadAlpha *
                   muzzleBehaviour.GetWeaponAttachmentData().MovSpreadAlpha *
                   gripBehaviour.GetWeaponAttachmentData().MovSpreadAlpha *
                   magazineBehaviour.GetWeaponAttachmentData().MovSpreadAlpha;
        }

        public override float GetT_ADSTimeAlpha()
        {
            return scopeBehaviour.GetWeaponAttachmentData().T_ADSTimeAlpha *
                   muzzleBehaviour.GetWeaponAttachmentData().T_ADSTimeAlpha *
                   gripBehaviour.GetWeaponAttachmentData().T_ADSTimeAlpha *
                   magazineBehaviour.GetWeaponAttachmentData().T_ADSTimeAlpha;
        }

        public override float GetT_SwitchGunAlpha()
        {
            return scopeBehaviour.GetWeaponAttachmentData().T_SwitchGunAlpha *
                   muzzleBehaviour.GetWeaponAttachmentData().T_SwitchGunAlpha *
                   gripBehaviour.GetWeaponAttachmentData().T_SwitchGunAlpha *
                   magazineBehaviour.GetWeaponAttachmentData().T_SwitchGunAlpha;
        }

        public override float GetT_ReloadAlpha()
        {
            return scopeBehaviour.GetWeaponAttachmentData().T_ReloadAlpha *
                   muzzleBehaviour.GetWeaponAttachmentData().T_ReloadAlpha *
                   gripBehaviour.GetWeaponAttachmentData().T_ReloadAlpha *
                   magazineBehaviour.GetWeaponAttachmentData().T_ReloadAlpha;
        }

        #endregion
    }
}