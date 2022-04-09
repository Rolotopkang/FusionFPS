//Copyright 2022, Infima Games. All Rights Reserved.

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using ExitGames.Client.Photon;
using InfimaGames.LowPolyShooterPack;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Assertions.Comparers;
using EventCode = Scripts.Weapon.EventCode;
using Random = UnityEngine.Random;

namespace InfimaGames.LowPolyShooterPack.Legacy
{
	public class Projectile : MonoBehaviour
	{

		[Range(5, 100)]
		[Tooltip("After how long time should the bullet prefab be destroyed?")]
		public float destroyAfter;

		[Tooltip("If enabled the bullet destroys on impact")]
		public bool destroyOnImpact = false;

		[Tooltip("Minimum time after impact that the bullet is destroyed")]
		public float minDestroyTime;

		[Tooltip("Maximum time after impact that the bullet is destroyed")]
		public float maxDestroyTime;

		public float BulletSpeed = 50;
		
		[Header("对人体部位的伤害系数")]
		[Range(0,5)][Tooltip("头部伤害系数")]
		[SerializeField] private float headDamageTimer = 2.0f;
		[Range(0,5)][Tooltip("身体伤害系数")]
		[SerializeField] private float bodyDamageTimer = 1.0f;
		[Range(0,5)][Tooltip("腿部伤害系数")]
		[SerializeField] private float legDamageTimer = 0.75f;


		[Header("Impact Effect Prefabs")]
		public Transform[] bloodImpactPrefabs;

		public Transform[] metalImpactPrefabs;
		public Transform[] dirtImpactPrefabs;
		public Transform[] concreteImpactPrefabs;
		public Transform[] woodImoactPrefabs;

		#region Static
		
		private float WeaponDMG;
		private String weaponName;
		private Player projectileOwner;

		private Collider projectileCollider;

		#endregion


		#region Unity

		private void Awake()
		{
			projectileCollider = GetComponent<Collider>();
		}

		private void Start()
		{
			//Start destroy timer
			StartCoroutine(DestroyAfter());
		}

		#endregion


		#region Function

