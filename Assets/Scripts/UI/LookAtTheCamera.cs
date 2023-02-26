using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class LookAtTheCamera : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject;
    private Camera _camera;
    void Start()
    {
        _camera=Camera.main;
    }
    void Update()
    {
        _gameObject.transform.rotation = Quaternion.LookRotation(_camera.transform.forward);
    }
}