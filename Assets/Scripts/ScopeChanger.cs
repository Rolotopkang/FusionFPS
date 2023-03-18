using System;
using System.Collections;
using System.Collections.Generic;
using InfimaGames.LowPolyShooterPack;
using InfimaGames.LowPolyShooterPack.Interface;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class ScopeChanger : MonoBehaviour
{
    #region FIELDS SERIALIZED

    [Header("Settings")]
    
    [Tooltip("Canvas to play animations on.")]
    [SerializeField]
    private GameObject animatedCanvas;

    [Tooltip("Animation played when showing this menu.")]
    [SerializeField]
    private AnimationClip animationShow;

    [Tooltip("画布")]
    [SerializeField]
    private Canvas ScopeChangerCanvas;

    #endregion

    #region FIELDS

    /// <summary>
    /// Game Mode Service.
    /// </summary>
    protected IGameModeService gameModeService;
        
    /// <summary>
    /// Player Character.
    /// </summary>
    protected CharacterBehaviour playerCharacter;
    
    /// <summary>
    /// Animation Component.
    /// </summary>
    private Animation animationComponent;
    /// <summary>
    /// If true, it means that this menu is enabled and showing properly.
    /// </summary>
    private bool menuIsEnabled;

    /// <summary>
    /// Main Post Processing Volume.
    /// </summary>
    private PostProcessVolume postProcessingVolume;

    /// <summary>
    /// Depth Of Field Settings.
    /// </summary>
    private DepthOfField depthOfField;

    private Vignette vignette;

    // /// <summary>
    // /// 世界画布上的事件相机
    // /// </summary>
    // private Camera EventCM;

    #endregion

    #region UNITY

    private void Awake()
    {
        gameModeService = ServiceLocator.Current.Get<IGameModeService>();
        //Get Player Character.
        playerCharacter = gameModeService.GetPlayerCharacter(PhotonNetwork.LocalPlayer);
    }

    private void Start()
    {
        ScopeChangerCanvas.GetComponent<Canvas>().worldCamera  = playerCharacter.GetCameraDepth();
        //Get canvas animation component.
        animationComponent = animatedCanvas.GetComponent<Animation>();
        //Find post process volumes in scene and assign them.
        postProcessingVolume = GameObject.Find("Post Processing Volume")?.GetComponent<PostProcessVolume>();
        //Get depth of field setting from main post process volume.
        if(postProcessingVolume != null)
            postProcessingVolume.profile.TryGetSettings(out depthOfField);
        if (postProcessingVolume != null)
            postProcessingVolume.profile.TryGetSettings(out vignette);

    }

    private void Update()
    {
        Tick();
    }

    private void Tick()
    {
        //Switch. Fades in or out the menu based on the cursor's state.
        bool scopeChanging = playerCharacter.IsScopeChanging();
        switch (scopeChanging)
        {
            //Hide.
            case false when menuIsEnabled:
                Hide();
                break;
            //Show.
            case true when !menuIsEnabled:
                Show();
                break;
        }
    }
    #endregion

    #region METHODS

    /// <summary>
    /// Shows the menu by playing an animation.
    /// </summary>
    private void Show()
    {
        //Enabled.
        menuIsEnabled = true;
        ScopeChangerCanvas.gameObject.SetActive(true);

        //Play Clip.
        animationComponent.clip = animationShow;
        animationComponent.Play();

        //Enable depth of field effect.
        if(depthOfField != null)
            depthOfField.active = true;
        if (vignette != null)
            vignette.active = true ;
    }
    /// <summary>
    /// Hides the menu by playing an animation.
    /// </summary>
    private void Hide()
    {
        //Disabled.
        menuIsEnabled = false;
        ScopeChangerCanvas.gameObject.SetActive(false);

        //Disable depth of field effect.
        if(depthOfField != null)
            depthOfField.active = false;
        if (vignette != null)
            vignette.active = false;
    }


    // #region SETER
    //
    // public void SetEventCM(Camera eventCamera)
    // {
    //     EventCM = eventCamera;
    // }
    //
    // #endregion
    //
    //
    // #region GETER
    //
    // public Camera GetEventCM() => EventCM;
    //
    // #endregion
    #endregion
}
