using UnityEngine;
using UnityEngine.UI; 
public class Enemy1Movement : MonoBehaviour
{
    protected Animator baseAnimator => animator;
    public SpriteRenderer spriteRendererPublic => spriteRenderer;

    public float attackRange = 0.5f;
    public float attackCooldown = 1f;
    protected float lastAttackTime = 0f;

    public Transform target;
    public float moveSpeed = 2f;
    public float stoppingDistance = 0.5f;

    private Rigidbody2D rb;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;

    public int maxHealth = 2;
    protected int currentHealth;
    protected bool isDead = false;
    protected Vector2 movement;

    public bool canChasePlayer = false;

    // 🔊 Sound Effects
    // [HideInInspector] public AudioClip attackSFX;
    // [HideInInspector] public AudioClip takeHitSFX;
    // [HideInInspector] public AudioClip dieSFX;
    // [HideInInspector] public AudioClip stepSFX;

    public AudioClip attackSFX;
    public AudioClip takeHitSFX;
    public AudioClip dieSFX;
    public AudioClip stepSFX;

    protected AudioSource audioSource;

    [Header("Chase Settings")]
    [SerializeField] protected float chaseRadius = 5f; // misal 5 unit

    [Header("Health UI")]
    public GameObject healthBarUIPrefab;

    private Slider healthSlider;
    private GameObject healthBarUIInstance;



    protected virtual void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        // Setelah audioSource = GetComponent<AudioSource>();
        if (healthBarUIPrefab != null)
        {
            Canvas canvas = FindObjectOfType<Canvas>(); // cari canvas utama
            healthBarUIInstance = Instantiate(healthBarUIPrefab, canvas.transform);
            healthSlider = healthBarUIInstance.GetComponentInChildren<Slider>();

            // Set target follow
            var follow = healthBarUIInstance.GetComponent<EnemyHealthBarFollow>();
            if (follow != null) follow.target = this.transform;

            // Set nilai awal
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

    }

    protected bool IsInActionState()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        return state.IsTag("Attack") || state.IsTag("TakeHit") || isDead;
    }

    protected virtual void Update()
    {
        if (isDead || target == null) return;

        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        // 🔍 Cek apakah player masuk area kejar
        if (!canChasePlayer && ShouldStartChasing(distanceToTarget))
        {
            canChasePlayer = true;
        }

        if (!canChasePlayer) return;

        Vector2 direction = new Vector2(target.position.x - transform.position.x, 0);
        float distance = direction.magnitude;

        if (IsInActionState())
        {
            movement = Vector2.zero;
            animator.SetFloat("Speed", 0f);
            return;
        }

        if (distance > stoppingDistance)
        {
            movement = direction.normalized;
            spriteRenderer.flipX = direction.x < 0;
            animator.SetFloat("Speed", 1f);
        }
        else
        {
            movement = Vector2.zero;
            animator.SetFloat("Speed", 0f);

            if (Time.time - lastAttackTime > attackCooldown && distance <= attackRange)
            {
                PerformAttack();
            }
        }
        Debug.Log("Can chase: " + canChasePlayer + " | Distance: " + distanceToTarget);

    }


    void FixedUpdate()
    {
        if (!isDead && canChasePlayer)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    public virtual void DealDamage()
    {
        Debug.Log("DealDamage() DIPANGGIL");

        Collider2D hit = Physics2D.OverlapCircle(transform.position, attackRange, LayerMask.GetMask("Player"));
        if (hit != null)
        {
            Debug.Log("Player DITEMUKAN DALAM RANGE");

            PlayerMovement player = hit.GetComponent<PlayerMovement>();
            if (player != null)
            {
                Debug.Log("PlayerMovement ditemukan - memanggil TakeDamage()");
                player.TakeDamage();
            }
        }
        else
        {
            Debug.Log("TIDAK ADA player dalam range");
        }
    }

    public virtual void TakeDamage()
    {
        if (isDead) return;

        currentHealth--;
        animator.SetTrigger("TakeHit");

        if (audioSource != null && takeHitSFX != null)
        {
            audioSource.PlayOneShot(takeHitSFX);
        }

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            isDead = true;
            movement = Vector2.zero;

            float takeHitDuration = 0.5f;
            Invoke(nameof(Die), takeHitDuration);
        }
    }

    protected virtual void Die()
    {
        animator.SetTrigger("Die");
        Debug.Log("Goblin mati!");

        if (audioSource != null && dieSFX != null)
        {
            audioSource.PlayOneShot(dieSFX);
        }

        if (healthBarUIInstance != null)
        {
            Destroy(healthBarUIInstance);
        }

        Destroy(gameObject, 1.2f);
    }

    protected virtual void PerformAttack()
    {
        animator.SetTrigger("Attack");
        lastAttackTime = Time.time;

        if (audioSource != null && attackSFX != null)
        {
            audioSource.PlayOneShot(attackSFX);
        }

        Debug.Log("Enemy menyerang biasa");
    }

    // 🎧 Dipanggil dari animasi jalan
    public void PlayFootstep()
    {
        if (!isDead && audioSource != null && stepSFX != null)
        {
            audioSource.PlayOneShot(stepSFX);
        }
    }

    // Fungsi penentu mulai mengejar
    protected virtual bool ShouldStartChasing(float distance)
    {
        return distance <= chaseRadius;
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
