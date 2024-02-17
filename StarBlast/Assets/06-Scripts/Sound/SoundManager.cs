using Lean.Pool;
using SerializableCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    MenuMusic,
    LevelMusic,
    BossMusic,
    VictoryMusic,
    LevelStart,
    BulletFire,
    UltraPower,
    Hit,
    EnemyDeath,
    BarrelRoll,
    Death,
    FinishSound,
    BonusPickUp,
    BombFire,
    GameOverMusic,
    MineExplosion
}
[Serializable]
public struct ClipInformation
{
    public List<AudioClip> AudioClips;
    public int Priority;
    public float Volume;
    public bool IsLooping;
    public bool IsPositioned;
}

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField]
    GameObject _audioPlayerPrefab;

    [SerializeField]
    SoundDictionary _soundList;

    private void Awake()
    {
        InitializeSingleton(true);

        foreach (SoundTypeClipInformationTuple pair in _soundList.Pairs)
        {
            if (pair.Value.Volume == 0.0f)
            {
                ClipInformation clipInformation = pair.Value;
                clipInformation.Volume = 1.0f;

                _soundList[pair.Key] = clipInformation;
            }

            if (pair.Value.Priority == 0)
            {
                ClipInformation clipInformation = pair.Value;
                clipInformation.Priority = 128;

                _soundList[pair.Key] = clipInformation;
            }
        }
    }

    public AudioPlayer PlaySound(SoundType soundType, Transform playTransform = null, bool isPlayer = false)
    {
        AudioPlayer audioPlayer = LeanPool.Spawn(_audioPlayerPrefab).GetComponent<AudioPlayer>();

        audioPlayer.PlayAudio(_soundList[soundType], playTransform, isPlayer);

        return audioPlayer;
    }

    public ClipInformation GetSound(SoundType soundType)
    {
        return _soundList[soundType];
    }
}

[Serializable]
public class SoundTypeClipInformationTuple : SerializableKeyValuePair<SoundType, ClipInformation>
{
	public SoundTypeClipInformationTuple(SoundType item1, ClipInformation item2) : base(item1, item2) { }
}

[Serializable]
public class SoundDictionary : SerializableDictionary<SoundType, ClipInformation>
{
	[SerializeField] private List<SoundTypeClipInformationTuple> _pairs = new List<SoundTypeClipInformationTuple>();

    public List<SoundTypeClipInformationTuple> Pairs => _pairs;

    protected override List<SerializableKeyValuePair<SoundType, ClipInformation>> _keyValuePairs
	{
		get
		{
			var list = new List<SerializableKeyValuePair<SoundType, ClipInformation>>();
			foreach (var pair in _pairs)
			{
				list.Add(new SerializableKeyValuePair<SoundType, ClipInformation>(pair.Key, pair.Value));
			}
			return list;
		}

		set
		{
			_pairs.Clear();
			foreach (var kvp in value)
			{
				_pairs.Add(new SoundTypeClipInformationTuple(kvp.Key, kvp.Value));
			}
		}
	}
}
