using System;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	public static T Instance
	{
		get
		{
			if (_instance == null) _instance = FindObjectOfType<T>();

			return _instance;
		}
	}
	private static T _instance;

	private static bool _persistenceSet = false;

	protected void InitializeSingleton(bool persistent = true)
	{
		if (_instance == null || (persistent && !_persistenceSet))
		{
			_instance = (T)Convert.ChangeType(this, typeof(T));
			if (persistent)
			{
				_persistenceSet = true;
				DontDestroyOnLoad(_instance);
			}
		}
		else
		{
			if (_instance.gameObject != gameObject)
			{
				Debug.LogWarning($"Another instance of Singleton<{typeof(T).Name}> detected on GO {name}. Destroyed", gameObject);
				Destroy(gameObject);
			}
		}
	}
}