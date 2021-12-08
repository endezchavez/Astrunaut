using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePingPongTween : MonoBehaviour
{
    [SerializeField]
    private float maxScale = 1.2f;

    [SerializeField]
    private float animTime = 1f;

    [SerializeField]
    private bool playOnAwake = true;

    [SerializeField]
    private float delay = 0f;

    private RectTransform rectTransform;



    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        if (playOnAwake)
        {
            Tween();
        }
    }

    void ScaleDown()
    {
        //LeanTween.scale(gameObject, new Vector3(1, 1, 1), 1f);
    }

    public void Tween()
    {
        rectTransform.localScale = Vector3.one;
        LeanTween.scale(rectTransform, new Vector3(maxScale, maxScale, maxScale), animTime).setDelay(delay).setLoopPingPong();
    }
}
