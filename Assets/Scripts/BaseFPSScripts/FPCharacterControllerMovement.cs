using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FPCharacterControllerMovement : MonoBehaviour
{
    private CharacterController characterController;
    [SerializeField] private Animator characterAnimator;
    [SerializeField] private Animator tp_CharacterAnimator;
    private Vector3 movementDirection;
    private Transform characterTransform;
    private float velocity;


    private bool isCrouched;
    private float originHeight;

    public float SprintingSpeed;
    public float WalkSpeed;

    public float SprintingSpeedWhenCrouched;
    public float WalkSpeedWhenCrouched;

    public float Gravity = 9.8f;
    public float JumpHeight;
    public float CrouchHeight = 1f;

    public float CurrentSpeed { get; private set; }
    private IEnumerator crouchCoroutine;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
//        characterAnimator = GetComponentInChildren<Animator>();
        characterTransform = transform;
        originHeight = characterController.height;
    }


    private void Update()
    {
        CurrentSpeed = WalkSpeed;
        if (characterController.isGrounded)
        {
            var tmp_Horizontal = Input.GetAxis("Horizontal");
            var tmp_Vertical = Input.GetAxis("Vertical");
            movementDirection =
                characterTransform.TransformDirection(new Vector3(tmp_Horizontal, 0, tmp_Vertical)).normalized;
            if (isCrouched)
            {
                CurrentSpeed = Input.GetKey(KeyCode.LeftShift) ? SprintingSpeedWhenCrouched : WalkSpeedWhenCrouched;
            }
            else
            {
                CurrentSpeed = Input.GetKey(KeyCode.LeftShift) ? SprintingSpeed : WalkSpeed;
            }

            if (Input.GetButtonDown("Jump"))
            {
                movementDirection.y = JumpHeight;
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

            if (characterAnimator != null)
            {
                characterAnimator.SetFloat("Velocity",
                    CurrentSpeed * movementDirection.normalized.magnitude,
                    0.25f,
                    Time.deltaTime);

                tp_CharacterAnimator.SetFloat("Velocity",
                    CurrentSpeed * movementDirection.normalized.magnitude,
                    0.25f,
                    Time.deltaTime);
                tp_CharacterAnimator.SetFloat("Movement_X", tmp_Horizontal, 0.25f, Time.deltaTime);
                tp_CharacterAnimator.SetFloat("Movement_Y", tmp_Vertical, 0.25f, Time.deltaTime);
            }
        }


        movementDirection.y -= Gravity * Time.deltaTime;
        var tmp_Movement = CurrentSpeed * Time.deltaTime * movementDirection;
        characterController.Move(tmp_Movement);
    }


    private IEnumerator DoCrouch(float _target)
    {
        float tmp_CurrentHeight = 0;
        while (Mathf.Abs(characterController.height - _target) > 0.1f)
        {
            yield return null;
            characterController.height =
                Mathf.SmoothDamp(characterController.height, _target,
                    ref tmp_CurrentHeight, Time.deltaTime * 5);
        }
    }

    internal void SetupAnimator(Animator _animator)
    {
        Debug.Log($"Execute! the animator is empty??? {_animator == null}");
        characterAnimator = _animator;
    }
}