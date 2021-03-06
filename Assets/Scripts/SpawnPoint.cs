using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] 
    private GameObject graphics;
    
    
    private bool isOpen =true;

    private void Awake()
    {
        foreach (var mesh in graphics.GetComponentsInChildren<MeshRenderer>())
        {
            mesh.enabled = false;
        }
    }
    

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            isOpen = true;
        }
    }
    
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            isOpen = false;
        }
    }

    public bool GetIsOpen() => isOpen;
    
}
