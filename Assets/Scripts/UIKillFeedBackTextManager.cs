using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIKillFeedBackTextManager : MonoBehaviour
{
    
    [SerializeField] private UIKillFeedBackText killFeedBackTextPrefab = null;
    [SerializeField] private Transform holder;
    private UIKillFeedBackText current;
    
    public static Action<string,string,int> CreateKillFeedbackText = delegate {};
    
    private void OnEnable()
    {
        CreateKillFeedbackText += Create;
    }

    private void OnDisable()
    {
        CreateKillFeedbackText -= Create;
    }
    
    void Create(string weaponName,string killedName,int score)
    {
        current = killFeedBackTextPrefab;
        
        UIKillFeedBackText tmpKillText = Instantiate(current,holder);
        tmpKillText.transform.SetSiblingIndex(0);
        tmpKillText.Register(weaponName, killedName, score);
    }
}
