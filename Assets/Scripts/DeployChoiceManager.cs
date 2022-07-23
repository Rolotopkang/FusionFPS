using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class DeployChoiceManager : Singleton<DeployChoiceManager>
{
    [SerializeField]
    private Transform PointsRoot;
    private ConquestPoint_Deploy[] ConquestPointDeploys;

    public Transform Canvas;

    private void Start()
    {
        ConquestPointDeploys = PointsRoot.GetComponentsInChildren<ConquestPoint_Deploy>();
        Canvas = transform.GetChild(0);
    }

    public void SetSelectedPoint(ConquestPoint_Deploy set)
    {
        foreach (ConquestPoint_Deploy conquestPointDeploy in ConquestPointDeploys)
        {
            conquestPointDeploy.isSelected = false;
        }
        set.isSelected = true;
    }

    public void DeployAtChoicePoint()
    {
        RoomManager.GetInstance().currentGamemodeManager.GetPlayerManager(PhotonNetwork.LocalPlayer).OnBTNDeploy();
    }

    public EnumTools.ConquestPoints GetSelectedPoint()
    {
        foreach (ConquestPoint_Deploy conquestPointDeploy in ConquestPointDeploys)
        {
            if (conquestPointDeploy.isSelected)
                return conquestPointDeploy.GetConquestPoint;
        }
        return EnumTools.ConquestPoints.None;
    }
    
}
