using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STUDYFPCaracterControllerMovement : MonoBehaviour
{
    private CharacterController CharacterController;
    private Transform characterTransform;
    private Vector3 movementDirection;
    public float WalkSpeed;
    public float Gravity = 9.8f;
    public float JumpHeight;
    public float CrouchHeight = 1f;
    private bool isCrouched;
    private float originHeight;
    
    public float CurrentSpeed { get; private set; }
    private IEnumerator crouchCoroutine;
    private void Start()
    {
        CharacterController = GetComponent<CharacterController>();
        characterTransform = transform;
        originHeight = CharacterController.height;
        
    }

    private void Update()
    {
        if (CharacterController.isGrounded)
        {
            var tmp_Horizontal = Input.GetAxis("Horizontal");
            var tmp_Vertical = Input.GetAxis("Vertical");
            movementDirection =
                characterTransform.TransformDirection
                    (new Vector3(tmp_Horizontal, 0, tmp_Vertical)).normalized;
            if (Input.GetButtonDown("Jump"))
            {
                movementDirection.y = JumpHeight;
            }

            //run
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                WalkSpeed = WalkSpeed * 2;
            }else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                WalkSpeed = WalkSpeed / 2;
            }
            
            if (Input.GetKeyDown(KeyCode.C))
            {
                var tmp_CurrentHeight = isCrouched ? originHeight : CrouchHeight;
                if (crouchCoroutine != null)
                {
                    StopCoroutine(crouchCoroutine);
                    crouchCoroutine = null;
                }

                crouchCoroutine = DoCrouch(tmp_CurrentHeight);
                StartCoroutine(crouchCoroutine);
                isCrouched = !isCrouched;
            }
        }
        movementDirection.y -= Gravity * Time.deltaTime;
        CharacterController.Move(movementDirection * Time.deltaTime * WalkSpeed);
    }
    
    private IEnumerator DoCrouch(float _target)
    {
        float tmp_CurrentHeight = 0;
        while (Mathf.Abs(CharacterController.height - _target) > 0.1f)
        {
            yield return null;
            CharacterController.height =
                Mathf.SmoothDamp(CharacterController.height, _target,
                    ref tmp_CurrentHeight, Time.deltaTime * 5);
        }
    }
}
