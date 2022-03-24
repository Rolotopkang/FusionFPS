using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

public class LoginUIAnimation : MonoBehaviour
{
    [SerializeField]
    private Animation _animation;
    [SerializeField]
    private AnimationClip UIshow;
    [SerializeField]
    private AnimationClip UIHide;

    private bool UIEnable = false;


    public void onTrySpace(InputAction.CallbackContext context)
    {
        //Switch.
        switch (context)
        {
            case {phase: InputActionPhase.Performed}:
                _animation.clip = UIEnable? UIHide: UIshow;
                _animation.Play();
                UIEnable = !UIEnable;
                break;
        }
    }
}
