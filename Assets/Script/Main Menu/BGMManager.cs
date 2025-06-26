using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class BGMManager : MonoBehaviour
{
    public static BGMManager instance;

    public AudioSource bgmAudioSource;

    [System.Serializable]
    public class SceneBGM
    {
        public string sceneName;
        public AudioClip bgmClip;
    }

    [Header("Scene BGM Settings")]
    public List<SceneBGM> sceneBGMs;

    private Dictionary<string, AudioClip> bgmDictionary = new Dictionary<string, AudioClip>();

    void Awake()
    {
        // Singleton pattern tanpa DontDestroyOnLoad
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        // Masukkan data ke dictionary untuk akses cepat
        foreach (SceneBGM sbgm in sceneBGMs)
        {
            if (!bgmDictionary.ContainsKey(sbgm.sceneName))
            {
                bgmDictionary.Add(sbgm.sceneName, sbgm.bgmClip);
            }
        }

        SceneManager.sceneLoaded += OnSceneLoaded;

        // Mainkan musik saat scene pertama kali load
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (bgmDictionary.ContainsKey(scene.name))
        {
            PlayBGM(bgmDictionary[scene.name]);
        }
        else
        {
            StopBGM();
        }
    }

    public void PlayBGM(AudioClip clip)
    {
        if (bgmAudioSource == null)
        {
            Debug.LogWarning("AudioSource belum di-assign di BGMManager!");
            return;
        }

        if (clip == null)
        {
            bgmAudioSource.Stop();
            return;
        }

        if (bgmAudioSource.clip == clip && bgmAudioSource.isPlaying)
            return;

        bgmAudioSource.clip = clip;
        bgmAudioSource.Play();
    }

    public void StopBGM()
    {
        if (bgmAudioSource != null)
            bgmAudioSource.Stop();
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
