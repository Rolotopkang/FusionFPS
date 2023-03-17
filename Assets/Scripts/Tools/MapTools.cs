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
using UnityEngine;

namespace UnityTemplateProjects.Tools
{
    public static class MapTools
    {
        #region Struts

        public enum GameMode
        {
            DeathMatch,
            TeamDeathMatch,
            Conquest,
            BombScenario,
            TeamAdversarial
        }
        

        public struct Map
        {
            public int SceneIndex;
            public String MapName;
            public String MapDiscripName;
            public Dictionary<GameMode, int> GameModeIndex;

            public Map(int sceneIndex,string mapName, string mapDiscripName, Dictionary<GameMode, int> gameModeIndexs)
            {
                SceneIndex = sceneIndex;
                MapName = mapName;
                MapDiscripName = mapDiscripName;
                GameModeIndex = gameModeIndexs;
            }
        }

        #endregion
        
        
        #region Maps

        public static Map wrong = new Map(
            -1,
            "",
            "",
            new Dictionary<GameMode, int>
            {
                { GameMode.Conquest ,-1},
                { GameMode.BombScenario,-1},
                { GameMode.DeathMatch ,-1},
                { GameMode.TeamAdversarial ,-1},
                { GameMode.TeamDeathMatch ,-1}
            });
        
        public static Map de_nukeTown = new Map(
            2,
            "Assets/Scenes/De_Nuke_Town.unity",
            "NukeTown",
            new Dictionary<GameMode, int>()
            {
                { GameMode.DeathMatch ,2},
                { GameMode.TeamDeathMatch ,0}
            }
            );
        
        public static Map de_dust2 = new Map(
            5,
            "Assets/Scenes/de_dust2.unity",
            "Dust2",
            new Dictionary<GameMode, int>()
            {
                { GameMode.DeathMatch ,1},
                { GameMode.TeamDeathMatch ,1}
            });

        public static Map de_Western_Front = new Map(
            4,
            "Assets/Scenes/De_Western_Front.unity",
            "Western_Front",
            new Dictionary<GameMode, int>()
            {
                { GameMode.Conquest, 0 }
            }
        );
        
        public static Map old_nukeTown = new Map(
            3,
            "Assets/Scenes/Nuketown.unity",
            "OLD_NukeTown",
            new Dictionary<GameMode, int>()
            {
                { GameMode.DeathMatch, 0 },
                { GameMode.TeamDeathMatch, 2 }
            }
        );
        
        
        

        #endregion

        
        #region Methods

        public static Map IndexToMap(int index , GameMode gameMode)
        {
            Map[] Maps = {wrong, de_nukeTown, de_dust2,old_nukeTown,de_Western_Front};
            foreach (Map map in Maps)
            {
                foreach (GameMode key in map.GameModeIndex.Keys)
                {
                    if (key.Equals(gameMode))
                    {
                        if (map.GameModeIndex[gameMode].Equals(index))
                        {
                            return map;
                        }
                    }
                }
            }

            return wrong;
        }

        #endregion
    }
}