using Dreamteck.Splines;
using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineLife : MonoBehaviour
{
    int _amountEnemies;

    public void Initialize(int amountEnemies)
    {
        _amountEnemies = amountEnemies;
    }

    public void OnCross(SplineUser user)
    {
        SplineFollowBehaviour splineFollowBehaviour = user.GetComponent<SplineFollowBehaviour>();

        if (splineFollowBehaviour)
        {
            splineFollowBehaviour.SwitchBehaviour();
        }
    }

    public void ReduceAmountEnemies()
    {
        _amountEnemies--;

        if(_amountEnemies == 0 && gameObject.activeInHierarchy)
        {
            LeanPool.Despawn(gameObject);
        }
    }
}
