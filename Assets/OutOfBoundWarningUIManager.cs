using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OutOfBoundWarningUIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI countDownUI;

    [SerializeField]
    private GameObject Canvas;
    
    private float leftTime;
    
    private Coroutine countDown;
    
    public static Action<float> StartCountDown = delegate {};
    public static Action StopCountDown = delegate {};

    private void Start()
    {
        Canvas.SetActive(false);
    }

    private void OnEnable()
    {
        StartCountDown += Create;
        StopCountDown += Stop;
    }

    private void OnDisable()
    {
        StartCountDown -= Create;
        StopCountDown -= Stop;
    }

    void Create(float set)
    {
        if (countDown != null)
        {
            StopCoroutine(countDown);
            countDown = null;
        }
        countDown = StartCoroutine(CountDown(set));
    }

    void Stop()
    {
        if (countDown != null)
        {
            StopCoroutine(countDown);
            countDown = null;
        }
        Canvas.SetActive(false);
    }

    private IEnumerator CountDown(float set)
    {
        Canvas.SetActive(true);
        leftTime = set;
        while (leftTime>0)
        {
            leftTime -= Time.deltaTime;
            int tmp_num = (int)leftTime;
            countDownUI.text = "00:" + tmp_num.ToString("00");
            yield return null;
        }
        Debug.Log("超时！");
        Canvas.SetActive(false);
        countDown = null;
    }
}
