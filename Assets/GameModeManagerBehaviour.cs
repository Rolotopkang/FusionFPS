using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class GameModeManagerBehaviour : MonoBehaviour,IOnEventCallback
{
    
    
    public void OnEvent(EventData photonEvent)
    {
        switch ((Scripts.Weapon.EventCode) photonEvent.Code)
        {
            case Scripts.Weapon.EventCode.KillPlayer:
                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                    OnPlayerDeath(photonEvent);
                }
                break;
        }
    }

    protected virtual void OnPlayerDeath(EventData eventData)
    {
        
    }
    
    
}
