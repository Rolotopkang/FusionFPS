using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeployUI_Top : MonoBehaviour
{
    [SerializeField] 
    protected TextMeshProUGUI gameLoopTime;
    [SerializeField]
    protected Image deployImageRight;
    [SerializeField]
    protected Image deployImageLeft;

    [SerializeField] 
    private bool isDeploy = true;

    protected GameModeManagerBehaviour GameModeManagerBehaviour;

    protected virtual void Start()
    {
        GameModeManagerBehaviour = RoomManager.GetInstance().currentGamemodeManager;
    }

    protected virtual void Update()
    {
        gameLoopTime.text = sec_to_hms(GameModeManagerBehaviour.GetGameLoopSec());
        if (isDeploy)
        {
            deployImageLeft.fillAmount =
                DeployManager.GetInstance().GetShowTimer / DeployManager.GetInstance().GetMaxDePloyTimer;
            deployImageRight.fillAmount =
                DeployManager.GetInstance().GetShowTimer / DeployManager.GetInstance().GetMaxDePloyTimer;
        }
    }
    
    private string sec_to_hms(int duration)
    {
        TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(duration));
        string str = "";
        if (ts.Hours > 0)
        {
            str = String.Format("{0:00}", ts.Hours) + ":" + String.Format("{0:00}", ts.Minutes) + ":" + String.Format("{0:00}", ts.Seconds);
        }
        if (ts.Hours == 0 && ts.Minutes > 0)
        {
            str = "00:" + String.Format("{0:00}", ts.Minutes) + ":" + String.Format("{0:00}", ts.Seconds);
        }
        if (ts.Hours == 0 && ts.Minutes == 0)
        {
            str = "00:00:" + String.Format("{0:00}", ts.Seconds);
        }
        return str;
    }

}
