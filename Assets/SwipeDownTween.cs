using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwipeDownTween : MonoBehaviour
{

    [SerializeField]
    private float yPos;

    [SerializeField]
    private float animTime;

    [SerializeField]
    private float delay;

    private SpriteRenderer sprRenderer;

    private void Awake()
    {
        sprRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        EventManager.Instance.onFirstSlideEncountered += PlayAnimation;
        sprRenderer.enabled = false;
    }

    void PlayAnimation()
    {
        sprRenderer.enabled = true;
        LeanTween.moveY(gameObject, yPos, animTime).setEaseOutBack().setDelay(delay).setLoopCount(2).setOnComplete(DisableSpriteRenderer);
    }

    void DisableSpriteRenderer()
    {
        sprRenderer.enabled = false;

    }

}
