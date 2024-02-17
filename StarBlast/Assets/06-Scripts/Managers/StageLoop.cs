using DG.Tweening;
using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Stage main loop
/// </summary>
public class StageLoop : Singleton<StageLoop>
{
	//
	[Header("Layout")]
	[SerializeField] Transform _stageTransform;

	[Header("Parameters")]
	[SerializeField] int _amountWavesBeforeBoss = 10;
	int _currentWave;

	[Header("References")]
	[SerializeField] List<WaveData> _waveDatas;

	[Header("Prefab")]
	[SerializeField] Player _prefabPlayer;

	[Header("Test")]
	[SerializeField] bool _isTestingWave;
	[SerializeField] WaveData _testedWave;

	//
	int _gameScore = 0;
	bool _stageIsActive = false;

	List<WaveData> _remainingWaves;
	AudioPlayer _currentAudio;
	Transform _playerTransform;

	Coroutine _spawnCoroutine;

	public Transform StageTransform => _stageTransform;
	public int GameScore => _gameScore;

    //------------------------------------------------------------------------------

    private void Awake()
    {
		InitializeSingleton();
	}

    private void Start()
    {
		EventManager.Instance.AddListener<GameFinishedEvent>(OnGameFinished);
    }

    #region loop
    public void StartStageLoop()
	{
		StageUI.Instance.Initialize();

		_stageIsActive = true;
		StartCoroutine(StageCoroutine());
	}

	/// <summary>
	/// stage loop
	/// </summary>
	private IEnumerator StageCoroutine()
	{
		Debug.Log("Start StageCoroutine");

		SetupStage();

		while (_stageIsActive)
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				//exit stage
				CleanupStage();
				StateMachine.Instance.SetNewState(State.MAIN_MENU);
				_stageIsActive = false;

				StopAllCoroutines();

				_spawnCoroutine = null;

				yield break;
			}
			yield return null;
		}
	}
	#endregion


	void SetupStage()
	{
		_gameScore = 0;
		_currentWave = 0;

		_currentAudio = SoundManager.Instance.PlaySound(SoundType.LevelMusic);

		//create player
		{
			Player player = Instantiate(_prefabPlayer, _stageTransform);
			if (player)
			{
				_playerTransform = player.transform;
				_playerTransform.position = new Vector3(0, -8, 0);
			}
		}

		StartCoroutine(PreStart());
	}

	IEnumerator PreStart()
    {
		_playerTransform.DOMove(new Vector3(0.0f, -4.0f, 0.0f), 2.0f).SetEase(Ease.OutSine);

		yield return new WaitForSeconds(2.0f);

		StageUI.Instance.PlayReadyGo();

		yield return new WaitForSeconds(2.0f);

		_playerTransform.GetComponent<Player>()?.StartRunning();

		_spawnCoroutine = StartCoroutine(SpawnWave());
	}

	public void CleanupStage()
	{
		//Delete all object in Stage
		{
			LeanPool.DespawnAll();

			for (var n = 0; n < _stageTransform.childCount; ++n)
			{
				Transform temp = _stageTransform.GetChild(n);
				Destroy(temp.gameObject);
			}
		}
	}

	IEnumerator SpawnWave()
	{
		_remainingWaves = new List<WaveData>(_waveDatas);

		while (_currentWave < _amountWavesBeforeBoss)
		{
			WaveData waveData = (_isTestingWave? _testedWave: _remainingWaves[Random.Range(0, _remainingWaves.Count)]);

			List<SubWaveData> subWaves = waveData.SubWaves;

			for (int i = 0; i < subWaves.Count; i++)
			{
				subWaves[i].Spawn();

				yield return new WaitForSeconds(waveData.SpawnDelays[i]);
			}

			_currentWave++;

			if (!_isTestingWave)
			{
				_remainingWaves.Remove(waveData);
			}
		}

		// Here would be the launch of the boss fight
		// Instead, we're celebrating early
		EventManager.Instance.QueueEvent(new GameFinishedEvent(true));

		_spawnCoroutine = null;
	}

	//------------------------------------------------------------------------------

	public void AddScore(int value)
	{
		_gameScore += value;
		StageUI.Instance.UpdateScore(_gameScore);
	}

	public void StopGameAudio()
    {
		if(_currentAudio != null)
        {
			_currentAudio.Stop();
		}
    }

	void OnGameFinished(GameFinishedEvent e)
	{
		_stageIsActive = false;

		if (_spawnCoroutine != null)
			StopCoroutine(_spawnCoroutine);

		if (e.playerWon)
        {
			StartVictoryPhase();
		}
		else
        {
			StartGameOverPhase();
        }
    }

	void StartVictoryPhase()
    {
		StopGameAudio();

		StateMachine.Instance.SetNewState(State.VICTORY);
	}

    void StartGameOverPhase()
	{
		StopGameAudio();

		StateMachine.Instance.SetNewState(State.GAME_OVER);
	}

	public Transform GetPlayerTransform()
	{
		return _playerTransform;
	}
}
