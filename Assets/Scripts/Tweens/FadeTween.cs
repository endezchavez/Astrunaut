using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FadeTween : MonoBehaviour
{
    [SerializeField]
    private float delay;

    CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        EventManager.Instance.onPlayButtonPressed += FadeText;
    }

    void FadeText()
    {
        LeanTween.alphaCanvas(canvasGroup, 0f, 1f).setDelay(delay);
    }

}
