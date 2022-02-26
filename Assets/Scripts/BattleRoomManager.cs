using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BattleRoomManager : MonoBehaviour
{
    public string PlayerPrefabName;

    private void Start()
    {
        StartSpawn(0);
        Player.Respawn += StartSpawn;
    }


    private void OnDisable()
    {
        Player.Respawn -= StartSpawn;
    }

    private void StartSpawn(float _timeToSpawn)
    {
        StartCoroutine(WaitToInstantiatePlayer(_timeToSpawn));
    }


    private IEnumerator WaitToInstantiatePlayer(float _timeToSpawn)
    {
        yield return new WaitForSeconds(_timeToSpawn);
        PhotonNetwork.Instantiate(PlayerPrefabName, Vector3.zero, Quaternion.identity);
    }
}