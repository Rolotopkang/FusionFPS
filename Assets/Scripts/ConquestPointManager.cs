using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityTemplateProjects.Tools;

public class ConquestPointManager : SingletonPunCallbacks<ConquestPointManager>
{
    private List<ConquestPoint> ConquestPoints;
    public float occupySpeed;



    #region Unity

    protected override void Awake()
    {
        base.Awake();
        ConquestPoints = GetComponentsInChildren<ConquestPoint>().ToList();
        
    }

    private void Start()
    {
        occupySpeed = ((ConquestManager) RoomManager.GetInstance().currentGamemodeManager).occupySpeed;
    }
    
    private void Update()
    {
        
    }

    #endregion
    
    #region Funtions

    public ConquestPoint GetConquestPointThroughName(string name)
    {
        foreach (ConquestPoint conquestPoint in ConquestPoints)
        {
            if (conquestPoint.pointName.ToString().Equals(name))
            {
                return conquestPoint;
            }
        }
        return null;
    }

    public void InitConquestPoints()
    {
        foreach (ConquestPoint conquestPoint in ConquestPoints)
        {
            conquestPoint.pointOwnerTeam = EnumTools.Teams.None;
            conquestPoint.occupyProgress = 0;
            conquestPoint.isOccupying = false;
            conquestPoint.isScrambling = false;
        }
    }
    #endregion
    
}
