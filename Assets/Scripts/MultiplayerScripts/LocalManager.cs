using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Rendering;

public class LocalManager : MonoBehaviour
{
    public List<MonoBehaviour> LocalScripts;
    public Camera FP_Camera;
    public Camera ENV_Camera;
    private PhotonView photonView;
    public List<Renderer> TPRenderers;
    public GameObject FPArms;
    public GameObject FPUI;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            gameObject.AddComponent<AudioListener>();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            FPUI.SetActive(true);
            FPArms.SetActive(true);
            return;
        }

        FPUI.SetActive(false);
        FPArms.SetActive(false);
        FP_Camera.enabled = false;
        ENV_Camera.enabled = false;
        
        foreach (MonoBehaviour behaviour in LocalScripts)
        {
            behaviour.enabled = false;
        }

        foreach (Renderer tpRenderer in TPRenderers)
        {
            tpRenderer.shadowCastingMode = ShadowCastingMode.On;
        }
        
    }
}