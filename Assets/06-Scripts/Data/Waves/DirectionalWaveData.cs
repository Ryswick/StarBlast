using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DirectionalWaveData_", menuName = "ScriptableObjects/WaveData/SubWave/" + nameof(DirectionalWaveData))]
public class DirectionalWaveData : PositionalWaveData
{
    [SerializeField] protected List<Vector3> _directions;

    public List<Vector3> Directions => _directions;

    public override void Spawn()
    {
        DirectionalWaveSpawner.Instance.SpawnWave(this);
    }
}
