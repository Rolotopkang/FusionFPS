using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIRoomElement : MonoBehaviour, IPointerDownHandler
{
    public Image PwdIcon;
    public Text RoomName;
    public Text NumbersOfPlayer;

    public Action<RoomInfo> OnSelectedRoom;
    private RoomInfo roomInfo;

    public void SetElementDetail(RoomInfo _roomInfo)
    {
        roomInfo = _roomInfo;
        RoomName.text = _roomInfo.Name;
        NumbersOfPlayer.text = $"{_roomInfo.PlayerCount}/{_roomInfo.MaxPlayers}";
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnSelectedRoom?.Invoke(roomInfo);
    }
}