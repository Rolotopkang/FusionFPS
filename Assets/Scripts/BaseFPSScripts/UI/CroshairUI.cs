using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Weapon;
using UnityEngine;

public class CroshairUI : MonoBehaviour
{
    public RectTransform Reticle;
    public CharacterController CharacterController;
    public WeaponManager WeaponManager;


    public float OriginalSize;
    public float TargetSize;
    public float ReturnTime = 3;


    private float currentSize;

    private void Start()
    {
        currentSize = OriginalSize;
    }

    private void Update()
    {
        bool tmp_IsMoving = CharacterController.velocity.magnitude > 0;


        if (WeaponManager.isAiming)
        {
            Reticle.transform.parent.gameObject.SetActive(false);
        }else
        {
            Reticle.transform.parent.gameObject.SetActive(true);
        }
        
        if (tmp_IsMoving)
        {
            currentSize = Mathf.Lerp(currentSize, TargetSize, Time.deltaTime * ReturnTime);
        }
        else
        {
            currentSize = Mathf.Lerp(currentSize, OriginalSize, Time.deltaTime * ReturnTime);
        }

        Reticle.sizeDelta = new Vector2(currentSize, currentSize);
    }
}