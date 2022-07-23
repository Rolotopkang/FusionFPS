using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Unity.Profiling.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class ConquestPoint_UI_Point : MonoBehaviour
{
    public EnumTools.ConquestPoints pointName;
    public bool isBirthPoint = false;
    public bool isDeployUI = false;
    public bool isIndicator = false;
    
    private ConquestPoint _conquestPoint; 
    [SerializeField]
    private Image fill_image;
    [SerializeField]
    private Image pointer_image;
    [SerializeField]
    private Text pointDistanceText;
    [SerializeField]
    private Text pointNameText;

    private GameObject selectedAble;
    private bool canDeploy =false;
    public bool GetCanDeploy => canDeploy;

    private void Start()
    {
        if(!isBirthPoint)
            _conquestPoint = ConquestPointManager.GetInstance().GetConquestPointThroughName(pointName.ToString());
        if (isDeployUI)
            selectedAble = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        if (PhotonNetwork.LocalPlayer.GetPhotonTeam() == null)
        {
            return;
        }
        if (isBirthPoint && isDeployUI)
        {
            if (pointName == EnumTools.ConquestPoints.BlueBirth && PhotonNetwork.LocalPlayer.GetPhotonTeam()
                .Name.Equals("Blue"))
            {
                canDeploy = true;
            }else if (pointName == EnumTools.ConquestPoints.RedBirth && PhotonNetwork.LocalPlayer.GetPhotonTeam()
                .Name.Equals("Red"))
            {
                canDeploy = true;
            }
            else
            {
                canDeploy = false;
            }
        }else if(isDeployUI)
        {
            canDeploy = PhotonNetwork.LocalPlayer.GetPhotonTeam().Name
                .Equals(_conquestPoint.pointOwnerTeam.ToString());
        }
        
        if (isBirthPoint)
        {
            fill_image.color = GetPointColor();
            pointNameText.color = GetPointColor();
            return;
        }

        if (isIndicator)
        {
            if (_conquestPoint == null)
            {
                _conquestPoint = transform.parent.GetComponent<ConquestPointIndicator>().ConquestPoint;
                if(_conquestPoint == null)
                    return;
                pointName = _conquestPoint.pointName;
                pointNameText.text = pointName.ToString(); 
            }
            pointDistanceText.color = GetPointColor(_conquestPoint);
            pointer_image.color = GetPointColor(_conquestPoint);
        }
        
        fill_image.color = GetFillColor();
        pointNameText.color = GetPointColor(_conquestPoint);
        fill_image.fillAmount = Mathf.Abs(_conquestPoint.occupyProgress);
        
        if (isDeployUI)
        {
            selectedAble.SetActive(PhotonNetwork.LocalPlayer.GetPhotonTeam().Name
                .Equals(_conquestPoint.pointOwnerTeam.ToString()));
        }
    }

    /// <summary>
    /// 返回出身点颜色
    /// </summary>
    /// <returns></returns>
    private Color GetPointColor()
    {
        return pointName switch
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
    
}
