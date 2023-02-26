//Copyright 2022, Infima Games. All Rights Reserved.

using System;
using System.Collections.Generic;
using Photon.Pun;
using SickscoreGames.HUDNavigationSystem;
using UnityEngine;
using UnityEngine.Rendering;
using UnityTemplateProjects.Tools;

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
        private AudioSource _audioSource;
        
        [SerializeField]
        private List<MonoBehaviour> LocalScripts;

        [SerializeField]
        private List<MonoBehaviour> DeathScripts;
        
        [SerializeField]
        private GameObject MainCM;

        private CharacterController CharacterController;

        [SerializeField]
        private RenderController TpRender;
        [SerializeField]
        private RenderController FpRender;

        [SerializeField] 
        private GameObject IndicatorSystem;

        private PhotonView PhotonView;
        
        public GameObject FP;
        
        public GameObject TPBody;

        public Outline OutlineScript;

        public bool isMine;
        #endregion

        #region SpawnObjects
        private GameObject Canvas;
        

        #endregion
        
        #region UNITY

        private void Start()
        {
            PhotonView = GetComponent<PhotonView>();
            if (singlePlay)
            {
                Canvas = Instantiate(canvasPrefab);
                Canvas.GetComponent<Canvas>().worldCamera = MainCM.GetComponentsInChildren<Camera>()[2];
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
                    isMine = true;
                    Canvas = Instantiate(canvasPrefab);
                    Canvas.GetComponent<Canvas>().worldCamera = MainCM.GetComponentsInChildren<Camera>()[2];
                    MainCM.AddComponent<AudioListener>();
                    TpRender.SetRenderers(ShadowCastingMode.ShadowsOnly);
                    FpRender.SetRenderersDisable(true);
                    FpRender.SetRenderers(ShadowCastingMode.Off);
                    IndicatorSystem.SetActive(false);
                }
                else
                {
                    isMine = false;
                    MainCM.SetActive(false);
                    TpRender.SetRenderers(ShadowCastingMode.On);
                    TpRender.SetRenderersDisable(true);
                    IndicatorSystem.SetActive(true);
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

        public void RemoteDeath()
        {
            //Destroy indicator
            IndicatorSystem.SetActive(false);
        }
        
        public void LocalDeath()
        {
            TpRender.SetRenderers(ShadowCastingMode.On);
            TpRender.SetRenderersDisable(true);
            
            Destroy(Canvas);

            // Destroy(Settings);
            FP.SetActive(false);

            foreach (MonoBehaviour behaviour in DeathScripts)
            {
                behaviour.enabled = false;
            }

            _audioSource.enabled = false;
        }
        #endregion
    }
}