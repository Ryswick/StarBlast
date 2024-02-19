using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalWaveSpawner : Singleton<DirectionalWaveSpawner>
{
    [Header("References")]
    [SerializeField] Transform _stageObject;

    // Start is called before the first frame update
    void Awake()
    {
        InitializeSingleton();
    }

    public void SpawnWave(DirectionalWaveData data)
    {
        for(int i = 0; i < data.EntityList.Count; i++)
        {
            StartCoroutine(SpawnEntity(data.EntityList[i], data.SpawnTimes[i], data.SpawnPositions[i], data.Directions[i]));
        }
    }

    IEnumerator SpawnEntity(GameObject entityPrefab, float spawnTime, Vector3 spawnPosition, Vector3 direction)
    {
        yield return new WaitForSeconds(spawnTime);

        GameObject entity = LeanPool.Spawn(entityPrefab, _stageObject);

        Enemy enemy = entity.GetComponent<Enemy>();
        enemy.transform.position = spawnPosition;

        if (enemy)
        {
            enemy.InitializeDirectionalBehaviour(direction);
            enemy.ActivateBehaviour();
        }
    }

    public void StopSpawning()
    {
        StopAllCoroutines();
    }
}
