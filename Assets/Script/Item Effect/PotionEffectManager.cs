using UnityEngine;

public class PotionEffectManager : MonoBehaviour
{
    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();

        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement not found in the scene.");
        }
    }

    public void UseHealthPotion(int healAmount)
    {
        if (playerMovement != null)
        {
            playerMovement.Heal(healAmount);
            Debug.Log($"Health Potion used. Player healed by {healAmount}.");
        }
    }
}
