using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class JoinRoomBTN : MonoBehaviourPunCallbacks
{
    public int min;
    public int max;
    private String roomName;
    
    private void Start()
    {
        roomName = GetComponentsInChildren<Text>()[0].text;
    }

    public void BTNJoinRoom()
    {
        if (!PhotonNetwork.InRoom)
        {
            if (min < max)
            {
                PhotonNetwork.JoinRoom(roomName);
            }
            else
            {
                transform.parent.parent.parent.parent.Find("WrongHint").GetComponent<Text>().text = "Room is full!!";
                Debug.Log("Room is full!");
            }
        }
        else
        {
            Debug.Log("IS IN A Room");
        }
    }
    
    
}
