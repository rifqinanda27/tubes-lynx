using UnityEngine;

public class HeartManager : MonoBehaviour
{
    public GameObject[] hearts; // drag 5 heart prefab dari inspector

    public void LoseHeart(int healthRemaining)
    {
        if (healthRemaining < 0 || healthRemaining >= hearts.Length)
            return;

        Animator heartAnim = hearts[healthRemaining].GetComponent<Animator>();
        if (heartAnim != null)
        {
            heartAnim.SetTrigger("Break"); // animasi hati pecah
        }
    }
}
