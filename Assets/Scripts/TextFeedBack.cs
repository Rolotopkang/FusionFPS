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

    protected override void Tick()
    {
        if (playerEventManager.GethitHint)
        {
            HitFeedBack(playerEventManager.GetHitKind);
        }
    }

    #endregion

    #region Funtions

    private void HitFeedBack(EnumTools.HitKinds hitKind)
    {
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
        Debug.Log("播放击中提示！播放的片段为"+hitKind);
        playerEventManager.SethitHint(false);
    }

    #endregion
}
