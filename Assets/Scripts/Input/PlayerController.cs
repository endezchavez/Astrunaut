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
        EventManager.Instance.onPlayButtonPressed += InitialJump;

        InputManager.Instance.swipeDetection.OnSwipeUp += Jump;
        InputManager.Instance.swipeDetection.OnSwipeDown += Slide;
    }

    private void OnDisable()
    {
        EventManager.Instance.onPlayButtonPressed -= Jump;

        InputManager.Instance.swipeDetection.OnSwipeUp -= Jump;
        InputManager.Instance.swipeDetection.OnSwipeDown -= Slide;
    }

    private void Update()
    {
 
        if (GameManager.Instance.isGamePlaying && GameManager.Instance.isPlayerAlive)
        {
            //Add more gravity if player is falling
            if (rb.velocity.y < 0)
            {
                rb.gravityScale = fallingGravityScale;
            }
            else
            {
                rb.gravityScale = originalGravityScale;
            }

            //If player is grounded and a frame has passed since player jumped allow for double jump
            if (IsGrounded() && framePassed)
            {
                animator.SetBool("IsJumping", false);
                hasDoubleJumped = false;
            }

            //If player shot spawn bulllet and start shoot cooldown
            if (InputManager.Instance.PlayerShotThisFrame() && canShoot && framePassed)
            {
                canShoot = false;
                animator.SetTrigger("Shot");
                AudioManager.Instance.Play("Shoot");
                StartCoroutine(SpawnBullet());
                StartCoroutine(ShootCooldown());
            }
        }
    }

    //Have the player do a jump when the game starts
    void InitialJump()
    {
        if (!GameManager.Instance.isGamePlaying)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetBool("IsJumping", true);
            AudioManager.Instance.Play("Jump");

            GameManager.Instance.isGamePlaying = true;
            EventManager.Instance.GameStarted();
            EventManager.Instance.LevelIncremented();
            StartCoroutine(WaitAFrame());
        }
    }

    //Adds an upwards force to the player when they jump or double jump
    void Jump()
    {
        if(GameManager.Instance.isGamePlaying && GameManager.Instance.isPlayerAlive)
        {
            if (IsGrounded())
            {
                framePassed = false;

                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                animator.SetBool("IsJumping", true);
                AudioManager.Instance.Play("Jump");
                StartCoroutine(WaitAFrame());
                if (!hasInitialJumped)
                {
                    hasInitialJumped = true;
                }
            }
            else if (!hasDoubleJumped)
            {
                rb.AddForce(Vector2.up * doubleJumpForce, ForceMode2D.Impulse);
                animator.SetBool("IsJumping", true);
                hasDoubleJumped = true;
                AudioManager.Instance.Play("Jump");
                framePassed = true;
            }
        }
    }

    //Switches the collider size of the player when they slide and adjusts the barrel position so they shoot lower
    void Slide()
    {
        if(GameManager.Instance.isGamePlaying && canSlide && IsGrounded())
        {
            standingCollider.enabled = false;
            slidingCollider.enabled = true;
            animator.SetBool("IsSliding", true);
            canSlide = false;
            barrelPos = slidingBarrelPos;
            AudioManager.Instance.Play("Slide");
            StartCoroutine(SlideTimer());
            StartCoroutine(SlideCooldown());

            GameManager.Instance.AddToSlideDistance(slideTime);
        }
    }

    //Waits for a frame to pass, used for animations as animators still think player is grounded when they have jumped
    IEnumerator WaitAFrame()
    {
        yield return 0;

        framePassed = true;
    }

    //If the player enters a trigger of an obstacle, play death animation and end the game
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

    //Resets the player collider and barrel position after the player stops sliding
    IEnumerator SlideTimer()
    {
        yield return new WaitForSeconds(slideTime);
        slidingCollider.enabled = false;
        standingCollider.enabled = true;
        animator.SetBool("IsSliding", false);
        barrelPos = standingBarrelPos;
    }

    //Spawns a bullet at the current barrel position
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
