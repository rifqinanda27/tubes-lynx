using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Endingmanager : MonoBehaviour
{
    public float scrollSpeed = 50f;
    public GameObject skipPanel;
    private bool isPaused = false;

    void Update()
    {
        if (!isPaused)
        {
            transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSkipPanel();
        }
    }

    public void ToggleSkipPanel()
    {
        isPaused = !isPaused;
        skipPanel.SetActive(isPaused);
    }

    public void SkipCredit()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ContinueCredit()
    {
        isPaused = false;
        skipPanel.SetActive(false);
    }
}
