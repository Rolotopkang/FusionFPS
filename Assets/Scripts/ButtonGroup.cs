using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InfimaGames.LowPolyShooterPack;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGroup : MonoBehaviour
{
    [Header("按钮组的种类")]
    [SerializeField]
    private ScopeChangerBTN.AttachmentKind AttachmentKind;

    [Header("资源")]
    [SerializeField] 
    private GameObject BTNPrefab;
    
    private int currentBTN = 0;
    
    private List<ScopeChangerBTN> ButtonList;
    private ScopeBehaviour[] ScopeBehaviours;
    private MuzzleBehaviour[] MuzzleBehaviours;
    private GripBehaviour[] GripBehaviours;
    private MagazineBehaviour[] magazineBehaviours;
    private WeaponAttachmentManager WeaponAttachmentManager;
    private RectTransform _rectTransform;
    
    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        WeaponAttachmentManager = GetComponentInParent<WeaponAttachmentManager>();
        GetAttachmentGroup();
        ButtonList = transform.GetComponentsInChildren<ScopeChangerBTN>().ToList();
        ButtonList[0].SetIsChecked(true);
    }

    private void GetAttachmentGroup()
    {
        switch (AttachmentKind)
        {
            case ScopeChangerBTN.AttachmentKind.Scope:
                ScopeBehaviours = WeaponAttachmentManager.GetScopeBehaviours();
                
                foreach (ScopeBehaviour scopeBehaviour in ScopeBehaviours)
                {
                    GameObject tmp_BTN = Instantiate(BTNPrefab, new Vector3(0,0,_rectTransform.position.z), quaternion.identity, transform);
                    tmp_BTN.GetComponent<RectTransform>().localRotation = Quaternion.identity;
                    tmp_BTN.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
                    ScopeChangerBTN tmp_ScopeChangerBTN = tmp_BTN.GetComponent<ScopeChangerBTN>();
                    tmp_ScopeChangerBTN.SetButtonIMGB(scopeBehaviour.GetBTNSpriteB());
                    tmp_ScopeChangerBTN.SetButtonIMGD(scopeBehaviour.GetBTNSpriteD());
                    tmp_ScopeChangerBTN.attachmentKind = ScopeChangerBTN.AttachmentKind.Scope;
                    tmp_ScopeChangerBTN.attachmentID = scopeBehaviour.GetID();
                }
                
                break;
            case ScopeChangerBTN.AttachmentKind.Muzzle:
                MuzzleBehaviours = WeaponAttachmentManager.GetMuzzleBehaviours();
                foreach (MuzzleBehaviour muzzleBehaviour in MuzzleBehaviours)
                {
                    GameObject tmp_BTN = Instantiate(BTNPrefab, new Vector3(0,0,_rectTransform.position.z), quaternion.identity, transform);
                    tmp_BTN.GetComponent<RectTransform>().localRotation = Quaternion.identity;
                    tmp_BTN.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
                    ScopeChangerBTN tmp_ScopeChangerBTN = tmp_BTN.GetComponent<ScopeChangerBTN>();
                    tmp_ScopeChangerBTN.SetButtonIMGB(muzzleBehaviour.GetBTNSpriteB());
                    tmp_ScopeChangerBTN.SetButtonIMGD(muzzleBehaviour.GetBTNSpriteD());
                    tmp_ScopeChangerBTN.attachmentKind = ScopeChangerBTN.AttachmentKind.Muzzle;
                    tmp_ScopeChangerBTN.attachmentID = muzzleBehaviour.GetID();
                }
                break;
            case ScopeChangerBTN.AttachmentKind.Magazine:
                magazineBehaviours = WeaponAttachmentManager.GetMagazineBehaviours();
                foreach (MagazineBehaviour magazineBehaviour in magazineBehaviours)
                {
                    GameObject tmp_BTN = Instantiate(BTNPrefab,new Vector3(0,0,_rectTransform.position.z), quaternion.identity, transform);
                    tmp_BTN.GetComponent<RectTransform>().localRotation = Quaternion.identity;
                    tmp_BTN.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
                    ScopeChangerBTN tmp_ScopeChangerBTN = tmp_BTN.GetComponent<ScopeChangerBTN>();
                    tmp_ScopeChangerBTN.SetButtonIMGB(magazineBehaviour.GetBTNSpriteB());
                    tmp_ScopeChangerBTN.SetButtonIMGD(magazineBehaviour.GetBTNSpriteD());
                    tmp_ScopeChangerBTN.attachmentKind = ScopeChangerBTN.AttachmentKind.Magazine;
                    tmp_ScopeChangerBTN.attachmentID = magazineBehaviour.GetID();
                }
                break;
            case ScopeChangerBTN.AttachmentKind.Grip:
                GripBehaviours = WeaponAttachmentManager.GetGripBehaviours();
                foreach (GripBehaviour gripBehaviour in GripBehaviours)
                {
                    GameObject tmp_BTN = Instantiate(BTNPrefab, new Vector3(0,0,_rectTransform.position.z), quaternion.identity, transform);
                    tmp_BTN.GetComponent<RectTransform>().localRotation = Quaternion.identity;
                    tmp_BTN.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
                    ScopeChangerBTN tmp_ScopeChangerBTN = tmp_BTN.GetComponent<ScopeChangerBTN>();
                    tmp_ScopeChangerBTN.SetButtonIMGB(gripBehaviour.GetBTNSpriteB());
                    tmp_ScopeChangerBTN.SetButtonIMGD(gripBehaviour.GetBTNSpriteD());
                    tmp_ScopeChangerBTN.attachmentKind = ScopeChangerBTN.AttachmentKind.Grip;
                    tmp_ScopeChangerBTN.attachmentID = gripBehaviour.GetID();
                }
                break;
            default:
                Debug.Log("wrong");
                break;
        }
    }

    public void BTN_Pressed()
    {
        foreach (ScopeChangerBTN btn in ButtonList)
        {
            btn.SetIsChecked(false);
        }
    }
}
