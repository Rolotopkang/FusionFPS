using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDCheckView : MonoBehaviour
{
    [SerializeField]
    private float MaxTimer = 0.3f;

    private HUDSynSystem _hudSynSystem;
    private Renderer _renderer;
    private float timer;
    private float timePass = 0.0f;

    private void Start()
    {
        _hudSynSystem = GetComponentInParent<HUDSynSystem>();
        _renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        //多人可能出错
        // _hudSynSystem._hudNavigationElement.enabled = IsVisibleFrom(_renderer.bounds, Camera.current);
    }
    
    // void CheckVisible ()
    // {
    //     player = GameObject.FindGameObjectWithTag("MainCamera").transform;
    //     if (IsVisibleFrom(_renderer,Camera.main))
    //     {
    //         if (Physics.Linecast(transform.position, player.position, mask))
    //         {
    //             visible = false;
    //             Debug.Log("Obscured");
    //         }
    //         else
    //         {
    //             visible = true;
    //             Debug.Log("Visible");
    //         }
    //     }
    // }
    // void OnWillRenderObject()
    // {
    //     timePass += Time.deltaTime;
    //
    //     if (timePass > 1.0f)
    //     {
    //         timePass = 0.0f;
    //         print(gameObject.name + " is being rendered by " + Camera.current.name + " at " + Time.time);
    //     }
    // }

    // public bool IsVisibleFrom(Bounds bounds, Camera camera)
    // {
    //     try
    //     {
    //         //获取到摄像机视锥的几个面
    //         Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
    //         //检查视锥面和物体边界是否相交
    //         return GeometryUtility.TestPlanesAABB(planes, bounds);
    //     }
    //     catch
    //     {
    //         Debug.Log("不兼容使用GeometryUtility");
    //         return false;
    //     }
    // }

    private void OnWillRenderObject()
    {
        if (!_hudSynSystem.isLocal)
        {

            _hudSynSystem._hudNavigationElement.enabled = true;
        }
    }

    private void OnBecameVisible()
    {
        
    }
    
    private void OnBecameInvisible()
    {
        if (!_hudSynSystem.isLocal)
        {

            _hudSynSystem._hudNavigationElement.enabled = false;
        }
    }


    // private void Update()
    // {
    //     if (timer > 0)
    //     {
    //         timer -= Time.deltaTime;
    //     }
    //     else
    //     {
    //         _hudSynSystem._hudNavigationElement.enabled = false;
    //     }
    // }
    
    public static bool IsVisibleFrom(Renderer renderer, Camera camera)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }
}
