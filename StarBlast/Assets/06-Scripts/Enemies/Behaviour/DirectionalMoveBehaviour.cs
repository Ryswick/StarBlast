using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalMoveBehaviour : EnemyBehaviour
{
    Vector3 _direction;
    float _moveSpeed;

    // Update is called once per frame
    private void Update()
    {
        //move
        transform.position += _direction * _moveSpeed * Time.deltaTime;
    }

    public void Initialize(Vector3 direction, float moveSpeed)
    {
        _direction = direction;

        _moveSpeed = moveSpeed;
    }
    public override void Stop()
    {
        base.Stop();
        Destroy(this);
    }
}
