//Copyright 2022, Infima Games. All Rights Reserved.

using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Rendering;

namespace InfimaGames.LowPolyShooterPack.Interface
{
    /// <summary>
    /// Player Interface.
    /// </summary>
    public class LoacalChanger : MonoBehaviourPun
    {
        #region FIELDS SERIALIZED

        [Header("Settings")]
        [SerializeField] private bool singlePlay =false;
        
        [Tooltip("Canvas prefab spawned at start. Displays the player's user interface.")]
        [SerializeField]
        private GameObject canvasPrefab;
        
        [SerializeField]
        private List<MonoBehaviour> LocalScripts;
        
        [Tooltip("Quality settings menu prefab spawned at start. Used for switching between different quality settings in-game.")]
        [SerializeField]
        private GameObject qualitySettingsPrefab;
        [SerializeField]
        private GameObject MainCM;

        private CharacterController CharacterController;

        [SerializeField]
        private RenderController TpRender;
        [SerializeField]
        private RenderController FpRender;

        private PhotonView PhotonView;

        [SerializeField]
        private GameObject FP;
        
        
        
        #endregion

        #region UNITY

        private void Start()
        {
            PhotonView = GetComponent<PhotonView>();
            if (singlePlay)
            {
                //Spawn Interface.
                GameObject tmp_canvasPrefab = Instantiate(canvasPrefab);
                tmp_canvasPrefab.GetComponent<Canvas>().worldCamera = MainCM.GetComponentsInChildren<Camera>()[2];
                //Spawn Quality Settings Menu.
                Instantiate(qualitySettingsPrefab);
                MainCM.AddComponent<AudioListener>();
                TpRender.SetRenderers(ShadowCastingMode.ShadowsOnly);

                FpRender.SetRenderersDisable(true);
                FpRender.SetRenderers(ShadowCastingMode.Off);
                return;
            }
            if (PhotonView != null)
            {
                if (PhotonView.IsMine)
                {
                    //Spawn Interface.
                    GameObject tmp_canvasPrefab = Instantiate(canvasPrefab);
                    tmp_canvasPrefab.GetComponent<Canvas>().worldCamera = MainCM.GetComponentsInChildren<Camera>()[2];
                    //Spawn Quality Settings Menu.
                    Instantiate(qualitySettingsPrefab);
                    MainCM.AddComponent<AudioListener>();
                    TpRender.SetRenderers(ShadowCastingMode.ShadowsOnly);

                    FpRender.SetRenderersDisable(true);
                    FpRender.SetRenderers(ShadowCastingMode.Off);
                }
                else
                {
                    MainCM.SetActive(false);
                    TpRender.SetRenderers(ShadowCastingMode.On);
                    TpRender.SetRenderersDisable(true);
                    FP.SetActive(false);
                    foreach (MonoBehaviour behaviour in LocalScripts)
                    {
                        behaviour.enabled = false;
                    }
                }
            }
            else
            {
                Debug.Log("离线");
            }
        }

        public void LocalDeath()
        {
            TpRender.SetRenderers(ShadowCastingMode.On);
            TpRender.SetRenderersDisable(true);
            FP.SetActive(false);
        }
        #endregion
    }
}