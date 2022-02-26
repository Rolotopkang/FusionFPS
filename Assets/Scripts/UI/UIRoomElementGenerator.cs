using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

public class UIRoomElementGenerator : MonoBehaviour
{
    public GameObject RoomUIElementPrefab;
    public Transform RoomUIElementRoot;
    public UIRoomListManager UiRoomListManager;
    public Dictionary<string, GameObject> roomUIElementDictionary = new Dictionary<string, GameObject>();
    private bool onDisplaying;

    private void GenerationRoomUI(RoomInfo _roomInfo)
    {
        if (roomUIElementDictionary.ContainsKey(_roomInfo.Name)) return;
        var tmp_RoomUIGo = Instantiate(RoomUIElementPrefab, RoomUIElementRoot);
        var tmp_Transform = tmp_RoomUIGo.transform;
        tmp_Transform.localPosition = Vector3.zero;
        tmp_Transform.localScale = Vector3.one;
        tmp_Transform.localRotation = Quaternion.identity;
        roomUIElementDictionary.Add(_roomInfo.Name, tmp_RoomUIGo);


        var tmp_UIRoomElementScript = tmp_RoomUIGo.GetComponent<UIRoomElement>();
        tmp_UIRoomElementScript.SetElementDetail(_roomInfo);
        tmp_UIRoomElementScript.OnSelectedRoom = FindObjectOfType<UIManager>().SetSelectedRoomDetails;
    }

    private void RemovedRoomUI(RoomInfo _roomInfo)
    {
        if (roomUIElementDictionary.TryGetValue(_roomInfo.Name, out GameObject tmp_GameObject))
        {
            Destroy(tmp_GameObject);
            roomUIElementDictionary.Remove(_roomInfo.Name);
        }
    }


    public void StartGenerationRoomUI()
    {
        onDisplaying = true;
        foreach (KeyValuePair<string, RoomInfo> tmp_Info in UiRoomListManager.GetRoomList)
        {
            GenerationRoomUI(tmp_Info.Value);
        }
    }

    public void RoomGeneratorHiding()
    {
        onDisplaying = false;
        foreach (KeyValuePair<string, GameObject> tmp_RoomUI in roomUIElementDictionary)
        {
            Destroy(tmp_RoomUI.Value);
        }

        roomUIElementDictionary.Clear();
    }


    private void OnEnable()
    {
        UiRoomListManager.OnRoomRemoved += OnRoomRemoved;
        UiRoomListManager.OnRoomAdded += OnRoomAdded;
    }

    private void OnRoomAdded(RoomInfo _roomInfo)
    {
        if (!onDisplaying)
            return;
        GenerationRoomUI(_roomInfo);
    }

    private void OnRoomRemoved(RoomInfo _roomInfo)
    {
        if (!onDisplaying)
            return;
        RemovedRoomUI(_roomInfo);
    }
}