using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField]
    private float tickLength;

    TextMeshProUGUI text;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        EventManager.Instance.onGameStarted += TrackScore;
    }

    void TrackScore()
    {
        StartCoroutine(UpdateScore());
    }

    IEnumerator UpdateScore()
    {
        while (GameManager.Instance.isPlayerAlive)
        {
            yield return new WaitForSeconds(tickLength);
            if (!GameManager.Instance.isPlayerAlive)
            {
                yield break;
            }
            GameManager.Instance.AddToScore(1);
            text.SetText(GameManager.Instance.score.ToString());

            if(GameManager.Instance.score % 100 == 0)
            {
                ScaleUpAndDown();
            }
        }
    }

    void ScaleUpAndDown()
    {
        LeanTween.scale(gameObject, Vector3.one * 1.2f, 0.5f).setOnComplete(ScaleDown);
    }

    void ScaleDown()
    {
        LeanTween.scale(gameObject, Vector3.one, 0.5f);
    }
}
