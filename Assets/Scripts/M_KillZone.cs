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
    private float killDMG = 1000f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals("Player"))
        {
            PhotonView tmp_PhotonView = other.transform.GetComponent<PhotonView>();
            if (tmp_PhotonView.Owner.Equals(PhotonNetwork.LocalPlayer))
            {
                Dictionary<byte, object> tmp_HitData = new Dictionary<byte, object>();
                //被击中人
                tmp_HitData.Add(0,tmp_PhotonView.Owner);
                //造成伤害人
                tmp_HitData.Add(1,tmp_PhotonView.Owner);
                //造成伤害人武器
                tmp_HitData.Add(2,"suicide");
                //造成伤害
                tmp_HitData.Add(3,killDMG);
                //是否爆头
                tmp_HitData.Add(4,false);
                //伤害来源点
                tmp_HitData.Add(5,Vector3.zero);
                //造成伤害时间戳
                tmp_HitData.Add(6,DateTime.Now.ToUniversalTime().Ticks);
				
                RaiseEventOptions tmp_RaiseEventOptions = new RaiseEventOptions() {Receivers = ReceiverGroup.All};
                SendOptions tmp_SendOptions = SendOptions.SendReliable;
                PhotonNetwork.RaiseEvent(
                    (byte)EventCode.HitPlayer,
                    tmp_HitData,
                    tmp_RaiseEventOptions,
                    tmp_SendOptions);
                Debug.Log("发送击中事件！");
            }
            
        }
    }
}
