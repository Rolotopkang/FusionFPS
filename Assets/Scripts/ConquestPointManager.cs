using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityTemplateProjects.Tools;

public class ConquestPointManager : SingletonPunCallbacks<ConquestPointManager>
{
    [SerializeField]
    private ConquestPoint_Birth blueConquestPointBirth;
    [SerializeField]
    private ConquestPoint_Birth redConquestPointBirth;
    
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

    public ConquestPoint GetConquestPointThroughName(string pointName)
    {
        foreach (ConquestPoint conquestPoint in ConquestPoints)
        {
            if (conquestPoint.pointName.ToString().Equals(pointName))
            {
                return conquestPoint;
            }
        }
        return null;
    }

    public Transform GetConquestPointBirthTransform(string pointName)
    {
        if (pointName.Equals(EnumTools.ConquestPoints.BlueBirth.ToString()))
        {
            return blueConquestPointBirth.GetBirthPoint();
        }
        if (pointName.Equals(EnumTools.ConquestPoints.RedBirth.ToString()))
        {
            return redConquestPointBirth.GetBirthPoint();
        }
        foreach (ConquestPoint conquestPoint in ConquestPoints)
        {
            if (conquestPoint.pointName.ToString().Equals(pointName))
            {
                return conquestPoint.GetBirthPoint();
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
