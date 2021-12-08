using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelCounter : MonoBehaviour
{
    TextMeshProUGUI text;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        EventManager.Instance.onLevelIncremented += IncrementLevel;
    }

    void IncrementLevel()
    {
        text.SetText("Level: " + GameManager.Instance.currentLevel);
        ScaleUpAndDown();
    }

    void ScaleUpAndDown()
    {
        LeanTween.scale(gameObject, Vector3.one * 1.2f, 0.5f).setOnComplete(ScaleDown);
    }

    void ScaleDown()
    {
        LeanTween.scale(gameObject, Vector3.one, 0.5f);
    }
}
