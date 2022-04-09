//Copyright 2022, Infima Games. All Rights Reserved.

using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Game Mode Service.
    /// </summary>
    public class GameModeService : MonoBehaviourPunCallbacks, IGameModeService
    {
        #region FIELDS

        /// <summary>
        /// The Player List.
        /// </summary>
        public static List<GameObject> playerlist = new List<GameObject>(); 
        
        #endregion

        #region FUNCTIONS

        public void AddPlayerIntoList(GameObject player)
        {
            playerlist.Add(player);
            Debug.Log("RoomPlayerNum:"+playerlist.Count);
        }
        
        public CharacterBehaviour GetPlayerCharacter(Player player)
        {
            foreach (GameObject playerGameObject in playerlist)
            {
                if (playerGameObject.GetComponent<PhotonView>().Owner.Equals(player))
                {
                    CharacterBehaviour tmp_CharacterBehaviour;
                    playerGameObject.TryGetComponent(out tmp_CharacterBehaviour);
                    return tmp_CharacterBehaviour;
                }
            }
            
            return null;
        }


        public GameObject GetPlayerGameObject(Player player)
        {
            foreach (GameObject playerGameObject in playerlist)
            {
                if (playerGameObject.GetComponent<PhotonView>().Owner.Equals(player))
                {
                    return playerGameObject;
                }
            }
            
            return null;
        }
        #endregion
    }
}