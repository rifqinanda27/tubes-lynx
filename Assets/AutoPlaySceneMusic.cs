using UnityEngine;

public class AutoPlaySceneMusic : MonoBehaviour
{
    public AudioClip sceneBGM;

    private void Start()
    {
        if (MusicManager.Instance != null && sceneBGM != null)
        {
            MusicManager.Instance.PlayMusic(sceneBGM);
        }
    }

}
