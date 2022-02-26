using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Scripts.Items;
using Scripts.Weapon;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class WeaponInfo
{
    public int WeaponId;
    public string WeaponName;
    public Firearms FP_Weapon;
    public Firearms TP_Weapon;
}


public class WeaponManager : MonoBehaviour
{
    public Firearms MainWeapon;
    public Firearms SecondaryWeapon;
    public Text AmmoCountTextLabel;

    private Firearms carriedWeapon;

    [SerializeField] private List<WeaponInfo> WeaponInfos;

    [SerializeField] private FPCharacterControllerMovement CharacterControllerMovement;

    private AnimatorStateInfo animationStateInfo;
    private IEnumerator waitingForHolsterEndCoroutine;
    private PhotonView photonView;

    public List<Firearms> Arms = new List<Firearms>();
    public Transform WorldCameraTransform;
    public float RaycastMaxDistance = 2;
    public LayerMask CheckItemLayerMask;


    private void UpdateAmmoInfo(int _ammo, int _remaningAmmo)
    {
        if (AmmoCountTextLabel)
            AmmoCountTextLabel.text = _ammo + "/" + _remaningAmmo;
    }


    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        Debug.Log($"Current weapon is null? {carriedWeapon == null}");

        MainWeapon = photonView.IsMine ? WeaponInfos[0].FP_Weapon : WeaponInfos[0].TP_Weapon;


        if (MainWeapon)
        {
            carriedWeapon = MainWeapon;
            CharacterControllerMovement.SetupAnimator(carriedWeapon.GunAnimator);
        }
    }

    private void Update()
    {
        if (!photonView.IsMine) return;
        CheckItem();

        if (!carriedWeapon) return;


        SwapWeapon();

        if (Input.GetMouseButton(0))
        {
            //TODO:hold the Trigger
            //carriedWeapon.HoldTrigger();
            photonView.RPC("RPC_HoldTrigger", RpcTarget.All);
        }

        if (Input.GetMouseButtonUp(0))
        {
            //TODO: release the Trigger
            //carriedWeapon.ReleaseTrigger();
            photonView.RPC("RPC_ReleaseTrigger", RpcTarget.All);
        }


        if (Input.GetKeyDown(KeyCode.R))
        {
            //TODO:Reloading the ammo
            //carriedWeapon.ReloadAmmo();
            photonView.RPC("RPC_ReloadAmmo", RpcTarget.All);
        }

        if (Input.GetMouseButtonDown(1))
        {
            //TODO:瞄准
            carriedWeapon.Aiming(true);
        }

        if (Input.GetMouseButtonUp(1))
        {
            //TODO:退出瞄准
            carriedWeapon.Aiming(false);
        }

        UpdateAmmoInfo(carriedWeapon.GetCurrentAmmo, carriedWeapon.GetCurrentMaxAmmoCarried);
    }


    private void CheckItem()
    {
        bool tmp_IsItem = Physics.Raycast(WorldCameraTransform.position,
            WorldCameraTransform.forward,
            out RaycastHit tmp_RaycastHit,
            RaycastMaxDistance, CheckItemLayerMask);


        if (tmp_IsItem)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                bool tmp_HasItem = tmp_RaycastHit.collider.TryGetComponent(out BaseItem tmp_BaseItem);
                if (tmp_HasItem)
                {
                    PickupWeapon(tmp_BaseItem);
                    PickupAttachment(tmp_BaseItem);
                }
            }
        }
    }


    private void PickupWeapon(BaseItem _baseItem)
    {
        if (!(_baseItem is FirearmsItem tmp_FirearmsItem)) return;
        foreach (Firearms tmp_Arm in Arms)
        {
            if (tmp_FirearmsItem.ArmsName.CompareTo(tmp_Arm.name) != 0) continue;
            switch (tmp_FirearmsItem.CurrentFirearmsType)
            {
                case FirearmsItem.FirearmsType.AssultRefile:
                    MainWeapon = tmp_Arm;
                    break;
                case FirearmsItem.FirearmsType.HandGun:
                    SecondaryWeapon = tmp_Arm;

                    break;
            }

            SetupCarriedWeapon(tmp_Arm);
        }
    }


    private void PickupAttachment(BaseItem _baseItem)
    {
        if (!(_baseItem is AttachmentItem tmp_AttachmentItem)) return;

        switch (tmp_AttachmentItem.CurrentAttachmentType)
        {
            case AttachmentItem.AttachmentType.Scope:
                foreach (ScopeInfo tmp_ScopeInfo in carriedWeapon.ScopeInfos)
                {
                    if (tmp_ScopeInfo.ScopeName.CompareTo(tmp_AttachmentItem.ItemName) != 0)
                    {
                        tmp_ScopeInfo.ScopeGameObject.SetActive(false);
                        continue;
                    }

                    tmp_ScopeInfo.ScopeGameObject.SetActive(true);
                    carriedWeapon.BaseIronSight.ScopeGameObject.SetActive(false);
                    carriedWeapon.SetupCarriedScope(tmp_ScopeInfo);
                }

                break;
            case AttachmentItem.AttachmentType.Other:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }


    private void SwapWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (MainWeapon == null) return;
            //更换为主武器
            if (carriedWeapon == MainWeapon) return;
            if (carriedWeapon.gameObject.activeInHierarchy)
            {
                StartWaitingForHolsterEndCoroutine();
                carriedWeapon.GunAnimator.SetTrigger("holster");
            }
            else
            {
                SetupCarriedWeapon(MainWeapon);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (SecondaryWeapon == null) return;
            //更换为副武器
            if (carriedWeapon == SecondaryWeapon) return;
            if (carriedWeapon.gameObject.activeInHierarchy)
            {
                StartWaitingForHolsterEndCoroutine();
                carriedWeapon.GunAnimator.SetTrigger("holster");
            }
            else
            {
                SetupCarriedWeapon(SecondaryWeapon);
            }
        }
    }


    private void StartWaitingForHolsterEndCoroutine()
    {
        if (waitingForHolsterEndCoroutine == null)
            waitingForHolsterEndCoroutine = WaitingForHolsterEnd();
        StartCoroutine(waitingForHolsterEndCoroutine);
    }


    private IEnumerator WaitingForHolsterEnd()
    {
        while (true)
        {
            AnimatorStateInfo tmp_AnimatorStateInfo = carriedWeapon.GunAnimator.GetCurrentAnimatorStateInfo(0);
            if (tmp_AnimatorStateInfo.IsTag("holster"))
            {
                if (tmp_AnimatorStateInfo.normalizedTime >= 0.9f)
                {
                    var tmp_TargetWeapon = carriedWeapon == MainWeapon ? SecondaryWeapon : MainWeapon;
                    SetupCarriedWeapon(tmp_TargetWeapon);
                    waitingForHolsterEndCoroutine = null;
                    yield break;
                }
            }

            yield return null;
        }
    }


    private void SetupCarriedWeapon(Firearms _targetWeapon)
    {
        if (carriedWeapon)
            carriedWeapon.gameObject.SetActive(false);
        carriedWeapon = _targetWeapon;
        carriedWeapon.gameObject.SetActive(true);
        CharacterControllerMovement.SetupAnimator(carriedWeapon.GunAnimator);
    }

    [PunRPC]
    private void RPC_HoldTrigger()
    {
        carriedWeapon.HoldTrigger();
    }

    [PunRPC]
    private void RPC_ReleaseTrigger()
    {
        carriedWeapon.ReleaseTrigger();
    }

    [PunRPC]
    private void RPC_ReloadAmmo()
    {
        carriedWeapon.ReloadAmmo();
    }
}