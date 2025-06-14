using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    protected Animator animator;
    public float speed = 5f;
    public float lifetime = 3f;
    public int damage = 1;

    private Vector2 direction;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        Destroy(gameObject, lifetime);
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.TakeDamage();
            }

            if (animator != null)
            {
                animator.SetTrigger("TakeHit");
            }

            StartCoroutine(DestroyAfterAnimation()); // ⏳ Delay destroy
        }

        if (other.CompareTag("Ground") || other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

    private System.Collections.IEnumerator DestroyAfterAnimation()
    {
        // Optional: matikan gerakan agar peluru berhenti
        speed = 0;

        // Tunggu durasi animasi "TakeHit", misalnya 0.3 detik
        yield return new WaitForSeconds(0.3f);

        Destroy(gameObject);
    }

}
