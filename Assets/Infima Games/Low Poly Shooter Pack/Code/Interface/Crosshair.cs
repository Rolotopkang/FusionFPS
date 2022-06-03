//Copyright 2022, Infima Games. All Rights Reserved.


using System;
using UnityEngine;
using UnityEngine.UI;
using UnityTemplateProjects.Tools;
using UnityTemplateProjects.UI;

namespace InfimaGames.LowPolyShooterPack.Interface
{
    /// <summary>
    /// Crosshair UI Element. This class makes sure to change the Crosshair at runtime to make sure that it
    /// looks as intended, and functions with the rest of the gameplay elements.
    /// </summary>
    public class Crosshair : Element ,ISettingChangeObserver
    {
        #region FIELDS SERIALIZED

        [Header("References")]

        [Tooltip("Object to which all crosshair pieces are parented.")]
        [SerializeField]
        private CanvasGroup crosshairCanvasGroup;

        [Tooltip("Little Dot!")]
        [SerializeField]
        private CanvasGroup dotCanvasGroup;
        
        [Tooltip("This is the rect transform of the object that actually gets scaled to make the crosshair " +
                 "look bigger.")]
        [SerializeField]
        private RectTransform mainRectTransform;

        [Header("Settings")]
        
        [Tooltip("Minimum and maximum scales for the crosshair.")]
        [SerializeField]
        private Vector2 minMaxScale = new Vector2(50.0f, 200.0f);

        [Tooltip("Interpolation speed of the crosshair' size.")]
        [SerializeField]
        private float interpolationSpeed = 7.0f;

        [Tooltip("Interpolation speed of the dot's visibility.")]
        [SerializeField]
        private float interpolationSpeedDot = 50.0f;

        [Tooltip("Default size of the crosshair. This is the size at which the crosshair stays when nothing is really " +
                 "happening.")]
        [SerializeField]
        private float defaultScale = 50.0f;
        
        [Header("Scale Additions")]
        
        [Tooltip("Value used to increase the crosshair' scale while jumping/falling.")]
        [SerializeField]
        private float jumpingScaleAddition = 50.0f;
        
        [Tooltip("Value used to increase the crosshair' scale while crouching.")]
        [SerializeField]
        private float crouchingScaleAddition = -20.0f;
        
        [Tooltip("Value used to increase the crosshair' scale while moving.")]
        [SerializeField]
        private float movementScaleAddition = 25.0f;

        [Header("Running")]

        [Tooltip("Determines the alpha value of the crosshair while the character is running.")]
        [SerializeField]
        private float runningVisibility = 0.6f;

        [Tooltip("Value used to increase the crosshair' scale while running.")]
        [SerializeField]
        private float runningScaleAddition = 15.0f;

        [SerializeField]
        private Image[] Images;
        
        #endregion

        #region FIELDS

        /// <summary>
        /// Character Movement Behaviour.
        /// </summary>
        private MovementBehaviour movementBehaviour;

        /// <summary>
        /// Crosshair Local Scale.
        /// </summary>
        private float crosshairLocalScale;
        /// <summary>
        /// Crosshair Visibility.
        /// </summary>
        private float crosshairVisibility;
        /// <summary>
        /// Dot Visibility.
        /// </summary>
        private float dotVisibility;

        #endregion

        #region Unity

        protected override void Awake()
        {
            base.Awake();
            if (PlayerPrefs.HasKey("CrosshairColor"))
            {
                foreach (Image image in Images)
                {
                    Color tmp_color;
                    ColorUtility.TryParseHtmlString(PlayerPrefs.GetString("CrosshairColor"),out tmp_color);
                    image.color = tmp_color;
                }
            }
        }

        private void OnEnable()
        {
            SettingManager.GetInstance().AddSettingChangeObserver(this);
        }

        private void OnDisable()
        {
            // SettingManager.GetInstance().RemoveSettingChangeObserver(this);
        }

        #endregion
        
        #region METHODS
        
        
        
