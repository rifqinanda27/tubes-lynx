using UnityEngine;
using System.Collections;

public class SceneIntroDialog : MonoBehaviour
{
    [TextArea(2, 4)]
    public string[] introLines;

    public DialogManager dialogManager; // 👉 drag dari scene
    public GameObject playerObject;     // 👉 drag Player (yang punya Animator)

    private bool started = false;
    private Animator playerAnimator;

    void Start()
    {
        StartCoroutine(DelayedStartDialog());
    }

    private IEnumerator DelayedStartDialog()
    {
        yield return null; // tunggu 1 frame agar semua komponen siap

        if (started)
        {
            Debug.Log("⚠️ Dialog sudah pernah dimulai, skip.");
            yield break;
        }

        if (introLines.Length == 0)
        {
            Debug.LogWarning("⚠️ introLines kosong!");
            yield break;
        }

        if (dialogManager == null)
        {
            Debug.LogError("❌ DialogManager belum di-assign di Inspector!");
            yield break;
        }

        if (playerObject != null)
        {
            playerAnimator = playerObject.GetComponent<Animator>();
            if (playerAnimator != null)
                playerAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        }

        started = true;

        Debug.Log("📢 Memulai dialog pembuka...");

        // Start dialog tanpa freeze animator
        Time.timeScale = 0f;
        dialogManager.StartDialog(introLines, () =>
        {
            // callback setelah dialog selesai
            if (playerAnimator != null)
                playerAnimator.updateMode = AnimatorUpdateMode.Normal;
        });
    }
}
