using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreUpdater : MonoBehaviour
{
    TextMeshProUGUI scoreText;

    private void Awake()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        EventManager.Instance.onHighScoreUpdated += UpdateText;

        scoreText.SetText(PlayerPrefs.GetInt("HighScore").ToString());
    }

    void UpdateText()
    {
        scoreText.SetText(PlayerPrefs.GetInt("HighScore").ToString());
    }
 
}
