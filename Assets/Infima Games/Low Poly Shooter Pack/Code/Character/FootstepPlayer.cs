//Copyright 2022, Infima Games. All Rights Reserved.

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

        private LoacalChanger _loacalChanger;
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