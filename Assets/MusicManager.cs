using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    private AudioSource audioSource;

    [Range(0f, 1f)]
    public float musicVolume = 1f;

    [Header("🎵 Music Clips")]
    public AudioClip normalBGM;
    public AudioClip bossBGM;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Hanya satu instance
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = musicVolume;
    }

    private void Start()
    {
        if (normalBGM != null)
        {
            Debug.Log("🎵 Memainkan normal BGM: " + normalBGM.name);
            PlayMusic(normalBGM);
        }
        else
        {
            Debug.LogWarning("⚠️ normalBGM belum di-assign di MusicManager!");
        }
    }

    public void PlayNormalBGM(float fadeDuration = 1.5f)
    {
        if (normalBGM != null)
        {
            PlayMusic(normalBGM, fadeDuration);
        }
    }

    public void PlayBossBGM(float fadeDuration = 1.5f)
    {
        if (bossBGM != null)
        {
            PlayMusic(bossBGM, fadeDuration);
        }
    }

    public void PlayMusic(AudioClip clip, float fadeDuration = 1.5f)
    {
        if (clip == null)
        {
            Debug.LogWarning("⚠️ Clip null, tidak bisa dimainkan.");
            return;
        }

        if (audioSource.clip == clip)
        {
            Debug.Log("ℹ️ Musik sudah diputar: " + clip.name);
            return;
        }

        Debug.Log("🎵 Transisi ke musik: " + clip.name);
        StartCoroutine(FadeToNewMusic(clip, fadeDuration));
    }

    public void StopMusic(float fadeDuration = 1.5f)
    {
        StartCoroutine(FadeOutAndStop(fadeDuration));
    }

    public void SetVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        audioSource.volume = musicVolume;
    }

    private IEnumerator FadeToNewMusic(AudioClip newClip, float duration)
    {
        float startVolume = audioSource.volume;

        // Fade out
        for (float t = 0; t < duration; t += Time.unscaledDeltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }

        audioSource.Stop();

        // Pastikan audio sudah siap
        newClip.LoadAudioData();
        yield return new WaitForSecondsRealtime(0.05f);

        audioSource.clip = newClip;
        audioSource.Play();

        // Fade in
        for (float t = 0; t < duration; t += Time.unscaledDeltaTime)
        {
            audioSource.volume = Mathf.Lerp(0f, musicVolume, t / duration);
            yield return null;
        }

        audioSource.volume = musicVolume;
    }

    private IEnumerator FadeOutAndStop(float duration)
    {
        float startVolume = audioSource.volume;

        for (float t = 0; t < duration; t += Time.unscaledDeltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }

        audioSource.Stop();
    }
}
