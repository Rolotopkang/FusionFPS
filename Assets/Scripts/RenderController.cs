using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Rendering;

public class RenderController : MonoBehaviour
{
    [Header("是否只渲染阴影")]
    [SerializeField]
    [Tooltip("是否开启网格渲染")]
    private bool isShadowOnly = true;
    private Renderer[] Renderers;
    private RigBuilder RigBuilder;
    private void Start()
    {
        Renderers = GetComponentsInChildren<Renderer>();
        RigBuilder = GetComponent<RigBuilder>();
        SetRenderers(isShadowOnly);
    }

    public void SetRenderers(bool isShadowOnly)
    {
        if (isShadowOnly)
        {
            foreach (Renderer renderer in Renderers)
            {
                renderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
            }
        }
        else
        {
            foreach (Renderer renderer in Renderers)
            {
                renderer.shadowCastingMode = ShadowCastingMode.On;
            }
        }
    }
}
