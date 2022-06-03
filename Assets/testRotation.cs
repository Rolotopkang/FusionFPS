using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testRotation : MonoBehaviour
{
    private void Update()
    {
        Debug.Log(Camera.current.transform.forward);
    }
}
