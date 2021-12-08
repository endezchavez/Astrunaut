using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenOntoScreen : MonoBehaviour
{
    [SerializeField]
    private float delay;

    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        EventManager.Instance.onPlayButtonPressed += TweenDown;
    }

    public void TweenDown()
    {
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, 100f);
        LeanTween.moveY(rectTransform, -32, 1.5f).setEaseOutBack().setDelay(delay);
    }


}
