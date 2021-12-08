using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsUpdater : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI highScoreText;
    [SerializeField]
    private TextMeshProUGUI highestLevelText;
    [SerializeField]
    private TextMeshProUGUI distanceTravelledText;
    [SerializeField]
    private TextMeshProUGUI distanceSlidText;
    [SerializeField]
    private TextMeshProUGUI meteorsDestroyedText;
    [SerializeField]
    private TextMeshProUGUI deathsText;

    private void OnEnable()
    {
        EventManager.Instance.onHighScoreUpdated += UpdateHighScore;
        EventManager.Instance.onHighestLevelUpdated += UpdateHighestLevel;
        EventManager.Instance.onDistanceTravelledUpdated += UpdateDistanceTravelled;
        EventManager.Instance.onDistanceSlidUpdated += UpdateDistanceSlid;
        EventManager.Instance.onMeteorsDestroyedUpdated += UpdateMeteorsDestroyed;
        EventManager.Instance.onPlayerDeath += UpdateDeaths;
    }

    private void OnDisable()
    {
        EventManager.Instance.onHighScoreUpdated -= UpdateHighScore;
        EventManager.Instance.onHighestLevelUpdated -= UpdateHighestLevel;
        EventManager.Instance.onDistanceTravelledUpdated -= UpdateDistanceTravelled;
        EventManager.Instance.onDistanceSlidUpdated -= UpdateDistanceSlid;
        EventManager.Instance.onMeteorsDestroyedUpdated -= UpdateMeteorsDestroyed;
        EventManager.Instance.onPlayerDeath -= UpdateDeaths;

    }

    private void Start()
    {
        UpdateHighScore();
        UpdateHighestLevel();
        UpdateDistanceTravelled();
        UpdateDistanceSlid();
        UpdateMeteorsDestroyed();
        UpdateDeaths();
    }

    void UpdateHighScore()
    {
        highScoreText.SetText("High Score: " + PlayerPrefs.GetInt("HighScore"));
    }

    void UpdateHighestLevel()
    {
        highestLevelText.SetText("Highest Level: " + PlayerPrefs.GetInt("HighestLevel"));
    }

    void UpdateDistanceTravelled()
    {
        distanceTravelledText.SetText("Distance Travelled: " + PlayerPrefs.GetFloat("DistanceTravelled") + "m");
    }

    void UpdateDistanceSlid()
    {
        distanceSlidText.SetText("Distance Slid: " + PlayerPrefs.GetFloat("DistanceSlid") + "m");
    }

    void UpdateMeteorsDestroyed()
    {
        meteorsDestroyedText.SetText("Meteors Destroyed: " + PlayerPrefs.GetInt("MeteorsDestroyed"));
    }

    void UpdateDeaths()
    {
        deathsText.SetText("Deaths: " + PlayerPrefs.GetInt("Deaths"));
    }

   
}
