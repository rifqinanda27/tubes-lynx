using UnityEngine;

public class Enemy1Movement : MonoBehaviour
{
    public Transform target;        // Referensi ke player
    public float moveSpeed = 2f;    // Kecepatan jalan musuh
    public float stoppingDistance = 0.5f; // Jarak aman ke player

    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;

            // Jarak saat musuh berhenti agar tidak nempel player
            float distance = Vector2.Distance(target.position, transform.position);
            if (distance > stoppingDistance)
            {
                movement = direction;
            }
            else
            {
                movement = Vector2.zero;
            }
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
