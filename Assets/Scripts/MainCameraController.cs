using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    public GameObject SkyLook;
    public GameObject cameraon;//初始摄像机的位置
    public GameObject camerto;//另一个点的位置

    [Tooltip("基准速度")]
    [SerializeField]
    private float baseSpeed = 1;
    [SerializeField]
    private AnimationCurve[] AnimationInfoCurves;
    
    
    
    private Vector3 velocity;//如果是3D场景就用Vector3,2D用Vector2
    private Vector3 tmp_oldLocation;
    private float totalLeanth;

    private AnimationCurve TransportCurves;
    private AnimationCurve RotationCurves;
    
    
    public bool isLocated = false;

    public void Dochange()
    {
        camerto = SkyLook;
        isLocated = false;
        tmp_oldLocation = cameraon.transform.position;
        totalLeanth = Mathf.Abs(tmp_oldLocation.z - SkyLook.transform.position.z);
        TransportCurves = AnimationInfoCurves[0];
        RotationCurves = AnimationInfoCurves[2];
        StartCoroutine(CameraTransport(camerto));
    }

    public void Dochange(GameObject target)
    {
        camerto = target;
        isLocated = false;
        tmp_oldLocation = cameraon.transform.position;
        totalLeanth = Mathf.Abs(tmp_oldLocation.z - target.transform.position.z);
        TransportCurves = AnimationInfoCurves[0];
        RotationCurves = AnimationInfoCurves[1];
        StartCoroutine(CameraTransport(camerto));
    }

    private IEnumerator CameraTransport(GameObject target)
    {
       
        while (true)
        {
            //根据曲线变换相机位置
            // cameraon.transform.position = Vector3.Lerp(cameraon.transform.position,camerto.transform.position,CalculateSpeed(AnimationInfoCurves[0]));
            // cameraon.transform.localRotation = Quaternion.Slerp(cameraon.transform.rotation, camerto.transform.rotation,CalculateSpeed(AnimationInfoCurves[1]));

            cameraon.transform.position = new Vector3(
                Mathf.SmoothDamp(cameraon.transform.position.x, camerto.transform.position.x, ref velocity.x, CalculateSpeed(TransportCurves)), 
                Mathf.SmoothDamp(cameraon.transform.position.y, camerto.transform.position.y, ref velocity.y, CalculateSpeed(TransportCurves)),
                Mathf.SmoothDamp (cameraon.transform.position.z,camerto.transform.position.z ,ref velocity.z , CalculateSpeed(TransportCurves)));
        
            cameraon.transform.localRotation =
                Quaternion.Slerp(cameraon.transform.rotation, camerto.transform.rotation, Time.deltaTime*baseSpeed*CalculateSpeed(RotationCurves));
            if (Mathf.Abs(cameraon.transform.position.z - camerto.transform.position.z) <0.005 &&
                Mathf.Abs(cameraon.transform.position.y - camerto.transform.position.y) <0.005 &&
                Mathf.Abs(cameraon.transform.position.x - camerto.transform.position.x) <0.005)
            {
                isLocated = true;
                yield break;
            }

            yield return null;
        }
    }

    private float CalculateSpeed(AnimationCurve animationCurve)
    {
        float tmp_doneLenth = Mathf.Abs(tmp_oldLocation.z - cameraon.transform.position.z);
        float tmp_Fraction = tmp_doneLenth / totalLeanth;
        return baseSpeed*animationCurve.Evaluate(tmp_Fraction);
    }

    public void ResetPos()
    {
        cameraon.transform.position = SkyLook.transform.position;
        cameraon.transform.rotation = SkyLook.transform.rotation;
    }
}
