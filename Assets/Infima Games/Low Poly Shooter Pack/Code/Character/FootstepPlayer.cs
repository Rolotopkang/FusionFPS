//Copyright 2022, Infima Games. All Rights Reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using InfimaGames.LowPolyShooterPack.Interface;
using Photon.Pun;
using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Footstep Player. This component is in charge of playing footstep sounds for the player.
    /// We have this a separate component so as to make sure that it can be super easily removed, and replaced
    /// for a more custom implementation, as our setup is quite basic at the moment.
    /// </summary>
    public class FootstepPlayer : MonoBehaviourPun
    {
        #region FIELDS SERIALIZED

        [Header("References")]
        [Tooltip("The character's Movement Behaviour component.")] 
        [SerializeField]
        private CharacterController characterController;

        [Tooltip("The character's Animator component.")]
        [SerializeField]
        private Animator characterAnimator;
        
        [Tooltip("The character's footstep-dedicated Audio Source component.")]
        [SerializeField]
        private AudioSource audioSource;

        [Header("Settings")]

        [Tooltip("Minimum magnitude of the movement velocity at which the audio clips will start playing.")]
        [SerializeField]
        private float minVelocityMagnitude = 1.0f;
        
        [Header("Audio Clips")]
        
        [Tooltip("The audio clip that is played while walking.")]
        [SerializeField]
        private AudioClip audioClipWalking;

        [Tooltip("The audio clip that is played while running.")]
        [SerializeField]
        private AudioClip audioClipRunning;

        [Header("脚步特效")]
        [SerializeField] private Transform waterRipples;

        [SerializeField] private float waterRipplesTime;

        [SerializeField] private float waterRipplesLastTime;


        private Coroutine coroutine_aterRipples;
        
        private LoacalChanger _loacalChanger;

        private Ray checkRay;
        
        private RaycastHit[] rayInfo;

        private bool touchWater;

        private Vector3 touchPoint; 
        
        #endregion
        
        #region UNITY
        
        /// <summary>
        /// Awake.
        /// </summary>
        private void Awake()
        {
            //Make sure we have an Audio Source assigned.
            if (audioSource != null)
            {
                //Audio Source Setup.
                audioSource.clip = audioClipWalking;
                audioSource.loop = true;   
            }
            
            _loacalChanger = GetComponent<LoacalChanger>();
            
            
        }

        /// <summary>
        /// Update.
        /// </summary>
        private void Update()
        {
            //Check for missing references.
            if (characterAnimator == null || characterController == null || audioSource == null)
            {
                //Reference Error.
                Log.ReferenceError(this, gameObject);
                //Return.
                return;
            }
            
            if (_loacalChanger.isMine)
            {
                audioSource.spatialBlend = 0f;
            }
            else
            {
                audioSource.spatialBlend = 1f;
            }

            if (!_loacalChanger.isMine)
            {
                return;
            }
            
            //Check if we're moving on the ground. We don't need footsteps in the air.
            if (characterController.isGrounded && characterController.velocity.sqrMagnitude > minVelocityMagnitude)
            {
                playSound();
                photonView.RPC(nameof(playSound),RpcTarget.Others);
            }
            //Pause it if we're doing something like flying, or not moving!
            else if (audioSource.isPlaying)
            {
                audioSource.Pause();
                photonView.RPC(nameof(PauseSound),RpcTarget.Others);
            }

            //检测脚下材质
            GroundMaterialCheck();

            MaterialProformance();
        }

        private void GroundMaterialCheck()
        {
            touchWater = false;
            touchPoint = new Vector3();
            
            checkRay = new Ray(gameObject.transform.position + Vector3.up.normalized*1.5f, Vector3.down*1.5f);
            Debug.DrawRay(gameObject.transform.position + Vector3.up.normalized*1.5f, Vector3.down*1.5f,Color.red,1);
            rayInfo = Physics.RaycastAll(checkRay, 1.5f,  1<<LayerMask.NameToLayer("Water"));
            foreach (RaycastHit hit in rayInfo)
            {
                if (hit.transform.tag.Equals("Water"))
                {
                    touchWater = true;
                    touchPoint = hit.collider.transform.position;
                }
            }
        }

        private void MaterialProformance()
        {
            if (touchWater)
            {
                if (coroutine_aterRipples == null)
                {
                    coroutine_aterRipples = StartCoroutine(WaterProformance());
                }
            }
        }

        private IEnumerator WaterProformance()
        {
            Instantiate(waterRipples,
                new Vector3(transform.position.x, touchPoint.y+0.01f,transform.position.z),
                Quaternion.Euler(Vector3.up)).gameObject.AddComponent<destroyMe>().deathtimer = waterRipplesLastTime;
            while (true)
            {
                yield return new WaitForSeconds(waterRipplesTime);
                Transform tmp_vx = Instantiate(waterRipples,
                            new Vector3(transform.position.x, touchPoint.y+0.01f,transform.position.z),
                            Quaternion.Euler(Vector3.up));
                tmp_vx.gameObject.AddComponent<destroyMe>().deathtimer = waterRipplesLastTime;
                
                if (!touchWater)
                {
                    coroutine_aterRipples = null;
                    yield break;
                }
            }
            
        }
        
        [PunRPC]
        private void playSound()
        {
            //Select the correct audio clip to play.
            audioSource.clip = characterAnimator.GetBool(AHashes.Running) ? audioClipRunning : audioClipWalking;
            //Play it!
            if (!audioSource.isPlaying)
                audioSource.Play();
        }

        [PunRPC]
        private void PauseSound()
        {
            audioSource.Pause();
        }
        
        #endregion
    }
}