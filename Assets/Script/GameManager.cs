using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool isDialogActive = false;
    public static bool isBossDead = false;

    void Awake()
    {
        isDialogActive = false;
        isBossDead = false;
    }

    public void SkipCredit()
    {
        Debug.Log("Skip Credit ditekan!");
        SceneManager.LoadScene("MainMenu");
    }
}
