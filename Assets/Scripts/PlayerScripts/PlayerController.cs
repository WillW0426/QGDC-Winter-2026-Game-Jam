using UnityEngine;

//Requirements
// - Rigidbody2D


public class PlayerController : MonoBehaviour
{
    [Header("Set-Up")]
    [SerializeField] GameInput gameInput;
    [SerializeField] Transform groundCheckPoint;

    [Header("Player Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;

    private Rigidbody2D rb;
    private Vector2 movementInput;
    private LayerMask groundLayer;

    public bool isGrounded = true;


    //Unity Methods

    private void Awake()
    {
        groundLayer = LayerMask.GetMask("Ground");

        if (gameInput == null)
        {
            gameInput = FindFirstObjectByType<GameInput>();
        }

        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        gameInput.OnPlayerJump += GameInput_OnPlayerJump;
    }

    private void Update()
    {
        movementInput = gameInput.GetMovementNormalized();

        if (Physics2D.OverlapCircle((Vector2)groundCheckPoint.position, 0.1f, groundLayer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocityX = movementInput.x * moveSpeed;
    }

    //Event Handlers

    private void GameInput_OnPlayerJump(object sender, System.EventArgs e)
    {
        if (!isGrounded) return;

        isGrounded = false;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}

