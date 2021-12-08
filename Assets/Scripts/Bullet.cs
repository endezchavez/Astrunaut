using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + -Vector2.left * Time.deltaTime * speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "BulletDestroy" || collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            //AudioManager.Instance.PlayWithRandomPitch("Hit");
            Destroy(this.gameObject);
        }
    }
}
