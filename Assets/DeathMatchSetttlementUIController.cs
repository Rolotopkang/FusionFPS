using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using InfimaGames.LowPolyShooterPack.Interface;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class DeathMatchSetttlementUIController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI score;
    [SerializeField]
    private TextMeshProUGUI EndingHint;
    [SerializeField]
    private CanvasFader _canvasFader;

    [SerializeField] private GameObject _canvas1;
    [SerializeField] private GameObject _canvas2;

    [SerializeField] private GameObject Cam;
    [SerializeField] private GameObject ScoreBoard;

    [SerializeField] private TextMeshProUGUI GoldCount;
    [SerializeField] private TextMeshProUGUI CountingDown;
    [SerializeField] private int WaitTime = 30;
    private float timer;
    
    private void Start()
    {
        _canvas1.SetActive(false);
        _canvas2.SetActive(false);
        Cam.SetActive(false);
        ScoreBoard.SetActive(false);
    }
    
    public void Initialized(EventData eventData)
    {
        Dictionary<byte, object> tmp_SettlementData = (Dictionary<byte, object>)eventData.CustomData;
        Player tmp_winPlayer = (Player) tmp_SettlementData[0];
        int tmp_KillCount = (int)tmp_SettlementData[1];
        int tmp_DeathCount= (int)tmp_SettlementData[2];
        
        _canvas1.SetActive(true);
        score.text = tmp_KillCount + "/" + tmp_DeathCount;

        if (tmp_winPlayer == null)
        {
            EndingHint.text = "无获胜者";
        }
        else
        {
            EndingHint.text = "获胜者--" + tmp_winPlayer.NickName;
        }
        

        _canvas1.SetActive(false);
        _canvas2.SetActive(true);
        _canvasFader.FadeIn(0.75f,OnFadeIn);
    }
    
    public void OnFadeIn()
    {
        Cam.SetActive(true);
        _canvas1.SetActive(true);
        _canvasFader.FadeOut(5f, () => StartCoroutine(ShowScoreboard()));
    }
    
    private IEnumerator ShowScoreboard()
    {
        yield return new WaitForSeconds(4f);
        _canvas1.SetActive(false);
        _canvas2.SetActive(false);
        ScoreBoard.SetActive(true);
        
        
        int tmp_AddGold =  PhotonNetwork.LocalPlayer.GetScore() / 10;
        
        //数据库通信

        GoldCount.text = "+" + tmp_AddGold;
        StartCoroutine(countingDownEnumerator());
    }

    IEnumerator countingDownEnumerator()
    {
        timer = WaitTime;
        while (timer >= 0)
        {
            CountingDown.text = "等待" + (int)timer + "秒开始下一局比赛";
            yield return new WaitForSeconds(1);
            timer--;
        }
        NextRound();
    }
    
    public void NextRound()
    {
        _canvas1.SetActive(false);
        _canvas2.SetActive(false);
        Cam.SetActive(false);
        ScoreBoard.SetActive(false);
        RoomManager.GetInstance().currentGamemodeManager.GetPlayerManager(PhotonNetwork.LocalPlayer).GameReset();
        RoomManager.GetInstance().currentGamemodeManager.ResetGame();
    }
    
    public void BTN_Quit()
    {
        SettingsMenu.GetInstance().Quit();
    }
}
