using System;
using System.Collections;
using System.Collections.Generic;
using InfimaGames.LowPolyShooterPack;
using TMPro;
using UnityEngine;

public class UI_AttachmentChanger_Info : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI WepName;
    [SerializeField]
    private TextMeshProUGUI ScopeName;
    [SerializeField]
    private TextMeshProUGUI MuzzleName;
    [SerializeField]
    private TextMeshProUGUI GripName;
    [SerializeField]
    private TextMeshProUGUI MagazineName;
    
    private WeaponAttachmentManager WeaponAttachmentManager;
    private CharacterBehaviour CharacterBehaviour;

    private void Start()
    {
        WeaponAttachmentManager = GetComponentInParent<WeaponAttachmentManager>();
        CharacterBehaviour = GetComponentInParent<CharacterBehaviour>();
    }

    private void Update()
    {
        WepName.text = CharacterBehaviour.GetInventory().GetEquipped().GetWeaponName();
        ScopeName.text = WeaponAttachmentManager.GetEquippedScope().GetName();
        MuzzleName.text = WeaponAttachmentManager.GetEquippedMuzzle().GetName();
        GripName.text = WeaponAttachmentManager.GetEquippedGrip().GetName();
        MagazineName.text = WeaponAttachmentManager.GetEquippedMagazine().GetName();
    }
}
