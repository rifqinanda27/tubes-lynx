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
        if (isDead) return;

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

            float takeHitDuration = 0.5f;
            Invoke(nameof(Die), takeHitDuration);
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

}
