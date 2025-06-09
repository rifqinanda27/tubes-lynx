using UnityEngine;
using Unity.Cinemachine;

public class BossTrigger : MonoBehaviour
{
    public CinemachineCamera bossCamera;
    public CinemachineCamera playerCamera;

    public GameObject bossHealthUI;
    public GameObject bossEnemy;
    public GameObject leftWall;
    public GameObject rightWall;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // bossHealthUI.gameObject.SetActive(false);
        if (other.CompareTag("Player"))
        {
            bossCamera.Priority = 10;
            playerCamera.Priority = 1;

            // Aktifkan dinding pembatas
            leftWall.GetComponent<Collider2D>().enabled = true;
            rightWall.GetComponent<Collider2D>().enabled = true;

            bossHealthUI.gameObject.SetActive(true);
            bossEnemy.GetComponent<Enemy1Movement>().canChasePlayer = true;
            // Opsional: hancurkan trigger biar cuma sekali
            Destroy(gameObject);
        }
        Debug.Log("Trigger entered: " + other.name);
    }
}
