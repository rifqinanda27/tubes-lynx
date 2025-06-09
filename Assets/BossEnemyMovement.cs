using UnityEngine;

public class BossEnemyMovement : Enemy1Movement
{
    private int currentAttackStep = 1;
    public float comboInterval = 0.3f;
    private bool inCombo = false;

    public BossArenaManager bossArenaManager;
    public BossHealthUI bossHealthUI;


    protected override void Start()
    {
        base.Start();
        if (bossHealthUI != null)
        {
            bossHealthUI.SetMaxHealth(maxHealth);
        }
    }



    protected override void Update()
    {
        base.Update();

        // Lanjutkan combo jika sedang dalam mode combo
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

            // Atur waktu supaya comboInterval benar-benar jalan di Update berikutnya
            lastAttackTime = Time.time - comboInterval + 0.01f;
        }
    }


    private void NextComboAttack()
    {
        currentAttackStep++;

        if (currentAttackStep > 2)
        {
            inCombo = false; // combo selesai
            return;
        }

        TriggerAttackAnimation(currentAttackStep);
        lastAttackTime = Time.time;
    }

    private void TriggerAttackAnimation(int step)
    {
        animator.SetTrigger("Attack" + step);
        Debug.Log("Boss menyerang dengan Attack" + step);
    }

    public override void DealDamage()
    {
        base.DealDamage(); // Gunakan damage logic bawaan
    }

    public override void TakeDamage()
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


    protected override void Die()
    {
        animator.SetTrigger("Die");
        Debug.Log("Boss mati!");

        // Panggil bossArenaManager kalau ada
        if (bossArenaManager != null)
        {
            bossArenaManager.OnBossDefeated();
        }

        Destroy(gameObject, 1.5f); // tunggu animasi death selesai
    }

}
