using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class MeshFracture : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private MeshCollider _meshCollider;
    private List<Transform> childs;
    private List<Rigidbody> childRbs;

    private void Awake()
    {
        childs = new List<Transform>();
        childRbs = new List<Rigidbody>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshCollider = GetComponent<MeshCollider>();
        foreach (Transform child in gameObject.transform)
        {
            childs.Add(child);
            childRbs.Add(child.gameObject.GetComponent<Rigidbody>());
            child.gameObject. SetActive(false);
        }
    }

    public void HitMesh(float power, Vector3 explosionPos,float radius)
    {
        hit();
        foreach (Rigidbody rb in childRbs)
        {
            rb.AddExplosionForce(power * 5, explosionPos, radius, 3.0F);
        }
    }
    
    private void hit()
    {
        _meshCollider.enabled = false;
        _meshRenderer.enabled = false;
        foreach (Transform child in childs)
        {
            child.gameObject.SetActive(true);
        }
    }
}
