using System;
using System.Collections;
using System.Collections.Generic;
using InfimaGames.LowPolyShooterPack.Interface;
using UnityEngine;

public class TextFeedBack : Element
{
    #region SerializeField

    [SerializeField]
    private AnimationClip HitFeedBackAnim;
    [SerializeField]
    private AnimationClip HitFeedBackKillAnim;
    [SerializeField]
    private AnimationClip HitFeedBackHeadShotAnim;


    #endregion

    #region Private

    private Animation crosshairFeedBack;

    #endregion

    #region Unity

    protected override void Awake()
    {
        base.Awake();
        crosshairFeedBack = GetComponent<Animation>();
    }

    private void OnEnable()
    {
        PlayerEventManager.onHitPlayer += HitFeedBack;
    }

    private void OnDestroy()
    {
        PlayerEventManager.onHitPlayer -= HitFeedBack;
    }

    #endregion

    #region Funtions

    private void HitFeedBack(EnumTools.HitKinds hitKind)
    {
        // Debug.Log(hitKind+"动画播出！");
        switch (hitKind)
        {
            case EnumTools.HitKinds.normal:
                crosshairFeedBack.clip = HitFeedBackAnim;
                break;
            case EnumTools.HitKinds.headShot:
                crosshairFeedBack.clip = HitFeedBackHeadShotAnim;
                break;
            case EnumTools.HitKinds.killShot:
                crosshairFeedBack.clip = HitFeedBackKillAnim;
                break;
            default:
                break;
        }
        crosshairFeedBack.Play();
    }

    #endregion
}
