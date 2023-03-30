// ******************************************************************
//       /\ /|       @file       FILENAME
//       \ V/        @brief      
//       | "")       @author     topkang
//       /  |                    
//      /  \\        @Modified   DATE
//    *(__\_\        @Copyright  Copyright (c) YEAR, TOPGAMING
// ******************************************************************

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityTemplateProjects.Tools;

namespace UnityTemplateProjects.UI
{
    //设置更改观察者
    public class SettingManager :Singleton<SettingManager>
    {
       
     #region ExternFuncs

    //使用查找任务栏
    [DllImport("user32.dll")]
    static extern IntPtr FindWindow(string strClassName, int nptWindowName);
    //当前窗口
    [DllImport("user32.dll")]static extern IntPtr GetForegroundWindow();
    //获取窗口位置以及大小
    [DllImport("user32.dll")]static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);
    [StructLayout(LayoutKind.Sequential)]
    private struct RECT
    {
        public int Left; //最左坐标
        public int Top; //最上坐标
        public int Right; //最右坐标
        public int Bottom; //最下坐标
    }
    //设置窗口位置，尺寸
    [DllImport("user32.dll")]static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
    //设置无windows自带边框
    [DllImport("user32.dll")]static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, int dwNewLong);

    #endregion
    

    const uint SWP_SHOWWINDOW = 0x0040;
    const int GWL_STYLE = -16;
    const int WS_BORDER = 1;
    
    Resolution[] resolutions;//分辨率
    private Rect screenPosition;//最终的屏幕的位置和长宽
    
    
    private List<ISettingChangeObserver> ISettingChangeObserverList = new List<ISettingChangeObserver>();
    void Awake () {
        base.Awake();
        // //获取当前屏幕分辩率  
        // resolutions = Screen.resolutions;
        // //除任务栏外最大化窗口
        // witnOutBorder();
        // //无边框
        // Setposition();
    }
    
    /// <summary>
    /// 添加到设置更改观察者
    /// </summary>
    /// <param name="settingChangeObserver"></param>
    public void AddSettingChangeObserver(ISettingChangeObserver settingChangeObserver)
    {
        ISettingChangeObserverList.Add(settingChangeObserver);
    }
        
    /// <summary>
    /// 移除到设置更改观察者
    /// </summary>
    /// <param name="settingChangeObserver"></param>
    public void RemoveSettingChangeObserver(ISettingChangeObserver settingChangeObserver)
    {
        ISettingChangeObserverList.Remove(settingChangeObserver);
    }
    
    /// <summary>
    /// 获取当前窗口尺寸
    /// </summary>
    /// <returns></returns>
    public Rect GetWindowInfo()
    {
        RECT rect = new RECT();
        Rect targetRect = new Rect();
        GetWindowRect(GetForegroundWindow(), ref rect);
        targetRect.width = Mathf.Abs(rect.Right - rect.Left);
        targetRect.height = Mathf.Abs(rect.Top - rect.Bottom);

        //锚点在左上角
        targetRect.x = rect.Left;
        targetRect.y = rect.Top;
        return targetRect;
    }
    /// <summary>
    /// 获取任务栏高度
    /// </summary>
    /// <returns>任务栏高度</returns>
    private int GetTaskBarHeight()
    {
        int taskbarHeight = 10;
        IntPtr hWnd = FindWindow("Shell_TrayWnd", 0);       //找到任务栏窗口
        RECT rect = new RECT();
        GetWindowRect(hWnd, ref rect);                      //获取任务栏的窗口位置及大小
        taskbarHeight = (int)(rect.Bottom - rect.Top);      //得到任务栏的高度
        return taskbarHeight;
    }
    /// <summary>
    /// 除任务栏外最大化窗口
    /// </summary>
    private  void witnOutBorder()
    {
        //新的屏幕宽度
        screenPosition.width = resolutions[resolutions.Length - 1].width;
        //新的屏幕高度=当前屏幕分辨率的高度-状态栏的高度
        int currMaxScreenHeight = Screen.currentResolution.height - GetTaskBarHeight();
        screenPosition.height = currMaxScreenHeight;
        //新的分辨率(exe文件新的宽高)
        Screen.SetResolution((int)screenPosition.width, (int)screenPosition.height, false);

        //screenPosition.x = (int)((Screen.currentResolution.width - screenPosition.width) / 2);//宽度居中
        //screenPosition.y = (int)((Screen.currentResolution.height - screenPosition.height) / 2);//高度居中
        
        //设置无框
        SetWindowLong(GetForegroundWindow(), GWL_STYLE, WS_BORDER);
        //exe居左上显示；
        bool result = SetWindowPos(GetForegroundWindow(), 0, 0, 0, (int)screenPosition.width, (int)screenPosition.height, SWP_SHOWWINDOW);
        //exe居中显示；
        //bool result = SetWindowPos(GetForegroundWindow(), 0, (int)screenPosition.x, (int)screenPosition.y,  (int)screenPosition.width, (int)screenPosition.height, SWP_SHOWWINDOW);
    }

    private void Setposition()
    {
        SetWindowLong(GetForegroundWindow(), GWL_STYLE, WS_BORDER);
        bool result = SetWindowPos(GetForegroundWindow(), 0, 0, 0, resolutions[resolutions.Length - 1].width,
            resolutions[resolutions.Length - 1].height, SWP_SHOWWINDOW);
    }


        /// <summary>
        /// 更新设置
        /// </summary>
        public void UpdateSettings()
        {
            if (ISettingChangeObserverList.Count.Equals(0))
            {
                return;
            }
            foreach (var settingChangeObserver in ISettingChangeObserverList)
            {
                settingChangeObserver.OnSettingChange();
            }
        }
        
        
        
        
        
        
    }
}