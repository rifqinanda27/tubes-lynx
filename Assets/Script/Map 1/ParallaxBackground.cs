using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public Transform player; // drag Player ke sini di Inspector
    public float parallaxMultiplier = 0.5f;
    private Vector3 lastPlayerPos;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        lastPlayerPos = player.position;
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = player.position - lastPlayerPos;
        transform.position += new Vector3(deltaMovement.x * parallaxMultiplier, deltaMovement.y * parallaxMultiplier, 0);
        lastPlayerPos = player.position;
    }
}
