using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityTemplateProjects.UI.Models;


public class UIManager : MonoBehaviour
{
    public MapModelsScriptableObject MapModelsScriptableObject;

    public CanvasGroup MenuItemCanvasGroup;
    public Button PlayButton;
    public Button CreateGameButton;
    public Button JoinGameButton;
    public Button SettingButton;


    [Space] public Animator CreateGamePanelAnimator;
    public Button CancelToCreateGameButton;
    public Button ConfirmToCreateGameButton;

    public InputField RoomName;
    public InputField RoomPsw;
    public Dropdown MaxPlayer;
    public string MapName;


    [Space] public Animator JoinGamePanelAnimator;
    public Button CancelJoinToCreateGameButton;
    public Button JoinToCreateGameButton;
    public Image SelectedMapImage;
    public Text SelectedRoomName;

    public Launcher Launcher;


    private void Start()
    {
        CreateGameButton.onClick.AddListener(() =>
        {
            CreateGamePanelAnimator.SetTrigger("FadeIn");
            MenuItemCanvasGroup.interactable = false;
        });

        CancelToCreateGameButton.onClick.AddListener(() =>
        {
            CreateGamePanelAnimator.SetTrigger("FadeOut");
            MenuItemCanvasGroup.interactable = true;
        });


        JoinGameButton.onClick.AddListener(() =>
        {
            JoinGamePanelAnimator.SetTrigger("FadeIn");
            MenuItemCanvasGroup.interactable = false;
        });


        CancelJoinToCreateGameButton.onClick.AddListener(() =>
        {
            JoinGamePanelAnimator.SetTrigger("FadeOut");
            MenuItemCanvasGroup.interactable = true;
            SelectedRoomName.text = null;
            SelectedMapImage.sprite = null;
        });


        ConfirmToCreateGameButton.onClick.AddListener(() =>
        {
            Launcher.CreateRoom(RoomName.text, (byte) (MaxPlayer.value), int.Parse(RoomPsw.text), MapName);
        });
        JoinToCreateGameButton.onClick.AddListener(() =>
        {
            if (string.IsNullOrEmpty(SelectedRoomName.text)) return;
            PhotonNetwork.JoinRoom(SelectedRoomName.text);
        });
    }


    public void SetSelectedRoomDetails(RoomInfo _roomInfo)
    {
        Assert.AreNotEqual(_roomInfo.CustomProperties.Count, 0);
        var tmp_MatchedModel = MapModelsScriptableObject.MapModels.Find(_mapModel =>
            _mapModel.MapName.CompareTo(_roomInfo.CustomProperties["Map"]) == 0);
        SelectedRoomName.text = _roomInfo.Name;
        SelectedMapImage.sprite = tmp_MatchedModel.MapSprite;
    }
}