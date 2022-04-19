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
    public class SettingManager :Singleton<SettingManager>
    {
        private List<ISettingChangeObserver> ISettingChangeObserverList = new List<ISettingChangeObserver>();

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
        }

        public void AddSettingChangeObserver(ISettingChangeObserver settingChangeObserver)
        {
            ISettingChangeObserverList.Add(settingChangeObserver);
        }
        
        public void RemoveSettingChangeObserver(ISettingChangeObserver settingChangeObserver)
        {
            ISettingChangeObserverList.Remove(settingChangeObserver);
        }

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