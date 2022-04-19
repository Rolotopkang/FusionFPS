using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class testinst : MonoBehaviour
{
    public GameObject test;
    public Transform point;

    private void Start()
    {
        Invoke(nameof(inst),5f);
    }

    void inst()
    {
        Debug.Log("开始生成");
        Instantiate(test,point.position,quaternion.identity);
        Debug.Log("生成完毕");
    }
}
