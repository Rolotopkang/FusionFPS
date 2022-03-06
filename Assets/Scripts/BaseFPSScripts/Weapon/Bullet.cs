using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityTemplateProjects.MultiplayerScripts;
using Random = UnityEngine.Random;

namespace Scripts.Weapon
{
    public class Bullet : MonoBehaviour
    {
        public float BulletSpeed;
        public int   BulletDMG;
        public Player Owner;
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

            switch (tmp_Hit.collider.tag)
            {
                //TODO 读取系数
                //头部
                case "PlayerHead":
                {
                    PlayerInjured(tmp_Hit,5,Owner);
                    BulletHitEvent(tmp_Hit,ImpactPrefab,true);
                    break;
                }
                case "Player":
                {
                    PlayerInjured(tmp_Hit,1,Owner);
                    BulletHitEvent(tmp_Hit,ImpactPrefab,true);
                    break;
                }
                case "PlayerChest":
                {
                    PlayerInjured(tmp_Hit,2 ,Owner);
                    BulletHitEvent(tmp_Hit,ImpactPrefab,true);
                    break;
                }
                case "PlayerLeg":
                {
                    PlayerInjured(tmp_Hit,1 ,Owner);
                    BulletHitEvent(tmp_Hit,ImpactPrefab,true);
                    break;
                }
                case "PlayerHand":
                {
                    PlayerInjured(tmp_Hit,1 ,Owner);
                    BulletHitEvent(tmp_Hit,ImpactPrefab,true);
                    break;
                }
                default:
                {
                    BulletHitEvent(tmp_Hit,ImpactPrefab,false);
                    break;
                }
            }
            
            Destroy(this.gameObject);
        }


        private void BulletHitEvent(RaycastHit tmp_Hit , GameObject ImpactPrefab,bool TargetHiden)
        {
            Dictionary<byte, object> tmp_HitData = new Dictionary<byte, object>();
            tmp_HitData.Add(0, tmp_Hit.point);
            tmp_HitData.Add(1, tmp_Hit.normal);

            tmp_HitData.Add(2, tmp_Hit.collider.tag);
            tmp_HitData.Add(3,ImpactPrefab.name);
            tmp_HitData.Add(4,TargetHiden);
            tmp_HitData.Add(5,Owner);
            if (TargetHiden)
            {
                tmp_HitData.Add(6,tmp_Hit.collider.gameObject.GetComponentInParent<PhotonView>().Owner);
            }



            RaiseEventOptions tmp_RaiseEventOptions = new RaiseEventOptions() {Receivers = ReceiverGroup.All};
            SendOptions tmp_SendOptions = SendOptions.SendReliable;
            PhotonNetwork.RaiseEvent((byte) EventCode.HitObject, tmp_HitData, tmp_RaiseEventOptions, tmp_SendOptions);
        }
        
        private void PlayerInjured(RaycastHit tmp_Hit , int DMGTimer ,Player DMGSource)
        {
            PlayerNumericalController tmp_Damager;
            tmp_Damager = tmp_Hit.collider.GetComponentInParent<PlayerNumericalController>();
            tmp_Damager.TakeDamage(BulletDMG*DMGTimer);
        }
    }
}