        protected override void Tick()
        {
            //Check for missing references.
            if (crosshairCanvasGroup == null || dotCanvasGroup == null || mainRectTransform == null ||
                playerCharacter == null)
            {
                //Reference Error.
                Log.ReferenceError(this, gameObject);
                
                //Return.
                return;
            }
            
            //Cache the Movement Behaviour component if needed.
            movementBehaviour ??= playerCharacter.GetComponent<MovementBehaviour>();
            if (movementBehaviour == null)
            {
                //Reference Error.
                Log.ReferenceError(this, gameObject);
                
                //Return.
                return;
            }

            //Scale of the character's movement. We use this to scale the crosshair while moving around.
            float movementScale = playerCharacter.GetInputMovement().sqrMagnitude * movementScaleAddition;
            //Size Target. This is the value that we're going to be interpolating to this frame.
            float sizeDeltaTarget = defaultScale;

            //Crosshair Scale Target.
            var crosshairLocalScaleTarget = 1.0f;
            //Crosshair Visibility.
            var crosshairVisibilityTarget = 1.0f;
            //Dot Scale Target.
            var dotVisibilityTarget = 1.0f;

            //We completely hide the crosshair, and also the dot, if the character doesn't want it visible.
            //In the future, we will likely change this to all be handled here, and not have any connection
            //to the character itself, but for now this works great.
            if (playerCharacter.IsCrosshairVisible() == false)
                crosshairLocalScaleTarget = dotVisibilityTarget = crosshairVisibilityTarget = 0.0f;
            else
            {
                //Falling Velocity. We add this to the crosshair's size to increase it while in the air.
                float fallingVelocity = (movementBehaviour.GetVelocity().y >= 0 ? Mathf.Clamp01(Mathf.Abs(movementBehaviour.GetVelocity().y)) : 1) * jumpingScaleAddition;
                //We add the crouching values, to reflect the crosshair's change when the character is crouched.
                sizeDeltaTarget += playerCharacter.IsCrouching() ? crouchingScaleAddition : 0.0f;

                //We check if the character is running.
                if (playerCharacter.IsRunning())
                {
                    //Add falling movement.
                    sizeDeltaTarget += movementBehaviour.IsGrounded() ? default : fallingVelocity;
                    
                    //This changes the crosshair's, and the dot's, alpha visibility. We do this because it looks
                    //cool.
                    dotVisibilityTarget = crosshairVisibilityTarget = runningVisibility;
                    //Scale.
                    crosshairLocalScaleTarget = 1.0f;
                    //Update the size too!
                    sizeDeltaTarget += runningScaleAddition;
                }
                //This checks if the character is just chilling.
                //And if so, we just let the crosshair be more relaxed.
                else
                {
                    //We add either the ground movement, or the falling velocity. This depends on whether the character
                    //is on the ground, or not.
                    sizeDeltaTarget += movementBehaviour.IsGrounded() ? movementScale : fallingVelocity;
                    
                    //Setup.
                    crosshairLocalScaleTarget = dotVisibilityTarget = 1.0f;
                }
            }

            //Interpolate the dot visibility value to this frame's target.
            dotVisibility = Mathf.Lerp(dotVisibility, Mathf.Clamp01(dotVisibilityTarget), Time.deltaTime * interpolationSpeedDot);
            //Interpolate the crosshair local scale value to this frame's target.
            crosshairLocalScale = Mathf.Lerp(crosshairLocalScale, Mathf.Clamp01(crosshairLocalScaleTarget), Time.deltaTime * interpolationSpeed);
            //Interpolate the crosshair visibility value to this frame's target.
            crosshairVisibility = Mathf.Lerp(crosshairVisibility, Mathf.Clamp01(crosshairVisibilityTarget), Time.deltaTime * interpolationSpeed);

            //Clamp the crosshair size. Without this, it can get pretty crazy at times!
            sizeDeltaTarget = Mathf.Clamp(sizeDeltaTarget, minMaxScale.x, minMaxScale.y);

            //Scale.
            mainRectTransform.sizeDelta = Vector2.Lerp(mainRectTransform.sizeDelta, sizeDeltaTarget * Vector2.one, Time.deltaTime * interpolationSpeed);
            mainRectTransform.localScale = crosshairLocalScale * Vector3.one;
            
            //Alpha.
            crosshairCanvasGroup.alpha = crosshairVisibility;
            dotCanvasGroup.alpha = dotVisibility;
        }
        
        public void OnSettingChange()
        {
            if (PlayerPrefs.HasKey("CrosshairColor"))
            {
                foreach (Image image in Images)
                {
                    Color tmp_color;
                    ColorUtility.TryParseHtmlString(PlayerPrefs.GetString("CrosshairColor"),out tmp_color);
                    image.color = tmp_color;
                }
            }
        }
        #endregion
    }
}