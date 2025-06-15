using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    public Slider slider;
    public Vector3 offset = new Vector3(0, 1.5f, 0); // posisinya di atas kepala
    private Transform target; // enemy target

    public void Initialize(Transform followTarget, int maxHealth)
    {
        target = followTarget;
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    public void UpdateHealth(int currentHealth)
    {
        slider.value = currentHealth;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
}
