using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Scripts.Weapon;
using UnityEngine;
using EventCode = Scripts.Weapon.EventCode;
using Random = UnityEngine.Random;

public class HitManager : MonoBehaviour, IOnEventCallback
{
    public List<GameObject> ImpactPrefabs;
    public ImpactAudioData ImpactAudioData;
    public float ImpactStayTime;


    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }


    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }


    public void OnEvent(EventData photonEvent)
    {
        switch ((EventCode) photonEvent.Code)
        {
            case EventCode.HitObject:
                var tmp_HitData = (Dictionary<byte, object>) photonEvent.CustomData;
                var tmp_HitPoint = (Vector3) tmp_HitData[0];
                var tmp_HitNormal = (Vector3) tmp_HitData[1];
                var tmp_HitTag = (string) tmp_HitData[2];
                String tmp_ImpactPrefab = (string)tmp_HitData[3];
                bool tmp_TargetHiden = (bool) tmp_HitData[4];
                Player tmp_ProjectileOwner = (Player) tmp_HitData[5];


                //受击者看不见自身出血特效
                //TODO 增加受击UI变化
                if (tmp_TargetHiden)
                {
                    Player tmp_ProjeectileHitTarget = (Player) tmp_HitData[6];
                    if (tmp_ProjeectileHitTarget.NickName.Equals(PhotonNetwork.LocalPlayer.NickName))
                    {
                        return;
                    }
                }
                
                

                GameObject tmp_Impact = null;

                foreach (var prefab in ImpactPrefabs)
                {
                    if (prefab.name.Equals(tmp_ImpactPrefab))
                    {
                        tmp_Impact = prefab;
                    }
                }

                if (tmp_Impact == null)
                {
                    Debug.Log("can't find ImpactPrefab");
                    break;
                }
                
                var tmp_BulletEffect = Instantiate(tmp_Impact, tmp_HitPoint,
                    Quaternion.LookRotation(tmp_HitNormal, Vector3.up));

                Destroy(tmp_BulletEffect, ImpactStayTime);

                var tmp_TagsWithAudio =
                    ImpactAudioData.ImpactTagsWithAudios.Find((_audioData) => _audioData.Tag.Equals(tmp_HitTag));
                if (tmp_TagsWithAudio == null) return;
                int tmp_Length = tmp_TagsWithAudio.ImpactAudioClips.Count;
                AudioClip tmp_AudioClip = tmp_TagsWithAudio.ImpactAudioClips[Random.Range(0, tmp_Length)];
                AudioSource.PlayClipAtPoint(tmp_AudioClip, tmp_HitPoint);
                break;
        }
    }
}