using DG.Tweening;
using Dreamteck.Splines;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineFollowBehaviour : EnemyBehaviour
{
    SplineComputer _splineComputer;
    SplineFollower _splineFollower;

    float _followSpeed; // Speed when not hunting the player
    float _huntingSpeed; // Speed when attacking the player

    bool _isHunting = false;

    int _speedChangeTweenID = -1;

    public void SwitchBehaviour()
    {
        _isHunting = !_isHunting;

        if(_isHunting)
        {
            int _speedChangeTweenID = DOTween.To(() => _splineFollower.followSpeed, x => _splineFollower.followSpeed = x, _huntingSpeed, 0.33f).intId;
        }
        else
        {
            int _speedChangeTweenID = DOTween.To(() => _splineFollower.followSpeed, x => _splineFollower.followSpeed = x, _followSpeed, 0.33f).intId;
        }

        Shooter shooter = gameObject.GetComponent<Shooter>();

        shooter?.ChangePlayerAttackingBehavior();
    }

    public void Initialize(SplineComputer splineComputer, float followSpeed, float huntingSpeed)
    {
        _splineComputer = splineComputer;
        _followSpeed = followSpeed;
        _huntingSpeed = huntingSpeed;

        _splineFollower = gameObject.AddComponent<SplineFollower>();
        _splineFollower.useTriggers = true;
        _splineFollower.spline = splineComputer;
        _splineFollower.motion.applyRotationX = false;
        _splineFollower.motion.applyRotationY = false;
        _splineFollower.motion.applyRotationZ = false;
        _splineFollower.followSpeed = followSpeed;
        _splineFollower.Restart(0);

        _speedChangeTweenID = -1;

        _splineFollower.onEndReached += OnSplineEndReached;
    }

    void OnSplineEndReached(double value)
    {
        Enemy enemy = GetComponent<Enemy>();

        enemy?.DeleteObject(false);
    }

    public override void Stop()
    {
        base.Stop();
        _splineFollower.onEndReached -= OnSplineEndReached;

        if(_speedChangeTweenID != -1 && DOTween.IsTweening(_speedChangeTweenID))
        {
            DOTween.Kill(_speedChangeTweenID);
        }

        _splineComputer?.GetComponent<SplineLife>()?.ReduceAmountEnemies();

        Destroy(_splineFollower);
        Destroy(this);
    }
}
