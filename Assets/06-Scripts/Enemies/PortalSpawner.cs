using DG.Tweening;
using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Enemy SpawnPoint
/// </summary>
public class PortalSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform _spawnPoint;

    AudioSource _portalOpeningSound;

    private void Awake()
    {
        _portalOpeningSound = GetComponent<AudioSource>();
    }

    public void Initialize(GameObject enemyToSpawn)
    {
        transform.localScale = Vector3.zero;

        StartCoroutine(SpawnEnemy(enemyToSpawn));
    }

    IEnumerator SpawnEnemy(GameObject enemyToSpawn)
    {
        _portalOpeningSound.Play();

        yield return new WaitForSeconds(0.33f);

        transform.DOScale(new Vector3(0.05f, 0.75f, 1.5f), 0.33f).SetEase(Ease.InOutElastic);

        yield return new WaitForSeconds(0.33f);

        transform.DOScale(new Vector3(1.5f, 1.0f, 1.5f), 0.33f).SetEase(Ease.InOutElastic);

        yield return new WaitForSeconds(0.33f);

        GameObject enemyObject = LeanPool.Spawn(enemyToSpawn, _spawnPoint.position, enemyToSpawn.transform.rotation, StageLoop.Instance.StageTransform);

        enemyObject.transform.DOMoveZ(0.0f, 1.5f);

        yield return new WaitForSeconds(1.5f);

        // Check if the object is still alive
        if(enemyObject.activeInHierarchy)
            enemyObject.GetComponent<Enemy>()?.ActivateBehaviour();

        transform.DOScale(Vector3.zero, 0.33f).SetEase(Ease.OutSine);

        yield return new WaitForSeconds(0.33f);

        LeanPool.Despawn(gameObject);
    }
}
