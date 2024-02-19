using Dreamteck.Splines;
using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[Header("Parameter")]
	[SerializeField] protected EnemyData _enemyData;
	[SerializeField] protected List<MeshRenderer> _meshRenderers;

	[Header("Test")]
	[SerializeField] bool _testEnemy = false;

	protected int _life = 0;

	List<Texture> _emissionMaps;
	List<ColorChanger> _emissionColors;

	protected Coroutine _hitBlinkCoroutine;

	protected EnemyBehaviour _enemyBehaviour;

	public float MoveSpeed => _enemyData.MoveSpeed;

	protected bool _isBehaviourActivated = false;

	//------------------------------------------------------------------------------

	protected virtual void Awake()
    {
		_emissionMaps = new List<Texture>();
		_emissionColors = new List<ColorChanger>();

		for (int i = 0; i < _meshRenderers.Count; i++)
        {
			_emissionMaps.Add(_meshRenderers[i].material.GetTexture("_EmissionMap"));
			_emissionColors.Add(new ColorChanger(_meshRenderers[i].material.GetColor("_EmissionColor")));
		}

		if (_testEnemy)
			ActivateBehaviour();
	}

	protected virtual void OnEnable()
	{
		_life = _enemyData.Life;

		transform.rotation = Quaternion.Euler(_enemyData.StartRotationAngle);
	}

	protected virtual void OnDisable()
    {
		// Set to full black
		for (int i = 0; i < _meshRenderers.Count; i++)
		{
			_meshRenderers[i].material.SetTexture("_EmissionMap", _emissionMaps[i]);
			_meshRenderers[i].material.SetColor("_EmissionColor", _emissionColors[i].originalColor);
		}

		if(_enemyBehaviour)
			_enemyBehaviour.Stop();

		_isBehaviourActivated = false;
	}

	#region Initialize
	public void InitializeDirectionalBehaviour(Vector3 direction)
	{
		DirectionalMoveBehaviour behaviour = gameObject.AddComponent<DirectionalMoveBehaviour>();

		behaviour.Initialize(direction, _enemyData.MoveSpeed);

		_enemyBehaviour = behaviour;
	}

	public void InitializePositionalSpawnBehaviour(Vector3 spawnPosition)
	{
		PositionalSpawnBehaviour behaviour = gameObject.AddComponent<PositionalSpawnBehaviour>();

		behaviour.Initialize(spawnPosition);

		_enemyBehaviour = behaviour;
	}

	public void InitializeSplineBehaviour(SplineComputer splineComputer, float followSpeed, float huntingSpeed)
	{
		SplineFollowBehaviour behaviour = gameObject.AddComponent<SplineFollowBehaviour>();
		behaviour.Initialize(splineComputer, followSpeed, huntingSpeed);
		_enemyBehaviour = behaviour;
	}
    #endregion

	public virtual void ActivateBehaviour()
    { 
		_isBehaviourActivated = true;
	}

    //------------------------------------------------------------------------------

    private void OnTriggerEnter(Collider other)
    {
		if (_life > 0)
		{
			if (other.gameObject.layer == LayerMask.NameToLayer("PlayerWeapon"))
			{
				Weapon weapon = other.transform.GetComponent<Weapon>();
				if (weapon)
				{
					HitByWeapon(weapon);

					weapon.HitTarget();
				}
			}
			else if (other.gameObject.layer == LayerMask.NameToLayer("PlayerWeapon"))
			{
				HitByPlayer();
			}
		}
	}

    void HitByWeapon(Weapon weapon)
    {
		_life -= weapon.Damage;

		if(_life <= 0)
		{
			if (StageLoop.Instance)
			{
				StageLoop.Instance.AddScore(_enemyData.Score);
			}

			DeleteObject(true);
		}
		else
        {
			if(_hitBlinkCoroutine == null)
				_hitBlinkCoroutine = StartCoroutine(HitBlink());
        }
    }

	public virtual void HitByPlayer()
	{
		DeleteObject(true);
	}

	IEnumerator HitBlink()
    {
		// Start from white (remove emission map if necessary)
		for(int i = 0; i < _meshRenderers.Count; i++)
		{
			_meshRenderers[i].material.SetTexture("_EmissionMap", null);
			_meshRenderers[i].material.SetColor("_EmissionColor", Color.white);
		}

		Color currentColor = Color.white;
		float startTime = Time.time;
		float percentage = 0.0f;
		float lerpDuration = 0.15f;

		// Slowly gets back to black
		while (percentage < 1f)
		{
			percentage = (Time.time - startTime) / lerpDuration;
			
			for (int i = 0; i < _meshRenderers.Count; i++)
			{
				currentColor = Color.Lerp(Color.white, _emissionColors[i].color, percentage);
				ColorChanger colorChanger = new ColorChanger(currentColor);
				_meshRenderers[i].material.SetColor("_EmissionColor", colorChanger.color);
			}

			yield return null;
		}

		// Set to full black / set back emission map
		for (int i = 0; i < _meshRenderers.Count; i++)
		{
			_meshRenderers[i].material.SetTexture("_EmissionMap", _emissionMaps[i]);
			_meshRenderers[i].material.SetColor("_EmissionColor", _emissionColors[i].originalColor);
		}

		_hitBlinkCoroutine = null;
	}

	public void DeleteObject(bool fromDeath)
	{
		if (fromDeath && _enemyData.DeathParticle != null)
		{
			LeanPool.Spawn(_enemyData.DeathParticle, transform.position, _enemyData.DeathParticle.transform.rotation);
			SoundManager.Instance.PlaySound(_enemyData.DeathSound);
		}

		if (_hitBlinkCoroutine != null)
		{
			StopCoroutine(_hitBlinkCoroutine);
			_hitBlinkCoroutine = null;
		}

		LeanPool.Despawn(gameObject);
	}
}
