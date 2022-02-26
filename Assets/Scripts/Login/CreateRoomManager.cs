using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using Logger = UnityEngine.Logger;

public class CreateRoomManager : MonoBehaviourPunCallbacks
{
    public InputField roomName;
    public Toggle friendlyFire;
    public Slider maxPlayer;
    public string PlayerPrefabName;
    public GameObject PlayerLocation;
    private LoginUIManager loginUIManager;

    
    private void Awake()
    {
        loginUIManager = GetComponentInParent<LoginUIManager>();
    }
    
    private void CreateRoom()
    {
        ExitGames.Client.Photon.Hashtable CustomRoomInfo = new ExitGames.Client.Photon.Hashtable();
        CustomRoomInfo.Add("pvp",friendlyFire.interactable);
        CustomRoomInfo.Add("host",PhotonNetwork.NickName);
        
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers =Convert.ToByte(maxPlayer.value),
            CustomRoomProperties = CustomRoomInfo
        };
        roomOptions.CustomRoomPropertiesForLobby = new string[] {"pvp","host"};
        PhotonNetwork.JoinOrCreateRoom(roomName.text, roomOptions, default);
    }

    public void BTNCreateRoom()
    {
        CreateRoom();
    }
    
    public override void OnJoinedRoom()
    {
        Debug.Log("Join Room!");
        PhotonNetwork.Instantiate(PlayerPrefabName,PlayerLocation.transform.position, quaternion.identity);
        loginUIManager.CloseUI();
    }
}
