//Copyright 2022, Infima Games. All Rights Reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
using UnityTemplateProjects.UI;
using EventCode = Scripts.Weapon.EventCode;
using Random = UnityEngine.Random;

namespace InfimaGames.LowPolyShooterPack.Interface
{
    /// <summary>
    /// Quality Settings Menu.
    /// </summary>
    public class In_Game_SettingsMenu : Singleton<In_Game_SettingsMenu>
    {
        #region FIELDS SERIALIZED
        
        [Header("Settings")]

        [Tooltip("Canvas to play animations on.")]
        [SerializeField]
        private GameObject animatedCanvas;

        [Tooltip("Animation played when showing this menu.")]
        [SerializeField]
        private AnimationClip animationShow;

        [Tooltip("Animation played when hiding this menu.")]
        [SerializeField]
        private AnimationClip animationHide;

        [SerializeField]
        private CanvasGroup CanvasGroup;

        [SerializeField]
        private UIBase _uiBase;

        #endregion
        
        #region FIELDS
        
        private Animation animationComponent;
        private bool menuIsEnabled;
        private PostProcessVolume postProcessingVolume;
        private DepthOfField depthOfField;
        private bool cursorLocked;
        [HideInInspector]
        public bool scopeChanging;
        public bool isLive;
        
        #endregion
        
        #region UNITY

        private void Start()
        {
            CanvasGroup.interactable = false;
            cursorLocked = false;
            animatedCanvas.GetComponent<CanvasGroup>().alpha = 0;
            animationComponent = animatedCanvas.GetComponent<Animation>();
            postProcessingVolume = GameObject.Find("Post Processing Volume")?.GetComponent<PostProcessVolume>();
            if(postProcessingVolume != null)
                postProcessingVolume.profile.TryGetSettings(out depthOfField);
        }

        #endregion

        #region METHODS

        private void Show()
        {
            CanvasGroup.interactable = true;
            menuIsEnabled = true;
            animationComponent.clip = animationShow;
            animationComponent.Play();
            if(depthOfField != null)
                depthOfField.active = true;
        }
        private void Hide()
        {
            CanvasGroup.interactable = false;
            menuIsEnabled = false;
            animationComponent.clip = animationHide;
            animationComponent.Play();
            if(depthOfField != null)
                depthOfField.active = false;
            _uiBase.CloseCurrentUI();
        }

        public void Restart()
        {
            //重生
            Sentenced();
        }
        public void Quit()
        {
            if (GameModeManagerBehaviour.GetInstance().GetPlayerManager(PhotonNetwork.LocalPlayer).GetPlayerOBJ())
            {
                PhotonNetwork.Destroy(GameModeManagerBehaviour.GetInstance().GetPlayerManager(PhotonNetwork.LocalPlayer).GetPlayerOBJ().GetPhotonView()); 
            }
            PhotonNetwork.AutomaticallySyncScene = false;
            //退出房间
            PhotonNetwork.LocalPlayer.LeaveCurrentTeam();
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LoadLevel(0);
        }

        private void Sentenced()
        {
            if (PhotonNetwork.LocalPlayer.CustomProperties[EnumTools.PlayerProperties.IsDeath.ToString()].Equals(true))
            {
                Debug.Log("角色已经死亡");
                return;
            }
        
            Dictionary<byte, object> tmp_HitData = new Dictionary<byte, object>();
            //被击中人
            tmp_HitData.Add(0,PhotonNetwork.LocalPlayer);
            //造成伤害人
            tmp_HitData.Add(1,PhotonNetwork.LocalPlayer);
            //造成伤害人武器
            tmp_HitData.Add(2,"suicide");
            //造成伤害
            tmp_HitData.Add(3,1000f);
            //是否爆头
            tmp_HitData.Add(4,false);
            //伤害来源点
            tmp_HitData.Add(5,Vector3.zero);
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
        
        #endregion

        #region Cursor

        public void OnLockCursor(InputAction.CallbackContext context)
        {
            switch (context)
            {
                //Performed.
                case {phase: InputActionPhase.Performed}:
                    //更换配件无法呼出菜单
                    if(scopeChanging){break;}
                    //Toggle the cursor locked value.
                    
                    if (menuIsEnabled)
                    {
                        Hide();
                        if (isLive)
                        {
                            setCursorLocked(true);
                        }
                        else
                        {
                            setCursorLocked(false);
                        }
                    }
                    else
                    {
                        Show();
                        setCursorLocked(false);
                    }
                    UpdateCursorState();
                    break;
            }
        }

        /// <summary>
        /// Updates the cursor state based on the value of the cursorLocked variable.
        /// </summary>
        public void UpdateCursorState()
        {
            //Update cursor visibility.
            Cursor.visible = !cursorLocked;
            //Update cursor lock state.
            Cursor.lockState = cursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
        }

        public void setCursorLocked(bool set)
        {
            cursorLocked = set;
            UpdateCursorState();
        }

        public void convertCursorLocked()
        {
            cursorLocked = !cursorLocked;
            UpdateCursorState();
        }

        public bool GetMenuEnable() => menuIsEnabled;

        public bool GetCursorLocked() => cursorLocked;

        #endregion
    }
}