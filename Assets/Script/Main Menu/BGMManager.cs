using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager instance;

    public AudioSource bgmAudioSource;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}

