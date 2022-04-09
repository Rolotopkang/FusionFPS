//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    public class Movement : MovementBehaviour
    {
        #region FIELDS SERIALIZED

        [Header("Speeds")]

        [Tooltip("Acceleration.")]
        [SerializeField]
        private float acceleration = 9.0f;

        [Tooltip("Acceleration value used when the character is in the air. This means either jumping, or falling.")]
        [SerializeField]
        private float accelerationInAir = 3.0f;

        [Tooltip("Deceleration.")]
        [SerializeField]
        private float deceleration = 11.0f;

        [Tooltip("The speed of the player while walking.")]
        [SerializeField]
        private float speedWalking = 4.0f;
        
        [Tooltip("How fast the player moves while aiming.")]
        [SerializeField]
        private float speedAiming = 3.2f;
        
        [Tooltip("How fast the player moves while aiming.")]
        [SerializeField]
        private float speedCrouching = 3.5f;

        [Tooltip("How fast the player moves while running."), SerializeField]
        private float speedRunning = 6.8f;
        
        [Header("Walking Multipliers")]
        
        [Tooltip("How fast the character moves forward."), SerializeField]
        [Range(0.0f, 1.0f)]
        private float walkingMultiplierForward = 1.0f;

        [Tooltip("How fast the character moves sideways.")]
        [Range(0.0f, 1.0f)]
        [SerializeField]
        private float walkingMultiplierSideways = 1.0f;

        [Tooltip("How fast the character moves backwards.")]
        [Range(0.0f, 1.0f)]
        [SerializeField]
        private float walkingMultiplierBackwards = 1.0f;

        [Header("Air")]

        [Tooltip("How much control the player has over changes in direction while the character is in the air.")]
        [Range(0.0f, 1.0f)]
        [SerializeField]
        private float airControl = 0.8f;

        [Tooltip("The value of the character's gravity. Basically, defines how fast the character falls.")]
        [SerializeField]
        private float gravity = 1.1f;

        [Tooltip("The value of the character's gravity while jumping.")]
        [SerializeField]
        private float jumpGravity = 1.0f;

        [Tooltip("The force of the jump.")]
        [SerializeField]
        private float jumpForce = 100.0f;

        [Tooltip("Force applied to keep the character from flying away while descending slopes.")]
        [SerializeField]
        private float stickToGroundForce = 0.03f;
        
        [Header("Crouching")]

        [Tooltip("Height of the character while crouching.")]
        [SerializeField]
        private float crouchHeight = 1.0f;

        #endregion

        #region FIELDS

        /// <summary>
        /// Controller.
        /// </summary>
        private CharacterController controller;

        /// <summary>
        /// Player Character.
        /// </summary>
        private CharacterBehaviour playerCharacter;
        /// <summary>
        /// The player character's equipped weapon.
        /// </summary>
        private WeaponBehaviour equippedWeapon;

        /// <summary>
        /// Default height of the character.
        /// </summary>
        private float standingHeight;

        /// <summary>
        /// Velocity.
        /// </summary>
        private Vector3 velocity;

        /// <summary>
        /// Is the character on the ground.
        /// </summary>
        private bool isGrounded;
        /// <summary>
        /// Was the character standing on the ground last frame.
        /// </summary>
        private bool wasGrounded;

        /// <summary>
        /// Is the character jumping?
        /// </summary>
        private bool jumping;

        #endregion

        #region UNITY FUNCTIONS

        /// <summary>
        /// Awake.
        /// </summary>
        protected override void Awake()
        {
            //Get Player Character.
            playerCharacter = GetComponent<CharacterBehaviour>();
        }

        /// Initializes the FpsController on start.
        protected override void Start()
        {
            //Cache the controller.
            controller = GetComponent<CharacterController>();
            
            //Save the default height.
            standingHeight = controller.height;
        }

        /// Moves the camera to the character, processes jumping and plays sounds every frame.
        protected override void Update()
        {
            //Get the equipped weapon!
            equippedWeapon = playerCharacter.GetInventory().GetEquipped();

            //Get this frame's grounded value.
            isGrounded = IsGrounded();
            //Check if it has changed from last frame.
            if (isGrounded && !wasGrounded)
                jumping = false;
            
            //Move.
            MoveCharacter();
            //Save the grounded value to check for difference next frame.
            wasGrounded = isGrounded;
        }

        #endregion

        #region METHODS

        /// <summary>
        /// Moves the character.
        /// </summary>
        private void MoveCharacter()
        {
            //Get Movement Input!
            Vector2 frameInput = Vector3.ClampMagnitude(playerCharacter.GetInputMovement(), 1.0f);
            //Calculate local-space direction by using the player's input.
            var desiredDirection = new Vector3(frameInput.x, 0.0f, frameInput.y);
            
            //Running speed calculation.
            if(playerCharacter.IsRunning())
                desiredDirection *= speedRunning;
            else
            {
                //Crouching Speed.
                if (playerCharacter.IsCrouching())
                    desiredDirection *= speedCrouching;
                else
                {
                    //Aiming speed calculation.
                    if (playerCharacter.IsAiming())
                        desiredDirection *= speedAiming;
                    else
                    {
                        //Multiply by the normal walking speed.
                        desiredDirection *= speedWalking;
                        //Multiply by the sideways multiplier, to get better feeling sideways movement.
                        desiredDirection.x *= walkingMultiplierSideways;
                        //Multiply by the forwards and backwards multiplier.
                        desiredDirection.z *=
                            (frameInput.y > 0 ? walkingMultiplierForward : walkingMultiplierBackwards);
                    }
                }
            } 

            //World space velocity calculation.
            desiredDirection = transform.TransformDirection(desiredDirection);
            //Multiply by the weapon movement speed multiplier. This helps us modify speeds based on the weapon!
            if (equippedWeapon != null)
                desiredDirection *= equippedWeapon.GetMultiplierMovementSpeed();
            
            //Apply gravity!
            if (isGrounded == false)
            {
                //Get rid of any upward velocity.
                if (wasGrounded && !jumping)
                    velocity.y = 0.0f;
                
                //Movement.
                velocity += desiredDirection * accelerationInAir * airControl * Time.deltaTime;
                //Gravity.
                velocity.y -= (velocity.y >= 0 ? jumpGravity : gravity) * Time.deltaTime;
            }
            //Normal Movement On Ground.
            else if(!jumping)
            {
                //Update velocity with movement on the ground values.
                velocity = Vector3.Lerp(velocity, new Vector3(desiredDirection.x, velocity.y, desiredDirection.z), Time.deltaTime * (desiredDirection.sqrMagnitude > 0.0f ? acceleration : deceleration));
            }

            //Velocity Applied.
            Vector3 applied = velocity * Time.deltaTime;
            //Stick To Ground Force. Helps with making the character walk down slopes without floating.
            if (controller.isGrounded && !jumping)
                applied.y = -stickToGroundForce;

            //Move.
            controller.Move(applied);
        }
        
        /// <summary>
        /// Jump.
        /// </summary>
        public override void Jump()
        {
            //Block jumping when we're not grounded. This avoids us double jumping.
            if (!isGrounded)
                return;

            //Jump.
            jumping = true;
            //Apply Jump Velocity.
            velocity = new Vector3(velocity.x, Mathf.Sqrt(2.0f * jumpForce * jumpGravity), velocity.z);
        }

        public override void Crouch(bool value)
        {
            //Update the capsule's height.
            controller.height = value ? crouchHeight : standingHeight;
            //Update the capsule's center.
            controller.center = controller.height / 2.0f * Vector3.up;
        }

        #endregion

        #region GETTERS

        /// <summary>
        /// Get Multiplier Forward.
        /// </summary>
        public override float GetMultiplierForward() => walkingMultiplierForward;
        /// <summary>
        /// Get Multiplier Sideways.
        /// </summary>
        public override float GetMultiplierSideways() => walkingMultiplierSideways;
        /// <summary>
        /// Get Multiplier Backwards.
        /// </summary>
        public override float GetMultiplierBackwards() => walkingMultiplierBackwards;
        
        /// <summary>
        /// Returns the value of Velocity.
        /// </summary>
        public override Vector3 GetVelocity() => controller.velocity;
        /// <summary>
        /// Returns the value of Grounded.
        /// </summary>
        public override bool IsGrounded() => controller.isGrounded;

        public override bool IsJumping() => jumping;

        #endregion
    }
}