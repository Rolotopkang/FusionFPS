using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEditor;
using UnityEngine;

public class TP_Synchronization : MonoBehaviourPun
{
    private TPInventory TpInventory;
    private TPWeaponController currentWeapon;

    private void Awake()
    {
        TpInventory = GetComponentInChildren<TPInventory>();
    }

    private void Update()
    {
        currentWeapon = TpInventory.GetEquipped();
    }

    public void Shoot()
    {
        //除了本地以外执行第三人称射击
        photonView.RPC("RPC_Shoot", RpcTarget.Others);
    }

    public void InstantiateProjectile(Vector3 transform,Quaternion rotation,float projectileImpulse)
    {
        photonView.RPC("RPC_InstantiateProjectile", RpcTarget.Others,transform,rotation,projectileImpulse);
    }

    public void Reload()
    {
        //除了本地以外执行第三人称换弹
        photonView.RPC("RPC_Reload", RpcTarget.Others);
    }

    public void ReloadOutOf()
    {
        //除了本地以外执行第三人称换弹
        photonView.RPC("RPC_ReloadOutOf", RpcTarget.Others);
    }

    public void ReloadOpen()
    {
        photonView.RPC("RPC_ReloadOpen",RpcTarget.Others);
    }

    [PunRPC]
    private void RPC_Shoot()
    {
        currentWeapon.Shoot();
    }

    [PunRPC]
    private void RPC_Reload()
    {
        currentWeapon.Reload();
    }

    [PunRPC]
    private void RPC_ReloadOutOf()
    {
        currentWeapon.ReloadOutOf();
    }

    [PunRPC]
    private void RPC_ReloadOpen()
    {
        currentWeapon.ReloadOpen();
    }
    [PunRPC]
    private void RPC_InstantiateProjectile(Vector3 transform,Quaternion rotation,float projectileImpulse)
    {
        currentWeapon.InstantiateProjectile(transform,rotation,projectileImpulse);
    }

}
