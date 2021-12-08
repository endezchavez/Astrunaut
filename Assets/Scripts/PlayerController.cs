using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    LayerMask whatIsGround;

    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    private float doubleJumpForce = 5f;
    [SerializeField]
    private float slideTime = 0.25f;
    [SerializeField]
    private float fallingGravityScale = 1.2f;
    [SerializeField]
    private float shootCooldown = 1f;
    [SerializeField]
    private float slideCooldown = 1f;

    [SerializeField]
    private Transform feetPos;
    [SerializeField]
    private Transform standingBarrelPos;
    [SerializeField]
    private Transform slidingBarrelPos;
    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    BoxCollider2D standingCollider;
    [SerializeField]
    BoxCollider2D slidingCollider;

    Rigidbody2D rb;

    Animator animator;

    bool isSliding;
    bool hasDoubleJumped;
    bool canShoot = true;
    bool canSlide = true;
    Transform barrelPos;
    float originalGravityScale;

    bool hasInitialJumped = false;
    bool framePassed = false;

    Vector3 originalPlayerPos;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        barrelPos = standingBarrelPos;
        originalGravityScale = rb.gravityScale;

        animator.SetBool("IsIdle", true);

        originalPlayerPos = transform.position;

    }

    private void OnEnable()
    {
        EventManager.Instance.onPlayButtonPressed += Jump;
    }

    private void OnDisable()
    {
        EventManager.Instance.onPlayButtonPressed -= Jump;
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Slide") || animator.GetCurrentAnimatorStateInfo(0).IsName("Slide Shoot"))
        {
            //barrelPos = slidingBarrelPos;
        }
        else
        {
            //barrelPos = standingBarrelPos;
        }

        if (GameManager.Instance.isGamePlaying && GameManager.Instance.isPlayerAlive)
        {
            if (rb.velocity.y < 0)
            {
                rb.gravityScale = fallingGravityScale;
            }
            else
            {
                rb.gravityScale = originalGravityScale;
            }

            if (IsGrounded())
            {
                animator.SetBool("IsJumping", false);
                hasDoubleJumped = false;
            }
            if (InputManager.Instance.PlayerJumpedThisFrame())
            {
                Jump();
            }
            if(InputManager.Instance.PlayerSlidThisFrame() && canSlide && IsGrounded())
            {
                standingCollider.enabled = false;
                slidingCollider.enabled = true;
                animator.SetBool("IsSliding", true);
                isSliding = true;
                canSlide = false;
                barrelPos = slidingBarrelPos;
                AudioManager.Instance.Play("Slide");
                StartCoroutine(SlideTimer());
                StartCoroutine(SlideCooldown());

                GameManager.Instance.AddToSlideDistance(slideTime);
            }
            if (InputManager.Instance.PlayerShotThisFrame() && canShoot)
            {
                canShoot = false;
                animator.SetTrigger("Shot");
                AudioManager.Instance.Play("Shoot");
                StartCoroutine(SpawnBullet());
                StartCoroutine(ShootCooldown());
            }
        }else if(GameManager.Instance.isPlayerAlive && !GameManager.Instance.isGamePlaying && hasInitialJumped)
        {
            if (IsGrounded() && framePassed)
            {
                animator.SetBool("IsJumping", false);
                GameManager.Instance.isGamePlaying = true;
                EventManager.Instance.GameStarted();
                EventManager.Instance.LevelIncremented();

            }
        }
    }

    void Jump()
    {
        if (IsGrounded())
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetBool("IsJumping", true);
            AudioManager.Instance.Play("Jump");
            if (!hasInitialJumped)
            {
                StartCoroutine(WaitAFrame());
                hasInitialJumped = true;
            }
        }
        else if (!hasDoubleJumped)
        {
            rb.AddForce(Vector2.up * doubleJumpForce, ForceMode2D.Impulse);
            animator.SetBool("IsJumping", true);
            hasDoubleJumped = true;
            AudioManager.Instance.Play("Jump");
        }
    }

    IEnumerator WaitAFrame()
    {
        yield return 0;

        framePassed = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Obstacle") && GameManager.Instance.isPlayerAlive)
        {
            animator.SetTrigger("Died");
            AudioManager.Instance.Play("Hit");
            AudioManager.Instance.Play("Death");
            GameManager.Instance.isPlayerAlive = false;
            PlayerPrefs.SetInt("Deaths", PlayerPrefs.GetInt("Deaths") + 1);
            EventManager.Instance.PlayerDied();
            AudioManager.Instance.StopTheme();
            AudioManager.Instance.Play("GameOver");
    
        }
    }

    IEnumerator SlideTimer()
    {
        yield return new WaitForSeconds(slideTime);
        slidingCollider.enabled = false;
        standingCollider.enabled = true;
        isSliding = false;
        animator.SetBool("IsSliding", false);
        barrelPos = standingBarrelPos;
    }

    IEnumerator SpawnBullet()
    {
        yield return new WaitForSeconds(0.05f);
        Transform bullet = Instantiate(bulletPrefab).transform;
        bullet.position = barrelPos.position;
    }

    IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }

    IEnumerator SlideCooldown()
    {
        yield return new WaitForSeconds(slideCooldown);
        canSlide = true;
    }

    bool IsGrounded()
    {
        return Physics2D.Raycast(feetPos.position, Vector2.down, 0.1f, whatIsGround);
    }

    public void Respawn()
    {
        animator.SetTrigger("Respawn");
        transform.position = originalPlayerPos;
    }

    public void ResetInitialJump()
    {
        hasInitialJumped = false;
    }

}
