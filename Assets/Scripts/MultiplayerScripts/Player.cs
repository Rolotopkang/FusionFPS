using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityTemplateProjects.MultiplayerScripts;

[RequireComponent(typeof(PhotonView))]
public class Player : MonoBehaviour, IDamager
{
    public int Heath;
    private PhotonView photonView;
    private GameObject globalCamera;

    public static event Action<float> Respawn;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            globalCamera = GameObject.FindWithTag("GlobalCamera");
            if (globalCamera)
                globalCamera.SetActive(false);
        }
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
            //gameObject.SetActive(false);
            PhotonNetwork.Destroy(this.gameObject);
            if (globalCamera)
                globalCamera.SetActive(true);

            Respawn?.Invoke(3);

            return;
        }

        Heath -= _damage;
    }


    private bool IsDeath()
    {
        return Heath <= 0;
    }
}