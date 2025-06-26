using UnityEngine;

public class CatFollower : MonoBehaviour
{
    public Transform player;
    public float followDistance = 5f;
    public float speed = 2f;
    public float stopDistance = 1.2f;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (player == null) return;

        float horizontalDistance = Mathf.Abs(transform.position.x - player.position.x);

        float moveSpeed = 0f;

        if (horizontalDistance < followDistance && horizontalDistance > stopDistance)
        {
            float directionX = Mathf.Sign(player.position.x - transform.position.x);
            Vector3 move = new Vector3(directionX * speed * Time.deltaTime, 0f, 0f);
            transform.position += move;
            moveSpeed = speed;

            // Flip arah menghadap
            if (directionX > 0)
                transform.localScale = new Vector3(1, 1, 1);
            else if (directionX < 0)
                transform.localScale = new Vector3(-1, 1, 1);
        }

        if (animator != null)
        {
            animator.SetFloat("Speed", moveSpeed);
        }
    }
}
