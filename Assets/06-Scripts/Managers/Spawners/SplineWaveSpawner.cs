using Dreamteck.Splines;
using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineWaveSpawner : Singleton<SplineWaveSpawner>
{
    [Header("References")]
    [SerializeField] Transform _stageObject;

    // Start is called before the first frame update
    void Awake()
    {
        InitializeSingleton();
    }

    public void SpawnWave(SplineWaveData data)
    {
        GameObject spline = LeanPool.Spawn(data.Spline, _stageObject);
        SplineComputer splineComputer = spline.GetComponent<SplineComputer>();

        for (int i = 0; i < data.EntityList.Count; i++)
        {
            StartCoroutine(SpawnEntity(data.EntityList[i], data.SpawnTimes[i], splineComputer, data.FollowSpeeds[i], data.HuntingSpeeds[i]));
        }

        splineComputer.GetComponent<SplineLife>()?.Initialize(data.EntityList.Count);
    }

    IEnumerator SpawnEntity(GameObject entityPrefab, float spawnTime, SplineComputer splineComputer, float followSpeed, float huntingSpeed)
    {
        yield return new WaitForSeconds(spawnTime);

        GameObject entity = LeanPool.Spawn(entityPrefab, new Vector3(20.0f, 20.0f, 0.0f), Quaternion.identity, _stageObject);
        Enemy enemy = entity.GetComponent<Enemy>();

        if (enemy)
        {
            enemy.InitializeSplineBehaviour(splineComputer, followSpeed, huntingSpeed);
            enemy.ActivateBehaviour();
        }
    }

    public void StopSpawning()
    {
        StopAllCoroutines();
    }
}
