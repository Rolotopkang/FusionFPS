using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class TestTrack : MonoBehaviour
{
    public GameObject cameraon;//初始摄像机的位置
    public GameObject camerto;//另一个点的位置
    private float speed =0.5f;//缓冲的时间  时间越大缓冲速度越慢
    private Vector3 velocity;//如果是3D场景就用Vector3,2D用Vector2
    

    public void Dochange(GameObject target)
    {
        camerto = target;
        StartCoroutine(EXChange().GetEnumerator());
    }

    private IEnumerable EXChange()
    {
        while (Mathf.Abs(cameraon.transform.position.z-camerto.transform.position.z)>0.01)
        {
            Debug.Log("执行！");
            cameraon.transform.position = new Vector3(Mathf.SmoothDamp(cameraon.transform.position.x, camerto.transform.position.x,
                ref velocity.x, speed), Mathf.SmoothDamp(cameraon.transform.position.y, camerto.transform.position.y,
                ref velocity.y, speed),Mathf.SmoothDamp (cameraon.transform.position.z,camerto.transform.position.z ,ref velocity.z , speed));
            
            cameraon.transform.localRotation =
                Quaternion.Slerp(cameraon.transform.rotation, camerto.transform.rotation, Time.deltaTime*speed*4);
        }

        // cameraon.SetActive(false);
        yield return null;
    }

}
