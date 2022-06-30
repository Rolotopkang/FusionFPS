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
        
        [Tooltip("How far the weapon can fire from the center of the screen.")]
        [SerializeField]
        private float spread = 0.25f;

        [Header("后坐力")]
        [Tooltip("后坐力系数")] 
        [SerializeField]
        private Vector2 Recoil = new Vector2(-0.2f, 0.2f);
        
        [Tooltip("后坐力随机百分比抖动")] 
        [SerializeField]
        [Range(0, 1)]
        private float RecoilOffset = 1;
        
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
        /// Equipped Laser Reference.
        /// </summary>
        private LaserBehaviour laserBehaviour;
        /// <summary>
        /// Equipped Grip Reference.
        /// </summary>
        private GripBehaviour gripBehaviour;

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

        private CameraLook CameraLook;
        
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
            //获取后坐力组件
            CameraLook = GetComponentInParent<CameraLook>();
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

            //Get Laser.
            laserBehaviour = attachmentManager.GetEquippedLaser();
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

        public override int GetAmmunitionTotal() => magazineBehaviour.GetAmmunitionTotal();
        public override bool HasCycledReload() => cycledReload;

        public override bool IsAutomatic() => automatic;
        public override bool IsBoltAction() => boltAction;

        public override bool GetAutomaticallyReloadOnEmpty() => automaticReloadOnEmpty;
        public override float GetAutomaticallyReloadOnEmptyDelay() => automaticReloadOnEmptyDelay;

        public override bool CanReloadWhenFull() => canReloadWhenFull;
        public override float GetRateOfFire() => roundsPerMinutes;
        
        public override bool IsFull() => ammunitionCurrent == magazineBehaviour.GetAmmunitionTotal();
        public override bool HasAmmunition() => ammunitionCurrent > 0;

        public override RuntimeAnimatorController GetAnimatorController() => controller;
        public override WeaponAttachmentManagerBehaviour GetAttachmentManager() => attachmentManager;
        
        public override Sway GetSway() => sway;
        public override float GetSwaySmoothValue() => swaySmoothValue;

        public override string GetWeaponName() => weaponName;

        public override string GetWeaponShowName() => weaponShowName;

        public override MagazineBehaviour GetMagazineBehaviour() => magazineBehaviour;

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
        public override void Fire(float spreadMultiplier = 1.0f)
        {
            //We need a muzzle in order to fire this weapon!
            if (muzzleBehaviour == null)
                return;
            
            //Make sure that we have a camera cached, otherwise we don't really have the ability to perform traces.
            if (playerCamera == null)
                return;

            //Get Muzzle Socket. This is the point we fire from.
            Transform muzzleSocket = muzzleBehaviour.GetSocket();
            
            //Play the firing animation.
            const string stateName = "Fire";
            animator.Play(stateName, 0, 0.0f);
            //Reduce ammunition! We just shot, so we need to get rid of one!
            ammunitionCurrent = Mathf.Clamp(ammunitionCurrent - 1, 0, magazineBehaviour.GetAmmunitionTotal());

            //Set the slide back if we just ran out of ammunition.
            if (ammunitionCurrent == 0)
                SetSlideBack(1);
            
            //Play all muzzle effects.
            muzzleBehaviour.Effect();

            //后坐力和震屏幕
            CameraLook.StartRecoil((Recoil+GetRandomRecoil(Recoil))*
                                   gripBehaviour.GetRecoilCoefficient(),false);

            //远程第三人称同步
            tpSynchronization.Shoot();
            
            //Spawn as many projectiles as we need.
            for (var i = 0; i < shotCount; i++)
            {
                //Determine a random spread value using all of our multipliers.
                Vector3 spreadValue = Random.insideUnitSphere * (spread * spreadMultiplier);
                //Remove the forward spread component, since locally this would go inside the object we're shooting!
                spreadValue.z = 0;
                //Convert to world space.
                spreadValue = muzzleSocket.TransformDirection(spreadValue);

                Quaternion rotation = Quaternion.Euler(playerCamera.eulerAngles + spreadValue);
                GameObject projectile = Instantiate(prefabProjectile, playerCamera.position, rotation);
                tpSynchronization.InstantiateProjectile(playerCamera.position,rotation,projectileImpulse);
                
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
                        //如果死亡不忽略
                        if ((bool) player.CustomProperties[EnumTools.PlayerProperties.IsDeath])
                        {
                            continue;
                        }
                        Collider[] tmp_colliders = ServiceLocator.Current.Get<IGameModeService>().GetPlayerGameObject(player).GetComponentInChildren<RagdollController>().GetColliders;
                        foreach (Collider collider in tmp_colliders)
                        {
                            Physics.IgnoreCollision(projectile.GetComponent<Collider>(),collider);
                        }
                    }
                }
                //Add velocity to the projectile.
                projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * projectileImpulse;
                projectile.GetComponent<Legacy.Projectile>().SetOwner(photonView.Owner);
                projectile.GetComponent<Legacy.Projectile>().SetWeaponDMG(DMG);
                projectile.GetComponent<Legacy.Projectile>().SetweaponName(weaponName);
            }
        }

        public override void FillAmmunition(int amount)
        {
            //Update the value by a certain amount.
            ammunitionCurrent = amount != 0 ? Mathf.Clamp(ammunitionCurrent + amount, 
                0, GetAmmunitionTotal()) : magazineBehaviour.GetAmmunitionTotal();
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
                case ScopeChangerBTN.AttachmentKind.Laser:
                    attachmentManager.LazerChangeTo(id);
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

            //Get Laser.
            laserBehaviour = attachmentManager.GetEquippedLaser();
            //Get Grip.
            gripBehaviour = attachmentManager.GetEquippedGrip();
        }

        private Vector2 GetRandomRecoil(Vector2 original)
        {
           float tmp_X = Random.Range(-original.x, original.x) * RecoilOffset;
           float tmp_Y = Random.Range(-original.y, original.y) * RecoilOffset;
           return new Vector2(tmp_X, tmp_Y);
        }
        
        #endregion
    }
}