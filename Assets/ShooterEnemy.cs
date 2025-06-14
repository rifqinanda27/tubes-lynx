using UnityEngine;

public class ShooterEnemy : Enemy1Movement
{
    public GameObject bulletPrefab;
    public Transform shootPoint;

    protected override void Update()
    {
        if (isDead || target == null) return;

        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        if (!canChasePlayer && ShouldStartChasing(distanceToTarget))
        {
            canChasePlayer = true;
        }

        if (!canChasePlayer) return;

        Vector2 direction = new Vector2(target.position.x - transform.position.x, 0f);
        float distance = direction.magnitude;

        // Selalu flip arah
        bool faceLeft = direction.x < 0;
        spriteRenderer.flipX = faceLeft;

        // Flip shootPoint juga
        if (shootPoint != null)
        {
            Vector3 localPos = shootPoint.localPosition;
            localPos.x = Mathf.Abs(localPos.x) * (faceLeft ? -1 : 1);
            shootPoint.localPosition = localPos;
        }

        // Cek animasi sedang menyerang atau terkena hit
        if (baseAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") ||
            baseAnimator.GetCurrentAnimatorStateInfo(0).IsTag("TakeHit") ||
            isDead)
        {
            movement = Vector2.zero;
            baseAnimator.SetFloat("Speed", 0f);
            return;
        }

        if (distance > stoppingDistance)
        {
            movement = direction.normalized;
            baseAnimator.SetFloat("Speed", 1f);
        }
        else
        {
            movement = Vector2.zero;
            baseAnimator.SetFloat("Speed", 0f);

            if (Time.time - lastAttackTime > attackCooldown && distance <= attackRange)
            {
                PerformAttack();
            }
        }
    }

    protected override void PerformAttack()
    {
        baseAnimator.SetTrigger("Attack");
        lastAttackTime = Time.time;

       

        // Peluru akan keluar dari Animation Event -> Shoot()
    }


    public void Shoot()
    {
        if (bulletPrefab != null && shootPoint != null)
        {

            Vector2 shootDir = spriteRenderer.flipX ? Vector2.left : Vector2.right;

            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            bullet.GetComponent<EnemyBullet>().SetDirection(shootDir);

            bullet.transform.localScale = new Vector3(
                Mathf.Abs(bullet.transform.localScale.x) * (spriteRenderer.flipX ? -1 : 1),
                bullet.transform.localScale.y,
                bullet.transform.localScale.z
            );
             if (attackSFX != null && audioSource != null)
            {
                audioSource.PlayOneShot(attackSFX);
            }
        }
    }

}
