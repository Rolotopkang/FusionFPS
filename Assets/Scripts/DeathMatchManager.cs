using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class DeathMatchManager : GameModeManagerBehaviour
{
    protected override void OnPlayerDeath(EventData eventData)
    {
        base.OnPlayerDeath(eventData);
        Debug.Log("死斗模式人物死亡");
    }

    protected override void TickSec()
    {
        base.TickSec();
    }

    protected override void TickAll()
    {
        base.TickAll();
    }

    protected override void TickMaster()
    {
        base.TickMaster();
    }
}
