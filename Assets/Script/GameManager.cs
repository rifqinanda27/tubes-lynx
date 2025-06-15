using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool isDialogActive = false;

    void Awake()
    {
        isDialogActive = false;
    }
}
