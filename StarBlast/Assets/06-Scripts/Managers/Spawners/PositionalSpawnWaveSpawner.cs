using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionalSpawnWaveSpawner : Singleton<PositionalSpawnWaveSpawner>
{
    [Header("References")]
    [SerializeField] Transform _stageObject;
    [SerializeField] GameObject _portalPrefab;

    // Start is called before the first frame update
    void Awake()
    {
        InitializeSingleton();
    }

    public void SpawnWave(PositionalWaveData data)
    {
        for (int i = 0; i < data.EntityList.Count; i++)
        {
            StartCoroutine(SpawnEntity(data.EntityList[i], data.SpawnTimes[i], data.SpawnPositions[i]));
        }
    }

    IEnumerator SpawnEntity(GameObject entityPrefab, float spawnTime, Vector3 spawnPosition)
    {
        yield return new WaitForSeconds(spawnTime);

        spawnPosition.z = 2.0f;

        GameObject entity = LeanPool.Spawn(_portalPrefab, spawnPosition, _portalPrefab.transform.rotation, _stageObject);

        PortalSpawner portalSpawner = entity.GetComponent<PortalSpawner>();

        portalSpawner?.Initialize(entityPrefab);
    }

    public void StopSpawning()
    {
        StopAllCoroutines();
    }
}
