using UnityEngine;
using System.Collections;

public class DialogTriggerZone : MonoBehaviour
{
    [TextArea(2, 4)]
    public string[] dialogLines;

    public DialogManager dialogManager;
    public bool isRepeatable = false;

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (!isRepeatable && hasTriggered) return;

        hasTriggered = true;

        StartCoroutine(StartDialogCutscene(other.transform));
    }

    private IEnumerator StartDialogCutscene(Transform player)
    {
        var playerMove = player.GetComponent<PlayerMovement>();
        var playerAnim = player.GetComponent<Animator>();

        if (playerMove != null)
            playerMove.inCutscene = true;

        if (playerAnim != null)
        {
            playerAnim.updateMode = AnimatorUpdateMode.UnscaledTime;
            playerAnim.CrossFade("Idle", 0.05f);
        }

        yield return new WaitForSecondsRealtime(0.5f); // biar animasi idle sempat play

        if (dialogManager != null && dialogLines.Length > 0)
        {
            dialogManager.StartDialog(dialogLines, () =>
            {
                if (playerMove != null)
                    playerMove.inCutscene = false;

                if (playerAnim != null)
                    playerAnim.updateMode = AnimatorUpdateMode.Normal;
            });
        }
        else
        {
            Debug.LogWarning("❗DialogManager belum di-assign atau dialog kosong.");
        }
    }
}
