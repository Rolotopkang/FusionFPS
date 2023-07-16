using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using UnityTemplateProjects.Tools;
using WebSocketSharp;
using Logger = UnityEngine.Logger;

public class CreateRoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private MapTools.GameMode _gameMode;
    [SerializeField] private TMP_Dropdown mapDropdown;
    [SerializeField] private TMP_InputField roomName;
    [SerializeField] private Toggle friendlyFire;
    [SerializeField] private Toggle privateToggle;
    [SerializeField] private Slider maxPlayer;
    [SerializeField] private Text WrongHint;
    
    [SerializeField] private UIBase Menu_UI;
    
    private void CreateRoom()
    {
        ExitGames.Client.Photon.Hashtable CustomRoomInfo = new ExitGames.Client.Photon.Hashtable();
        
        if (friendlyFire) { CustomRoomInfo.Add("pvp",friendlyFire.interactable); }
        CustomRoomInfo.Add("host",PhotonNetwork.NickName);
        CustomRoomInfo.Add("GameMode",_gameMode.ToString());
        MapTools.Map tmp_map = MapTools.IndexToMap(mapDropdown.value, _gameMode);
        CustomRoomInfo.Add("mapIndex",tmp_map.SceneIndex);
        CustomRoomInfo.Add("mapDiscripName",tmp_map.MapDiscripName);
        CustomRoomInfo.Add("State",false);


        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers =Convert.ToByte(maxPlayer.value),
            CustomRoomProperties = CustomRoomInfo,
            IsVisible = true,
            IsOpen = true,
        };
        roomOptions.CustomRoomPropertiesForLobby = new string[] {"pvp","host","GameMode","mapIndex","mapDiscripName","State"};

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.CreateRoom(
            roomName.text.Equals(String.Empty) ? "" : roomName.text,
            roomOptions, TypedLobby.Default);
    }

    public void BTNCreateRoom()
    {
        if (PhotonNetwork.InRoom) {return;}
        
        CreateRoom();
    }
    
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        if (PhotonNetwork.IsMasterClient)
        {
            ChangeScene.GetInstance().SetMapToChange(mapDropdown.value,_gameMode);
            ChangeScene.GetInstance().OnBTNChangeScene();
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        WrongHint.text = "创建房间失败!"+returnCode;
        Debug.Log(message+returnCode);
    }
}
