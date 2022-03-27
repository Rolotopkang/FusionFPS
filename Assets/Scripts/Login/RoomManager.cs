using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityTemplateProjects.Tools;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public GameObject RoomNameButtonPrefab;
    public Transform gridLayout;

    public override void OnEnable()
    {
        Debug.Log("startJoinLobby");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("JoinLobby!");
    }

    private void Update()
    {
        Debug.Log(PhotonNetwork.CountOfRooms+"roomNUM");
        Debug.Log(PhotonNetwork.CountOfPlayersInRooms+"inrooms");
    }

    // public override void OnDisable()
    // {
    //     PhotonNetwork.LeaveLobby();
    //     Debug.Log("QuitLobby!");
    // }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("RoomUpdate!");
        
         for (int i = 0; i < gridLayout.childCount; i++)
         {
             if (gridLayout.GetChild(i).gameObject.GetComponentsInChildren<Text>()[0].text == roomList[i].Name)
             {
                 Destroy(gridLayout.GetChild(i).gameObject);
                 if (roomList[i].PlayerCount == 0)
                 {
                     roomList.Remove(roomList[i]);
                     Debug.Log("remove one room");
                 }
             }
         }
         
        foreach (RoomInfo room in roomList)
        {
            Debug.Log(roomList.Count);
            if (room.PlayerCount != 0)
            {
                GameObject newRoom = Instantiate(RoomNameButtonPrefab, gridLayout.position, Quaternion.identity);
                newRoom.GetComponent<JoinRoomBTN>().min = room.PlayerCount;
                newRoom.GetComponent<JoinRoomBTN>().max = room.MaxPlayers;
                Text[] texts = newRoom.GetComponentsInChildren<Text>();
                texts[0].text = room.Name;
                texts[2].text = (String)room.CustomProperties["host"];
                texts[4].text = room.PlayerCount+"/"+room.MaxPlayers;
                texts[6].text = room.CustomProperties["GameMode"].ToString();
                texts[8].text = room.CustomProperties["mapIndex"].ToString();
                newRoom.transform.SetParent(gridLayout);
            }
            else
            {
                roomList.Remove(room);
                Debug.Log("remove one room");
            }
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        // ChangeScene.GetInstance().SetMapToChange(PhotonNetwork.CurrentRoom.);
        // ChangeScene.GetInstance().OnBTNChangeScene();
        Debug.Log("Join Room!");
    }
}
