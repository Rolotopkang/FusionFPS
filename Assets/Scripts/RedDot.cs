using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedDot : MonoBehaviour
{
    public Transform CamTransform = null;
    [SerializeField]
    private float size = 0f;
    [SerializeField]
    private float xScale = 1f;
    [SerializeField]
    private Material mat = null;

    private void Start()
    {
        CamTransform = GameObject.FindGameObjectWithTag("GunCM").transform;
    }

    private void LateUpdate()
    {
        Vector3 pos = Vector3.Scale(transform.InverseTransformPoint(CamTransform.position)
            ,transform.localScale);
        Vector2 coord = new Vector2(pos.x / transform.localScale.x / 10f, 
            pos.z / transform.localScale.z / 10f);
        float r = pos.y * size;
        float scaleVal = 1f / r;
        Vector2 scale = new Vector2(scaleVal * xScale, scaleVal);
        float texOffset = 0.5f * (1f - r);
        Vector2 offset = Vector2.Scale(coord - new Vector2(texOffset, texOffset), scale);
        mat.SetTextureScale("_MainTex",scale);
        mat.SetTextureOffset("_MainTex",offset);
    }
}
