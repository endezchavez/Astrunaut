using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private TextMeshProUGUI highScoreText;
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private TextMeshProUGUI meteorsDestroyedText;

    private void OnEnable()
    {
        EventManager.Instance.onPlayerDeath += ShowUI;
        EventManager.Instance.onPlayerDeath += UpdateText;
        EventManager.Instance.onPlayerRespawn += HideUI;
    }

    private void OnDisable()
    {
        EventManager.Instance.onPlayerDeath -= ShowUI;
        EventManager.Instance.onPlayerDeath -= UpdateText;
        EventManager.Instance.onPlayerRespawn -= HideUI;
    }


    void ShowUI()
    {
        panel.SetActive(true);
    }

    void HideUI()
    {
        panel.SetActive(false);
    }


    void UpdateText()
    {
        if (GameManager.Instance.score > PlayerPrefs.GetInt("HighScore"))
        {
            highScoreText.SetText("New High Score: " + GameManager.Instance.score);
            highScoreText.transform.GetComponent<ScalePingPongTween>().Tween();
        }
        else
        {
            highScoreText.SetText("High Score: " + PlayerPrefs.GetInt("HighScore"));
        }
        scoreText.SetText("Score: " + GameManager.Instance.score);
        levelText.SetText("Level: " + GameManager.Instance.currentLevel);
        meteorsDestroyedText.SetText("Meteors Destroyed: " + GameManager.Instance.meteorsDestroyed);
    }
}
