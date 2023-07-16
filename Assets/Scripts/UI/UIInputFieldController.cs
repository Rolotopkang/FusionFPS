using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInputFieldController : MonoBehaviour
{
    /// <summary>
    /// Define if the place holder is hide on selection
    /// </summary>
    [SerializeField]
    private bool _isPlaceholderHideOnSelect;
    
    private TMP_InputField _inputField;

    private void Awake()
    {
        _inputField = GetComponent<TMP_InputField>();
    }

    /// <summary>
    /// The input field select
    /// </summary>
    public void OnInputFieldSelect()
    {
        if (this._isPlaceholderHideOnSelect == true)
        {
            this._inputField.placeholder.gameObject.SetActive(false);
        }
    }
 
    /// <summary>
    /// The input field deselect
    /// </summary>
    public void OnInputFieldDeselect()
    {
        if (this._isPlaceholderHideOnSelect == true)
        {
            this._inputField.placeholder.gameObject.SetActive(true);
        }
    }
}