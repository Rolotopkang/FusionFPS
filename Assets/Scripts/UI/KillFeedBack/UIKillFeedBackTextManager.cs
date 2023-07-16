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
    public static Action<string,int> CreateGetScoreFeedbackText = delegate {};
    
    private void OnEnable()
    {
        CreateKillFeedbackText += Create;
        CreateGetScoreFeedbackText += Create;
    }

    private void OnDisable()
    {
        CreateKillFeedbackText -= Create;
        CreateGetScoreFeedbackText -= Create;
    }
    
    void Create(string weaponName,string killedName,int score)
    {
        current = killFeedBackTextPrefab;
        UIKillFeedBackText tmpKillText = Instantiate(current,holder);
        tmpKillText.transform.SetSiblingIndex(0);
        tmpKillText.Register(weaponName, killedName, score);
    }

    void Create(String discription, int score)
    {
        current = killFeedBackTextPrefab;
        UIKillFeedBackText tmpKillText = Instantiate(current,holder);
        tmpKillText.transform.SetSiblingIndex(0);
        tmpKillText.Register(discription,score);
    }
}
