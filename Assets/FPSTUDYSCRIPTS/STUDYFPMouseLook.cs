using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STUDYFPMouseLook : MonoBehaviour
{
    [SerializeField] private Transform characterTransform;
    public float MouseSensitivity;
    public Vector2 MaxMinAngle;
    
    private Transform cmTransform;
    private Vector3 cmRotation;
    private void Start()
    {
        cmTransform = transform;
    }

    private void Update()
    {
        var tmp_MouseX = Input.GetAxis("Mouse X");
        var tmp_MouseY = Input.GetAxis("Mouse Y");
        cmRotation.x -= tmp_MouseY * MouseSensitivity;
        cmRotation.y += tmp_MouseX * MouseSensitivity;

        cmRotation.x = Mathf.Clamp(cmRotation.x, MaxMinAngle.x, MaxMinAngle.y);
        
        cmTransform.rotation = Quaternion.Euler(cmRotation.x,cmRotation.y,0);
        
        characterTransform.rotation = Quaternion.Euler(0,cmRotation.y,0);
    }
    
}
