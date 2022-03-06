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

public class RoomManager : MonoBehaviourPunCallbacks
{
    public GameObject RoomNameButtonPrefab;
    public Transform gridLayout;
    public string PlayerPrefabName;
    public GameObject PlayerLocation;
    private LoginUIManager loginUIManager;
    

    private void Awake()
    {
        loginUIManager = GetComponentInParent<LoginUIManager>();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //房间断联保护
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
            if (room.PlayerCount != 0)
            {
                GameObject newRoom = Instantiate(RoomNameButtonPrefab, gridLayout.position, Quaternion.identity);
                newRoom.GetComponent<JoinRoomBTN>().min = room.PlayerCount;
                newRoom.GetComponent<JoinRoomBTN>().max = room.MaxPlayers;
                Text[] texts = newRoom.GetComponentsInChildren<Text>();
                texts[0].text = room.Name;
                texts[3].text = (String)room.CustomProperties["host"];
                texts[4].text = room.PlayerCount+"/"+room.MaxPlayers;
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
        Debug.Log("Join Room!");
        PhotonNetwork.Instantiate(PlayerPrefabName,PlayerLocation.transform.position, quaternion.identity);
        loginUIManager.CloseUI();
    }
}
