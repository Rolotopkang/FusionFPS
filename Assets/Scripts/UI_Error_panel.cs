using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_Error_panel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI WrongHint;

    [SerializeField]
    private TextMeshProUGUI ButtonHint;
    
    [SerializeField]
    private Button _button;



    public void OnExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Init(String wrongHint, String buttonHint, UnityAction Onresponse)
    {
        WrongHint.text = wrongHint;
        ButtonHint.text = buttonHint;
        _button.onClick.AddListener(Onresponse);
    }
    
}
