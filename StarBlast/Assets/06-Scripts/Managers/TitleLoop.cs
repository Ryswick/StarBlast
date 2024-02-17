using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Title Screen Loop
/// </summary>
public class TitleLoop : MonoBehaviour
{
	[Header("References")]
	[SerializeField] TextMeshProUGUI _scoreText;

	//------------------------------------------------------------------------------
	AudioPlayer _menuMusic;

	private void Start()
	{
		StateMachine.Instance.SetNewState(State.MAIN_MENU);
	}

	//
	#region loop
	public void StartTitleLoop()
	{
		_menuMusic = SoundManager.Instance.PlaySound(SoundType.MenuMusic);

		_scoreText.text = $"Highscore {PlayerData.Instance.Highscore:00000}";

		StartCoroutine(TitleCoroutine());
	}

	/// <summary>
	/// Title loop
	/// </summary>
	private IEnumerator TitleCoroutine()
	{
		Debug.Log($"Start TitleCoroutine");

		// Waiting for game t
		while (true)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				StopMenuAudio();
				StateMachine.Instance.SetNewState(State.GAME_PHASE);
				yield break;
			}
			yield return null;
		}
	}
	#endregion

	public void StopMenuAudio()
	{
		_menuMusic.Stop();
	}
}
