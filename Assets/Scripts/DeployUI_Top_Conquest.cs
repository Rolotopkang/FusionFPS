using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeployUI_Top_Conquest : DeployUI_Top
{
    [SerializeField]
    private Image mineFill;
    [SerializeField]
    private Image enemyFill;
    [SerializeField]
    private TextMeshProUGUI mineText;
    [SerializeField]
    private TextMeshProUGUI enemyText;
    
    
    private ConquestManager ConquestManager;

    protected override void Start()
    {
        base.Start();
        ConquestManager = (ConquestManager) GameModeManagerBehaviour;
    }

    protected override void Update()
    {
        base.Update();
        mineFill.fillAmount = (float)ConquestManager.GetRebornCount(PhotonNetwork.LocalPlayer,true)/
                              ConquestManager.maxRebornCount;
        enemyFill.fillAmount =(float)ConquestManager.GetRebornCount(PhotonNetwork.LocalPlayer,false)/
                              ConquestManager.maxRebornCount;
        mineText.text = ConquestManager.GetRebornCount(PhotonNetwork.LocalPlayer, true).ToString();
        enemyText.text = ConquestManager.GetRebornCount(PhotonNetwork.LocalPlayer, false).ToString();
    }
}
