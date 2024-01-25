//Copyright 2022, Infima Games. All Rights Reserved.

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using ExitGames.Client.Photon;
using InfimaGames.LowPolyShooterPack;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine.Assertions.Comparers;
using UnityTemplateProjects.Tools;
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
		
		#region Static
		
		protected float WeaponDMG;
		protected String weaponName;
		protected Player projectileOwner;

		protected Collider projectileCollider;
		protected TrailRenderer _trailRenderer;

		#endregion


		#region Unity

		private void Awake()
		{
			projectileCollider = GetComponent<Collider>();
			_trailRenderer = GetComponent<TrailRenderer>();
		}

		protected virtual void Start()
		{
			StartCoroutine(DestroyAfter());
		}

		#endregion


		#region Function

		protected  virtual bool HitPlayer(float DMGtime, Collision hitPlayer)
		{
			return true;
		}

		#endregion





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