using UnityEngine;
using System.Collections;

public class AutoPlaySceneMusic : MonoBehaviour
{
    public AudioClip sceneBGM;
    public AudioClip localBossClip;

    private void Start()
    {
        StartCoroutine(PlaySceneMusicSafely());
    }

    private IEnumerator PlaySceneMusicSafely()
    {
        if (sceneBGM != null && sceneBGM.loadState != AudioDataLoadState.Loaded)
        {
            sceneBGM.LoadAudioData();
            while (sceneBGM.loadState == AudioDataLoadState.Loading)
                yield return null;
        }

        if (MusicManager.Instance != null && sceneBGM != null)
        {
            MusicManager.Instance.PlayMusic(sceneBGM);
        }

        // Optional preload boss music juga
        if (localBossClip != null && localBossClip.loadState != AudioDataLoadState.Loaded)
        {
            localBossClip.LoadAudioData();
            while (localBossClip.loadState == AudioDataLoadState.Loading)
                yield return null;
        }

        if (MusicManager.Instance != null && localBossClip != null)
        {
            MusicManager.Instance.bossBGM = localBossClip;
        }
    }


}
