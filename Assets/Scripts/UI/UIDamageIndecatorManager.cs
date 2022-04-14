using System;
using System.Collections;
using System.Collections.Generic;
using InfimaGames.LowPolyShooterPack.Interface;
using UnityEngine;

public class UIDamageIndecatorManager : Element
{
    [SerializeField] private UIDamageIndecator indicatorPrefab = null;
    [SerializeField] private RectTransform holder = null;

    private Dictionary<Transform, UIDamageIndecator> Indicators = new Dictionary<Transform, UIDamageIndecator>();



    #region Delegates

    public static Action<Transform> CreateIndicator = delegate{  };
    public static Func<Transform, bool> CheckIfObjectInSight = null;

    #endregion

    private void OnEnable()
    {
        CreateIndicator += Create;
        
    }

    private void OnDisable()
    {
        CreateIndicator -= Create;
    }

    void Create(Transform target)
    {
        if (Indicators.ContainsKey(target))
        {
            Indicators[target].Restart();
            return;
        }

        UIDamageIndecator tmpIndicator = Instantiate(indicatorPrefab, holder);
        tmpIndicator.Register(target,playerCharacter.transform, new Action(() => { Indicators.Remove(target); }));
        Indicators.Add(target,tmpIndicator);
        
    }

    void InSight(Transform t)
    {
        
    }
}
