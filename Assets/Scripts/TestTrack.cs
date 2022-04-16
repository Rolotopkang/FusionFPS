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

    private PlayerManager PlayerManager;

    public bool isLocated = false;
    

    public void Dochange(GameObject target,PlayerManager playerManager)
    {
        camerto = target;
        isLocated = false;
        PlayerManager = playerManager;
    }

    public void Dochange(GameObject target)
    {
        camerto = target;
        isLocated = false;
    }

    private void Update()
    {
        // isLocated = (Mathf.Abs(cameraon.transform.position.x - camerto.transform.position.x) < 0.1) &&
        //             (Mathf.Abs(cameraon.transform.position.y - camerto.transform.position.y) < 0.1) &&
        //             (Mathf.Abs(cameraon.transform.position.z - camerto.transform.position.z) < 0.1);

        
        if (!isLocated)
        {
            cameraon.transform.position = new Vector3(Mathf.SmoothDamp(cameraon.transform.position.x, camerto.transform.position.x, 
                ref velocity.x, speed), Mathf.SmoothDamp(cameraon.transform.position.y, camerto.transform.position.y,
                ref velocity.y, speed),Mathf.SmoothDamp (cameraon.transform.position.z,camerto.transform.position.z ,ref velocity.z , speed));
            
            cameraon.transform.localRotation =
                Quaternion.Slerp(cameraon.transform.rotation, camerto.transform.rotation, Time.deltaTime*speed*4);
        }

        Debug.Log(cameraon.transform.position+"目前" +  camerto.transform.position+"目标");
        if (camerto)
        {
            isLocated = Mathf.Abs(cameraon.transform.position.z - camerto.transform.position.z) < 0.1;
        }
    }

}
