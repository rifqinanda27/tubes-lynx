using UnityEngine;

public class ButtonSFX : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip sfxClip;

    public void PlaySFX()
    {
        audioSource.PlayOneShot(sfxClip);
    }
}
