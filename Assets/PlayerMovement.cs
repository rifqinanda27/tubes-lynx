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
    private bool canAcceptInput = true;

    private float comboTimer = 0f;
    public float comboResetTime = 0.5f; // durasi maksimum tunggu input combo selanjutnya

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Jangan gerak saat menyerang
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

        // Input Attack
        if (Input.GetKeyDown(KeyCode.J) && canAcceptInput && isGrounded)
        {
            comboStep++;
            if (comboStep > 4) comboStep = 1;

            Debug.Log("ComboStep: " + comboStep);
            animator.SetInteger("ComboStep", comboStep);
            canAcceptInput = false;
            comboTimer = 0f; // reset timer saat serangan berikutnya dimulai
        }

        // Combo timeout check
        if (comboStep > 0)
        {
            comboTimer += Time.deltaTime;
            if (comboTimer > comboResetTime && !canAcceptInput)
            {
                Debug.Log("Combo timeout - ResetAttack otomatis");
                ResetAttack();
            }
        }

        // Debug
        Debug.Log("ComboStep: " + comboStep);
        Debug.Log("CanAcceptInput: " + canAcceptInput);
        Debug.Log("IsGrounded: " + isGrounded);
        Debug.Log("CurrentAnimState: " + animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
            animator.SetBool("IsJumping", false);
        }
    }

    // Event dipanggil di tengah animasi attack
    public void EnableNextInput()
    {
        Debug.Log("EnableNextInput dipanggil pada ComboStep: " + comboStep);
        if (isGrounded)
            canAcceptInput = true;
    }

    // Event di akhir animasi attack
    public void ResetAttack()
    {
        Debug.Log("ResetAttack dipanggil pada ComboStep: " + comboStep);
        comboStep = 0;
        animator.SetInteger("ComboStep", 0);
        canAcceptInput = true;
        comboTimer = 0f;
    }
}
