using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOver : MonoBehaviour
{
    
    public float delayBeforeScene = 0.3f; // Waktu tunggu sebelum pindah scene
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartGame()
    {
        StartCoroutine(DelayedSceneLoad("Cutscene 1"));
    }

    public void MainMenu()
    {
        StartCoroutine(DelayedSceneLoad("MainMenu"));
    }

    private IEnumerator DelayedSceneLoad(string sceneName)
    {
        yield return new WaitForSeconds(delayBeforeScene);
        SceneManager.LoadScene(sceneName);
    }
}
