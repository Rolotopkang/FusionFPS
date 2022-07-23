using System;
using System.Collections;
using System.Collections.Generic;
using SickscoreGames.HUDNavigationSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class HUDInputSystem : MonoBehaviour
{
    private HUDNavigationSystem _HUDNavigationSystem;
    private HUDNavigationCanvas _HUDNavigationCanvas;

    #region Minimap

    [SerializeField]
    [Header("地图缩放设置")]
    [Tooltip("地图最大尺寸")]
    private float MaxScale = 0.2f;
    private float CurrentScale = 0f;
    private float CurrentRadius = 0f;


    private Animation minimapAnimation;
    private bool isMiniMapOpen =false;


    #endregion
    
    private void Start()
    {
        _HUDNavigationSystem = HUDNavigationSystem.Instance;
        _HUDNavigationCanvas = HUDNavigationCanvas.Instance;
        minimapAnimation = _HUDNavigationCanvas.Minimap.Panel.GetComponent<Animation>();
        CurrentRadius = _HUDNavigationSystem.minimapRadius;
    }

    public void OnTryMinimap(InputAction.CallbackContext context)
    {
        switch (context)
        {
            case {phase: InputActionPhase.Performed}:
                if (!minimapAnimation.isPlaying)
                {
                    minimapAnimation["MinimapOpen"].time =
                        isMiniMapOpen ? minimapAnimation["MinimapOpen"].clip.length : 0;
                    minimapAnimation["MinimapOpen"].speed = isMiniMapOpen? -1: 1;
                    minimapAnimation.Play();
                    StartCoroutine(MinimapOpen());
                }
                break;
            
        }
    }

    private IEnumerator MinimapOpen()
    {
        if (!isMiniMapOpen)
        {
            CurrentScale = _HUDNavigationSystem.minimapScale;
        }

        _HUDNavigationSystem.minimapRadius =isMiniMapOpen? CurrentRadius :500f;
        while (minimapAnimation.isPlaying)
        {
            _HUDNavigationSystem.minimapScale = !isMiniMapOpen?
                CurrentScale+(MaxScale-CurrentScale)*(minimapAnimation["MinimapOpen"].time/minimapAnimation["MinimapOpen"].clip.length) 
                : MaxScale- (MaxScale-CurrentScale)*((minimapAnimation["MinimapOpen"].clip.length - minimapAnimation["MinimapOpen"].time)/minimapAnimation["MinimapOpen"].clip.length);
            yield return null;
        }
        isMiniMapOpen = !isMiniMapOpen;
    }
}
