using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SplineWaveData_", menuName = "ScriptableObjects/WaveData/SubWave/" + nameof(SplineWaveData))]
public class SplineWaveData : SubWaveData
{
    [SerializeField] protected List<float> _followSpeeds;
    [SerializeField] protected List<float> _huntingSpeeds;
    [SerializeField] protected GameObject _spline;

    public List<float> FollowSpeeds => _followSpeeds;
    public List<float> HuntingSpeeds => _huntingSpeeds;
    public GameObject Spline => _spline;

    public override void Spawn()
    {
        base.Spawn();

        SplineWaveSpawner.Instance.SpawnWave(this);
    }
}
