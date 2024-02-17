using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class AutoDestructShuriken : MonoBehaviour
{
	// If true, deactivate the object instead of destroying it
	public bool OnlyDeactivate;

	void OnEnable()
	{
		StartCoroutine("CheckIfAlive");
	}

	IEnumerator CheckIfAlive()
	{
		ParticleSystem ps = this.GetComponent<ParticleSystem>();

		while (true && ps != null)
		{
			yield return new WaitForSeconds(0.5f);
			if (!ps.IsAlive(true))
			{
				if (OnlyDeactivate)
				{
					Lean.Pool.LeanPool.Despawn(gameObject);
				}
				else
					GameObject.Destroy(this.gameObject);
				break;
			}
		}
	}
}
