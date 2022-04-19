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
    private Renderer[] Renderers;
    private RigBuilder RigBuilder;

    private void Awake()
    {
        Renderers = GetComponentsInChildren<Renderer>();
        RigBuilder = GetComponent<RigBuilder>();
    }

    public void SetRenderers(ShadowCastingMode shadowCastingMode)
    {
        foreach (Renderer renderer in Renderers)
        {
            renderer.shadowCastingMode = shadowCastingMode;
        }
    }

    public void SetRenderersDisable(bool set)
    {
        foreach (Renderer renderer in Renderers)
        {
            renderer.enabled = set;
        }
    }
}
