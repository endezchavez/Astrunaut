using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOverTime : MonoBehaviour
{
    [SerializeField]
    private float minRotSpeed = 50f;
    [SerializeField]
    private float maxRotSpeed = 10f;

    float rotSpeed;

    private void Start()
    {
        rotSpeed = Random.Range(minRotSpeed, maxRotSpeed);
        if(Random.Range(0, 100) < 50)
        {
            rotSpeed *= -1;
        }
        Vector3 euler = transform.eulerAngles;
        euler.z = Random.Range(0f, 360f);
        transform.eulerAngles = euler;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * rotSpeed);
    }
}
