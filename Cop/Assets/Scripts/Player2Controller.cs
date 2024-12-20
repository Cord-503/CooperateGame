using UnityEngine;

public class Player2Controller : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public int maxJumpCount = 2;

    [Header("Input Keys")]
    public KeyCode leftKey = KeyCode.LeftArrow;
    public KeyCode rightKey = KeyCode.RightArrow;
    public KeyCode jumpKey = KeyCode.UpArrow;


    [SerializeField] private GameManager gameManager;

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

        if (Input.GetKeyDown(jumpKey) && currentJumpCount > 0)
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

        if (collision.gameObject.CompareTag("Apple"))
        {
            gameManager.AddPlayer2Score(1);

            Destroy(collision.gameObject);
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