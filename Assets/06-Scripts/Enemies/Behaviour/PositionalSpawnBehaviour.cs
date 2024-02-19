using Dreamteck.Splines;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionalSpawnBehaviour : EnemyBehaviour
{
    public void Initialize(Vector3 spawnPosition)
    {
        transform.position = spawnPosition;
    }

    public override void Stop()
    {
        base.Stop();
        Destroy(this);
    }
}
