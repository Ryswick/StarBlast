using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Game Over Screen Loop
/// </summary>
public class GameOverLoop : MonoBehaviour
{
	[Header("References")]
	[SerializeField] TextMeshProUGUI _scoreText;
	[SerializeField] GameObject _newHighscoreObject;

	AudioPlayer _gameOverMusic;
	//------------------------------------------------------------------------------

	//
	#region loop
	public void StartGameOverLoop()
	{
		int score = StageLoop.Instance.GameScore;

		_gameOverMusic = SoundManager.Instance.PlaySound(SoundType.GameOverMusic);
		_scoreText.text = $"Score {score:00000}";

		// Check in player data if the new score is an highscore, and if so show the highscore object
		_newHighscoreObject.SetActive(PlayerData.Instance.UpdateHighScore(score));

		StartCoroutine(GameOverCoroutine());
	}

	/// <summary>
	/// Title loop
	/// </summary>
	private IEnumerator GameOverCoroutine()
	{
		Debug.Log($"Start GameOverCoroutine");

		// Waiting for game to restart
		while (true)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				StopMenuAudio();
				StageLoop.Instance.CleanupStage();
				StateMachine.Instance.SetNewState(State.GAME_PHASE);
				yield break;
			}
			yield return null;
		}
	}

	public void StopMenuAudio()
    {
		_gameOverMusic.Stop();
	}
	#endregion
}
