using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomScale : MonoBehaviour
{
    [SerializeField]
    private float minScale;
    [SerializeField]
    private float maxScale;

    private void Start()
    {
        float rand = Random.Range(minScale, maxScale);
        transform.localScale = Vector3.one * rand;
    }
}
