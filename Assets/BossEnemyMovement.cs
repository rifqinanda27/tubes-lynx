using UnityEngine;

public class BossEnemyMovement : Enemy1Movement
{
    private int currentAttackStep = 1;
    public float comboInterval = 0.3f;
    private bool inCombo = false;

    public BossArenaManager bossArenaManager;
    public BossHealthUI bossHealthUI;

    // 🔊 Sound Effects
    public AudioClip BossAttackSFX;
    public AudioClip BossTakeHitSFX;
    public AudioClip BossDieSFX;
    public AudioClip BossStepSFX;

    private AudioSource audioSource;

    [Header("Counter Settings")]
    public int hitsToTriggerInvincible = 3;
    private int hitCounter = 0;
    public float invincibilityDuration = 1f;
    private bool isInvincible = false;



    protected override void Start()
    {
        base.Start();
        audioSource = GetComponent<AudioSource>();

        if (bossHealthUI != null)
        {
            bossHealthUI.SetMaxHealth(maxHealth);
        }
    }

    protected override void Update()
    {
        base.Update();

        if (inCombo && Time.time - lastAttackTime > comboInterval)
        {
            NextComboAttack();
        }
    }

    protected override void PerformAttack()
    {
        if (!inCombo)
        {
            inCombo = true;
            currentAttackStep = 1;
            TriggerAttackAnimation(currentAttackStep);
            lastAttackTime = Time.time - comboInterval + 0.01f;
        }
    }

    private void NextComboAttack()
    {
        currentAttackStep++;

        if (currentAttackStep > 2)
        {
            inCombo = false;
            return;
        }

        TriggerAttackAnimation(currentAttackStep);
        lastAttackTime = Time.time;
    }

    private void TriggerAttackAnimation(int step)
    {
        animator.SetTrigger("Attack" + step);
        Debug.Log("Boss menyerang dengan Attack" + step);

        if (audioSource != null && BossAttackSFX != null)
        {
            audioSource.PlayOneShot(BossAttackSFX);
        }
    }

    public override void DealDamage()
    {
        base.DealDamage();
    }

    public override void TakeDamage()
    {
        if (isDead || isInvincible) return;

        currentHealth--;
        animator.SetTrigger("TakeHit");

        if (audioSource != null && BossTakeHitSFX != null)
        {
            audioSource.PlayOneShot(BossTakeHitSFX);
        }

        if (bossHealthUI != null)
        {
            bossHealthUI.SetHealth(currentHealth);
        }

        if (currentHealth <= 0)
        {
            isDead = true;
            movement = Vector2.zero;
            Invoke(nameof(Die), 0.5f);
        }
        else
        {
            hitCounter++;

            if (hitCounter >= hitsToTriggerInvincible)
            {
                StartCoroutine(HandleInvincibilityAndCounter());
            }
        }
    }



    protected override void Die()
    {
        animator.SetTrigger("Die");
        Debug.Log("Boss mati!");

        if (audioSource != null && BossDieSFX != null)
        {
            audioSource.PlayOneShot(BossDieSFX);
        }

        if (bossArenaManager != null)
        {
            bossArenaManager.OnBossDefeated();
        }

        Destroy(gameObject, 1.5f);
    }

    // 🎧 Dipanggil dari animasi jalan (melangkah)
    public void PlayFootstep()
    {
        if (!isDead && audioSource != null && BossStepSFX != null)
        {
            audioSource.PlayOneShot(BossStepSFX);
        }
    }

    protected override bool ShouldStartChasing(float distance)
    {
        return false; // Boss selalu bisa mulai ngejar kapan saja
    }

    private System.Collections.IEnumerator HandleInvincibilityAndCounter()
    {
        isInvincible = true;
        hitCounter = 0; // reset counter

        // Efek visual (opsional)
        spriteRenderer.color = new Color(1f, 0.5f, 0.5f, 1f);

        // Langsung counter attack
        PerformAttack();

        yield return new WaitForSeconds(invincibilityDuration);

        isInvincible = false;
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
    }
}
