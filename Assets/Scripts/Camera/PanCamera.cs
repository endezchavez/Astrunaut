using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanCamera : MonoBehaviour
{

    [SerializeField]
    private Transform endPos;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float panTime;

    [SerializeField]
    private float panDelay;

    Vector3 dirToTarget;

    bool isPanning = false;

    // Start is called before the first frame update
    void Start()
    {
        /*EventManager.Instance.onPlayerDeath += Pan;*/

        dirToTarget = (endPos.position - transform.position).normalized;

        Pan();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (isPanning)
        {
            if (Vector2.Distance(transform.position, endPos.position) >= 0.1f)
            {
                transform.position = transform.position + (dirToTarget * Time.deltaTime * speed);
            }
            else
            {
                transform.position = endPos.position;
            }
        }
        
    }

    void Pan()
    {
        StartCoroutine(PanTimer());
    }



    IEnumerator PanTimer()
    {
        yield return new WaitForSeconds(panDelay);

        isPanning = true;

    }
}
