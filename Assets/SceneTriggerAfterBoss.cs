using UnityEngine;
using System.Collections;

public class SceneTriggerAfterBossDialog : MonoBehaviour
{
    [Header("🎬 Scene Settings")]
    public string sceneToLoad;

    [Header("💬 Dialog")]
    [TextArea(2, 4)]
    public string[] dialogLines;
    public DialogManager dialogManager;

    private bool triggered = false;

    [Header("🧟 Boss Check")]
    public GameObject bossObject;

    private void Start()
    {
        Debug.Log("🎯 Start()");
        Debug.Log("DialogManager: " + (dialogManager != null ? dialogManager.name : "NULL"));
        Debug.Log("Dialog Lines: " + (dialogLines != null ? dialogLines.Length.ToString() : "NULL"));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        // Boss belum mati (masih ada gameObject-nya)
        if (bossObject != null)
        {
            Debug.Log("❌ Boss belum mati. Tidak bisa pindah scene.");
            return;
        }

        triggered = true;
        StartCoroutine(PlayDialogThenScene(other.transform));
    }

    private IEnumerator PlayDialogThenScene(Transform player)
    {
        var playerMove = player.GetComponent<PlayerMovement>();
        var playerAnim = player.GetComponent<Animator>();

        // Bekukan player
        if (playerMove != null)
            playerMove.inCutscene = true;

        if (playerAnim != null)
        {
            playerAnim.updateMode = AnimatorUpdateMode.UnscaledTime;
            playerAnim.CrossFade("Idle", 0.05f);
        }

        yield return new WaitForSecondsRealtime(0.5f); // beri waktu idle muncul
        Debug.Log($"📋 dialogLines.Length = {dialogLines.Length}");
        Debug.Log($"📋 dialogManager = {(dialogManager != null ? "OK" : "NULL")}");

        if (dialogManager != null && dialogLines.Length > 0)
        {
            dialogManager.StartDialog(dialogLines, () =>
            {
                if (playerMove != null)
                    playerMove.inCutscene = false;

                if (playerAnim != null)
                    playerAnim.updateMode = AnimatorUpdateMode.Normal;

                Debug.Log("✅ Dialog selesai. Fade out music lalu pindah scene");
                StartCoroutine(FadeOutMusicThenLoadScene());
            });
        }
        else
        {
            Debug.LogWarning("❗DialogManager belum di-assign atau dialog kosong.");
        }
    }

    private IEnumerator FadeOutMusicThenLoadScene()
    {
        if (MusicManager.Instance != null)
        {
            yield return MusicManager.Instance.StopMusicCoroutine(1.5f); // pastikan ada fungsi ini di MusicManager
        }

        if (SceneTransitionManager1.Instance != null)
            SceneTransitionManager1.Instance.FadeToScene(sceneToLoad);
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
    }
}
