using UnityEngine;
using System.Collections;

public class SceneFadeIn : MonoBehaviour
{
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 1f;
    public GameObject targetToActivate;
    private void Start()
    {
        targetToActivate.SetActive(true); 
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float timer = 0f;
        fadeCanvasGroup.alpha = 1f;
        while (timer <= fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = 1f - Mathf.Clamp01(timer / fadeDuration);
            fadeCanvasGroup.alpha = alpha;
            yield return null;
        }
    }
}
