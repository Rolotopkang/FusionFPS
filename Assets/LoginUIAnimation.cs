using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
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


    public void UIFade()
    {
        _animation.clip = UIHide;
        _animation.Play();
        Invoke("destroyMe",0.3f);
    }
    
    
    public void onTrySpace(InputAction.CallbackContext context)
    {
        //Switch.
        switch (context)
        {
            case {phase: InputActionPhase.Performed}:
                if (!PhotonNetwork.IsConnected)
                {
                    _animation.clip = UIEnable? UIHide: UIshow;
                    _animation.Play();
                    UIEnable = !UIEnable;
                }
                break;
        }
    }

    private void destroyMe()
    {
        gameObject.SetActive(false);
    }
}
