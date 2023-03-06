using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugInfoController : Singleton<DebugInfoController>
{
    public Text PingText;
    public Text FPSText;

    public float fpsMeasuringDelta = 2.0f;
 
    private float timePassed;
    private int m_FrameCount = 0;
    private float m_FPS = 0.0f;

    private void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        ContingFPS();
        if (PhotonNetwork.IsConnected)
        {
            PingText.text = PhotonNetwork.GetPing().ToString();
        }
    }

    private void ContingFPS()
    {
        m_FrameCount = m_FrameCount + 1;
        timePassed = timePassed + Time.deltaTime;
 
        if (timePassed > fpsMeasuringDelta)
        {
            m_FPS = m_FrameCount / timePassed;
            FPSText.text = m_FPS.ToString("F1");
            timePassed = 0.0f;
            m_FrameCount = 0;
        }
    }
}
