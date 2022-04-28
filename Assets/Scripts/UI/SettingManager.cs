// ******************************************************************
//       /\ /|       @file       FILENAME
//       \ V/        @brief      
//       | "")       @author     topkang
//       /  |                    
//      /  \\        @Modified   DATE
//    *(__\_\        @Copyright  Copyright (c) YEAR, TOPGAMING
// ******************************************************************

using System.Collections.Generic;
using UnityEngine;
using UnityTemplateProjects.Tools;

namespace UnityTemplateProjects.UI
{
    //设置更改观察者
    public class SettingManager :Singleton<SettingManager>
    {
        private List<ISettingChangeObserver> ISettingChangeObserverList = new List<ISettingChangeObserver>();

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
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