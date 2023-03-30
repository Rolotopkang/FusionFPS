﻿//Copyright 2022, Infima Games. All Rights Reserved.

using Photon.Pun;
using UnityEngine;
using UnityEngine.UIElements;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Helper StateMachineBehaviour that allows us to more easily play a specific weapon sound.
    /// </summary>
    public class PlaySoundCharacterBehaviour : StateMachineBehaviour
    {
        /// <summary>
        /// Type of weapon sound.
        /// </summary>
        private enum SoundType
        {
            //Character Actions.
            GrenadeThrow, Melee,
            //Holsters.
            Holster, Unholster,
            //Normal Reloads.
            Reload, ReloadEmpty,
            //Cycled Reloads.
            ReloadOpen, ReloadInsert, ReloadClose,
            //Firing.
            Fire, FireEmpty,
            //Bolt.
            BoltAction
        }

        #region FIELDS SERIALIZED

        [Header("Setup")]
        
        [Tooltip("Delay at which the audio is played.")]
        [SerializeField]
        private float delay;

        [Tooltip("Type of weapon sound to play.")]
        [SerializeField]
        private SoundType soundType;

        [Tooltip("是否跟随父类")]
        [SerializeField]
        private bool attachToParent;

        [Header("Audio Settings")]

        [Tooltip("Audio Settings.")]
        [SerializeField]
        private AudioSettings audioSettings = new AudioSettings(1.0f, 0.0f, true ,false, Vector3.zero, null,100);

        #endregion

        #region FIELDS

        /// <summary>
        /// Player Character.
        /// </summary>
        private CharacterBehaviour playerCharacter;

        /// <summary>
        /// Player Inventory.
        /// </summary>
        private InventoryBehaviour playerInventory;

        /// <summary>
        /// The service that handles sounds.
        /// </summary>
        private IAudioManagerService audioManagerService;

        #endregion
        
        #region UNITY

        
        
        /// <summary>
        /// On State Enter.
        /// </summary>
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            PhotonView tmpPhotonView = animator.transform.GetComponent<PhotonView>();

            if (tmpPhotonView == null)
            {
                Debug.Log("没有photonview");
                return;
            }
            if (tmpPhotonView.Owner.CustomProperties[EnumTools.PlayerProperties.CurrentWeaponID.ToString()] == null)
            {
                Debug.Log("没有武器属性");
                return;
            }
            WeaponData weaponData = ServiceLocator.Current.Get<IWeaponInfoService>()
                .GetWeaponInfoFromID((int)tmpPhotonView.Owner.CustomProperties[EnumTools.PlayerProperties.CurrentWeaponID.ToString()]);
            bool isMuzzle = (bool)tmpPhotonView.Owner.CustomProperties[EnumTools.PlayerProperties.IsMuzzle.ToString()];
            // //We need to get the character component.
            // playerCharacter ??= ServiceLocator.Current.Get<IGameModeService>().GetPlayerCharacter(tmpPhotonView.Owner);
            //
            // //Get Inventory.
            // playerInventory ??= playerCharacter.GetInventory();
            // if (playerInventory == null)
            // {
            //     return;
            // }
            // //Try to get the equipped weapon's Weapon component.
            // if (!(playerInventory.GetEquipped() is { } weaponBehaviour))
            //     return;
            
            //Try grab a reference to the sound managing service.
            audioManagerService ??= ServiceLocator.Current.Get<IAudioManagerService>();

            #region Select Correct Clip To Play

            //Switch.
            AudioClip clip = soundType switch
            {
                // //Grenade Throw.
                // SoundType.GrenadeThrow => playerCharacter.GetAudioClipsGrenadeThrow().GetRandom(),
                // //Melee.
                // SoundType.Melee => playerCharacter.GetAudioClipsMelee().GetRandom(),
                
                //Holster.
                SoundType.Holster => weaponData.audioClipHolster,
                //Unholster.
                SoundType.Unholster => weaponData.audioClipUnholster,
                
                //Reload.
                SoundType.Reload => weaponData.audioClipReload,
                //Reload Empty.
                SoundType.ReloadEmpty => weaponData.audioClipReloadEmpty,
                
                //Reload Open.
                SoundType.ReloadOpen => weaponData.audioClipReloadOpen,
                //Reload Insert.
                SoundType.ReloadInsert => weaponData.audioClipReloadInsert,
                //Reload Close.
                SoundType.ReloadClose => weaponData.audioClipReloadClose,
                
                //Fire.
                SoundType.Fire => isMuzzle ? weaponData.audioClipFireMuzzle : weaponData.audioClipFire,
                //Fire Empty.
                SoundType.FireEmpty => weaponData.audioClipFireEmpty,
                
                //Bolt Action.
                SoundType.BoltAction => weaponData.audioClipBoltAction,
                
                //Default.
                _ => default
            };

            audioSettings.SetMaxDistance(soundType switch
            {
                //Melee.
                SoundType.Melee => 5,

                //Reload.
                SoundType.Reload => 8,
                //Reload Empty.
                SoundType.ReloadEmpty => 8,

                //Reload Open.
                SoundType.ReloadOpen => 8,
                //Reload Insert.
                SoundType.ReloadInsert => 8,
                //Reload Close.
                SoundType.ReloadClose => 8,

                //Fire.
                SoundType.Fire => 40,

                //Default.
                _ => 0,
            });
                
            
            #endregion

            if (audioSettings.SpatialBlend > 0)
            {
                //更改设置
                audioSettings.SetPosition(animator.transform.position+Vector3.up*1);
            }

            if (attachToParent)
            {
                audioSettings.SetParent(animator.transform.parent);
            }
            
            audioManagerService.PlayOneShotDelayed(clip, audioSettings, delay);

        }
        #endregion
    }
}