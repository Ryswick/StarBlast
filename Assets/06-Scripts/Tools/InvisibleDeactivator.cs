using Dreamteck.Splines;
using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleDeactivator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject _objectToDeactivate;

    bool _isInvisible = false;

    private void OnBecameInvisible()
    {
        if (_objectToDeactivate != null && _objectToDeactivate.activeInHierarchy)
        {
            // Deactivate any object that is not currently on a spline and could come back
            if (!_objectToDeactivate.GetComponent<SplineFollower>())
            {
                LeanPool.Despawn(_objectToDeactivate);
            }
        }
    }
}
