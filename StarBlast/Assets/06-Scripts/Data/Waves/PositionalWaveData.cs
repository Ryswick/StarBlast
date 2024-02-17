using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PositionalWaveData_", menuName = "ScriptableObjects/WaveData/SubWave/" + nameof(PositionalWaveData))]
public class PositionalWaveData : SubWaveData
{
    [SerializeField] protected List<Vector3> _spawnPositions;

    public List<Vector3> SpawnPositions => _spawnPositions;
    
    public override void Spawn()
    {
        base.Spawn();

        PositionalSpawnWaveSpawner.Instance.SpawnWave(this);
    }
}
