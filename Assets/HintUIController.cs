using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HintUIController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI TextMeshPro;

    private void Update()
    {
        TextMeshPro.alpha = Mathf.PingPong(Time.time, 1);
    }
    
    public void onTrySpace(InputAction.CallbackContext context)
    {
        //Switch.
        switch (context)
        {
            case {phase: InputActionPhase.Performed}:
                Destroy(gameObject);
                break;
        }
    }
}
