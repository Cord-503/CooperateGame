using UnityEngine;

public class Player1 : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public int maxJumpCount = 2;

    [Header("Input Keys")]
    public KeyCode leftKey = KeyCode.A;    // A¼ü×óÒÆ
    public KeyCode rightKey = KeyCode.D;   // D¼üÓÒÒÆ
    public KeyCode jumpKey = KeyCode.W;    // W¼üÌøÔ¾
    public KeyCode jumpKey2 = KeyCode.Space; // ¿Õ¸ñÌøÔ¾

    private Rigidbody2D rb;
    private Animator animator;
    private bool ifOnGround;
    private bool ifRun;
    private bool isFacingRight = true;
    private int currentJumpCount;

    private readonly int hashIsGrounded = Animator.StringToHash("IsGrounded");
    private readonly int hashIsRunning = Animator.StringToHash("IsRunning");
    private readonly int hashJump = Animator.StringToHash("Jump");
    private readonly int hashVerticalSpeed = Animator.StringToHash("VerticalSpeed");

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentJumpCount = maxJumpCount;
    }

    private void Update()
    {
        HandleInput();
        UpdateAnimationState();
    }

    private void HandleInput()
    {
        // Handle horizontal movement
        if (Input.GetKey(rightKey))
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            if (!isFacingRight) Flip();
            ifRun = ifOnGround;
        }
        else if (Input.GetKey(leftKey))
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            if (isFacingRight) Flip();
            ifRun = ifOnGround;
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            ifRun = false;
        }

        // Handle jumping (Ö§³ÖW¼üºÍ¿Õ¸ñ¼üÌøÔ¾)
        if ((Input.GetKeyDown(jumpKey) || Input.GetKeyDown(jumpKey2)) && currentJumpCount > 0)
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        currentJumpCount--;
        animator?.SetTrigger(hashJump);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private void UpdateAnimationState()
    {
        if (animator != null)
        {
            animator.SetBool(hashIsGrounded, ifOnGround);
            animator.SetBool(hashIsRunning, ifRun);
            animator.SetFloat(hashVerticalSpeed, rb.velocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            ifOnGround = true;
            currentJumpCount = maxJumpCount;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            ifOnGround = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            ifOnGround = true;
        }
    }
}