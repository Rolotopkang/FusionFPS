//Copyright 2022, Infima Games. All Rights Reserved.

using Photon.Pun;
using UnityEngine;

namespace InfimaGames.LowPolyShooterPack.Interface
{
    /// <summary>
    /// Player Interface.
    /// </summary>
    public class CanvasSpawner : MonoBehaviourPun
    {
        #region FIELDS SERIALIZED

        [Header("Settings")]
        
        [Tooltip("Canvas prefab spawned at start. Displays the player's user interface.")]
        [SerializeField]
        private GameObject canvasPrefab;
        
        [Tooltip("Quality settings menu prefab spawned at start. Used for switching between different quality settings in-game.")]
        [SerializeField]
        private GameObject qualitySettingsPrefab;
        [SerializeField]
        private GameObject MainCM;

        #endregion

        #region UNITY

        /// <summary>
        /// Awake.
        /// </summary>
        private void Awake()
        {
            if (photonView != null)
            {
                if (photonView.IsMine)
                {
                    //Spawn Interface.
                    Instantiate(canvasPrefab);
                    //Spawn Quality Settings Menu.
                    Instantiate(qualitySettingsPrefab);
                    MainCM.AddComponent<AudioListener>();
                }
                else
                {
                    MainCM.SetActive(false);
                }
            }
            else
            {
                Debug.Log("离线");
            }
           
        }

        #endregion
    }
}