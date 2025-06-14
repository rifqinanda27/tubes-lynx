using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public Slider healthSlider;
    private HeartManager heartManager;

    public float runSpeed = 10f;
    public float jumpForce = 15f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded = false;

    private int comboStep = 0;
    private bool canAcceptInput = true;

    private float comboTimer = 0f;
    public float comboResetTime = 0.5f;

    // ==== Health System ====
    public int maxHealth = 5;
    private int currentHealth;
    private bool isDead = false;

    // ==== Jumping System ====
    public int maxJumps = 2;
    private int jumpCount = 0;

    // ==== Sound Effects ====
    public AudioClip attackSFX;
    public AudioClip jumpSFX;
    public AudioClip stepSFX;
    public AudioClip hitSFX;
    public AudioClip dieSFX;

    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        heartManager = FindFirstObjectByType<HeartManager>();
        audioSource = GetComponent<AudioSource>();

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    void Update()
    {
        if (isDead) return;

        if (comboStep == 0)
        {
            float moveInput = Input.GetAxisRaw("Horizontal");
            rb.linearVelocity = new Vector2(moveInput * runSpeed, rb.linearVelocity.y);

            if (moveInput > 0) spriteRenderer.flipX = false;
            else if (moveInput < 0) spriteRenderer.flipX = true;

            animator.SetFloat("Speed", Mathf.Abs(moveInput));
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            animator.SetFloat("Speed", 0);
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps && comboStep == 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetBool("IsJumping", true);
            isGrounded = false;
            jumpCount++;

            if (audioSource != null && jumpSFX != null)
                audioSource.PlayOneShot(jumpSFX);
        }

        if (Input.GetKeyDown(KeyCode.J) && canAcceptInput && isGrounded)
        {
            comboStep++;
            if (comboStep > 4) comboStep = 1;

            animator.SetInteger("ComboStep", comboStep);
            canAcceptInput = false;
            comboTimer = 0f;

            if (audioSource != null && attackSFX != null)
                audioSource.PlayOneShot(attackSFX);
        }

        if (comboStep > 0)
        {
            comboTimer += Time.deltaTime;
            if (comboTimer > comboResetTime && !canAcceptInput)
            {
                ResetAttack();
            }
        }

        if (isGrounded == false && Mathf.Abs(rb.linearVelocity.y) < 0.1f)
        {
            rb.linearVelocity += new Vector2(spriteRenderer.flipX ? 0.1f : -0.1f, 0f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
            animator.SetBool("IsJumping", false);
            jumpCount = 0; // reset lompatan saat mendarat
        }
    }

    public void EnableNextInput()
    {
        if (isGrounded)
            canAcceptInput = true;
    }

    public void ResetAttack()
    {
        comboStep = 0;
        animator.SetInteger("ComboStep", 0);
        canAcceptInput = true;
        comboTimer = 0f;
    }

    public void AttackEnemy()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, 2f, LayerMask.GetMask("Enemy"));
        if (hit != null)
        {
            Enemy1Movement enemy = hit.GetComponent<Enemy1Movement>();
            if (enemy != null)
            {
                enemy.TakeDamage();
            }
        }
    }

    public void TakeDamage()
    {
        if (isDead) return;

        currentHealth--;
        animator.SetTrigger("TakeHit");

        if (audioSource != null && hitSFX != null)
            audioSource.PlayOneShot(hitSFX);

        Debug.Log("Player terkena serangan! Sisa HP: " + currentHealth);

        // if (heartManager != null)
        // {
        //     heartManager.LoseHeart(currentHealth);
        // }

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        animator.SetTrigger("Die");

        if (audioSource != null && dieSFX != null)
            audioSource.PlayOneShot(dieSFX);

        Debug.Log("Player mati!");
        Destroy(gameObject, 1.2f);
    }

    public void PlayStepSound()
    {
        if (isGrounded && audioSource != null && stepSFX != null)
        {
            audioSource.PlayOneShot(stepSFX);
        }
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            // Jika nabrak dari samping
            if (Mathf.Abs(contact.normal.x) > 0.5f)
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // hentikan horizontal
            }
        }
    }

}
