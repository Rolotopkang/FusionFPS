//Copyright 2022, Infima Games. All Rights Reserved.

using Photon.Realtime;
using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Game Mode Service.
    /// </summary>
    public interface IGameModeService : IGameService
    {
        /// <summary>
        /// Returns the Player Character.
        /// </summary>
        CharacterBehaviour GetPlayerCharacter(Player player);

        GameObject GetPlayerGameObject(Player player);

        public void AddPlayerIntoList(GameObject player);
    }
}