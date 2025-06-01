using UnityEngine;
using UnityEngine.SceneManagement; // Untuk pindah scene
using System.Collections;

public class PilihLevel : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip buttonClickSound;
    public float delayBeforeScene = 0.3f; // Waktu tunggu sebelum pindah scene

    private void PlaySFX()
    {
        if (audioSource != null && buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }

    // Fungsi tombol Level1
    public void Level1()
    {
        // Ganti ke Scene Tebak Club Bola
        Destroy(GameObject.Find("BGMPlayer"));
        StartCoroutine(DelayedSceneLoad("main_menu"));
    }

    // Fungsi tombol Level2
    public void Level2()
    {
        // Bisa buka scene Hockey Sepak Bola
        Destroy(GameObject.Find("BGMPlayer"));
        StartCoroutine(DelayedSceneLoad("Menu"));
    }

    // Fungsi tombol Level 3
    public void Level3()
    {
        // Game yang sedang digarap
        Destroy(GameObject.Find("BGMPlayer"));
        StartCoroutine(DelayedSceneLoad("Menu_3"));
    }

    // Fungsi tombol "Back"
    public void BackBotton()
    {
        StartCoroutine(DelayedSceneLoad("MainMenu")); // Kembali Ke Main Mwnu
    }

    private IEnumerator DelayedSceneLoad(string sceneName)
    {
        PlaySFX();
        yield return new WaitForSeconds(delayBeforeScene);
        SceneManager.LoadScene(sceneName);
    }

}
