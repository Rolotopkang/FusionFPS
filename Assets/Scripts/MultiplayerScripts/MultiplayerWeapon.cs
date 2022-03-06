using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Scripts.Weapon;
using UnityEngine;
using UnityEngine.Rendering;

public class MultiplayerWeapon : Firearms
{
    private IEnumerator reloadAmmoCheckerCoroutine;

    public RuntimeAnimatorController AnimatorController;

    public List<Renderer> WeaponRenderer;

    public GameObject WeaponRightHandIK;
    public GameObject WeaponLeftHandIK;


    protected override void Shooting()
    {
        if (CurrentAmmo <= 0) return;
        if (!IsAllowShooting()) return;
        if (IsReloading) return;
        if (WeaponManager.isChanging) return;
        
        
        MuzzleParticle.Play();
        CurrentAmmo -= 1;
        GunAnimator.Play("Fire",  0, 0);
        CasingParticle.Play();
        LastFireTime = Time.time;
    }

    protected override void Reload()
    {
        if(CurrentAmmo.Equals(AmmoInMag)){return;}
        
        
        GunAnimator.SetLayerWeight(1, 1);
        GunAnimator.SetTrigger(CurrentAmmo > 0 ? "ReloadLeft" : "ReloadOutOf");
        
        
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

    public void HideSelf(bool set , bool isMine)
    {
        // if(isMine){return;}
        
        if (!set)
        {
            foreach (Renderer tpRenderer in WeaponRenderer)
            {
                tpRenderer.shadowCastingMode = ShadowCastingMode.On;
            }

            Debug.Log(GunAnimator.runtimeAnimatorController.name +"old" );
            GunAnimator.runtimeAnimatorController = AnimatorController;
            Debug.Log(GunAnimator.runtimeAnimatorController.name +"new" );
        }
        else
        {
            foreach (Renderer tpRenderer in WeaponRenderer)
            {
                tpRenderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
            }
        }
        
    }
}
