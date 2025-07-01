using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;
using System.Collections;

public class BossArenaManager : MonoBehaviour
{
    public CinemachineCamera bossCamera;
    public CinemachineCamera playerCamera;
    public GameObject leftWall;
    public GameObject rightWall;
    public GameObject bossHealthUI;

    [Header("Scene Transition (Optional)")]
    public string nextSceneName; // kosongkan jika tidak ingin pindah scene
    public float delayBeforeSceneLoad = 5f;

    [Header("Fade Transition")]
    public CanvasGroup fadeCanvasGroup; // assign dari inspector
    public float fadeDuration = 1f;

    public void OnBossDefeated()
    {
        if (playerCamera != null && bossCamera != null)
        {
            playerCamera.Priority = 10;
            bossCamera.Priority = 5;
        }

        if (leftWall != null) Destroy(leftWall);
        if (rightWall != null) Destroy(rightWall);
        if (bossHealthUI != null) bossHealthUI.SetActive(false);

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            StartCoroutine(TransitionWithFade());
        }
    }

    private IEnumerator TransitionWithFade()
    {
        yield return new WaitForSeconds(delayBeforeSceneLoad);

        // Fade out screen
        yield return StartCoroutine(FadeOut());

        // Load next scene
        SceneManager.LoadScene(nextSceneName);
    }

    private IEnumerator FadeOut()
    {
        float timer = 0f;
        while (timer <= fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Clamp01(timer / fadeDuration);
            fadeCanvasGroup.alpha = alpha;
            yield return null;
        }
    }
}
