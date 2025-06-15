using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    protected Animator animator;
    public float speed = 5f;
    public float lifetime = 3f;
    public int damage = 1;
    public float maxDistance = 10f;

    private Vector2 direction;
    private Vector2 startPos;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        Destroy(gameObject, lifetime);
        startPos = transform.position;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        float distanceTravelled = Vector2.Distance(startPos, transform.position);
        if (distanceTravelled >= maxDistance)
        {
            if (animator != null)
            {
                animator.SetTrigger("TakeHit");
                StartCoroutine(DestroyAfterAnimation());
            }
            else
            {
                Destroy(gameObject);
            }

            // Pastikan peluru tidak bergerak saat animasi jalan
            speed = 0f;
            enabled = false; // opsional, matikan Update agar tidak terus jalan
        }
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
