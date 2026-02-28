using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Transform playerModel;
    public InputActionAsset inputActions;
    private InputAction moveAction;

    private PlayerController playerController;
    private Rigidbody2D rb;

    public groundCheck groundChecker;

    private Animator animator;
    private Vector3 defaultScale;


    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();

        moveAction = InputSystem.actions.FindAction("Player/Move");
        defaultScale = playerModel.localScale;

        groundChecker = GetComponent<groundCheck>();
    }

    private void Start()
    {
        moveAction.started += MoveAction_started;
        moveAction.canceled += MoveAction_canceled;
    }

    private void Update()
    {
        animator.SetBool("IsGrounded", groundChecker.isGrounded);
        animator.SetFloat("VerticalVelocity", rb.linearVelocity.y);
    }

    private void MoveAction_canceled(InputAction.CallbackContext obj)
    {
        animator.SetBool("IsMoving", false);
    }

    private void MoveAction_started(InputAction.CallbackContext obj)
    {
        animator.SetBool("IsMoving", true);

        Vector2 moveInput = obj.ReadValue<Vector2>();

        if (moveInput.x >= 0)
        {
            playerModel.localScale = defaultScale;
        }
        else
        {
            playerModel.localScale = defaultScale.x * new Vector3(-1, 1, 1);
        }
    }
}
