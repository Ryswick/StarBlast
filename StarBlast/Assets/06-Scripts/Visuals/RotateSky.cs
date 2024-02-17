using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSky : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float _rotationSpeed = 0.01f;

    bool _isRotating = false;

    // Update is called once per frame
    void Update()
    {
        if(_isRotating)
            transform.Rotate(-_rotationSpeed, 0, 0);
    }

    public void ActivateRotation()
    {
        _isRotating = true;
    }

    public void DeactivateRotation()
    {
        _isRotating = false;
    }
}
