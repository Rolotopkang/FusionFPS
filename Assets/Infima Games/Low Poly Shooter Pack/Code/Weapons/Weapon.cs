//Copyright 2022, Infima Games. All Rights Reserved.

using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using UnityTemplateProjects.Tools;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Weapon. This class handles most of the things that weapons need.
    /// </summary>
    public class Weapon : WeaponBehaviour
    {
        #region FIELDS SERIALIZED

        [Header("Settings")]
        
        [SerializeField]
        private int weaponID;
        
        [Tooltip("Weapon Name. Currently not used for anything, but in the future, we will use this for pickups!")]
        [SerializeField] 
        private string weaponName;
        
        [SerializeField]
        private string weaponShowName;

        [Tooltip("How much the character's movement speed is multiplied by when wielding this weapon.")]
        [SerializeField]
        private float multiplierMovementSpeed = 1.0f;
        
        [Header("Firing")]

        [Tooltip("Is this weapon automatic? If yes, then holding down the firing button will continuously fire.")]
        [SerializeField] 
        private bool automatic;
        
        [Tooltip("Is this weapon bolt-action? If yes, then a bolt-action animation will play after every shot.")]
        [SerializeField]
        private bool boltAction;

        [Tooltip("Amount of shots fired at once. Helpful for things like shotguns, where there are multiple projectiles fired at once.")]
        [SerializeField]
        private int shotCount = 1;

        [Tooltip("一次装弹装多少")]
        [SerializeField]
        private int ReloadCount = 0;
        
        [Tooltip("How far the weapon can fire from the center of the screen.")]
        [SerializeField]
        private float spread = 0.25f;
        
        [Tooltip("散布的移动惩罚系数")]
        public float spreadSpeedTimer;

        [Tooltip("首颗子弹是否有散射保护")]
        public bool firstBulletAcc;

        [SerializeField]
        [Tooltip("后坐力重置时间")]
        public float recoilReturnTime;
        
        [Header("枪械属性")]
        [Tooltip("How fast the projectiles are.")]
        [SerializeField]
        private float projectileImpulse = 400.0f;

        [Tooltip("枪械伤害")]
        [SerializeField]
        private float DMG = 10f;
        
        [Tooltip("Amount of shots this weapon can shoot in a minute. It determines how fast the weapon shoots.")]
        [SerializeField] 
        private int roundsPerMinutes = 200;

        [Tooltip("武器初始携带弹药量")]
        [SerializeField]
        private int MaxAmmunition;

        [Tooltip("Mask of things recognized when firing.")]
        [SerializeField]
        private LayerMask mask;

        [Tooltip("Maximum distance at which this weapon can fire accurately. Shots beyond this distance will not use linetracing for accuracy.")]
        [SerializeField]
        private float maximumDistance = 500.0f;

        [Header("Reloading")]
        
        [Tooltip("Determines if this weapon reloads in cycles, meaning that it inserts one bullet at a time, or not.")]
        [SerializeField]
        private bool cycledReload;
        
        [Tooltip("Determines if the player can reload this weapon when it is full of ammunition.")]
        [SerializeField]
        private bool canReloadWhenFull = true;

        [Tooltip("Should this weapon be reloaded automatically after firing its last shot?")]
        [SerializeField]
        private bool automaticReloadOnEmpty;

        [Tooltip("Time after the last shot at which a reload will automatically start.")]
        [SerializeField]
        private float automaticReloadOnEmptyDelay = 0.25f;

        [Header("Animation")]

        [Tooltip("Transform that represents the weapon's ejection port, meaning the part of the weapon that casings shoot from.")]
        [SerializeField]
        private Transform socketEjection;

        [Tooltip("Weapon Bone Offsets.")]
        [SerializeField]
        private Offsets weaponOffsets;

        [Tooltip("Sway smoothing value. Makes the weapon sway smoother.")]
        [SerializeField]
        private float swaySmoothValue = 10.0f;
        
        [Tooltip("Character arms sway when wielding this weapon.")]
        [SerializeField]
        private Sway sway;

        [Header("Resources")]

        [Tooltip("Casing Prefab.")]
        [SerializeField]
        private GameObject prefabCasing;
        
        [Tooltip("Projectile Prefab. This is the prefab spawned when the weapon shoots.")]
        [SerializeField]
        private GameObject prefabProjectile;
        
        [Tooltip("The AnimatorController a player character needs to use while wielding this weapon.")]
        [SerializeField] 
        public RuntimeAnimatorController controller;

        private TP_Synchronization tpSynchronization;

        [Tooltip("Weapon Body Texture.")]
        [SerializeField]
        private Sprite spriteBody;
        
        [Header("Audio Clips Holster")]

        [Tooltip("Holster Audio Clip.")]
        [SerializeField]
        private AudioClip audioClipHolster;

        [Tooltip("Unholster Audio Clip.")]
        [SerializeField]
        private AudioClip audioClipUnholster;
        
        [Header("Audio Clips Reloads")]

        [Tooltip("Reload Audio Clip.")]
        [SerializeField]
        private AudioClip audioClipReload;
        
        [Tooltip("Reload Empty Audio Clip.")]
        [SerializeField]
        private AudioClip audioClipReloadEmpty;
        
        [Header("Audio Clips Reloads Cycled")]
        
        [Tooltip("Reload Open Audio Clip.")]
        [SerializeField]
        private AudioClip audioClipReloadOpen;
        
        [Tooltip("Reload Insert Audio Clip.")]
        [SerializeField]
        private AudioClip audioClipReloadInsert;
        
        [Tooltip("Reload Close Audio Clip.")]
        [SerializeField]
        private AudioClip audioClipReloadClose;
        
        [Header("Audio Clips Other")]

        [Tooltip("AudioClip played when this weapon is fired without any ammunition.")]
        [SerializeField]
        private AudioClip audioClipFireEmpty;
        
        [Tooltip("")]
        [SerializeField]
        private AudioClip audioClipBoltAction;

        #endregion

        #region FIELDS

        /// <summary>
        /// Weapon Animator.
        /// </summary>
        private Animator animator;
        /// <summary>
        /// Attachment Manager.
        /// </summary>
        private WeaponAttachmentManagerBehaviour attachmentManager;

        /// <summary>
        /// Amount of ammunition left.
        /// </summary>
        private int ammunitionCurrent;

        #region Attachment Behaviours
        
        /// <summary>
        /// Equipped scope Reference.
        /// </summary>
        private ScopeBehaviour scopeBehaviour;
        
        /// <summary>
        /// Equipped Magazine Reference.
        /// </summary>
        private MagazineBehaviour magazineBehaviour;
        /// <summary>
        /// Equipped Muzzle Reference.
        /// </summary>
        private MuzzleBehaviour muzzleBehaviour;
        /// <summary>
        /// Equipped Grip Reference.
        /// </summary>
        private GripBehaviour gripBehaviour;

        
        private int LeftAmmunition;

        #endregion

        /// <summary>
        /// The GameModeService used in this game!
        /// </summary>
        private IGameModeService gameModeService;
        /// <summary>
        /// The main player character behaviour component.
        /// </summary>
        private CharacterBehaviour characterBehaviour;

        private RagdollController RagdollController;

        /// <summary>
        /// The player character's camera.
        /// </summary>
        private Transform playerCamera;

        private Collider[] Colliders;
        
        #endregion

        #region UNITY
        
        protected override void Awake()
        {
            //Get Animator.
            animator = GetComponent<Animator>();
            //Get Attachment Manager.
            attachmentManager = GetComponent<WeaponAttachmentManagerBehaviour>();
            tpSynchronization = GetComponentInParent<TP_Synchronization>();
            //Cache the game mode service. We only need this right here, but we'll cache it in case we ever need it again.
            gameModeService = ServiceLocator.Current.Get<IGameModeService>();
            //Cache the player character.
            characterBehaviour = GetComponentInParent<CharacterBehaviour>();
            //Cache the world camera. We use this in line traces.
            playerCamera = characterBehaviour.GetCameraWorld().transform;

            //从SO读取枪械信息
            WeaponData weaponData = ServiceLocator.Current.Get<IWeaponInfoService>().GetWeaponInfoFromID(weaponID);
            weaponName = weaponData.weaponName;
            weaponShowName = weaponData.weaponShowName;
            multiplierMovementSpeed = weaponData.multiplierMovementSpeed;
            automatic = weaponData.automatic;
            boltAction = weaponData.boltAction;
            shotCount = weaponData.shotCount;
            spread = weaponData.spread;
            spreadSpeedTimer = weaponData.spreadSpeedTimer;
            firstBulletAcc = weaponData.firstBulletAcc;
            recoilReturnTime = weaponData.recoilReturnTime;
            projectileImpulse = weaponData.projectileImpulse;
            DMG = weaponData.DMG;
            roundsPerMinutes = weaponData.roundsPerMinutes;
            MaxAmmunition = weaponData.MaxAmmunition;
            maximumDistance = weaponData.maximumDistance;
            cycledReload = weaponData.cycledReload;
            canReloadWhenFull = weaponData.canReloadWhenFull;
            automaticReloadOnEmpty = weaponData.automaticReloadOnEmpty;
            automaticReloadOnEmptyDelay = weaponData.automaticReloadOnEmptyDelay;
            weaponOffsets = weaponData.weaponOffsets;
            swaySmoothValue = weaponData.swaySmoothValue;
            sway = weaponData.sway;
            prefabCasing = weaponData.prefabCasing;
            prefabProjectile = weaponData.prefabProjectile;
            controller = weaponData.controller;

            spriteBody = weaponData.spriteBody;
            audioClipHolster = weaponData.audioClipHolster;
            audioClipUnholster = weaponData.audioClipUnholster;
            audioClipReload = weaponData.audioClipReload;
            audioClipFireEmpty = weaponData.audioClipFireEmpty;
            audioClipReloadEmpty = weaponData.audioClipReloadEmpty;
            audioClipReloadOpen = weaponData.audioClipReloadOpen;
            audioClipReloadInsert = weaponData.audioClipReloadInsert;
            audioClipReloadClose = weaponData.audioClipReloadClose;
            audioClipBoltAction = weaponData.audioClipBoltAction;
            LeftAmmunition = MaxAmmunition;
        }
        protected override void Start()
        {
            #region Cache Attachment References

            //Get Scope.
            scopeBehaviour = attachmentManager.GetEquippedScope();
            
            //Get Magazine.
            magazineBehaviour = attachmentManager.GetEquippedMagazine();
            //Get Muzzle.
            muzzleBehaviour = attachmentManager.GetEquippedMuzzle();
            //Get Grip.
            gripBehaviour = attachmentManager.GetEquippedGrip();

            #endregion

            //Max Out Ammo.
            ammunitionCurrent = magazineBehaviour.GetAmmunitionTotal();

            Colliders = characterBehaviour.gameObject.GetComponentInChildren<RagdollController>().GetColliders;
        }

        #endregion

        #region GETTERS

        
        public override Offsets GetWeaponOffsets() => weaponOffsets;
        
        public override float GetFieldOfViewMultiplierAim()
        {
            //Make sure we don't have any issues even with a broken setup!
            if (scopeBehaviour != null) 
                return scopeBehaviour.GetFieldOfViewMultiplierAim();
            
            //Error.
            Debug.LogError("Weapon has no scope equipped!");
  
            //Return.
            return 1.0f;
        }
        public override float GetFieldOfViewMultiplierAimWeapon()
        {
            //Make sure we don't have any issues even with a broken setup!
            if (scopeBehaviour != null) 
                return scopeBehaviour.GetFieldOfViewMultiplierAimWeapon();
            
            //Error.
            Debug.LogError("Weapon has no scope equipped!");
  
            //Return.
            return 1.0f;
        }

        public override Animator GetAnimator() => animator;

        public override int GetWeaponID() => weaponID;

        public override Sprite GetSpriteBody() => spriteBody;
        public override float GetMultiplierMovementSpeed() => multiplierMovementSpeed;

        public override AudioClip GetAudioClipHolster() => audioClipHolster;
        public override AudioClip GetAudioClipUnholster() => audioClipUnholster;

        public override AudioClip GetAudioClipReload() => audioClipReload;
        public override AudioClip GetAudioClipReloadEmpty() => audioClipReloadEmpty;
        
        public override AudioClip GetAudioClipReloadOpen() => audioClipReloadOpen;
        public override AudioClip GetAudioClipReloadInsert() => audioClipReloadInsert;
        public override AudioClip GetAudioClipReloadClose() => audioClipReloadClose;

        public override AudioClip GetAudioClipFireEmpty() => audioClipFireEmpty;
        public override AudioClip GetAudioClipBoltAction() => audioClipBoltAction;
        
        public override AudioClip GetAudioClipFire() => muzzleBehaviour.GetAudioClipFire();
        
        public override int GetAmmunitionCurrent() => ammunitionCurrent;
        
        public override int GetAmmunitionTotal() =>(int)magazineBehaviour?.GetAmmunitionTotal();
        public override bool HasCycledReload() => cycledReload;

        public override bool IsAutomatic() => automatic;

        public override float GetRevoilReturnTime() => recoilReturnTime;

        public override bool IsBoltAction() => boltAction;

        public override bool GetAutomaticallyReloadOnEmpty() => automaticReloadOnEmpty;
        public override float GetAutomaticallyReloadOnEmptyDelay() => automaticReloadOnEmptyDelay;

        public override bool CanReloadWhenFull() => canReloadWhenFull;
        public override float GetRateOfFire() => roundsPerMinutes * attachmentManager.GetShootSpeedAlpha();
        
        public override bool IsFull() => ammunitionCurrent == magazineBehaviour.GetAmmunitionTotal();
        public override bool HasAmmunition() => ammunitionCurrent > 0;

        public override RuntimeAnimatorController GetAnimatorController() => controller;
        public override WeaponAttachmentManagerBehaviour GetAttachmentManager() => attachmentManager;
        
        public override Sway GetSway() => sway;
        public override float GetSwaySmoothValue() => swaySmoothValue;

        public override string GetWeaponName() => weaponName;

        public override string GetWeaponShowName() => weaponShowName;

        public override int GetLeftAmmunition() => LeftAmmunition;

        public override MagazineBehaviour GetMagazineBehaviour() => magazineBehaviour;

        public override bool CanReload() => LeftAmmunition > 0 &&  LeftAmmunition >= GetAmmunitionTotal() ;

        #endregion

        #region METHODS

        public override void Reload()
        {
            //Set Reloading Bool. This helps cycled reloads know when they need to stop cycling.
            const string boolName = "Reloading";
            animator.SetBool(boolName, true);
            
            //Play Reload Animation.
            animator.Play(cycledReload ? "Reload Open" : (HasAmmunition() ? "Reload" : "Reload Empty"), 0, 0.0f);
            
            //远程第三人称同步
            if (cycledReload)
            {
                tpSynchronization.ReloadOpen();
            }else if (HasAmmunition())
            {
                tpSynchronization.Reload();
            }else
            {
                tpSynchronization.ReloadOutOf();
            }
        }

        public override void EndReload()
        {
            const string boolName = "Reloading";
            animator.SetBool(boolName, false);
            
            SetSlideBack(0);
        }

        public override void Fire(float spreadMultiplier = 1.0f)
        {
            
            if (muzzleBehaviour == null)
                return;

            if (playerCamera == null)
                return;
            
            Transform muzzleSocket = muzzleBehaviour.GetSocket();
            const string stateName = "Fire";
            animator.Play(stateName, 0, 0.0f);
            
            //弹药减少
            ammunitionCurrent = Mathf.Clamp(ammunitionCurrent - 1, 0, magazineBehaviour.GetAmmunitionTotal());

            //当空仓时退栓
            if (ammunitionCurrent == 0)
                SetSlideBack(1);
            
            //枪口特效
            muzzleBehaviour.Effect();

            //远程第三人称同步
            tpSynchronization.Shoot();
            
            //生成子弹
            for (var i = 0; i < shotCount; i++)
            {
                //随机散布部分
                Vector3 spreadValue = Random.insideUnitSphere * 
                                      (spread * 
                                       (1 + characterBehaviour.GetSpeed() *
                                           spreadSpeedTimer * 
                                           attachmentManager.GetMovSpreadAlpha()));
                spreadValue *= spreadMultiplier;
                
                //向量转换
                spreadValue.z = 0;
                spreadValue = muzzleSocket.TransformDirection(spreadValue);
                
                //选择是否开启首枪保护
                if (characterBehaviour.GetShotfired() == 0 && firstBulletAcc && characterBehaviour.GetSpeed()<0.01f && characterBehaviour.IsAiming())
                {
                    spreadValue = Vector3.zero;
                }
                Quaternion rotation = Quaternion.Euler(playerCamera.eulerAngles + spreadValue);
                Vector3 CMposition = playerCamera.position;
                GameObject projectile = Instantiate(prefabProjectile, CMposition, rotation);
                tpSynchronization.InstantiateProjectile(CMposition,rotation,projectileImpulse * attachmentManager.GetFlySpeedAlpha());
                
                //忽略自身碰撞
                foreach (Collider collider in Colliders)
                {
                    Physics.IgnoreCollision(projectile.GetComponent<Collider>(),collider);
                }
                
                //忽略队友
                if (!RoomManager.GetInstance().isFriendlyFire())
                {
                    PhotonTeamsManager.Instance.TryGetTeamMembers(photonView.Owner.GetPhotonTeam(), out Player[] tmp_players);
                    foreach (Player player in tmp_players)
                    {
                        if (player.Equals(PhotonNetwork.LocalPlayer))
                        {
                            continue;
                        }
                        //如果死亡不忽略
                        if ((bool) player.CustomProperties[EnumTools.PlayerProperties.IsDeath.ToString()])
                        {
                            continue;
                        }

                        //如果队友有碰撞体则忽略
                        if (ServiceLocator.Current.Get<IGameModeService>().GetPlayerGameObject(player))
                        {
                            Collider[] tmp_colliders = ServiceLocator.Current.Get<IGameModeService>().GetPlayerGameObject(player).GetComponentInChildren<RagdollController>().GetColliders;
                            foreach (Collider collider in tmp_colliders)
                            {
                                Physics.IgnoreCollision(projectile.GetComponent<Collider>(),collider);
                            }
                        }
                    }
                }
                //Add velocity to the projectile.
                projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * projectileImpulse * attachmentManager.GetFlySpeedAlpha();
                projectile.GetComponent<Legacy.Projectile>().SetOwner(photonView.Owner);
                projectile.GetComponent<Legacy.Projectile>().SetWeaponDMG(DMG * attachmentManager.GetDamageAlpha());
                projectile.GetComponent<Legacy.Projectile>().SetweaponName(weaponName);
            }
        }

        public override void FillAmmunition(int amount)
        {
            //Update the value by a certain amount.
            if (LeftAmmunition >= GetAmmunitionTotal())
            {
                if (amount != 0)
                {
                    ammunitionCurrent = Mathf.Clamp(ammunitionCurrent + amount,
                        0, GetAmmunitionTotal());
                    LeftAmmunition -= amount;
                }
                else
                {
                    ammunitionCurrent = magazineBehaviour.GetAmmunitionTotal();
                    LeftAmmunition -= GetAmmunitionTotal();
                }
                
            }
            else if(LeftAmmunition>0)
            {
                if (amount != 0)
                {
                    //如果数量不足则直接停止换弹
                    if (LeftAmmunition - amount < 0)
                    {
                        characterBehaviour.AnimationEndedReload();
                    }
                    ammunitionCurrent = Mathf.Clamp(ammunitionCurrent + amount,
                        0, GetAmmunitionTotal());
                    LeftAmmunition -= amount;
                    if (LeftAmmunition <= 0)
                    {
                        characterBehaviour.AnimationEndedReload();
                    }
                }
                else
                {
                    if (LeftAmmunition + ammunitionCurrent >= GetAmmunitionTotal())
                    {
                        LeftAmmunition = LeftAmmunition + ammunitionCurrent - GetAmmunitionTotal();
                        ammunitionCurrent = GetAmmunitionTotal();
                    }
                    else
                    {
                        ammunitionCurrent = LeftAmmunition + ammunitionCurrent;
                        LeftAmmunition = 0;
                    }
                }
            }
            else
            {
                Debug.Log("停止！");
                characterBehaviour.AnimationEndedReload();
            }
        }

        public override void AmmunitionBack()
        {
            LeftAmmunition += ammunitionCurrent;
            ammunitionCurrent = 0;
        }
        
        public override void SetSlideBack(int back)
        {
            //Set the slide back bool.
            const string boolName = "Slide Back";
            animator.SetBool(boolName, back != 0);
        }

        public override void EjectCasing()
        {
            //Spawn casing prefab at spawn point.
            if(prefabCasing != null && socketEjection != null)
                Instantiate(prefabCasing, socketEjection.position, socketEjection.rotation);
        }

        public override void ChangeAttachment(ScopeChangerBTN.AttachmentKind attachmentKind , int id)
        {
            switch (attachmentKind)
            {
                case ScopeChangerBTN.AttachmentKind.Scope:
                    attachmentManager.ScopeChangeTo(id);
                    RefreshAttachment();
                    break;
                case ScopeChangerBTN.AttachmentKind.Muzzle:
                    attachmentManager.MuzzleChangeTo(id);
                    RefreshAttachment();
                    break;
                case ScopeChangerBTN.AttachmentKind.Magazine:
                    attachmentManager.MagazineChangeTo(id);
                    RefreshAttachment();
                    break;
                case ScopeChangerBTN.AttachmentKind.Grip:
                    attachmentManager.GripChangeTo(id);
                    RefreshAttachment();
                    break;
            }
        }

        public override void RefreshAttachment()
        {
            //Get Scope.
            scopeBehaviour = attachmentManager.GetEquippedScope();
            
            //Get Magazine.
            magazineBehaviour = attachmentManager.GetEquippedMagazine();
            //Get Muzzle.
            muzzleBehaviour = attachmentManager.GetEquippedMuzzle();
            //Get Grip.
            gripBehaviour = attachmentManager.GetEquippedGrip();
        }

        #endregion
    }
}