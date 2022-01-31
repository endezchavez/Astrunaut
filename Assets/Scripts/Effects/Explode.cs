using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    Animator animator;
    new Collider2D collider;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Projectile"))
        {
            animator.SetTrigger("Explode");
            collider.enabled = false;
            
            AudioManager.Instance.PlayWithRandomPitch("Explode");
            Destroy(collision.gameObject);
            GameManager.Instance.AddToMeteorsDestroyed();
        }
    }

    private void OnDisable()
    {
        animator.SetTrigger("Reset");
        collider.enabled = true;
    }
}
