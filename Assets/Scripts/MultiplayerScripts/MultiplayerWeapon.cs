using System.Collections;
using System.Collections.Generic;
using Scripts.Weapon;
using UnityEngine;

public class MultiplayerWeapon : Firearms
{
    private IEnumerator reloadAmmoCheckerCoroutine;

    protected override void Shooting()
    {
        if (CurrentAmmo <= 0) return;
        if (!IsAllowShooting()) return;
        MuzzleParticle.Play();
        CurrentAmmo -= 1;
        GunAnimator.Play("Fire",  0, 0);
        CasingParticle.Play();
        LastFireTime = Time.time;
    }

    protected override void Reload()
    {
        //GunAnimator.SetLayerWeight(1, 1);
        GunAnimator.SetTrigger(CurrentAmmo > 0 ? "ReloadLeft" : "ReloadOutOf");
        
        
        if (reloadAmmoCheckerCoroutine == null)
        {
            reloadAmmoCheckerCoroutine = CheckReloadAmmoAnimationEnd();
            StartCoroutine(reloadAmmoCheckerCoroutine);
        }
        else
        {
            StopCoroutine(reloadAmmoCheckerCoroutine);
            reloadAmmoCheckerCoroutine = null;
            reloadAmmoCheckerCoroutine = CheckReloadAmmoAnimationEnd();
            StartCoroutine(reloadAmmoCheckerCoroutine);
        }
    }
    
}
