using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    
    private SpawnPoint[] SpawnPoints;
    
    protected override void Awake()
    {
        base.Awake();
        SpawnPoints = GetComponentsInChildren<SpawnPoint>();
    }

    public Transform GetSpawnPoint()
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
            return transform;
        }
        
        return tmp_spawnPoints[Random.Range(0,tmp_spawnPoints.Count)].transform;
    }
}
