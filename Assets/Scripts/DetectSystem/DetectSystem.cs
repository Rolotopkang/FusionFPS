using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectSystem : MonoBehaviour
{
    [SerializeField]
    private int AroundLength = 25;
    
    [SerializeField]
    private int GazeLength = 15;


    private bool isGazed = false;
    private bool isAround = false;

    private DetectTarget tmp_detectTarget;

    private RaycastHit G;
    private RaycastHit A;
    
    void Update()
    {
        RaycastUpdate();
    }

    private void OnDestroy()
    {
        if (tmp_detectTarget)
        {
            tmp_detectTarget.RayCastHit(EnumTools.DetectTargetKind.Gaze, false);
        }
    }

    private void RaycastUpdate()
    {
        Ray m_ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(m_ray,out RaycastHit Hit_G, GazeLength))
        {
            if (Hit_G.transform.tag.Equals("DetectTarget"))
            {
                Hit_G.transform.GetComponent<DetectTarget>().RayCastHit(EnumTools.DetectTargetKind.Gaze, true);
                if (Hit_G.transform.GetComponent<DetectTarget>() != tmp_detectTarget  && tmp_detectTarget)
                {
                    tmp_detectTarget.RayCastHit(EnumTools.DetectTargetKind.Gaze, false);
                }
                tmp_detectTarget = Hit_G.transform.GetComponent<DetectTarget>();
                isGazed = true;
                G = Hit_G;
            }
            else
            {
                if (isGazed)
                {
                    if(G.transform)
                        G.transform.GetComponent<DetectTarget>().RayCastHit(EnumTools.DetectTargetKind.Gaze, false);
                    isGazed = false;
                }
            }
        }else if (Physics.Raycast(m_ray, out RaycastHit Hit_A, AroundLength))
        {
            if (Hit_A.transform.tag.Equals("DetectTarget"))
            {
                if (isGazed)
                {
                    if(G.transform)
                        G.transform.GetComponent<DetectTarget>().RayCastHit(EnumTools.DetectTargetKind.Gaze, false);
                    isGazed = false;
                }
                Hit_A.transform.GetComponent<DetectTarget>().RayCastHit(EnumTools.DetectTargetKind.Around, true);
                if (Hit_A.transform.GetComponent<DetectTarget>() != tmp_detectTarget  && tmp_detectTarget)
                {
                    tmp_detectTarget.RayCastHit(EnumTools.DetectTargetKind.Around, false);
                }
                tmp_detectTarget = Hit_A.transform.GetComponent<DetectTarget>();
                isAround = true;
                A = Hit_A;
            }
            else
            {
                if (isGazed)
                {
                    if(G.transform)
                        G.transform.GetComponent<DetectTarget>().RayCastHit(EnumTools.DetectTargetKind.Gaze, false);
                    isGazed = false;
                }
                if (isAround)
                {
                    if(A.transform)
                        A.transform.GetComponent<DetectTarget>().RayCastHit(EnumTools.DetectTargetKind.Around, false);
                    isAround = false;
                }
            }
        }
        else
        {
            if (isGazed)
            {
                if(G.transform)
                    G.transform.GetComponent<DetectTarget>().RayCastHit(EnumTools.DetectTargetKind.Gaze, false);
                isGazed = false;
            }
            if (isAround)
            {
                if(A.transform)
                    A.transform.GetComponent<DetectTarget>().RayCastHit(EnumTools.DetectTargetKind.Around, false);
                isAround = false;
            }
        }

    }
}
