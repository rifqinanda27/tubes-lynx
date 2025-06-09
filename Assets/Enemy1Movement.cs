using UnityEngine;

public class Enemy1Movement : MonoBehaviour
{
    public float attackRange = 0.5f;
    public float attackCooldown = 1f;
    protected float lastAttackTime = 0f;

    public Transform target;
    public float moveSpeed = 2f;
    public float stoppingDistance = 0.5f;

    private Rigidbody2D rb;
    private Vector2 movement;
    protected Animator animator;
    private SpriteRenderer spriteRenderer;

    public int maxHealth = 2;
    private int currentHealth;
    private bool isDead = false;

    public BossHealthUI bossHealthUI;

    void Start()
    {
        currentHealth = maxHealth;

        if (bossHealthUI != null)
        {
            bossHealthUI.SetMaxHealth(maxHealth);
        }

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    private bool IsInActionState()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        return state.IsTag("Attack") || state.IsTag("TakeHit") || isDead;
    }

    protected virtual void Update()
    {
        if (target != null && !isDead)
        {
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
        }
    }

    void FixedUpdate()
    {
        if (!isDead)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    // Dipanggil dari animasi
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

    public void TakeDamage()
    {
        if (isDead) return;

        currentHealth--;
        animator.SetTrigger("TakeHit");

        if (bossHealthUI != null)
        {
            bossHealthUI.SetHealth(currentHealth);
        }

        if (currentHealth <= 0)
        {
            isDead = true;
            movement = Vector2.zero;

            float takeHitDuration = 0.5f;
            Invoke(nameof(Die), takeHitDuration);
        }
    }


    void Die()
    {
        animator.SetTrigger("Die");
        Debug.Log("Goblin mati!");

        // Hapus setelah animasi Death selesai
        Destroy(gameObject, 1.2f); // sesuaikan durasi animasi death
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    protected virtual void PerformAttack()
    {
        animator.SetTrigger("Attack");
        lastAttackTime = Time.time;
        Debug.Log("Enemy menyerang biasa");
    }

}
