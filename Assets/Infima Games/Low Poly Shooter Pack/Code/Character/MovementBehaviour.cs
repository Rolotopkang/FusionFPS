//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Abstract movement class. Handles interactions with the main movement component.
    /// </summary>
    public abstract class MovementBehaviour : MonoBehaviour
    {
        #region UNITY

        /// <summary>
        /// Awake.
        /// </summary>
        protected virtual void Awake(){}

        /// <summary>
        /// Start.
        /// </summary>
        protected virtual void Start(){}

        /// <summary>
        /// Update.
        /// </summary>
        protected virtual void Update(){}

        /// <summary>
        /// Fixed Update.
        /// </summary>
        protected virtual void FixedUpdate(){}

        /// <summary>
        /// Late Update.
        /// </summary>
        protected virtual void LateUpdate(){}

        #endregion

        #region GETTERS

        /// <summary>
        /// Returns the value of MultiplierForward.
        /// </summary>
        /// <returns></returns>
        public abstract float GetMultiplierForward();
        /// <summary>
        /// Returns the value of MultiplierSideways.
        /// </summary>
        /// <returns></returns>
        public abstract float GetMultiplierSideways();
        /// <summary>
        /// Returns the value of MultiplierBackwards.
        /// </summary>
        /// <returns></returns>
        public abstract float GetMultiplierBackwards();

        /// <summary>
        /// Returns the character's current velocity.
        /// </summary>
        public abstract Vector3 GetVelocity();
        /// <summary>
        /// Returns true if the character is grounded.
        /// </summary>
        public abstract bool IsGrounded();

        #endregion

        #region METHODS

        /// <summary>
        /// Calling this will make the character jump!
        /// </summary>
        public abstract void Jump();
        /// <summary>
        /// Tries to crouch/uncrouch!
        /// </summary>
        public abstract void Crouch(bool value);

        #endregion
    }
}