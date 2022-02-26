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
    public GameObject ImpactPrefab;
    public ImpactAudioData ImpactAudioData;


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


                var tmp_BulletEffect = Instantiate(ImpactPrefab, tmp_HitPoint,
                    Quaternion.LookRotation(tmp_HitNormal, Vector3.up));

                Destroy(tmp_BulletEffect, 3);

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