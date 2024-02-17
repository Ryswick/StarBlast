using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData_", menuName = "ScriptableObjects/WeaponData/" + nameof(WeaponData))]
public class WeaponData : ScriptableObject
{
    [SerializeField] int _damage;
    [SerializeField] float _moveSpeed;
    [SerializeField] float _lifeTime;
    [SerializeField] ParticleSystem _hitParticle;

    public int Damage => _damage;
    public float MoveSpeed => _moveSpeed;
    public float LifeTime => _lifeTime;
    public ParticleSystem HitParticle => _hitParticle;
}
