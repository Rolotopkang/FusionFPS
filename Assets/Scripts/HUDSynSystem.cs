using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using SickscoreGames.HUDNavigationSystem;
using UnityEngine;

public class HUDSynSystem : MonoBehaviour
{
    public bool isLocal;
    private PhotonView PhotonView;
    public HUDNavigationElement _hudNavigationElement;
    

    private void Start()
    {
        PhotonView = GetComponent<PhotonView>();
        _hudNavigationElement = GetComponent<HUDNavigationElement>();
        isLocal = PhotonView.Owner.Equals(PhotonNetwork.LocalPlayer);
        _hudNavigationElement.enabled = false;
    }
}
