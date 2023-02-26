using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class MenusJump : MonoBehaviour
{
    [SerializeField]
    private GameObject Login;
    [SerializeField]
    private MenuController Main;
    [SerializeField]
    private UIBase MainBase;
    [SerializeField]
    private UIBase LobbyBase;

    public enum Page
    {
        Login,
        Main,
        Room,
        Setting
    }

    public void JumpToUI(Page page)
    {
        switch(page)
        {
            case Page.Room:
                Login.SetActive(false);
                Main.onUIshow();
                MainBase.ChangeToUI(1);
                LobbyBase.ChangeToUI(0);
                break;
            
            default:
                break;
        }
    }
}
