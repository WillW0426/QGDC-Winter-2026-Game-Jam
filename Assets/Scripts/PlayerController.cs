using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public InputActionAsset inputActions;
    public float downwardRaycastDistance = 1f;
    public GameObject groundCheck;
    public LayerMask groundLayerMask;

    [SerializeField] private Transform playerModel;

    private InputAction moveAction;
    private InputAction jumpAction;
    private Rigidbody2D rb;

    private Vector2 moveAmount;
    private bool isGrounded;

    [Header("Player Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    private Animator animator;
    private Vector3 defaultScale;

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


        animator = GetComponentInChildren<Animator>();
        defaultScale = playerModel.localScale;

        rb = GetComponent<Rigidbody2D>();

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        jumpAction.performed += JumpAction_performed;
        moveAction.performed += MoveAction_started;
        moveAction.canceled += MoveAction_canceled;
    }

    private void MoveAction_canceled(InputAction.CallbackContext obj)
    {
        animator.SetBool("IsMoving", false);
    }

    private void MoveAction_started(InputAction.CallbackContext obj)
    {
        animator.SetBool("IsMoving", true);
        Debug.Log(moveAmount.x);

        Vector2 moveInput = obj.ReadValue<Vector2>();

        if (moveInput.x >= 0)
        {
            playerModel.localScale = defaultScale;
        } else
        {
            playerModel.localScale = defaultScale.x *  new Vector3(-1, 1, 1);
        }
    }

    private void JumpAction_performed(InputAction.CallbackContext obj)
    {
        if (groundCheck.GetComponent<groundCheck>().isGrounded == true)
        {
            Jump();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // get movement inputs
        moveAmount = moveAction.ReadValue<Vector2>();
    }
    

    // apply walking movement
    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveAmount.x * moveSpeed, rb.linearVelocity.y);
    }
    
    public void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    
}
