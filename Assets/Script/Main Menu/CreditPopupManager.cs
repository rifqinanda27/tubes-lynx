using UnityEngine;

public class CreditPopupManager : MonoBehaviour
{
    public GameObject CreditPopup;

    public void ClosePopup()
    {
        CreditPopup.SetActive(false);
        Time.timeScale = 1f;
    }
}
