using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginUIManager : MonoBehaviour
{
    public List<GameObject> UIList;
    private int currentUI = 0;
    private int commingUI;

    public bool IsSetting = false;

    private void Awake()
    {
        UIList[currentUI].SetActive(true);
    }

    private void OnEnable()
    {
        if (IsSetting)
        {
            foreach (GameObject UI in UIList)
            {
                UI.SetActive(true);
            }

            SettingManager tmp_SettingManager = gameObject.transform.parent.GetComponent<SettingManager>();
            tmp_SettingManager.AudioSettings(false);
            tmp_SettingManager.GameSettings(false);
            foreach (GameObject UI in UIList)
            {
                UI.SetActive(false);
            }
            UIList[currentUI].SetActive(true);
        }
    }

    public void ChangeToUI(int set)
    {
        if (set.Equals(currentUI))
        {
            return;
        }
        UIList[currentUI].SetActive(false);
        currentUI = set;
        UIList[currentUI].SetActive(true);
    }

    public void CloseUI()
    {
        UIList[currentUI].SetActive(false);
        gameObject.SetActive(false);
    }
}
