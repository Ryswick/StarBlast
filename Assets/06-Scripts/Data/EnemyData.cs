using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData_", menuName = "ScriptableObjects/EnemyData/" + nameof(EnemyData))]
public class EnemyData : ScriptableObject
{
    [SerializeField] int _life;
    [SerializeField] float _moveSpeed;
    [SerializeField] int _score;
    [SerializeField] Vector3 _startRotationAngle;
    [SerializeField] ParticleSystem _deathParticle;
    [SerializeField] SoundType _deathSound;

    public int Life => _life;
    public float MoveSpeed => _moveSpeed;
    public int Score => _score;
    public ParticleSystem DeathParticle => _deathParticle;
    public Vector3 StartRotationAngle => _startRotationAngle;
    public SoundType DeathSound => _deathSound;
}
