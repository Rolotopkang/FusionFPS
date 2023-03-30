using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEditor;
using UnityEngine;

public class RagdollController : MonoBehaviourPun
{
    private Animator Animator;
    
    private Collider[] Colliders;
    private Rigidbody[] Rigidbodies;
    
    #region Unity

    private void Awake()
    {
        Colliders = GetComponentsInChildren<Collider>();
        Rigidbodies= GetComponentsInChildren<Rigidbody>();
        Animator = GetComponent<Animator>();
    }

    private void Start()
    {
        SetCollidersEnable(false);
    }

    private void Update()
    {
    }

    #endregion
    
    #region Methods

    /// <summary>
    /// 设置Ragdoll身体碰撞器是否开启
    /// </summary>
    /// <param name="set"></param>
    public void SetCollidersEnable(bool set)
    {
        foreach (Rigidbody rigidbody in Rigidbodies)
        {
            rigidbody.isKinematic = !set;
        }
    }

    public void Death(bool death)
    {
        if (death)
        {
            SetCollidersEnable(true);
            Animator.enabled = false;
        }
    }
    
    public Collider[] GetColliders => Colliders;

    #endregion
}
