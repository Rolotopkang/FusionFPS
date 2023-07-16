using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class UIKillFeedBackRoom : MonoBehaviour
{
    public struct RoomKillMes
    {
        public string deathPlayerName;
        public Color deathPlayerColor;
        public string how;
        public bool isHeadShot;
        public string killerName;
        public Color KillerColor;
        public RoomKillMes(string deathPlayerName, Color deathPlayerColor, string how, 
            bool isHeadShot, string killerName, Color killerColor)
        {
            this.deathPlayerName = deathPlayerName;
            this.deathPlayerColor = deathPlayerColor;
            this.how = how;
            this.isHeadShot = isHeadShot;
            this.killerName = killerName;
            this.KillerColor = killerColor;
        }
    }
    
    private const float MaxTimer = 5f;
    private float timer = MaxTimer;
    
    [SerializeField]
    private TextMeshProUGUI deathPlayer;
    [SerializeField]
    private TextMeshProUGUI how;
    [SerializeField]
    private TextMeshProUGUI killer;
    [SerializeField]
    private GameObject headShot;
    
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

    public void Register(RoomKillMes roomKillMes)
    {
        headShot.SetActive(roomKillMes.isHeadShot);
        deathPlayer.text = roomKillMes.deathPlayerName;
        deathPlayer.color = roomKillMes.deathPlayerColor;
        how.text ="["+roomKillMes.how+"]";
        how.color = new Color(0, 1, 1);
        killer.text = roomKillMes.killerName;
        killer.color = roomKillMes.KillerColor;
        
        //消失倒计时
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
