using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : MonoBehaviour
{
    public Slider healthSlider;

    public void SetMaxHealth(int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
    }

    public void SetHealth(int health)
    {
        healthSlider.value = health;
    }
}
