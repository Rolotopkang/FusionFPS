using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Scripts.Weapon;
using UnityEngine;
using UnityEngine.Rendering;

public class TPWeaponController : MonoBehaviour
{
    public String WeaponName;
    public RuntimeAnimatorController AnimatorController;
    public List<Renderer> WeaponRenderer;
    public GameObject WeaponRightHandIK;
    public GameObject WeaponLeftHandIK;
    public Animator TPAnimator;
    public AudioSource FirearmsShootingAudioSource;
    public AudioSource FirearmsReloadAudioSource;
    public FirearmsAudioData FirearmsAudioData;
    public ParticleSystem MuzzleParticle;
    public ParticleSystem CasingParticle;


    private AnimatorStateInfo TPAnimatorStateInfo;
    private bool IsReloading = false;
    private IEnumerator reloadAmmoCheckerCoroutine;

    public void Shoot()
    {
        Debug.Log("第三人称射击！");
        //特效播放
        MuzzleParticle.Play();
        CasingParticle.Play();
        //动画播放
        TPAnimator.Play("Fire",  0, 0);
        
        // //声音？
        // FirearmsShootingAudioSource.clip = FirearmsAudioData.ShootingAudio;
        // FirearmsShootingAudioSource.Play();
        
    }

    public void Reload()
    {
        TPAnimator.SetLayerWeight(1,1);
        TPAnimator.SetTrigger("ReloadLeft");
        Debug.Log("animator set!!!!");
        
        // if (reloadAmmoCheckerCoroutine == null)
        // {
        //     reloadAmmoCheckerCoroutine = CheckReloadAmmoAnimationEnd();
        //     StartCoroutine(reloadAmmoCheckerCoroutine);
        //     IsReloading = true;
        // }
        // else
        // {
        //     StopCoroutine(reloadAmmoCheckerCoroutine);
        //     reloadAmmoCheckerCoroutine = null;
        //     reloadAmmoCheckerCoroutine = CheckReloadAmmoAnimationEnd();
        //     StartCoroutine(reloadAmmoCheckerCoroutine);
        //     IsReloading = true;
        // }
    }


    public void ReloadOutOf()
    {
        TPAnimator.SetLayerWeight(1,1);
        TPAnimator.SetTrigger("ReloadOutOf");
        Debug.Log("animator set OUtof!!!!");
    }
    
    
    // protected IEnumerator CheckReloadAmmoAnimationEnd()
    // {
    //     while (true)
    //     {
    //         yield return null;
    //
    //         TPAnimatorStateInfo = TPAnimator.GetCurrentAnimatorStateInfo(TPAnimator.GetLayerIndex("Reload Layer"));
    //         Debug.Log(TPAnimatorStateInfo.IsTag("ReloadAmmo"));
    //         if (TPAnimatorStateInfo.IsTag("ReloadAmmo"))
    //         {
    //             if (TPAnimatorStateInfo.normalizedTime >= 0.9f)
    //             {
    //                 IsReloading = false;
    //                 yield break;
    //             }
    //         }
    //     }
    // }
    
    
    
    /// <summary>
    /// 切换枪械
    /// </summary>
    /// <param name="set">是否隐藏这把枪</param>
    /// <param name="isMine">是否本地</param>
    public void HideSelf(bool set , bool isMine)
    {
        // if(isMine){return;}
        
        if (!set)
        {
            foreach (Renderer tpRenderer in WeaponRenderer)
            {
                tpRenderer.shadowCastingMode = ShadowCastingMode.On;
            }

            Debug.Log(TPAnimator.runtimeAnimatorController.name +"old" );
            TPAnimator.runtimeAnimatorController = AnimatorController;
            Debug.Log(TPAnimator.runtimeAnimatorController.name +"new" );
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
