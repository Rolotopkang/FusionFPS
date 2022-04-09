using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Scripts.Weapon;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Rendering;

public class TPWeaponController : MonoBehaviourPun
{
    [SerializeField]
    private String WeaponName;
    
    public RuntimeAnimatorController RuntimeAnimatorController;
    
   
    
    
    // public AudioSource FirearmsShootingAudioSource;
    // public AudioSource FirearmsReloadAudioSource;
    // public FirearmsAudioData FirearmsAudioData;
    
    
    [Tooltip("Casing Prefab.")]
    [SerializeField]
    private GameObject prefabCasing;
    
    public GameObject ProjectilePrefab;

    
    [SerializeField]
    private Animator TPAnimator;
    [Tooltip("Transform that represents the weapon's ejection port, meaning the part of the weapon that casings shoot from.")]
    [SerializeField]
    private Transform socketEjection;
    private Animator GunAnimator;
    private AnimatorStateInfo TPAnimatorStateInfo;
    private bool IsReloading = false;
    private IEnumerator reloadAmmoCheckerCoroutine;

    
    private int layerOverlay;
    private int layerAction;
    private int layerBase;
    
    private void Awake()
    {
        GunAnimator = GetComponent<Animator>();
        layerOverlay = TPAnimator.GetLayerIndex("Layer Overlay");
        layerAction = TPAnimator.GetLayerIndex("Layer Actions");
        layerBase = GunAnimator.GetLayerIndex("Layer Base");
    }

    public void Shoot()
    {
        //特效播放
        EjectCasing();

        //动画播放
        //Play the firing animation.
        const string stateName = "Fire";
        TPAnimator.Play(stateName, layerOverlay, 0.0f);
        GunAnimator.Play(stateName, layerBase, 0.0f);
        
        // //声音？
    }
    
    public void EjectCasing()
    {
        //Spawn casing prefab at spawn point.
        if(prefabCasing != null && socketEjection != null)
           Instantiate(prefabCasing, socketEjection.position, socketEjection.rotation);
    }
    
    // 远端生成子弹
    // 非本地
    public void InstantiateProjectile(Vector3 transform,Quaternion rotation,float projectileImpulse)
    {
        GameObject projectile =Instantiate(ProjectilePrefab, transform, rotation);
        //Add velocity to the projectile.
        projectile.GetComponent<InfimaGames.LowPolyShooterPack.Legacy.Projectile>().SetOwner(photonView.Owner);
        projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * projectileImpulse;  
    }

    public void Reload()
    {
        const string stateName = "Reload";
        TPAnimator.Play(stateName,layerAction,0);
        GunAnimator.Play(stateName,layerBase,0);
    }
    
    public void ReloadOutOf()
    {
        const string stateName = "Reload Empty";
        TPAnimator.Play(stateName,layerAction,0);
        GunAnimator.Play(stateName,layerBase,0);
    }

    public void ReloadOpen()
    {
        const string stateName = "Reload Open";
        TPAnimator.Play(stateName,layerAction,0);
        GunAnimator.Play(stateName,layerBase,0);
    }

    public String GetWeaponName() => WeaponName;

}
