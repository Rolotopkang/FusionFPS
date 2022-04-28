using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class UIKillFeedBackText : MonoBehaviour
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

    private TextMeshProUGUI textMeshProUGUI = null;
    protected TextMeshProUGUI TextMeshProUGUI
    {
        get
        {
            if (textMeshProUGUI == null)
            {
                textMeshProUGUI = GetComponent<TextMeshProUGUI>();
                if (textMeshProUGUI == null)
                {
                    textMeshProUGUI = gameObject.AddComponent<TextMeshProUGUI>();
                }
            }
            return textMeshProUGUI;
        }
    }
    
    
    private IEnumerator IE_Countdown = null;
    private Action unRegister = null;
    
    
    public void Register(string weaponName,string killedName,int score)
    {
        StringBuilder tmp_stringBuilder = new StringBuilder();
        if (!weaponName.Equals(String.Empty))
        {
            tmp_stringBuilder.Append("<#00FFDC>");
            tmp_stringBuilder.Append("[");
            tmp_stringBuilder.Append(weaponName);
            tmp_stringBuilder.Append("] ");
        }
        tmp_stringBuilder.Append("<#FF0100>");
        tmp_stringBuilder.Append(killedName);
        tmp_stringBuilder.Append(" <#FFFFFF>");
        tmp_stringBuilder.Append(score);
        
        TextMeshProUGUI.text = tmp_stringBuilder.ToString();
        
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

        // unRegister();
        Destroy(gameObject);
    }
    
}
