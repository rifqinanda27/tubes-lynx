using UnityEngine;

public class ExitPopupManager : MonoBehaviour
{
    public GameObject ExitPopup;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Tampilkan popup kalau belum ditampilkan
            if (!ExitPopup.activeSelf)
            {
                ExitPopup.SetActive(true);
                Time.timeScale = 0f; // Opsional: freeze game
            }
            else
            {
                ClosePopup();
            }
        }
    }

    public void ConfirmExit()
    {
        Time.timeScale = 1f; // Pastikan normal dulu
        Application.Quit();
        Debug.Log("Keluar dari game");
    }

    public void ClosePopup()
    {
        ExitPopup.SetActive(false);
        Time.timeScale = 1f;
    }
}
