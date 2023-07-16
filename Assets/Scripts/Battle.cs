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
    
    private bool isDeath = false;


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
            isDeath = true;
            return true;
        }

        return false;
    }

    public bool CheckDeathDamage(float dmg)
    {
        return health - dmg <= 0;
    }

    #region Getter

    public float GetCurrentHealth() => health;

    public float GetCurrentHealthP() => health / MaxHealth;
    public float GetMaxHealth() => MaxHealth;

    public bool GetIsDeath() => isDeath;
    
    #endregion

    
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            if (PhotonNetwork.LocalPlayer.Equals(photonView.Owner))
            {
                stream.SendNext(health);
                stream.SendNext(isDeath);
            }
        }
        else
        {
            if (!PhotonNetwork.LocalPlayer.Equals(photonView.Owner))
            {
                health = (float)stream.ReceiveNext();
                isDeath = (bool)stream.ReceiveNext();
            }
        }
    }
}
