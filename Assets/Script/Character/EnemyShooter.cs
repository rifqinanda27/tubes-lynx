using UnityEngine;

public class EnemyShooter : Enemy1Movement
{
    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float bulletSpeed = 5f;

    protected override void PerformAttack()
    {
        animator.SetTrigger("Attack");
        lastAttackTime = Time.time;

        if (audioSource != null && attackSFX != null)
        {
            audioSource.PlayOneShot(attackSFX);
        }

        // Tembakkan peluru
        Shoot();
    }

    private void Shoot()
    {
        if (bulletPrefab == null || shootPoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);

        // arah ke player
        Vector2 direction = (target.position - shootPoint.position).normalized;
        bullet.GetComponent<Rigidbody2D>().linearVelocity = direction * bulletSpeed;

        // rotasi peluru (opsional biar hadap arah gerak)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
