using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TimelineToScene : MonoBehaviour
{
    [Header("🎬 Timeline")]
    public PlayableDirector timeline;

    [Header("🎯 Scene Berikutnya")]
    public string nextSceneName = "Credit";

    [Header("🎵 Musik")]
    public bool stopMusicOnEnd = true;
    public float musicFadeOutDuration = 1.5f;

    private void Start()
    {
        if (timeline != null)
        {
            timeline.stopped += OnTimelineFinished;
        }
        else
        {
            Debug.LogWarning("❗PlayableDirector (timeline) belum di-assign!");
        }
    }

    private void OnTimelineFinished(PlayableDirector director)
    {
        Debug.Log("✅ Timeline selesai. Ganti scene ke: " + nextSceneName);

        if (stopMusicOnEnd && MusicManager.Instance != null)
        {
            MusicManager.Instance.StopMusic(musicFadeOutDuration);
        }

        SceneManager.LoadScene(nextSceneName);
    }
}
