using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Scripts.Weapon;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Rendering;

public class TPWeaponController : MonoBehaviour
{
    public String WeaponName;
    public RuntimeAnimatorController AnimatorController;
    public List<Renderer> WeaponRenderer;
    public Rig Rig;
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
        Debug.Log(MuzzleParticle.name+"name");
        CasingParticle.Play();
        
        //动画播放
        TPAnimator.Play("Fire",  2, 0);
        
        // //声音？
        // FirearmsShootingAudioSource.clip = FirearmsAudioData.ShootingAudio;
        // FirearmsShootingAudioSource.Play();
        
    }

    public void Reload()
    {
        TPAnimator.SetTrigger("rl");
        Debug.Log("animator set!!!!");
    }
    
    public void ReloadOutOf()
    {
        TPAnimator.SetTrigger("rof");
        Debug.Log("animator set OUtof!!!!");
    }
    
    /// <summary>
    /// 切换枪械
    /// </summary>
    /// <param name="set">是否隐藏这把枪</param>
    /// <param name="isMine">是否本地</param>
    public void HideSelf(bool set , bool isMine)
    {
        if(isMine){return;}
        
        if (!set)
        {
            foreach (Renderer tpRenderer in WeaponRenderer)
            {
                tpRenderer.shadowCastingMode = ShadowCastingMode.On;
            }
            
            TPAnimator.runtimeAnimatorController = AnimatorController;
            Rig.weight = 1;
        }
        else
        {
            foreach (Renderer tpRenderer in WeaponRenderer)
            {
                tpRenderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
            }
            Rig.weight = 0;
        }
        
    }
    
}
