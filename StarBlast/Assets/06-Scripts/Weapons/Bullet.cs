using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Weapon
{
    private void OnEnable()
    {
        Invoke(nameof(DeleteObject), _weaponData.LifeTime);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(DeleteObject));
    }

    public override void HitTarget()
	{
		base.HitTarget();

		DeleteObject();
	}
}
