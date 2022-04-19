using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public bool isDown;
    [SerializeField]
    private String ItemName;

    public String ShowName;

    private Image image;
    

    public Sprite BTB;

    public Sprite BTD;

    public Sprite BTDS;

    private DeployManager DeployManager;
    

    private void Awake()
    {
        image = GetComponent<Image>();
        DeployManager = GetComponentInParent<DeployManager>();
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

    public String GetItemName() => ItemName;
}
