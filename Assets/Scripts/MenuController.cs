using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Rendering;

public class MenuController :MonoBehaviourPun
{
    [SerializeField]
    private Animation _animation;
    [SerializeField]
    private AnimationClip UIshow;
    [SerializeField]
    private AnimationClip UIHide;

    
    private Coroutine _timerRoutine;
    

    public void onUIshow()
    {
        _animation.clip = UIshow;
        _animation.Play();
    }
    public void onUIHide()
    {
        _animation.clip = UIHide;
        _animation.Play();
    }

    public void BTN_Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif

    }
}
