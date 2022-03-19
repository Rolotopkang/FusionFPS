using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class AimingCtrl : MonoBehaviour
//, IPunObservable
{
    public Transform FP_Arms;
    public Transform AimTarget;
    public float AimTargetDistance = 5f;
    private Vector3 localPosition;
    private Quaternion localRotation;
    // private PhotonView photonView;

    private void Start()
    {
        // photonView = GetComponent<PhotonView>();
        localPosition = AimTarget.position;
    }

    private void Update()
    {
        // if (photonView.IsMine)
        // {
            localRotation = FP_Arms.localRotation;
            localPosition = localRotation * Vector3.forward * AimTargetDistance;
        // }

        AimTarget.localPosition = Vector3.Lerp(AimTarget.localPosition, new Vector3(localPosition.x,localPosition.y+1.5f,localPosition.z), Time.deltaTime * 20);
    }

    // public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    // {
    //     if (stream.IsWriting)
    //     {
    //         stream.SendNext(localPosition);
    //     }
    //     else
    //     {
    //         localPosition = (Vector3) stream.ReceiveNext();
    //     }
    // }
}