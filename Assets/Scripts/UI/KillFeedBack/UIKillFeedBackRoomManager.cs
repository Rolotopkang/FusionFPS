using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIKillFeedBackRoomManager : MonoBehaviour
{
    [SerializeField] private UIKillFeedBackRoom _killFeedBackRoomPrefab = null;
    [SerializeField] private Transform holder;
    [SerializeField] private ScrollRect scrollControl;
    
    public static Action<UIKillFeedBackRoom.RoomKillMes> CreateKillFeedbackRoom = delegate {};

    private void OnEnable()
    {
        CreateKillFeedbackRoom += Create;
    }

    private void OnDisable()
    {
        CreateKillFeedbackRoom -= Create;
    }

    void Create(UIKillFeedBackRoom.RoomKillMes roomKillMes)
    {
        UIKillFeedBackRoom tmokillRoom = Instantiate(_killFeedBackRoomPrefab,holder);
        tmokillRoom.Register(roomKillMes);
        LayoutRebuilder.ForceRebuildLayoutImmediate(holder.GetComponent<RectTransform>());
        StartCoroutine("ScrollToBottom");
    }
    
    //自动更新到最底
    IEnumerator ScrollToBottom()
    {
        yield return new WaitForEndOfFrame();        
        scrollControl.verticalNormalizedPosition = 0f;
    }
}