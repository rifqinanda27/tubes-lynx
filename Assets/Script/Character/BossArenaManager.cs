using UnityEngine;
using Unity.Cinemachine;

public class BossArenaManager : MonoBehaviour
{
    public CinemachineCamera bossCamera;
    public CinemachineCamera playerCamera;
    public GameObject leftWall;
    public GameObject rightWall;
    public GameObject bossHealthUI;

    public void OnBossDefeated()
    {
        // Aktifkan kamera normal lagi
        if (playerCamera != null && bossCamera != null)
        {
            playerCamera.Priority = 10;
            bossCamera.Priority = 5;
        }

        // Hancurkan/disable pembatas arena
        if (leftWall != null) Destroy(leftWall);
        if (rightWall != null) Destroy(rightWall);
        bossHealthUI.gameObject.SetActive(false);
    }
}
