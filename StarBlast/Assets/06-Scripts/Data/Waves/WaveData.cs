using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveData_", menuName = "ScriptableObjects/WaveData/" + nameof(WaveData))]
public class WaveData : ScriptableObject
{
    [SerializeField] List<SubWaveData> _subWaves;
    [SerializeField] List<float> _spawnDelays; // Spawn delay between each subwave

    public List<SubWaveData> SubWaves => _subWaves;
    public List<float> SpawnDelays => _spawnDelays;
}
