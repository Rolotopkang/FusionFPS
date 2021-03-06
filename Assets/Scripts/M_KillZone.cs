using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using EventCode = Scripts.Weapon.EventCode;

public class M_KillZone : MonoBehaviour
{
    public float DeathTime = 0;

    public bool reverse = false;

    public float killDMG = 1000f;

    private Coroutine DeathTimerCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals("Player"))
        {
            PhotonView tmp_PhotonView = other.transform.GetComponent<PhotonView>();
            if (tmp_PhotonView.Owner.Equals(PhotonNetwork.LocalPlayer))
            {
                if (reverse)
                {
                    if (DeathTimerCoroutine!=null)
                    {
                        PostProcessingManager.GetInstance().PostProcessings.PP_DeathZone.SetActive(false);
                        StopCoroutine(DeathTimerCoroutine);
                        OutOfBoundWarningUIManager.StopCountDown();
                        DeathTimerCoroutine = null;
                    }
                }
                else
                {
                    DeathTimerCoroutine = StartCoroutine(DeathTimer());
                }
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag.Equals("Player"))
        {
            PhotonView tmp_PhotonView = other.transform.GetComponent<PhotonView>();
            if (tmp_PhotonView.Owner.Equals(PhotonNetwork.LocalPlayer))
            {
                if (!reverse)
                {
                    if (DeathTimerCoroutine!=null)
                    {
                        PostProcessingManager.GetInstance().PostProcessings.PP_DeathZone.SetActive(false);
                        StopCoroutine(DeathTimerCoroutine);
                        OutOfBoundWarningUIManager.StopCountDown();
                        DeathTimerCoroutine = null;
                    }
                }
                else
                {
                    DeathTimerCoroutine = StartCoroutine(DeathTimer());
                }
            }
            
        }
    }


    private IEnumerator DeathTimer()
    {
        //???????????????
        PostProcessingManager.GetInstance().PostProcessings.PP_DeathZone.SetActive(true);
        
        //??????UI
        OutOfBoundWarningUIManager.StartCountDown(DeathTime);
        Debug.Log("???????????????");
        
        yield return new WaitForSeconds(DeathTime);

        Sentenced();
    }

    public void DEBUGSentenced()
    {
        Sentenced();
    }
    
    private void Sentenced()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties[EnumTools.PlayerProperties.IsDeath.ToString()].Equals(true))
        {
            Debug.Log("??????????????????");
            return;
        }
        
        Dictionary<byte, object> tmp_HitData = new Dictionary<byte, object>();
        //????????????
        tmp_HitData.Add(0,PhotonNetwork.LocalPlayer);
        //???????????????
        tmp_HitData.Add(1,PhotonNetwork.LocalPlayer);
        //?????????????????????
        tmp_HitData.Add(2,"suicide");
        //????????????
        tmp_HitData.Add(3,killDMG);
        //????????????
        tmp_HitData.Add(4,false);
        //???????????????
        tmp_HitData.Add(5,Vector3.zero);
        //?????????????????????
        tmp_HitData.Add(6,DateTime.Now.ToUniversalTime().Ticks);
				
        RaiseEventOptions tmp_RaiseEventOptions = new RaiseEventOptions() {Receivers = ReceiverGroup.All};
        SendOptions tmp_SendOptions = SendOptions.SendReliable;
        PhotonNetwork.RaiseEvent(
            (byte)EventCode.HitPlayer,
            tmp_HitData,
            tmp_RaiseEventOptions,
            tmp_SendOptions);
        Debug.Log("?????????????????????");
    }
}
