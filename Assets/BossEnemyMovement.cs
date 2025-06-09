using UnityEngine;

public class BossEnemyMovement : Enemy1Movement
{
    private int currentAttackStep = 1;
    public float comboInterval = 0.3f;
    private bool inCombo = false;

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
}
