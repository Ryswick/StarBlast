using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : Enemy
{
    [Header("References")]
    [SerializeField] Cannon _cannon;

    Transform _playerTarget;

    Vector3 _previousPosition;

    bool _isAimingPlayer = false;

    protected override void OnEnable()
    {
        base.OnEnable();

        _isAimingPlayer = false;
        _previousPosition = transform.position;
        _playerTarget = StageLoop.Instance.GetPlayerTransform();
        EventManager.Instance.AddListener<GameFinishedEvent>(OnGameFinished);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        EventManager.Instance?.RemoveListener<GameFinishedEvent>(OnGameFinished);
    }

    public override void ActivateBehaviour()
    {
        base.ActivateBehaviour();

        if (!GetComponent<SplineFollower>())
            ChangePlayerAttackingBehavior();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isBehaviourActivated)
        {
            if (_isAimingPlayer && _playerTarget != null)
            {
                var lookPos = _playerTarget.position - transform.position;
                Quaternion lookRot = Quaternion.LookRotation(lookPos, Vector3.back);

                transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, 5.0f * Time.deltaTime);
            }
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

    // Change behaviour from attacking the player to piloting or the opposite
    public void ChangePlayerAttackingBehavior()
    {
        if(_isAimingPlayer)
        {
            _isAimingPlayer = false;

            _cannon.StopFiring();
        }
        else
        {
            // We only start hunting if the player is still active
            if(_playerTarget)
            {
                _isAimingPlayer = true;

                _cannon.StartFiring(1.0f);
            }
        }
    }

    void OnGameFinished(GameFinishedEvent e)
    {
        _isAimingPlayer = false;
        _cannon.StopFiring();
    }
}
