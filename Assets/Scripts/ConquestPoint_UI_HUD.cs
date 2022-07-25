using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ConquestPoint_UI_HUD : Singleton<ConquestPoint_UI_HUD>
{
    public EnumTools.ConquestPoints pointName;
    private ConquestPoint _conquestPoint; 
    
    [SerializeField]
    private Image fill_image;
    [SerializeField]
    private Image MineMemImg;
    [SerializeField]
    private Image EnemyMemImg;
    [SerializeField]
    private TextMeshProUGUI pointNameText;
    [SerializeField]
    private TextMeshProUGUI MineMemTex;
    [SerializeField]
    private TextMeshProUGUI EnemyMemTex;
    [SerializeField]
    private GameObject canvas;
    [SerializeField]
    private GameObject scrambleBase;
    private void Start()
    {
        
    }

    private void OnEnable()
    {
        canvas.SetActive(false);
    }

    private void Update()
    {
        if (PhotonNetwork.LocalPlayer.GetPhotonTeam() == null)
        {
            return;
        }

        if (_conquestPoint == null)
        {
            return;
        }
        pointNameText.color = GetPointColor(_conquestPoint);
        fill_image.color = GetFillColor();
        pointNameText.text = _conquestPoint.pointName.ToString();
        fill_image.fillAmount = Mathf.Abs(_conquestPoint.occupyProgress);
        if (_conquestPoint.isScrambling)
        {
            int tmp_sum = _conquestPoint.GetInPointBlueTeamsNum() + _conquestPoint.GetInPointRedTeamsNum();
            MineMemTex.text = GetPointNum(PhotonNetwork.LocalPlayer, true).ToString();
            EnemyMemTex.text = GetPointNum(PhotonNetwork.LocalPlayer, false).ToString();
            MineMemImg.fillAmount = (float)GetPointNum(PhotonNetwork.LocalPlayer, true) / tmp_sum;
            EnemyMemImg.fillAmount = (float)GetPointNum(PhotonNetwork.LocalPlayer, false) / tmp_sum;
            scrambleBase.SetActive(true);
        }
        else
        {
            scrambleBase.SetActive(false);
        }
    }
    
    /// <summary>
    /// 返回据点填充颜色
    /// </summary>
    /// <returns></returns>
    private Color GetFillColor()
    {
        if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Name
            .Equals(EnumTools.Teams.Blue.ToString()))
        {
            return _conquestPoint.occupyProgress <= 0
                ? EnumTools.GetTeamColor(EnumTools.TeamColor.Enemy)
                : EnumTools.GetTeamColor(EnumTools.TeamColor.Mine);
        }
        if((PhotonNetwork.LocalPlayer.GetPhotonTeam().Name
            .Equals(EnumTools.Teams.Red.ToString())))
        {
            return _conquestPoint.occupyProgress >= 0
                ? EnumTools.GetTeamColor(EnumTools.TeamColor.Enemy)
                : EnumTools.GetTeamColor(EnumTools.TeamColor.Mine);
        }

        return EnumTools.GetTeamColor(EnumTools.TeamColor.None);
    }
    
    /// <summary>
    /// 返回据点对应本地角色颜色
    /// </summary>
    /// <param name="conquestPoint"></param>
    /// <returns></returns>
    private Color GetPointColor(ConquestPoint conquestPoint)
    {
        return conquestPoint.pointOwnerTeam switch
        {
            EnumTools.Teams.None => EnumTools.GetTeamColor(EnumTools.TeamColor.None),
            EnumTools.Teams.Blue => PhotonNetwork.LocalPlayer.GetPhotonTeam().Name.Equals(EnumTools.Teams.Blue.ToString())? 
                EnumTools.GetTeamColor(EnumTools.TeamColor.Mine): EnumTools.GetTeamColor(EnumTools.TeamColor.Enemy),
            EnumTools.Teams.Red => PhotonNetwork.LocalPlayer.GetPhotonTeam().Name.Equals(EnumTools.Teams.Red.ToString())? 
                EnumTools.GetTeamColor(EnumTools.TeamColor.Mine): EnumTools.GetTeamColor(EnumTools.TeamColor.Enemy),
        };
    }

    private int GetPointNum(Player player, bool isMineTeam)
    {
        if (player.GetPhotonTeam() == null)
            return 0;
        if (isMineTeam)
        {
            if (player.GetPhotonTeam().Name.Equals(EnumTools.Teams.Blue.ToString()))
            {
                return _conquestPoint.GetInPointBlueTeamsNum();
            }
            //else
            return _conquestPoint.GetInPointRedTeamsNum();
        }
        else
        {
            if (player.GetPhotonTeam().Name.Equals(EnumTools.Teams.Blue.ToString()))
            {
                return _conquestPoint.GetInPointRedTeamsNum();
            }
            //else
            return _conquestPoint.GetInPointBlueTeamsNum();
        }
    }
    
    public void OpenConquestPoint_UI_HUD(EnumTools.ConquestPoints points)
    {
        pointName = points;
        _conquestPoint = ConquestPointManager.GetInstance().GetConquestPointThroughName(pointName.ToString());
        canvas.SetActive(true);
    }
    public void CloseConquestPoint_UI_HUD()
    {
        canvas.SetActive(false);
    }
}
