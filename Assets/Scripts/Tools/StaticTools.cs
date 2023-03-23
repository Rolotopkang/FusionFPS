using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;

public static class StaticTools
{
    public static bool IsEnemy(Player local , PhotonView target)
    {
        // Debug.Log("LocalName"+local.GetPhotonTeam().Name);
        // Debug.Log("targetName"+target.Owner.GetPhotonTeam().Name);
        return !local.GetPhotonTeam().Name.
            Equals(target.Owner.GetPhotonTeam().Name);
    }

    public static bool IsEnemy(Player local, Player target)
    {
        return !local.GetPhotonTeam().Name.
            Equals(target.GetPhotonTeam().Name);
    }
}
