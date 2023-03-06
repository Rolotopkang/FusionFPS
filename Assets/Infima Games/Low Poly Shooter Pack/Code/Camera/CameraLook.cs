//Copyright 2022, Infima Games. All Rights Reserved.

using System;
using InfimaGames.LowPolyShooterPack.Interface;
using Photon.Pun;
using UnityEngine;
using UnityTemplateProjects.Tools;
using UnityTemplateProjects.UI;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Camera Look. Handles the rotation of the camera.
    /// </summary>
    public class CameraLook : MonoBehaviour,ISettingChangeObserver
    {
        #region FIELDS SERIALIZED
        
        [Header("Settings")]
        
        [SerializeField]
        private CharacterBehaviour playerCharacter;
        
        
        [Tooltip("Sensitivity when looking around.")]
        [SerializeField]
        private Vector2 sensitivityBase = new Vector2(1, 1);

        [Tooltip("Minimum and maximum up/down rotation angle the camera can have.")]
        [SerializeField]
        private Vector2 yClamp = new Vector2(-60, 60);
        
        [Header("Interpolation")]

        [Tooltip("Should the look rotation be interpolated?")]
        [SerializeField]
        private bool smooth;

        [Tooltip("The speed at which the look rotation is interpolated.")]
        [SerializeField]
        private float interpolationSpeed = 25.0f;

        [Header("后坐力部分")]
        [Tooltip("是否显示后坐力")] 
        [SerializeField]
        private bool isRecoilCurve;
        
        [Tooltip("后坐力曲线")]
        [SerializeField]
        private AnimationCurve RecoilCurve;
        
        // [Tooltip("后坐力大小")]
        // [SerializeField]
        // private Vector2 RecoilRange;

        [Tooltip("后坐力淡出时间")]
        [SerializeField]
        private float RecoilFadeOutTime = 0.3f;

        #endregion
        
        #region FIELDS
        
        /// <summary>
        /// Player Character.
        /// </summary>

        /// <summary>
        /// The player character's rigidbody component.
        /// </summary>
        private Rigidbody playerCharacterRigidbody;

        /// <summary>
        /// The player character's rotation.
        /// </summary>
        private Quaternion rotationCharacter;
        /// <summary>
        /// The camera's rotation.
        /// </summary>
        private Quaternion rotationCamera;

        private float currentRecoilTime;
        
        private Vector2 currentRecoil;

        private Vector3 CameraRotation = Vector3.zero;

        private Vector2 sensitivity;
        
        #endregion
        
        #region UNITY

        private void Awake()
        {
            if (PlayerPrefs.HasKey("MouseSensitivity"))
            {
                sensitivity = sensitivityBase * PlayerPrefs.GetFloat("MouseSensitivity");
            }
            else
            {
                sensitivity = sensitivityBase;
            }
        }
        
        private void OnEnable()
        {
            SettingManager.GetInstance().AddSettingChangeObserver(this);
        }

        private void OnDisable()
        {
            SettingManager.GetInstance().RemoveSettingChangeObserver(this);
        }
        
        

        private void Start()
        {
            //Cache the character's initial rotation.
            rotationCharacter = playerCharacter.transform.localRotation;
            //Cache the camera's initial rotation.
            rotationCamera = transform.localRotation;
        }
        private void LateUpdate()
        {
            //Frame Input. The Input to add this frame!
            Vector2 frameInput = In_Game_SettingsMenu.GetInstance().GetCursorLocked() ? playerCharacter.GetInputLook() : default;
            
            //Sensitivity.
            frameInput *= sensitivity;
            
            
            #region ADD
            CalculateRecoilOffset();
            
            frameInput.y += currentRecoil.y;
            frameInput.x -= currentRecoil.x;

            #endregion
            
            
            //Yaw.
            Quaternion rotationYaw = Quaternion.Euler(0.0f, frameInput.x, 0.0f);
            //Pitch.
            Quaternion rotationPitch = Quaternion.Euler(-frameInput.y, 0.0f, 0.0f);
            
            //Save rotation. We use this for smooth rotation.
            rotationCamera *= rotationPitch;
            rotationCamera = Clamp(rotationCamera);
            
            rotationCharacter *= rotationYaw;
            
            //Local Rotation.
            Quaternion localRotation = transform.localRotation;

            //Smooth.
            if (smooth)
            {
                // Interpolate local rotation.
                localRotation = Quaternion.Slerp(localRotation, rotationCamera, Time.deltaTime * interpolationSpeed);
                //Clamp.
                localRotation = Clamp(localRotation);
                //Interpolate character rotation.
                playerCharacter.transform.rotation = Quaternion.Slerp(playerCharacter.transform.rotation, rotationCharacter, Time.deltaTime * interpolationSpeed);
            }
            else
            {
                //Rotate local.
                localRotation *= rotationPitch;
                //Clamp.
                localRotation = Clamp(localRotation);
            
                //Rotate character.
                playerCharacter.transform.rotation *= rotationYaw;
            }
            
            //Set.
            transform.localRotation = localRotation;
        }

        #endregion

        #region FUNCTIONS

        /// <summary>
        /// Clamps the pitch of a quaternion according to our clamps.
        /// </summary>
        private Quaternion Clamp(Quaternion rotation)
        {
            rotation.x /= rotation.w;
            rotation.y /= rotation.w;
            rotation.z /= rotation.w;
            rotation.w = 1.0f;

            //Pitch.
            float pitch = 2.0f * Mathf.Rad2Deg * Mathf.Atan(rotation.x);

            //Clamp.
            pitch = Mathf.Clamp(pitch, yClamp.x, yClamp.y);
            rotation.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * pitch);

            //Return.
            return rotation;
        }

        private void CalculateRecoilOffset()
        {
            currentRecoilTime += Time.deltaTime;
            float tmp_RecoilFraction = currentRecoilTime / RecoilFadeOutTime;
            float tmp_RecoilValue = RecoilCurve.Evaluate(tmp_RecoilFraction);
            currentRecoil = Vector2.Lerp(Vector2.zero, currentRecoil, tmp_RecoilValue);
        }
        
        public void StartRecoil(Vector2 RecoilRange, bool isSpring)
        {
            if (isRecoilCurve)
            {
                currentRecoil += RecoilRange;
                currentRecoilTime = 0;
            }
        }
        
        public void OnSettingChange()
        {
            sensitivity = sensitivityBase * PlayerPrefs.GetFloat("MouseSensitivity");
        }
        #endregion


    }
}