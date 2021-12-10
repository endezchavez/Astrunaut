using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CloudOnce;


public class CloudOnceServices : MonoBehaviour
{
    private static CloudOnceServices _instance;

    public static CloudOnceServices Instance { get { return _instance; } }

    [HideInInspector]
    public bool isLeaderboardOpened = false;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SubmitScoreToLeaderboard(int score)
    {
        Leaderboards.HighScore.SubmitScore(score);
    }

}
