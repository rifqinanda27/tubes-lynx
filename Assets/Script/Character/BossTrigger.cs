using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class BossTrigger : MonoBehaviour
{
    public CinemachineCamera bossCamera;
    public CinemachineCamera playerCamera;

    public GameObject bossHealthUI;
    public GameObject bossEnemy;
    public GameObject leftWall;
    public GameObject rightWall;

    public string[] dialogLines;

    private bool triggered = false;
    private Transform player;

    private Vector3 originalBossScale;

    private Animator bossAnimator;
    private Animator playerAnimator;

    public AudioSource bgmSource;
    public AudioClip bossBGM;
    public float fadeOutDuration = 1.5f;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            player = other.transform;

            // Ambil animator
            bossAnimator = bossEnemy.GetComponent<Animator>();
            playerAnimator = player.GetComponent<Animator>();

            // Simpan scale asli boss
            originalBossScale = bossEnemy.transform.localScale;

            // Pindah kamera
            bossCamera.Priority = 10;
            playerCamera.Priority = 1;

            // Boss hadap ke player
            FacePlayer();

            // Mulai cutscene
            StartCoroutine(WaitCameraTransitionThenCutscene());
        }
    }

    void FacePlayer()
    {
        Vector3 scale = bossEnemy.transform.localScale;
        float direction = player.position.x - bossEnemy.transform.position.x;

        if (direction < 0)
            scale.x = Mathf.Abs(originalBossScale.x) * -1f;
        else
            scale.x = Mathf.Abs(originalBossScale.x);

        bossEnemy.transform.localScale = scale;
    }

    IEnumerator WaitCameraTransitionThenCutscene()
    {
        // Tunggu sampai kamera tidak lagi bergerak
        CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();
        while (brain.IsBlending) yield return null;

        // Bekukan gerakan player & boss tanpa freeze time
        bossEnemy.GetComponent<Enemy1Movement>().inCutscene = true;
        player.GetComponent<PlayerMovement>().inCutscene = true;

        // Putar animasi idle (jika perlu paksa play)
        // bossAnimator?.CrossFade("Idle", 0.1f);
        // playerAnimator?.CrossFade("Idle", 0.1f);
        bossAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        playerAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;


        yield return new WaitForSeconds(1.5f); // jeda sinematik

        // Mulai dialog
        FindObjectOfType<DialogManager>().StartDialog(dialogLines, AfterDialog);
    }

    void AfterDialog()
    {
        bossAnimator.updateMode = AnimatorUpdateMode.Normal;
        playerAnimator.updateMode = AnimatorUpdateMode.Normal;

        // Kembalikan arah boss
        bossEnemy.transform.localScale = originalBossScale;

        // Buka arena pertarungan
        leftWall.GetComponent<Collider2D>().enabled = true;
        rightWall.GetComponent<Collider2D>().enabled = true;

        // Tampilkan UI HP boss
        bossHealthUI.SetActive(true);

        // Aktifkan AI boss
        var enemy = bossEnemy.GetComponent<Enemy1Movement>();
        enemy.canChasePlayer = true;
        enemy.inCutscene = false;

        // Aktifkan kembali player
        player.GetComponent<PlayerMovement>().inCutscene = false;

        if (bgmSource != null && bossBGM != null)
        {
            bgmSource.clip = bossBGM;
            bgmSource.loop = true;
            bgmSource.Play();
        }

        // Hapus trigger
        Destroy(gameObject);
    }
}
