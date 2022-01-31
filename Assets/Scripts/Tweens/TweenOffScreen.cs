using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenOffScreen : MonoBehaviour
{
    [SerializeField]
    private float delay;

    [SerializeField]
    private float animTime;

    [SerializeField]
    float yPos;

    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        EventManager.Instance.onPlayButtonPressed += TweenUp;
    }

    public void TweenUp()
    {
        //rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, 100f);
        LeanTween.moveY(rectTransform, yPos, animTime).setEaseInBack().setDelay(delay);
    }

}
