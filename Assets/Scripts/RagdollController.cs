using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    public bool testDeath;
    private Animator Animator;
    
    private Collider[] Colliders;
    private Rigidbody[] Rigidbodies;
    
    #region Unity
    private void Start()
    {
        Colliders = GetComponentsInChildren<Collider>();
        Rigidbodies= GetComponentsInChildren<Rigidbody>();
        Animator = GetComponent<Animator>();
        SetCollidersEnable(false);
    }

    private void Update()
    {
        if (testDeath)
        {
            SetCollidersEnable(true);
            
            Animator.enabled = false;
        }
    }

    #endregion


    #region Methods

    /// <summary>
    /// 设置Ragdoll身体碰撞器是否开启
    /// </summary>
    /// <param name="set"></param>
    public void SetCollidersEnable(bool set)
    {
        foreach (Collider collider in Colliders)
        { 
            collider.enabled = set;
        }

        foreach (Rigidbody rigidbody in Rigidbodies)
        {
            rigidbody.isKinematic = !set;
        }
    }
    
    #endregion
}
