using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool isDialogActive = false;
    public static bool isBossDead = false;

    void Awake()
    {
        isDialogActive = false;
        isBossDead = false;
    }
}
