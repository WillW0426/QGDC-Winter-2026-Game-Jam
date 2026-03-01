using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputActionAsset inputActions;
    public float downwardRaycastDistance = 1f;
    public GameObject groundCheck;
    public LayerMask groundLayerMask;

    [SerializeField] private Transform playerModel;
    public bool carrying = false;

    private InputAction moveAction;
    private InputAction jumpAction;
    private Rigidbody2D rb;

    private Vector2 moveAmount;

    [Header("Player Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    [SerializeField] private float riseGravity = 5f;
    [SerializeField] private float fallGravity = 3.5f;
    [SerializeField] private float shortHopGravity = 8f;

    private bool isJumpHeld;

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
        //jumpAction.performed += JumpAction_performed;
        jumpAction.started += JumpAction_started;
        jumpAction.canceled += JumpAction_canceled;
    }

    private void JumpAction_canceled(InputAction.CallbackContext obj)
    {
        isJumpHeld = false;
    }

    private void JumpAction_started(InputAction.CallbackContext obj)
    {
        isJumpHeld = true;

        if (groundCheck.GetComponent<groundCheck>().isGrounded && !carrying)
        {
            Jump();
        }
    }

    //private void JumpAction_performed(InputAction.CallbackContext obj)
    //{


    //    //if (groundCheck.GetComponent<groundCheck>().isGrounded == true && !carrying)
    //    //{
    //    //    Jump();
    //    //}
    //}

    // Update is called once per frame
    void Update()
    {
        // get movement inputs
        moveAmount = moveAction.ReadValue<Vector2>();
        
    }
    

    // apply walking movement
    private void FixedUpdate()
    {
        HandlePlayerJumpGravity();

        //rb.linearVelocity = new Vector2(moveAmount.x * moveSpeed, rb.linearVelocity.y);
        float targetSpeed = moveAmount.x * moveSpeed;
        float acceleration = 20f;

        float newX = Mathf.MoveTowards(
            rb.linearVelocity.x,
            targetSpeed,
            acceleration * Time.deltaTime
        );

        rb.linearVelocity = new Vector2(newX, rb.linearVelocity.y);
    }

    private void OnDestroy()
    {
        jumpAction.started -= JumpAction_started;
        jumpAction.canceled -= JumpAction_canceled;
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        GetComponent<PlayerSFX>()?.PlayJumpSFX();
    }

    private void HandlePlayerJumpGravity()
    {
        if (rb.linearVelocity.y > 0)
        {
            rb.gravityScale = isJumpHeld ? riseGravity : shortHopGravity;
        }
        else if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = fallGravity;
        }
        else
        {
            rb.gravityScale = 1f; // Normal gravity
        }

        if (groundCheck.GetComponent<groundCheck>().isGrounded && rb.linearVelocity.y < 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        }
    }


}
