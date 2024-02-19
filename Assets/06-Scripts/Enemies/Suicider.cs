using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suicider : Enemy
{
    [Header("Parameters")]
    [SerializeField] float _targetDuration = 2.0f;
    [SerializeField] float _accelerationSpeedMultiplier = 5.0f;

    Transform _playerTarget;

    float _targetDurationLeft = 0.0f;
    float _accelerationSpeed = 0.0f;
    Vector3 _direction;
    Vector3 _previousPosition;

    protected override void OnEnable()
    {
        base.OnEnable();

        _accelerationSpeed = 0.0f;
        _targetDurationLeft = _targetDuration;
        _previousPosition = transform.position;
        _playerTarget = StageLoop.Instance.GetPlayerTransform();
    }
    public override void ActivateBehaviour()
    {
        if (!GetComponent<SplineFollower>())
            _isBehaviourActivated = true;
    }

    private void Update()
    {
        if (_isBehaviourActivated)
        {
            // Run towards the player
            if (_targetDurationLeft < 0.0f)
            {
                transform.position += _direction * _accelerationSpeed * Time.deltaTime * _accelerationSpeedMultiplier;

                _accelerationSpeed += Time.deltaTime;
            }
            // Look at the player and evaluate the direction to rush to them
            else if (_playerTarget != null)
            {
                var lookPos = _playerTarget.position - transform.position;
                Quaternion lookRot = Quaternion.LookRotation(lookPos, Vector3.back);

                transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, 5.0f * Time.deltaTime);

                _targetDurationLeft -= Time.deltaTime;

                _direction = lookPos.normalized;
            }
        }
        // Just face the direction you're going towards
        else
        {
            var lookPos = transform.position - _previousPosition;
            if (lookPos != Vector3.zero)
            {
                Quaternion lookRot = Quaternion.LookRotation(lookPos, Vector3.back);

                transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, 5.0f * Time.deltaTime);
            }
        }

        _previousPosition = transform.position;
    }
}
