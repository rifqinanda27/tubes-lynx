using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float runSpeed = 10f;
    public float jumpForce = 15f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded = false;

    private int comboStep = 0;
    private bool canAcceptInput = true; // untuk mencegah input ganda

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Movement hanya jalan kalau tidak attack
        if (comboStep == 0)
        {
            float moveInput = Input.GetAxisRaw("Horizontal");
            rb.linearVelocity = new Vector2(moveInput * runSpeed, rb.linearVelocity.y);

            if (moveInput > 0) spriteRenderer.flipX = false;
            else if (moveInput < 0) spriteRenderer.flipX = true;

            animator.SetFloat("Speed", Mathf.Abs(moveInput));
        }
        else
        {
            // Saat attack, stop gerak horizontal
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            animator.SetFloat("Speed", 0);
        }

        // Lompat
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && comboStep == 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetBool("IsJumping", true);
            isGrounded = false;
        }

        /// Input attack
        if (Input.GetKeyDown(KeyCode.J) && canAcceptInput && isGrounded && comboStep == 0)
        {
            comboStep++;
            if (comboStep > 4) comboStep = 1;

            Debug.Log("ComboStep: " + comboStep); 

            animator.SetInteger("ComboStep", comboStep);
            canAcceptInput = false; // tolak input selagi animasi belum selesai
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
            animator.SetBool("IsJumping", false);
        }
    }

    // Fungsi ini harus dipanggil oleh Animation Event di akhir setiap animasi attack
    public void ResetAttack()
    {
        comboStep = 0;
        animator.SetInteger("ComboStep", 0);
        canAcceptInput = true; // boleh terima input attack baru lagi
    }

    // Fungsi ini harus dipanggil oleh Animation Event di frame "attack siap terima input berikutnya"
    // Misal di tengah animasi attack, supaya combo bisa diteruskan
    public void EnableNextInput()
    {
        if (isGrounded)
            canAcceptInput = true;
    }
}
