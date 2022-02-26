using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Launcher : MonoBehaviourPunCallbacks
{
    private bool connectedToMaster;
    private bool joinedRoom;


    private void Awake()
    {
        ConnectToMaster();
    }


    public void ConnectToMaster()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = "Alpha";
    }


    public void CreateRoom(string _roomName, byte _playerCount, int _password, string _mapName)
    {
        if (!connectedToMaster || joinedRoom) return;

        Hashtable tmp_RoomProperties = new Hashtable {{"psw", _password}, {"Map", _mapName}};
        PhotonNetwork.CreateRoom(_roomName,
            new RoomOptions
            {
                MaxPlayers = _playerCount,
                PublishUserId = true,
                CustomRoomProperties = tmp_RoomProperties,
                CustomRoomPropertiesForLobby = new[] {"Map"}
            },
            TypedLobby.Default);
    }


    public void JoinRoom()
    {
        if (!connectedToMaster || joinedRoom) return;
        //PhotonNetwork.JoinRoom(RoomName.text);
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        connectedToMaster = true;
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("Joined Lobby");
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        joinedRoom = true;
    }


    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined Room");
        PhotonNetwork.LoadLevel(PhotonNetwork.CurrentRoom.CustomProperties["Map"].ToString());

//        StartSpawn(0);
//        Player.Respawn += StartSpawn;
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
    }
}