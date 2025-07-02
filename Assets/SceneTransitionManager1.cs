using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTransitionManager1 : MonoBehaviour
{
    public static SceneTransitionManager1 Instance;

    [Header("🎬 UI Fade")]
    public Image fadeImage;
    public float fadeDuration = 1f;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (fadeImage != null)
            fadeImage.gameObject.SetActive(true);
    }

    private void Start()
    {
        // Fade in ketika scene baru dimulai
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 1f;
            fadeImage.color = c;
            StartCoroutine(Fade(1f, 0f));
        }
    }

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeAndLoadScene(sceneName));
    }

    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        if (fadeImage == null)
        {
            SceneManager.LoadScene(sceneName);
            yield break;
        }

        // Fade out
        yield return Fade(0f, 1f);

        // ⏳ Tambahkan delay sebentar agar fade hitam terlihat
        yield return new WaitForSecondsRealtime(0.5f);

        // Load scene
        SceneManager.LoadScene(sceneName);

        // Tunggu 1 frame
        yield return null;

        // Cari ulang fadeImage jika perlu
        if (fadeImage == null)
            fadeImage = GameObject.Find("FadeImage")?.GetComponent<Image>();

        // Fade in
        yield return Fade(1f, 0f);
    }


    private IEnumerator Fade(float from, float to)
    {
        float t = 0f;
        Color color = fadeImage.color;

        while (t < fadeDuration)
        {
            float a = Mathf.Lerp(from, to, t / fadeDuration);
            color.a = a;
            fadeImage.color = color;
            t += Time.unscaledDeltaTime;
            yield return null;
        }

        color.a = to;
        fadeImage.color = color;
    }
}
