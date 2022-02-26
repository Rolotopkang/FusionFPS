using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class UIRoomListManager : MonoBehaviourPunCallbacks
{
    private Dictionary<string,RoomInfo> roomInfoDictionary = new Dictionary<string, RoomInfo>();
    public Dictionary<string, RoomInfo> GetRoomList => roomInfoDictionary;


    public event Action<RoomInfo> OnRoomRemoved;
    public event Action<RoomInfo> OnRoomAdded; 
    
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        foreach (RoomInfo tmp_Info in roomList)
        {
            if (tmp_Info.RemovedFromList)
            {
                if (roomInfoDictionary.ContainsKey(tmp_Info.Name))
                {
                    roomInfoDictionary.Remove(tmp_Info.Name); 
                    OnRoomRemoved?.Invoke(tmp_Info);
                }
                return;
            }
            
            if (roomInfoDictionary.ContainsKey(tmp_Info.Name))
            {
                continue;
            }
            
            roomInfoDictionary.Add(tmp_Info.Name,tmp_Info);
            OnRoomAdded?.Invoke(tmp_Info);
        }
    }
    
    
    #if UNITY_EDITOR
    private void OnGUI()
    {
        if (GUILayout.Button("Debug Room List"))
        {
            foreach (KeyValuePair<string,RoomInfo> tmp_Info in roomInfoDictionary)
            {
                Debug.Log(tmp_Info.Key);
            }
        }
    }
#endif
    
}
