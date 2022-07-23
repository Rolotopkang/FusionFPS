using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConquestPoint_Deploy : MonoBehaviour
{
    /// <summary>
    /// 是否被选中
    /// </summary>
    public bool isSelected;
    
    [SerializeField]
    private EnumTools.ConquestPoints conquestPoint;
    public EnumTools.ConquestPoints GetConquestPoint => conquestPoint;
    [SerializeField]
    private GameObject selectedIMG;

    [SerializeField] 
    private Button deployPointButton;

    private ConquestPoint_UI_Point _conquestPointUIPoint;

    private void Start()
    {
        selectedIMG.SetActive(false);
        deployPointButton.enabled = true;
        _conquestPointUIPoint = GetComponent<ConquestPoint_UI_Point>();
    }

    private void Update()
    {
        deployPointButton.enabled = _conquestPointUIPoint.GetCanDeploy;
        selectedIMG.SetActive(isSelected);
    }

    public void OnButtonPressed()
    {
        if (!isSelected)
        {
            DeployChoiceManager.GetInstance().SetSelectedPoint(this);
        }
        else
        {
            //部署
            Debug.Log("征服通过点位按钮部署！");
            DeployChoiceManager.GetInstance().DeployAtChoicePoint();
        }
    }
    
}
