using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public InputActionAsset inputActions;
    public float downwardRaycastDistance = 1f;
    public GameObject groundCheck;
    public LayerMask groundLayerMask;

    private InputAction moveAction;
    private InputAction jumpAction;
    private Rigidbody2D rb;

    private Vector2 moveAmount;
    private bool isGrounded;

    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    // Enable action map
    private void OnEnable()
    {
        inputActions.FindActionMap("Player").Enable();
    }

    // Disable action map on player destruction
    private void OnDisable()
    {
        inputActions.FindActionMap("Player").Disable();
    }

    private void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Player/Move");
        jumpAction = InputSystem.actions.FindAction("Player/Jump");

        rb = GetComponent<Rigidbody2D>();

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // get movement inputs
        moveAmount = moveAction.ReadValue<Vector2>();
        if (jumpAction.WasPressedThisFrame() && groundCheck.GetComponent<groundCheck>().isGrounded == true)
        {
            Jump();
        }
    }
    /*
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            RaycastHit2D rcHit = Physics2D.BoxCast(transform.position, new Vector2(1f, 1f), 0f, Vector2.down, downwardRaycastDistance, groundLayerMask);
            //Debug.DrawRay(transform.position, Vector2.down, Color.blue, downwardRaycastDistance);
            if (rcHit.collider != null)
            {
                isGrounded = true;
            }

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.DrawRay(transform.position, Vector2.down, Color.blue, downwardRaycastDistance);
        isGrounded = false;
    }
    */
    // apply walking movement
    private void FixedUpdate()
    {
        //transform.Translate(new Vector2(moveAmount.x, 0) * moveSpeed * Time.deltaTime);
        rb.linearVelocity = new Vector2(moveAmount.x * moveSpeed, rb.linearVelocity.y);
    }
    
    public void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}
