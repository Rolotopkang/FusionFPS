using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace Scripts.Weapon
{
    public class AssualtRifle : Firearms
    {
        private IEnumerator reloadAmmoCheckerCoroutine;


        private FPMouseLook mouseLook;
        public GameObject _ImpactPrefab;
        public float BulletLifeTime = 5f;


        protected override void Awake()
        {
            base.Awake();
            reloadAmmoCheckerCoroutine = CheckReloadAmmoAnimationEnd();
            mouseLook = FindObjectOfType<FPMouseLook>();
        }


        protected override void Shooting()
        {
            if (CurrentAmmo <= 0) return;
            
            if (!IsAllowShooting()) return;

            if (IsReloading) return;
            
            if (WeaponManager.isChanging) return;
            
            
            MuzzleParticle.Play();
            CurrentAmmo -= 1;
            GunAnimator.Play("Fire", IsAiming ? 1 : 0, 0);
            FirearmsShootingAudioSource.clip = FirearmsAudioData.ShootingAudio;
            FirearmsShootingAudioSource.Play();
            
            //通知第三人称进行射击
            PlayerNumericalController.Shoot();
            
            CreateBullet();
            CasingParticle.Play();
            if (mouseLook)
                mouseLook.FiringForTest();
            LastFireTime = Time.time;

           
        }

        protected override void Reload()
        {
            if(CurrentAmmo.Equals(AmmoInMag)){return;}
            
            
            
            GunAnimator.SetLayerWeight(2, 1);
            GunAnimator.SetTrigger(CurrentAmmo > 0 ? "ReloadLeft" : "ReloadOutOf");

            FirearmsReloadAudioSource.clip =
                CurrentAmmo > 0
                    ? FirearmsAudioData.ReloadLeft
                    : FirearmsAudioData.ReloadOutOf;

            FirearmsReloadAudioSource.Play();

            if (CurrentAmmo > 0)
            {
                PlayerNumericalController.Reload();
            }
            else
            {
                PlayerNumericalController.ReloadOutOf();
            }
            
            
            if (reloadAmmoCheckerCoroutine == null)
            {
                reloadAmmoCheckerCoroutine = CheckReloadAmmoAnimationEnd();
                StartCoroutine(reloadAmmoCheckerCoroutine);
                IsReloading = true;
            }
            else
            {
                StopCoroutine(reloadAmmoCheckerCoroutine);
                reloadAmmoCheckerCoroutine = null;
                reloadAmmoCheckerCoroutine = CheckReloadAmmoAnimationEnd();
                StartCoroutine(reloadAmmoCheckerCoroutine);
                IsReloading = true;
            }
        }

        private void CreateBullet()
        {
            GameObject tmp_Bullet = Instantiate(BulletPrefab, MuzzlePointFront.position, MuzzlePointFront.rotation);
            tmp_Bullet.transform.eulerAngles += CalculateSpreadOffset();
            var tmp_BulletScript = tmp_Bullet.AddComponent<Bullet>();
            tmp_BulletScript.ImpactPrefab = _ImpactPrefab;
            tmp_BulletScript.BulletSpeed = 500f;
            tmp_BulletScript.BulletDMG = GunDMG;
            Destroy(tmp_Bullet,BulletLifeTime);
        }
    }
}