using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityTemplateProjects.MultiplayerScripts;
using Random = UnityEngine.Random;

namespace Scripts.Weapon
{
    public class Bullet : MonoBehaviour
    {
        public float BulletSpeed;
        public GameObject ImpactPrefab;
        
        private Transform bulletTransform;
        private Vector3 prevPosition;
        

        private void Start()
        {
            bulletTransform = transform;
            prevPosition = bulletTransform.position;
        }

        private void Update()
        {
            prevPosition = bulletTransform.position;

            bulletTransform.Translate(0, 0, BulletSpeed * Time.deltaTime);

            if (!Physics.Raycast(prevPosition,
                (bulletTransform.position - prevPosition).normalized,
                out RaycastHit tmp_Hit,
                (bulletTransform.position - prevPosition).magnitude))
            {
                return;
            }

            
            
            if (tmp_Hit.collider.TryGetComponent(out IDamager tmp_Damager))
            {
                tmp_Damager.TakeDamage(10);
            }
            else
            {
                Instantiate(ImpactPrefab, tmp_Hit.point, Quaternion.LookRotation(tmp_Hit.normal, Vector3.up));
            }


            Dictionary<byte, object> tmp_HitData = new Dictionary<byte, object>();
            tmp_HitData.Add(0, tmp_Hit.point);
            tmp_HitData.Add(1, tmp_Hit.normal);
            tmp_HitData.Add(2, tmp_Hit.collider.tag);
            

            RaiseEventOptions tmp_RaiseEventOptions = new RaiseEventOptions() {Receivers = ReceiverGroup.All};
            SendOptions tmp_SendOptions = SendOptions.SendReliable;
            PhotonNetwork.RaiseEvent((byte) EventCode.HitObject, tmp_HitData, tmp_RaiseEventOptions, tmp_SendOptions);


            Destroy(this.gameObject);
        }
    }
}