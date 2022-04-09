using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Playables;

public class Battle : MonoBehaviourPun,IPunObservable
{
    [SerializeField]
    private float MaxHealth;


    private float health;


    private void Awake()
    {
        health = MaxHealth;
    }

    public bool Damage(float dmg)
    {
        health -= dmg;
        
        
        if (health <= 0)
        {
            return true;
        }

        return false;
    }

    #region Getter

    public float GetCurrentHealth() => health;
    public float GetMaxHealth() => MaxHealth;
    
    #endregion

    
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            if (PhotonNetwork.LocalPlayer.Equals(photonView.Owner))
            {
                stream.SendNext(health);
            }
        }
        else
        {
            if (!PhotonNetwork.LocalPlayer.Equals(photonView.Owner))
            {
                health = (float)stream.ReceiveNext();
            }
        }
    }
}
