using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBase : MonoBehaviour
{
    public List<GameObject> UIList;
    private int currentUI = 0;
    private int commingUI;
    public bool aotoShowFirstPage = true;

    protected void Awake()
    {
        UIList[currentUI].SetActive(aotoShowFirstPage);
    }

    /// <summary>
    /// 切换到index层UI
    /// </summary>
    /// <param name="set">目标UI层级</param>
    public void ChangeToUI(int set)
    {
        if (set.Equals(currentUI))
        {
            if (!aotoShowFirstPage)
            {
                UIList[currentUI].SetActive(true);
            }
            return;
        }
        if (currentUI != -1)
        {
            UIList[currentUI].SetActive(false);
        }
        currentUI = set;
        UIList[currentUI].SetActive(true);
    }

    /// <summary>
    /// 切换到index层UI，如果传入UI显示则关闭
    /// </summary>
    /// <param name="set">目标UI层级</param>
    public void ChangeToUIAndClose(int set)
    {
        if (set.Equals(currentUI))
        {
            UIList[currentUI].SetActive(false);
            currentUI = -1;
        }
        else
        {
            ChangeToUI(set);
        }
    }

    public void CloseUI()
    {
        UIList[currentUI].SetActive(false);
        gameObject.SetActive(false);
    }

    public void CloseCurrentUI()
    {
        UIList[currentUI].SetActive(false);
    }
}
