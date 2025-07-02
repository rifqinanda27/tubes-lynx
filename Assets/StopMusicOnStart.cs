using UnityEngine;

public class StopMusicOnStart : MonoBehaviour
{
    public float fadeOutDuration = 1.5f;

    private void Start()
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.StopMusic(fadeOutDuration);
            Debug.Log("🛑 Musik dihentikan saat masuk scene.");
        }
    }
}
