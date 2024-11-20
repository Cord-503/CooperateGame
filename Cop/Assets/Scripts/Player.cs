using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Player1Controller : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public int maxJumpCount = 2;

    [Header("Input Keys")]
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode jumpKey = KeyCode.W;
    public KeyCode jumpKey2 = KeyCode.Space;
    public KeyCode grabKey = KeyCode.Q;

    [Header("Grab and Throw Settings")]
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private float throwAngle = 45f;
    [SerializeField] private Transform holdPosition;

    [SerializeField] private GameManager gameManager;

    private Rigidbody2D rb;
    private Animator animator;
    private bool ifOnGround;
    private bool ifRun;
    private bool isFacingRight = true;
    private int currentJumpCount;

    private GameObject grabbedPlayer;
    private bool isGrabbing = false;
    private bool canGrab = false;
    private bool isPlayer1Locked = false;

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

        if (Input.GetKeyUp(grabKey) && isGrabbing)
        {
            ThrowPlayer();
        }
    }

    private void HandleInput()
    {
        if (isPlayer1Locked)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }

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

        if ((Input.GetKeyDown(jumpKey) || Input.GetKeyDown(jumpKey2)) && currentJumpCount > 0)
        {
            Jump();
        }

        if (Input.GetKeyDown(grabKey) && canGrab)
        {
            GrabPlayer();
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

        if (collision.gameObject.CompareTag("Player2"))
        {
            canGrab = true;
            grabbedPlayer = collision.gameObject;
        }

        if (collision.gameObject.CompareTag("Pumpkin"))
        {
            gameManager.AddPlayer1Score(1);

            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            ifOnGround = false;
        }

        if (collision.gameObject.CompareTag("Player2"))
        {
            canGrab = false;
            grabbedPlayer = null;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            ifOnGround = true;
        }
    }

    private void GrabPlayer()
    {
        if (grabbedPlayer != null)
        {
            isGrabbing = true;

            Rigidbody2D player2Rb = grabbedPlayer.GetComponent<Rigidbody2D>();
            player2Rb.velocity = Vector2.zero;
            player2Rb.angularVelocity = 0f; 

            grabbedPlayer.GetComponent<Rigidbody2D>().isKinematic = true;
            grabbedPlayer.GetComponent<Player2Controller>().enabled = false;
            grabbedPlayer.transform.position = holdPosition.position;

            isPlayer1Locked = true;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }


    private void ThrowPlayer()
    {
        if (grabbedPlayer != null)
        {
            isGrabbing = false;

            Rigidbody2D rb = grabbedPlayer.GetComponent<Rigidbody2D>();
            rb.isKinematic = false;

            float radAngle = throwAngle * Mathf.Deg2Rad;
            Vector2 throwVelocity = new Vector2(
                throwForce * Mathf.Cos(radAngle),
                throwForce * Mathf.Sin(radAngle)
            );
            rb.velocity = isFacingRight ? throwVelocity : new Vector2(-throwVelocity.x, throwVelocity.y);

            grabbedPlayer.GetComponent<Player2Controller>().enabled = true;
            grabbedPlayer = null;

            isPlayer1Locked = false;
        }
    }
}
