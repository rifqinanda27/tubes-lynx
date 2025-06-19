using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public Slider healthSlider;
    private HeartManager heartManager;

    public bool inCutscene = false;

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

    //=== Effect Potion SpeedBoost===
    private bool isSpeedBoostActive = false;
    public bool IsSpeedBoostActive => isSpeedBoostActive;
    private float speedBoostRemainingDuration = 0f;

    //=== Effect Potion JumpBoost ===
    private bool isJumpBoostActive = false;
    private float jumpBoostRemainingDuration = 0f;

    //=== Pesan Noifikasi Icon UI===
    public TMPro.TextMeshProUGUI potionMessageText;
    public GameObject iconSpeedBoost;
    public GameObject iconJumpBoost;  // Kalau mau tambahkan UI icon-nya


    // ==== Sound Effects ====
    public AudioClip attackSFX;
    public AudioClip jumpSFX;
    public AudioClip stepSFX;
    public AudioClip hitSFX;
    public AudioClip dieSFX;
    public AudioClip errorSFX;

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
        if (GameManager.isDialogActive) return;
        if (isDead) return;

        if (inCutscene)
        {
            animator.SetFloat("Speed", 0f);
            return;
        }


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
        GameObject musicObj = GameObject.FindGameObjectWithTag("BossBGM");
        if (musicObj != null)
        {
            AudioSource music = musicObj.GetComponent<AudioSource>();
            if (music != null)
            {
                music.Stop();
            }
        }
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

    public void Heal(int amount)
    {
        if (isDead) return;

        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        if (healthSlider != null)
            healthSlider.value = currentHealth;

        Debug.Log("Player memulihkan HP: " + currentHealth);
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth += amount;  // Optional: langsung nambah current HP juga

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        Debug.Log($"Max Health bertambah {amount}. Max sekarang: {maxHealth}, Current: {currentHealth}");
    }

    public void ApplySpeedBoost(float multiplier, float duration)
    {
        if (isSpeedBoostActive)
        {
            // Tambahkan durasi ke sisa waktu boost yang sedang aktif
            speedBoostRemainingDuration += duration;
            Debug.Log($"Speed boost diperpanjang! Sisa durasi: {speedBoostRemainingDuration} detik");
        }
        else
        {
            // Kalau belum aktif, mulai boost baru
            StartCoroutine(SpeedBoostRoutine(multiplier, duration));
        }
    }


    private System.Collections.IEnumerator SpeedBoostRoutine(float multiplier, float duration)
    {
        isSpeedBoostActive = true;
        float originalSpeed = runSpeed;
        runSpeed *= multiplier;

        speedBoostRemainingDuration = duration;
        Debug.Log($"Speed boosted! runSpeed jadi {runSpeed}");

        // Tampilkan icon speed boost
        if (iconSpeedBoost != null)
            iconSpeedBoost.SetActive(true);

        while (speedBoostRemainingDuration > 0f)
        {
            speedBoostRemainingDuration -= Time.deltaTime;
            yield return null;
        }

        // Saat durasi habis
        runSpeed = originalSpeed;
        isSpeedBoostActive = false;
        Debug.Log("Speed boost selesai. runSpeed kembali ke normal");

        // Sembunyikan icon speed boost
        if (iconSpeedBoost != null)
            iconSpeedBoost.SetActive(false);
    }

    public void ApplyJumpBoost(float bonusAmount, float duration)
    {
        if (isJumpBoostActive)
        {
            // Tambahkan durasi jika masih aktif
            jumpBoostRemainingDuration += duration;
            Debug.Log($"Jump Boost diperpanjang! Sisa durasi: {jumpBoostRemainingDuration} detik");
        }
        else
        {
            StartCoroutine(JumpBoostRoutine(bonusAmount, duration));
        }
    }

    private System.Collections.IEnumerator JumpBoostRoutine(float bonusAmount, float duration)
    {
        isJumpBoostActive = true;
        float originalJumpForce = jumpForce;
        jumpForce += bonusAmount;

        jumpBoostRemainingDuration = duration;
        Debug.Log($"Jump Boost aktif! jumpForce jadi {jumpForce}");

        // Tampilkan icon boost kalau ada
        if (iconJumpBoost != null)
            iconJumpBoost.SetActive(true);

        while (jumpBoostRemainingDuration > 0f)
        {
            jumpBoostRemainingDuration -= Time.deltaTime;
            yield return null;
        }

        jumpForce -= bonusAmount;
        isJumpBoostActive = false;
        Debug.Log("Jump Boost selesai. jumpForce kembali ke normal");

        // Sembunyikan icon boost kalau ada
        if (iconJumpBoost != null)
            iconJumpBoost.SetActive(false);
    }

    public void PlayErrorSound()
    {
        if (audioSource != null && errorSFX != null)
        {
            audioSource.PlayOneShot(errorSFX);
        }
    }

    public void ShowPotionMessage(string message, float duration)
    {
        StartCoroutine(ShowPotionMessageRoutine(message, duration));
    }

    private System.Collections.IEnumerator ShowPotionMessageRoutine(string message, float duration)
    {
        if (potionMessageText != null)
        {
            potionMessageText.text = message;
            potionMessageText.gameObject.SetActive(true);
            yield return new WaitForSeconds(duration);
            potionMessageText.text = "";
            potionMessageText.gameObject.SetActive(false);
        }
    }
}
