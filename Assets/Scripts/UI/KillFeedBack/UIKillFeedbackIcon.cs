using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIKillFeedbackIcon : MonoBehaviour
{
    private const float MaxTimer = 4f;
    private float timer = MaxTimer;
    private CanvasGroup canvasGroup = null;
    protected CanvasGroup CanvasGroup
    {
        get
        {
            if (canvasGroup == null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                {
                    canvasGroup = gameObject.AddComponent<CanvasGroup>();
                }
            }
            return canvasGroup;
        }
    }
    private IEnumerator IE_Countdown = null;
    private Action unRegister = null;
    private Animation animation;
    protected Animation Animation
    {
        get
        {
            if (animation == null)
            {
                animation = GetComponent<Animation>();
                if (animation == null)
                {
                    animation = gameObject.AddComponent<Animation>();
                }
            }
            return animation;
        }
    }
    

    public void Register(Action unRegister)
    {
        this.unRegister = unRegister;
        Animation.Play();
        StartTimer();
    }

    private void StartTimer()
    {
        if (IE_Countdown != null)
        {
            StopCoroutine(IE_Countdown);
        }

        IE_Countdown = Countdown();
        StartCoroutine(IE_Countdown);
    }
    
    private IEnumerator Countdown()
    {
        while (timer>0)
        {
            timer--;
            yield return new WaitForSeconds(1);
        }

        while (CanvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= 2 * Time.deltaTime;
            yield return null;
        }

        unRegister();
        Destroy(gameObject);
    }
}
