using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Game Over Screen Loop
/// </summary>
public class VictoryLoop : MonoBehaviour
{
	[Header("References")]
	[SerializeField] TextMeshProUGUI _scoreText;
	[SerializeField] GameObject _newHighscoreObject;
	//------------------------------------------------------------------------------
	AudioPlayer _victoryMusic;

	//
	#region loop
	public void StartVictoryLoop()
	{
		int score = StageLoop.Instance.GameScore;

		_victoryMusic = SoundManager.Instance.PlaySound(SoundType.VictoryMusic);
		_scoreText.text = $"Score {StageLoop.Instance.GameScore:00000}";

		// Check in player data if the new score is an highscore, and if so show the highscore object
		_newHighscoreObject.SetActive(PlayerData.Instance.UpdateHighScore(score));

		StartCoroutine(VictoryCoroutine());
	}

	/// <summary>
	/// Title loop
	/// </summary>
	private IEnumerator VictoryCoroutine()
	{
		Debug.Log($"Start VictoryCoroutine");

		// Waiting for game to restart
		while (true)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				StopAudio();
				StageLoop.Instance.CleanupStage();
				StateMachine.Instance.SetNewState(State.GAME_PHASE);
				yield break;
			}
			yield return null;
		}
	}

	public void StopAudio()
    {
		_victoryMusic.Stop();
	}
	#endregion
}
