using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FadeAndScaleTween : MonoBehaviour
{
    CanvasGroup canvasGroup;
    RectTransform rectTransform;

    [SerializeField]
    private float delay;
    [SerializeField]
    private float animTime;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        canvasGroup.alpha = 0f;
        LeanTween.scale(rectTransform, Vector3.one, animTime).setEaseOutBack().setDelay(delay);
        LeanTween.alphaCanvas(canvasGroup, 1f, animTime).setDelay(delay);

    }

    private void OnDisable()
    {
        LeanTween.cancel(gameObject);
    }


}
