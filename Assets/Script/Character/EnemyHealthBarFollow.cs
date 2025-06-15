using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarFollow : MonoBehaviour
{
    public Transform target; // Enemy transform
    public Vector3 offset = new Vector3(0, 1.5f, 0); // Untuk posisi di atas kepala
    private RectTransform rectTransform;
    private Canvas canvas;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    void Update()
    {
        if (target == null || canvas == null) return;

        // Dapatkan kamera dari canvas
        Camera cam = canvas.worldCamera;

        // World position (enemy + offset)
        Vector3 worldPos = target.position + offset;

        // Konversi ke posisi layar
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(cam, worldPos);

        // Konversi ke local position dalam canvas
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPos,
            cam,
            out Vector2 localPos
        );

        rectTransform.localPosition = localPos;
    }
}
