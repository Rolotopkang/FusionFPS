using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

public class Bounder_CheckTeam : MonoBehaviour
{
    [SerializeField]
    private EnumTools.Teams Team;
    private void Update()
    {
        if (PhotonNetwork.LocalPlayer.GetPhotonTeam() == null)
        {
            return;
        }
        if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name == Team.ToString())
        {
            Destroy(gameObject);
        }
        else
        {
            enabled = false;
        }
    }
}
