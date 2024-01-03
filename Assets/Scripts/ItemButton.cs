using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public bool isDown;
    [SerializeField] private String ItemName;

    public String ShowName;

    private Image image;


    private Sprite BTB;

    private Sprite BTD;

    public Sprite BTDS;

    private DeployManager DeployManager;


    private void Awake()
    {
        image = GetComponent<Image>();
        DeployManager = GetComponentInParent<DeployManager>();
    }

    public void Init(String _ItemName, String _ShowName, Sprite _DeployB, Sprite _DeployD, Sprite _BTD)
    {
        ItemName = _ItemName;
        ShowName = _ShowName;
        BTB = _DeployB;
        BTD = _DeployD;
        BTDS = _BTD;
    }

    private void Update()
    {
        image.sprite = isDown? BTB : BTD;
        // image.color = isDown? Color.blue : Color.black;
    }
    
    
    public void SetMainisDown(bool set)
    {
        DeployManager.SetMainIsChecked();
        isDown = set;
    }
    
    public void SetSecisDown(bool set)
    {
        DeployManager.SetSecIsChecked();
        isDown = set;
    }

    public void SetGrenadeDown(bool set)
    {
        DeployManager.SetGrenadeChecked();
        isDown = set;
    }

    public void SetItemDown(bool set)
    {
        DeployManager.SetItemChecked();
        isDown = set;
    }

    public String GetItemName() => ItemName;
}
