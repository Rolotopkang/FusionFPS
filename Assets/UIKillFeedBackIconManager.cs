using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIKillFeedBackIconManager : MonoBehaviour
{
    [SerializeField]
    private UIKillFeedbackIcon normalKillPrefab = null;
    [SerializeField]
    private UIKillFeedbackIcon headShotKillPrefab = null;
    [SerializeField]
    private Transform holder;

    private UIKillFeedbackIcon current = null;

    private List<UIKillFeedbackIcon> KillFeedbackIcons = new List<UIKillFeedbackIcon>();


    public static Action<EnumTools.KillKind> CreateKillFeedbackIcon = delegate {};

    private void OnEnable()
    {
        CreateKillFeedbackIcon += Create;
    }

    private void OnDisable()
    {
        CreateKillFeedbackIcon -= Create;
    }


    void Create(EnumTools.KillKind killKind)
    {
        switch (killKind)
        {
            case EnumTools.KillKind.player:
                current = normalKillPrefab;
                break;
            case EnumTools.KillKind.playerHeadshot:
                current = headShotKillPrefab;
                break;
            default:
                current = normalKillPrefab;
                break;
        }
        UIKillFeedbackIcon tmpKillIcon = Instantiate(current, holder);
        tmpKillIcon.Register(new Action(() => { KillFeedbackIcons.Remove(tmpKillIcon);}));
        KillFeedbackIcons.Add(tmpKillIcon);
    }
}
