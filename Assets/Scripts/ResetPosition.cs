using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    private Vector3 originalPos;

    private void OnDisable()
    {
        EventManager.Instance.onPlayerRespawn -= Reset;
    }

    private void Start()
    {
        EventManager.Instance.onPlayerRespawn += Reset;

        originalPos = transform.position;
    }

    void Reset()
    {
        transform.position = originalPos;
    }
}
