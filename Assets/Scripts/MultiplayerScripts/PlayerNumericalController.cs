using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Scripts.Weapon;
using UnityEngine;
using UnityTemplateProjects.MultiplayerScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;

[RequireComponent(typeof(PhotonView))]
public class PlayerNumericalController : MonoBehaviourPun, IDamager,IPunObservable
{
    public int Heath;
    private GameObject globalCamera;
    public String currentWeaponName;
    public String newWeaponName;
    public TPWeaponController currentWeapon;
    private TPWeaponController newWeapon;

    public List<TPWeaponController> MultiplayerWeaponList;
    public static event Action<float> Respawn;
    
    private void Awake()
    {
        if (photonView.IsMine)
        {
            globalCamera = GameObject.FindWithTag("GlobalCamera");
            if (globalCamera)
                globalCamera.SetActive(false);
        }
        
        
        //TODO
        //改到游戏类里
        Hashtable tmp_hashtable = new Hashtable();
        tmp_hashtable.Add("Weapon","handgun_01");
        PhotonNetwork.SetPlayerCustomProperties(tmp_hashtable);
    }

    public void Shoot()
    {
        //除了本地以外执行第三人称射击
        photonView.RPC("RPC_Shoot",RpcTarget.Others);
    }

    public void Reload()
    {
        //除了本地以外执行第三人称换弹
        photonView.RPC("RPC_Reload",RpcTarget.Others);
    }

    public void ReloadOutOf()
    {
        //除了本地以外执行第三人称换弹
        photonView.RPC("RPC_ReloadOutOf",RpcTarget.Others);
    }
    
    
    private void Update()
    {
        ChangeTPWeapon();
    }

    private void ChangeTPWeapon()
    {
        newWeaponName = (String)photonView.Owner.CustomProperties["Weapon"];

        if (newWeaponName == null) { return; }

        if(newWeaponName.Equals(currentWeaponName) || newWeaponName.Equals(String.Empty)){return;}

        foreach (var weapon in MultiplayerWeaponList)
        {
            if (weapon.WeaponName.Equals(newWeaponName))
            {
                newWeapon = weapon;
            }

            if (weapon.WeaponName.Equals(currentWeaponName))
            {
                currentWeapon = weapon;
            }
        }
        
        currentWeapon.HideSelf(true,photonView.IsMine);
        currentWeapon.gameObject.SetActive(false);
        
        newWeapon.gameObject.SetActive(true);
        newWeapon.HideSelf(false,photonView.IsMine);
        
        currentWeaponName = newWeaponName;
    }
    
    public void TakeDamage(int _damage)
    {
        photonView.RpcSecure("RPC_TakeDamage", RpcTarget.All, true, _damage);
    }


    [PunRPC]
    private void RPC_TakeDamage(int _damage, PhotonMessageInfo _info)
    {
        if (IsDeath() && photonView.IsMine)
        {
            PhotonNetwork.Destroy(this.gameObject);
            if (globalCamera)
                globalCamera.SetActive(true);

            Respawn?.Invoke(3);

            return;
        }

        Heath -= _damage;
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
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }

    private bool IsDeath()
    {
        return Heath <= 0;
    }
}