using System;
using System.Collections;
using System.Collections.Generic;
using InfimaGames.LowPolyShooterPack;
using UnityEngine;
using UnityEngine.UI;

public class ScopeChangerBTN : MonoBehaviour
{
    #region ENUM
    public enum AttachmentKind
    {
        Scope,
        Muzzle,
        Magazine,
        Grip
    }
    #endregion


    #region FIELDS SERIALIZED
    
    [Header("配件按钮信息")]
    [Tooltip("配件种类")]
    [SerializeField]
    public AttachmentKind attachmentKind;

    [Tooltip("配件ID")]
    [SerializeField]
    public int attachmentID;

    [Header("资源")]


    
    #endregion

    #region FIELDS

    private bool isChecked =false;
    
    private Sprite ButtonIMGB;
    
    private Sprite ButtonIMGD;

    private Button Button;
    
    private Image Image;

    private RectTransform _rectTransform;

    private ButtonGroup ButtonGroup;

    private CharacterBehaviour CharacterBehaviour;

    #endregion

    #region Unity

    private void Start()
    {
        Button = GetComponent<Button>();
        Image = GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();
        ButtonGroup = transform.parent.GetComponent<ButtonGroup>();
        CharacterBehaviour = GetComponentInParent<CharacterBehaviour>();
        Image.sprite = ButtonIMGD;
    }

    private void Update()
    {
        tick();
    }

    #endregion


    #region Functions

    private void tick()
    {
        Image.sprite = isChecked ? ButtonIMGB : ButtonIMGD;
        // _rectTransform.position = new Vector3(_rectTransform.position.x, _rectTransform.position.y,isChecked ?-10 :0);
    }


    #endregion

    #region Funtions

    public void BTN_Pressed()
    {
        ButtonGroup.BTN_Pressed();
        isChecked = true;
        CharacterBehaviour.ChangeAttachment(attachmentKind,attachmentID);
    }
    

    #endregion
    
    #region Getter
    
    public bool GetIsChecked() => isChecked;

    #endregion

    #region Setter

    public void SetIsChecked(bool set)
    {
        isChecked = set;
    }
    
    public void SetButtonIMGB(Sprite set)
    {
        ButtonIMGB = set;
    }
    
    public void SetButtonIMGD(Sprite set)
    {
        ButtonIMGD = set;
    }

    #endregion
}

