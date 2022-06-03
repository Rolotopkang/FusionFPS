using System;
using System.Collections;
using System.Collections.Generic;
using InfimaGames.LowPolyShooterPack;
using UnityEngine;

public interface IWeaponInfoService : IGameService
{
    public WeaponData GetWeaponInfoFromID(int id);

    public WeaponData GetWeaponInfoFromName(String weaponName);
}
