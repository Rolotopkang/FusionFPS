using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using UnityEngine.UI;

public class ConquestPoint_UI_Area : MonoBehaviour
{
    public EnumTools.ConquestPoints pointName;
    public bool isBirthPoint = false;

    private ConquestPoint _conquestPoint; 
    private Image area_image;

    private void Start()
    {
        area_image = GetComponent<Image>();
        if(!isBirthPoint)
            _conquestPoint = ConquestPointManager.GetInstance().GetConquestPointThroughName(pointName.ToString());
    }

    private void Update()
    {
        if (PhotonNetwork.LocalPlayer.GetPhotonTeam() == null)
        {
            return;
        }
        if (isBirthPoint)
        {
            area_image.color = pointName switch
            {
                EnumTools.ConquestPoints.BlueBirth => PhotonNetwork.LocalPlayer.GetPhotonTeam().Name
                    .Equals(EnumTools.Teams.Blue.ToString())
                    ? EnumTools.GetTeamColor(EnumTools.TeamColor.Mine)
                    : EnumTools.GetTeamColor(EnumTools.TeamColor.Enemy),
                EnumTools.ConquestPoints.RedBirth => PhotonNetwork.LocalPlayer.GetPhotonTeam().Name
                    .Equals(EnumTools.Teams.Red.ToString())
                    ? EnumTools.GetTeamColor(EnumTools.TeamColor.Mine)
                    : EnumTools.GetTeamColor(EnumTools.TeamColor.Enemy),
            };
            return;
        }
        area_image.color = _conquestPoint.pointOwnerTeam switch
        {
            EnumTools.Teams.None => EnumTools.GetTeamColor(EnumTools.TeamColor.None),
            EnumTools.Teams.Blue => PhotonNetwork.LocalPlayer.GetPhotonTeam().Name.Equals(EnumTools.Teams.Blue.ToString())? 
                EnumTools.GetTeamColor(EnumTools.TeamColor.Mine): EnumTools.GetTeamColor(EnumTools.TeamColor.Enemy),
            EnumTools.Teams.Red => PhotonNetwork.LocalPlayer.GetPhotonTeam().Name.Equals(EnumTools.Teams.Red.ToString())? 
                EnumTools.GetTeamColor(EnumTools.TeamColor.Mine): EnumTools.GetTeamColor(EnumTools.TeamColor.Enemy),
            
        };
    }
}
