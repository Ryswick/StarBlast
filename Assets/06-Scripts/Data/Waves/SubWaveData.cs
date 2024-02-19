using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubWaveData : ScriptableObject
{
    [SerializeField] protected List<GameObject> _entityList;
    [SerializeField] protected List<float> _spawnTimes;

    public List<GameObject> EntityList => _entityList;
    public List<float> SpawnTimes => _spawnTimes;

    public virtual void Spawn()
    {

    }
}
