using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTween : MonoBehaviour
{
    [SerializeField]
    private float animTime;

    [SerializeField]
    private float delay;

    [SerializeField]
    private LeanTweenType tweenType;

    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        LeanTween.scale(gameObject, Vector3.one, animTime).setDelay(delay).setEase(tweenType);
    }


}
