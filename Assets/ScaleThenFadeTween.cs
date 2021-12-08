using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScaleThenFadeTween : MonoBehaviour
{
    CanvasGroup canvasGroup;
    TextMeshProUGUI text;
    RectTransform rectTransform;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        text = GetComponent<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        EventManager.Instance.onLevelIncremented += IncrementLevel;
    }

    void IncrementLevel()
    {
        AudioManager.Instance.Play("LevelUp");
        transform.localScale = Vector3.zero;
        canvasGroup.alpha = 1f;
        text.SetText("LEVEL " + GameManager.Instance.currentLevel);
        LeanTween.scale(rectTransform, new Vector3(1.2f, 1.2f, 1.2f), 1f).setOnComplete(ScaleDown);
    }

    void ScaleDown()
    {
        LeanTween.alphaCanvas(canvasGroup, 0f, 1f).setDelay(0.5f);
    }
}
