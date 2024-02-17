using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] Weapon _weaponPrefab;
    [SerializeField] SoundType _fireSound;
    
    [Header("Parameter")]
    [SerializeField] float _fireRate = 0.1f;
	[SerializeField] bool _isOwnedByPlayer = false;

	[SerializeField] List<Transform> _firePoints;

	bool _isFiring = false;

	Transform _stageTransform;

    Coroutine _fireCoroutine;

    private void OnEnable()
    {
		_stageTransform = StageLoop.Instance.StageTransform;
	}

    private void OnDisable()
    {
		if (_fireCoroutine != null)
		{
			StopCoroutine(_fireCoroutine);
			_fireCoroutine = null;
		}
    }

    public void StartFiring(float startDelay = 0.0f)
    {
		_isFiring = true;

		// If there's no fire coroutine running, we start a new one
		if (_fireCoroutine == null)
		{
			_fireCoroutine = StartCoroutine(FireCoroutine(startDelay));
		}
	}

	public void StopFiring()
    {
		_isFiring = false;
    }

	private IEnumerator FireCoroutine(float startDelay)
	{
		yield return new WaitForSeconds(startDelay);

		do
		{
			for (int i = 0; i < _firePoints.Count; i++)
			{
				FireBullet(_firePoints[i].position);
			}

			SoundManager.Instance.PlaySound(_fireSound);

			yield return new WaitForSeconds(_fireRate);
		} while (_isFiring);

		_fireCoroutine = null;
	}

	void FireBullet(Vector3 position)
	{
		position.z = 0.0f;
		Weapon weapon = LeanPool.Spawn(_weaponPrefab, transform.position, _weaponPrefab.transform.rotation, _stageTransform);
		weapon.transform.position = position;

		Vector3 direction = transform.forward;
		direction.z = 0.0f;
		weapon.Initialize(direction, _isOwnedByPlayer);
	}
}
