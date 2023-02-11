using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectTarget : MonoBehaviour
{
    [SerializeField]
    private HUDSynSystem _hudSynSystem;

    public void RayCastHit(EnumTools.DetectTargetKind DetectTargetKind , bool set)
    {
        switch (DetectTargetKind)
        {
          case EnumTools.DetectTargetKind.Gaze :
              _hudSynSystem.OnClientGaze(set); 
              break;
          case EnumTools.DetectTargetKind.Around :
              _hudSynSystem.OnClientAround(set); 
              break;
        }
    }
}
