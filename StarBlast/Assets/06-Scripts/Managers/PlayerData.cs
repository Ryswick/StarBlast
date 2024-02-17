using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : Singleton<PlayerData>
{
    int _highscore;

    public int Highscore => _highscore;

    // Start is called before the first frame update
    void Awake()
    {
        InitializeSingleton();
        LoadData();
    }

    // To simplify for now, we'll do it through PlayerPrefs
    void LoadData()
    {
        _highscore = PlayerPrefs.GetInt("Highscore", 0);
    }

    // Return true if the previous highscore has been beaten
    public bool UpdateHighScore(int lastScore)
    {
        if(_highscore < lastScore)
        {
            _highscore = lastScore;

            PlayerPrefs.SetInt("Highscore", lastScore);
            PlayerPrefs.Save();

            return true;
        }

        return false;
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}
