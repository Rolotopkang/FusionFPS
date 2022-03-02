using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Weapon
{
    public abstract class Firearms : IWeapon
    {
        public GameObject BulletPrefab;
        public WeaponManager WeaponManager;
        public Camera EyeCamera;
        public Camera GunCamera;

        public Transform MuzzlePointFront;
        public Transform CasingPoint;

        public ParticleSystem MuzzleParticle;
        public ParticleSystem CasingParticle;


        public AudioSource FirearmsShootingAudioSource;
        public AudioSource FirearmsReloadAudioSource;
        public FirearmsAudioData FirearmsAudioData;

        // public GameObject BulletImpactPrefab;

        public float FireRate;
        public float AimTime = 3;

        public int AmmoInMag = 30;
        public int MaxAmmoCarried = 120;

        public float SpreadAngle;

        [SerializeField] internal Animator GunAnimator;

        public List<ScopeInfo> ScopeInfos;

        public ScopeInfo BaseIronSight;

        protected ScopeInfo rigoutScopeInfo;

        public int GetCurrentAmmo => CurrentAmmo;
        public int GetCurrentMaxAmmoCarried => CurrentMaxAmmoCarried;

        
        protected int CurrentAmmo;
        protected int CurrentMaxAmmoCarried;
        protected float LastFireTime;
        protected AnimatorStateInfo GunStateInfo;
        protected float EyeOriginFOV;
        protected float GunOriginFOV;
        protected bool IsAiming;
        protected bool IsReloading;
        protected bool IsHoldingTrigger;
        private IEnumerator doAimCoroutine;
        private Vector3 originalEyePosition;
        protected Transform gunCameraTransform;

        protected void Start()
        {
        }

        protected virtual void Awake()
        {
            CurrentAmmo = AmmoInMag;
            CurrentMaxAmmoCarried = MaxAmmoCarried;
            //GunAnimator = GetComponent<Animator>();
            if (EyeCamera)
                EyeOriginFOV = EyeCamera.fieldOfView;
            if (GunCamera)
            {
                GunOriginFOV = GunCamera.fieldOfView;
                doAimCoroutine = DoAim();
                gunCameraTransform = GunCamera.transform;
                originalEyePosition = gunCameraTransform.localPosition;
            }

            rigoutScopeInfo = BaseIronSight;
        }


        public override void DoAttack()
        {
            Shooting();
        }


        protected abstract void Shooting();
        protected abstract void Reload();

        //protected abstract void Aim();


        protected bool IsAllowShooting()
        {
            return Time.time - LastFireTime > 1 / FireRate;
        }


        protected Vector3 CalculateSpreadOffset()
        {
            float tmp_SpreadPercent = SpreadAngle / EyeCamera.fieldOfView;

            return tmp_SpreadPercent * UnityEngine.Random.insideUnitCircle;
        }


        protected IEnumerator CheckReloadAmmoAnimationEnd()
        {
            while (true)
            {
                yield return null;

                GunStateInfo = GunAnimator.GetCurrentAnimatorStateInfo(GunAnimator.GetLayerIndex("Reload Layer"));
                if (GunStateInfo.IsTag("ReloadAmmo"))
                {
                    if (GunStateInfo.normalizedTime >= 0.9f)
                    {
                        int tmp_NeedAmmoCount = AmmoInMag - CurrentAmmo;
                        int tmp_RemainingAmmo = CurrentMaxAmmoCarried - tmp_NeedAmmoCount;
                        if (tmp_RemainingAmmo <= 0)
                        {
                            CurrentAmmo += CurrentMaxAmmoCarried;
                        }
                        else
                        {
                            CurrentAmmo = AmmoInMag;
                        }

                        CurrentMaxAmmoCarried = tmp_RemainingAmmo <= 0 ? 0 : tmp_RemainingAmmo;
                        IsReloading = false;
                        yield break;
                    }

                    //换枪停止换弹
                    if (WeaponManager.isChanging)
                    {
                        IsReloading = false;
                        yield break;
                    }
                }
            }
        }

        protected IEnumerator DoAim()
        {
            while (true)
            {
                yield return null;

                float tmp_EyeCurrentFOV = 0;
                EyeCamera.fieldOfView =
                    Mathf.SmoothDamp(EyeCamera.fieldOfView,
                        IsAiming ? rigoutScopeInfo.EyeFov : EyeOriginFOV,
                        ref tmp_EyeCurrentFOV,
                        Time.deltaTime * AimTime);

                float tmp_GunCurrentFOV = 0;
                GunCamera.fieldOfView =
                    Mathf.SmoothDamp(GunCamera.fieldOfView,
                        IsAiming ? rigoutScopeInfo.GunFov : GunOriginFOV,
                        ref tmp_GunCurrentFOV,
                        Time.deltaTime * AimTime);

                Vector3 tmp_RefPosition = Vector3.zero;
                gunCameraTransform.localPosition = Vector3.SmoothDamp(gunCameraTransform.localPosition,
                    IsAiming ? rigoutScopeInfo.GunCameraPosition : originalEyePosition,
                    ref tmp_RefPosition,
                    Time.deltaTime * AimTime);
            }
        }

        internal void Aiming(bool _isAiming)
        {
            IsAiming = _isAiming;

            GunAnimator.SetBool("Aim", IsAiming);
            if (doAimCoroutine == null)
            {
                doAimCoroutine = DoAim();
                StartCoroutine(doAimCoroutine);
            }
            else
            {
                StopCoroutine(doAimCoroutine);
                doAimCoroutine = null;
                doAimCoroutine = DoAim();
                StartCoroutine(doAimCoroutine);
            }
        }

        internal void SetupCarriedScope(ScopeInfo _scopeInfo)
        {
            rigoutScopeInfo = _scopeInfo;
        }

        internal void HoldTrigger()
        {
            DoAttack();
            IsHoldingTrigger = true;
        }

        internal void ReleaseTrigger()
        {
            IsHoldingTrigger = false;
        }

        internal void ReloadAmmo()
        {
            Reload();
        }
    }


    [System.Serializable]
    public class ScopeInfo
    {
        public string ScopeName;
        public GameObject ScopeGameObject;
        public float EyeFov;
        public float GunFov;
        public Vector3 GunCameraPosition;
    }
}