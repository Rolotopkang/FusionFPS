using System.Collections;
using System.Collections.Generic;
using InfimaGames.LowPolyShooterPack.Interface;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class TextWeaponInfo : Element
{
    
    [SerializeField]
    private TextMeshProUGUI PlayerName;

    protected override void Start()
    {
        PlayerName.text = PhotonNetwork.LocalPlayer.NickName;
    }
}
