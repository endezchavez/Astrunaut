using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapTween : MonoBehaviour
{
    [SerializeField]
    private float animTime;

    [SerializeField]
    private float scale;

    [SerializeField]
    private float delay;

    [SerializeField]
    private Sprite tapSpr;

    private SpriteRenderer sprRenderer;
    private Sprite originalSpr;

    private void Awake()
    {
        sprRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.onFirstShootEncountered += PlayAnimation;
        sprRenderer.enabled = false;
        originalSpr = sprRenderer.sprite;
        //PlayAnimation();
    }


    void PlayAnimation()
    {
        sprRenderer.enabled = true;
        LeanTween.scale(gameObject, new Vector3(scale, scale, scale), animTime).setEaseInOutElastic().setDelay(delay).setLoopCount(2).setOnComplete(DisableRenderer);
        StartCoroutine(SwitchSprite());
    }

    private void DisableRenderer()
    {
        sprRenderer.enabled = false;
    }

    IEnumerator SwitchSprite()
    {
        yield return new WaitForSeconds(0.8f);
        sprRenderer.sprite = tapSpr;

        yield return new WaitForSeconds(0.7f);
        sprRenderer.sprite = originalSpr;

        yield return new WaitForSeconds(0.8f);
        sprRenderer.sprite = tapSpr;

        yield return new WaitForSeconds(0.7f);
        sprRenderer.sprite = originalSpr;

    }
}
