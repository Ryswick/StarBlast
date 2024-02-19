using DG.Tweening;
using Lean.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player Character
/// </summary>
public class Player : MonoBehaviour
{
	[Header("Parameter")]
	[SerializeField] float _moveSpeed = 5;
	[SerializeField] float _maxVelocity = 12;

	[Header("References")]
	[SerializeField] Transform _model;
	[SerializeField] Cannon _frontCannons;
	[SerializeField] Cannon _rearCannons;
	[SerializeField] SoundType _deathSound;
	[SerializeField] ParticleSystem _deathExplosionPrefab;
	[SerializeField] Rigidbody _rigidbody;

	Camera _mainCamera;

	bool _isAlive = false;
	bool _canBeControlled = false;
	bool _areRearCannonsActivated = false;
	bool _isFiring = false;
	bool _isBarrelRolling = false;


	//------------------------------------------------------------------------------

	public void StartRunning()
	{
		_isAlive = true;
		_canBeControlled = true;
		_mainCamera = Camera.main;
	}

    private void OnEnable()
    {
		EventManager.Instance?.AddListener<GameFinishedEvent>(OnGameFinished);
    }

    private void OnDisable()
    {
		EventManager.Instance?.RemoveListener<GameFinishedEvent>(OnGameFinished);
	}

    //
    private void Update()
    {
		if (_canBeControlled)
		{
			// Shoot
			{
				if (Input.GetButtonDown("Fire1") && !_isFiring)
				{
					ActivateCannons();
				}
				else if (Input.GetButtonUp("Fire1") && _isFiring)
				{
					DeactivateCannons();
				}
			}

			// Rotation
			{
				Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				mousePosition.z = 0.0f;

				var lookPos = mousePosition - transform.position;

				if (lookPos != Vector3.zero)
				{
					Quaternion lookRot = Quaternion.LookRotation(lookPos, Vector3.back);

					transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, 15.0f * Time.deltaTime);
				}

				// Model tilt
				_model.localRotation = Quaternion.Slerp(_model.localRotation, Quaternion.Euler(_rigidbody.velocity.y * 3.0f, 0.0f, _rigidbody.velocity.x * -5f), 0.3f);
			}
		}
	}

    private void FixedUpdate()
	{
		if (_canBeControlled)
		{
			{
				// Quickly decelerate if no input
				if (!Input.GetButton("Horizontal") && !Input.GetButton("Vertical"))
				{
					_rigidbody.velocity = _rigidbody.velocity / 10.0f * Time.deltaTime;
				}
				// Else move
				else
				{
					Vector3 currentVelocity = _rigidbody.velocity;

					if (Input.GetKey(KeyCode.LeftArrow))
					{
						if (currentVelocity.x > 0.0f)
						{
							_rigidbody.velocity = new Vector3(0.0f, currentVelocity.y, currentVelocity.z);
						}

						_rigidbody.AddForce(Vector3.left * _moveSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);

						if (_rigidbody.velocity.x < -_maxVelocity)
						{
							_rigidbody.velocity = new Vector3(-_maxVelocity, currentVelocity.y, currentVelocity.z);
						}
					}
					else if (Input.GetKey(KeyCode.RightArrow))
					{
						if (currentVelocity.x < 0.0f)
						{
							_rigidbody.velocity = new Vector3(0.0f, currentVelocity.y, currentVelocity.z);
						}

						_rigidbody.AddForce(Vector3.right * _moveSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);

						if (currentVelocity.x > _maxVelocity)
						{
							_rigidbody.velocity = new Vector3(_maxVelocity, currentVelocity.y, currentVelocity.z);
						}
					}
					if (Input.GetKey(KeyCode.UpArrow))
					{
						if (currentVelocity.y < 0.0f)
						{
							_rigidbody.velocity = new Vector3(currentVelocity.x, 0.0f, currentVelocity.z);
						}
						_rigidbody.AddForce(Vector3.up * _moveSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);

						if (currentVelocity.y > _maxVelocity)
						{
							_rigidbody.velocity = new Vector3(currentVelocity.x, _maxVelocity, currentVelocity.z);
						}
					}
					else if (Input.GetKey(KeyCode.DownArrow))
					{
						if (currentVelocity.y > 0.0f)
						{
							_rigidbody.velocity = new Vector3(currentVelocity.x, 0.0f, currentVelocity.z);
						}

						_rigidbody.AddForce(Vector3.down * _moveSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);

						if (currentVelocity.y < -_maxVelocity)
						{
							_rigidbody.velocity = new Vector3(currentVelocity.x, -_maxVelocity, currentVelocity.z);
						}
					}
				}
			}
		}
	}

    private void LateUpdate()
    {
		if (_canBeControlled)
		{
			// Keep the player in camera range
			Vector3 pos = _mainCamera.WorldToViewportPoint(transform.position);
			pos.x = Mathf.Clamp01(pos.x);
			pos.y = Mathf.Clamp01(pos.y);
			transform.position = _mainCamera.ViewportToWorldPoint(pos);
		}
	}

    private void OnTriggerEnter(Collider other)
	{
		if (_isAlive)
		{
			if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
			{
				Kill();
			}
			else if (other.gameObject.layer == LayerMask.NameToLayer("EnemyWeapon"))
			{
				Weapon weapon = other.transform.GetComponent<Weapon>();

				if (weapon)
				{
					weapon.HitTarget();

					Kill();
				}
			}
			/*Weapon weapon = other.transform.GetComponent<Weapon>();
			if (weapon)
			{
				HitByWeapon(weapon);

				weapon.HitTarget();
			}*/
		}
	}

	void Kill()
	{
		_isAlive = false;
		_canBeControlled = false;

		SoundManager.Instance.PlaySound(_deathSound);

		if (_deathExplosionPrefab != null)
		{
			Instantiate(_deathExplosionPrefab, transform.position, _deathExplosionPrefab.transform.rotation, StageLoop.Instance.StageTransform);
		}

		EventManager.Instance.QueueEvent(new GameFinishedEvent(false));

		DeleteObject();
    }

	#region Cannons
	private void ActivateCannons()
	{
		_isFiring = true;

		_frontCannons.StartFiring();

		if (_areRearCannonsActivated)
		{
			_rearCannons.StartFiring();
		}
	}

	private void DeactivateCannons()
	{
		_isFiring = false;

		_frontCannons.StopFiring();

		if (_areRearCannonsActivated)
		{
			_rearCannons.StopFiring();
		}
	}
    #endregion Cannons

	private void OnGameFinished(GameFinishedEvent e)
	{
		_canBeControlled = false;
	}

	private void DeleteObject()
	{
		Lean.Pool.LeanPool.Despawn(gameObject);
	}
}
