using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ConquestPoint_Birth : MonoBehaviour
{
    private SpawnPoint[] SpawnPoints;

    private void Awake()
    {
        SpawnPoints = GetComponentsInChildren<SpawnPoint>();
    }
    
    public Transform GetBirthPoint()
    {
        List<SpawnPoint> tmp_spawnPoints = new List<SpawnPoint>();
        foreach (SpawnPoint spawnPoint in SpawnPoints)
        {
            if (spawnPoint.GetIsOpen())
            {
                tmp_spawnPoints.Add(spawnPoint);
            }
        }
        
        if (tmp_spawnPoints.Count.Equals(0))
        {
            Debug.Log("所有重生点均不可用");
            return SpawnPoints[0].transform;
        }
        
        return tmp_spawnPoints[Random.Range(0,tmp_spawnPoints.Count)].transform;
    }
}
