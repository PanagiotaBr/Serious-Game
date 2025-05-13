using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Fall Respawn")]
    [SerializeField] private GameObject fallDetector;

    [Header("Ladder Settings")]
    [SerializeField] private LayerMask ladderLayer;
    [SerializeField] private float climbSpeed = 4f;

    [SerializeField] private Transform ladderCheck;
    [SerializeField] private float ladderCheckRadius = 0.2f;


    private Rigidbody2D rb;
    private Animator anim;
    private bool isGrounded;
    private bool isOnLadder;
    private Vector3 respawnPoint;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        respawnPoint = transform.position;
    }

    private void Update()
    {
        CheckLadder();      // <--- πρώτα η σκάλα
        Move();
        CheckJump();
        UpdateFallDetector();
        UpdateAnimations();
    }

    private void Move()
    {
        float inputX = Input.GetAxis("Horizontal");

        // Αν δεν είναι πάνω σε σκάλα, κινούμαστε φυσιολογικά
        if (!isOnLadder)
            rb.linearVelocity = new Vector2(inputX * moveSpeed, rb.linearVelocity.y);
        else
            rb.linearVelocity = new Vector2(inputX * moveSpeed, rb.linearVelocity.y);

        if (inputX > 0.01f)
            transform.localScale = Vector3.one;
        else if (inputX < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void CheckJump()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded && !isOnLadder)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private void CheckLadder()
    {
        isOnLadder = Physics2D.OverlapCircle(ladderCheck.position, ladderCheckRadius, ladderLayer);

        if (isOnLadder)
        {
            float vertical = Input.GetAxis("Vertical");
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, vertical * climbSpeed);
        }
        else
        {
            rb.gravityScale = 1f;
        }
    }


    private void UpdateFallDetector()
    {
        if (fallDetector != null)
        {
            fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.position.y);
        }
    }

    private void UpdateAnimations()
    {
        if (anim == null) return;

        anim.SetBool("run", Mathf.Abs(rb.linearVelocity.x) > 0.1f);
        anim.SetBool("grounded", isGrounded);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Respawn"))
        {
            transform.position = respawnPoint;
        }
    }
}