		private bool HitPlayer(float DMGtime, Collision hitPlayer)
		{
			Player tmp_hitOwner = hitPlayer.gameObject.GetComponentInParent<PhotonView>().Owner;

			if (tmp_hitOwner.Equals(projectileOwner))
			{
				Physics.IgnoreCollision(projectileCollider,hitPlayer.collider);

				return false;
			}
			
			//溅血特效
			if (!tmp_hitOwner.Equals(PhotonNetwork.LocalPlayer))
			{
				Instantiate(bloodImpactPrefabs[Random.Range(0, bloodImpactPrefabs.Length)],
					transform.position,
					Quaternion.LookRotation(hitPlayer.contacts[0].normal));
			}
			
			//TODO 队友伤害关闭

			//向photon网络发送击中事件
			if (PhotonNetwork.LocalPlayer.Equals(projectileOwner))
			{
				Dictionary<byte, object> tmp_HitData = new Dictionary<byte, object>();
				//被击中人
				tmp_HitData.Add(0,tmp_hitOwner);
				//造成伤害人
				tmp_HitData.Add(1,projectileOwner);
				//造成伤害人武器
				tmp_HitData.Add(2,weaponName);
				//造成伤害
				tmp_HitData.Add(3,WeaponDMG*DMGtime);
				//是否爆头
				tmp_HitData.Add(4,DMGtime.Equals(headDamageTimer));
				//伤害来源点
				tmp_HitData.Add(5,hitPlayer.contacts[0].point);
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
			return true;
		}

		#endregion
		
		
		
		
		//If the bullet collides with anything
		private void OnCollisionEnter(Collision collision)
		{

			// if (!destroyOnImpact)
			 // {
			 // 	StartCoroutine(DestroyTimer());
			 // }
			 // //Otherwise, destroy bullet on impact
			 // else
			 // {
				//  Debug.Log("碰撞销毁了！");
				//  Destroy(gameObject);
			 // }
		
			//If bullet collides with "Metal" tag
			if (collision.transform.tag.Equals("Metal"))
			{

				//Instantiate random impact prefab from array
				Instantiate(metalImpactPrefabs[Random.Range
						(0, metalImpactPrefabs.Length)], transform.position,
					Quaternion.LookRotation(collision.contacts[0].normal));
				//Destroy bullet object
				Destroy(gameObject);
			}
		
			//If bullet collides with "Dirt" tag
			if (collision.transform.tag.Equals("Dirt"))
			{

				//Instantiate random impact prefab from array
				Instantiate(dirtImpactPrefabs[Random.Range
						(0, dirtImpactPrefabs.Length)], transform.position,
					Quaternion.LookRotation(collision.contacts[0].normal));
				//Destroy bullet object
				Destroy(gameObject);
			}
		
			//If bullet collides with "Wood" tag
			if (collision.transform.tag.Equals("Wood") )
			{

				//Instantiate random impact prefab from array
				Instantiate(woodImoactPrefabs[Random.Range
						(0, woodImoactPrefabs.Length)], transform.position,
					Quaternion.LookRotation(collision.contacts[0].normal));
				//Destroy bullet object
				Destroy(gameObject);
			}
			
			//If bullet collides with "Concrete" tag
			if (collision.transform.tag.Equals("Concrete"))
			{

				//Instantiate random impact prefab from array
				Instantiate(concreteImpactPrefabs[Random.Range
						(0, concreteImpactPrefabs.Length)], transform.position,
					Quaternion.LookRotation(collision.contacts[0].normal));
				//Destroy bullet object
				Destroy(gameObject);
			}
		
			//If bullet collides with "Target" tag
			if (collision.transform.tag.Equals("Target"))
			{

				//Toggle "isHit" on target object
				collision.transform.gameObject.GetComponent
					<TargetScript>().isHit = true;
				//Destroy bullet object
				Destroy(gameObject);
			}
		
			//If bullet collides with "ExplosiveBarrel" tag
			if (collision.transform.tag.Equals("ExplosiveBarrel"))
			{

				//Toggle "explode" on explosive barrel object
				collision.transform.gameObject.GetComponent
					<ExplosiveBarrelScript>().explode = true;
				//Destroy bullet object
				Destroy(gameObject);
			}
		
			//If bullet collides with "GasTank" tag
			if (collision.transform.tag.Equals("GasTank"))
			{

				//Toggle "isHit" on gas tank object
				collision.transform.gameObject.GetComponent
					<GasTankScript>().isHit = true;
				//Destroy bullet object
				Destroy(gameObject);
			}
			
			if (collision.transform.tag.Equals("PlayerHead"))
			{

				if (HitPlayer(headDamageTimer, collision))
				{
					Destroy(gameObject);
				}
			}
			
			if (collision.transform.tag.Equals("PlayerChest"))
			{
				if (HitPlayer(bodyDamageTimer, collision))
				{
					Destroy(gameObject);
				}
			}			
			
			if (collision.transform.tag.Equals("PlayerLeg"))
			{
				if (HitPlayer(legDamageTimer, collision))
				{
					Destroy(gameObject);
				}
			}
			
		}
		

		#region IEnumerators
		private IEnumerator DestroyTimer()
		{
			//Wait random time based on min and max values
			yield return new WaitForSeconds
				(Random.Range(minDestroyTime, maxDestroyTime));
			//Destroy bullet object
			Destroy(gameObject);
		}

		private IEnumerator DestroyAfter()
		{
			//Wait for set amount of time
			yield return new WaitForSeconds(destroyAfter);
			//Destroy bullet object
			Destroy(gameObject);
		}

		#endregion

		#region GetterSetter

		public void SetOwner(Player set)
		{
			projectileOwner = set;
		}

		public void SetWeaponDMG(float set)
		{
			WeaponDMG = set;
		}

		public void SetweaponName(string set)
		{
			weaponName = set;
		}
		
		public Player GetOwner() => projectileOwner;

		#endregion
	}
}