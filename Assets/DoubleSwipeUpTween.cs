using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoubleSwipeUpTween : MonoBehaviour
{

    [SerializeField]
    private float swipeDistance;

    [SerializeField]
    private float animTime;

    [SerializeField]
    private float initialDelay;

    [SerializeField]
    private float delay;

    [SerializeField]
    private float finalDelay;

    private SpriteRenderer sprRenderer;

    int numAnimationsPlayed = 0;

    Vector3 originalPos;

    private void Awake()
    {
        sprRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        EventManager.Instance.onFirstDoubleJumpEncountered += PlayAnimation;
        sprRenderer.enabled = false;
        originalPos = transform.position;
        //PlayAnimation();
    }

    void PlayAnimation()
    {
        sprRenderer.enabled = true;

        Swipe();
        
    }

    void Swipe()
    {
     
        LeanTween.moveY(gameObject, transform.position.y + swipeDistance, animTime).setEaseOutBack().setDelay(initialDelay).setOnComplete(
        () => { LeanTween.moveY(gameObject, transform.position.y + swipeDistance, animTime).setEaseOutBack().setDelay(delay).setOnComplete(SwipeAgain); });

        
    }

    void SwipeAgain()
    {
        StartCoroutine(WaitBeforeNextAnim());
    }

    IEnumerator WaitBeforeNextAnim()
    {
        yield return new WaitForSeconds(finalDelay);

        numAnimationsPlayed++;

        if (numAnimationsPlayed < 2)
        {
            gameObject.transform.position = originalPos;
            Swipe();
        }
        else
        {
            sprRenderer.enabled = false;
            numAnimationsPlayed = 0;
        }
    }

}
