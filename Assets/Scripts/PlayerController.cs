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
        jumpAction.performed += JumpAction_performed;
    }

    private void JumpAction_performed(InputAction.CallbackContext obj)
    {
        if (groundCheck.GetComponent<groundCheck>().isGrounded == true && !carrying)
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
        jumpAction.performed -= JumpAction_performed;
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    
}
