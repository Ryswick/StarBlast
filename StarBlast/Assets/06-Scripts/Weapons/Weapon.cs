using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Weapon
/// </summary>
public class Weapon : MonoBehaviour
{
	[Header("Parameter")]
	[SerializeField] protected WeaponData _weaponData;

	public int Damage => _weaponData.Damage;

	Vector3 _direction;

	//

	public void Initialize(Vector3 direction, bool isPlayerWeapon)
    {
		_direction = direction;

		transform.LookAt(transform.position + _direction);

		if(isPlayerWeapon)
        {
			gameObject.layer = 8;
        }
		else
        {
			gameObject.layer = 10;
		}
	}

	protected virtual void Update()
	{
		transform.position += _direction *_weaponData.MoveSpeed * Time.deltaTime;
	}

	public virtual void HitTarget()
    {
		if (_weaponData.HitParticle != null)
		{
			LeanPool.Spawn(_weaponData.HitParticle, transform.position, _weaponData.HitParticle.transform.rotation);
		}
	}

	public void DeleteObject()
	{
		Lean.Pool.LeanPool.Despawn(gameObject);
	}
}
