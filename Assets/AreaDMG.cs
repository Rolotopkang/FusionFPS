using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

public class AreaDMG : MonoBehaviour
{

    private SphereCollider _sphereCollider;

    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
    }

    public void Init(float r, Player owner, float dmg)
    {
        _sphereCollider.radius = r;
        
    }

    private void OnCollisionEnter(Collision other)
    {
        
    }
}
