using UnityEngine;

public class InfiniteLoop : MonoBehaviour
{
    public float spriteWidth;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float distFromPlayer = player.position.x - transform.position.x;
        if (Mathf.Abs(distFromPlayer) > spriteWidth)
        {
            float offset = spriteWidth * Mathf.Sign(distFromPlayer);
            transform.position += new Vector3(offset, 0, 0);
        }
    }
}
