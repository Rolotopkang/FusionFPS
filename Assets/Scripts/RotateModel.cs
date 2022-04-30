using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RotateModel : MonoBehaviour
{
    [SerializeField]
    private float rotateScale = 1f;
    public int yMinLimit = -20;
    public int yMaxLimit = 80;
    public Transform modelTransform;


    
    private bool isRotate;
    private Vector3 startPoint;
    private Vector3 startAngel;
    private Vector2 axisPos;
    
    private float x = 0.0f;
    private float y = 0.0f;


    private bool mouseLeftBTNDown;

    private void Awake()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if (mouseLeftBTNDown && !isRotate)
        {
            isRotate = true;
            startPoint = axisPos;
            Debug.Log("startpoint"+startPoint);
            startAngel = modelTransform.eulerAngles;
        }

        if (!mouseLeftBTNDown)
        {
            isRotate = false;
        }

        if (isRotate)
        {
            var currentPoint = axisPos;
            x -=  currentPoint.x*rotateScale;
            y -=  currentPoint.y*rotateScale;
            
            y = ClampAngle(y, yMinLimit, yMaxLimit);

            Quaternion rotation = Quaternion.Euler(y,x,0);
            modelTransform.rotation = rotation;
            // modelTransform.eulerAngles = startAngel + new Vector3(y, x, 0) * rotateScale;
        }
        
    }

    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

    
    public void OnTryMouseLeft(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                //Started.
                mouseLeftBTNDown = true;
                break;
            case InputActionPhase.Canceled:
                //Canceled.
                mouseLeftBTNDown = false;
                break;
        }
    }

    public void OnMouseMove(InputAction.CallbackContext context)
    {
        axisPos = context.ReadValue<Vector2>();
    }
}
