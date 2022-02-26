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

    private void Awake()
    {
        ChangeToUI(0);
    }

    public void ChangeToUI(int set)
    {
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
