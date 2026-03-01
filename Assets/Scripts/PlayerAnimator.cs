using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimator : MonoBehaviour
{
    [Header("Set Up")]
    [SerializeField] private Transform playerModel;
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private groundCheck groundChecker;

    [Header("Animatiors")]
    [SerializeField] private Animator bodyAnimator;
    [SerializeField] private Animator handAnimator;

    private InputAction moveAction;
    private Rigidbody2D rb;
    private Vector3 defaultScale;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        moveAction = InputSystem.actions.FindAction("Player/Move");
        defaultScale = playerModel.localScale;

        groundChecker = GetComponentInChildren<groundCheck>();
    }

    private void Start()
    {
        moveAction.started += MoveAction_started;
        moveAction.canceled += MoveAction_canceled;
    }

    private void Update()
    {
        bodyAnimator.SetBool("Grounded", groundChecker.isGrounded);
        handAnimator.SetBool("Grounded", groundChecker.isGrounded);

        bodyAnimator.SetFloat("YVelocity", rb.linearVelocity.y);
    }

    private void MoveAction_canceled(InputAction.CallbackContext obj)
    {
        handAnimator.SetBool("IsMoving", false);
        bodyAnimator.SetBool("IsMoving", false);
    }

    private void MoveAction_started(InputAction.CallbackContext obj)
    {
        handAnimator.SetBool("IsMoving", true);
        bodyAnimator.SetBool("IsMoving", true);

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

    //When the Scene is unloaded or the GameObject is destroyed, we need to unsubscribe from the events to prevent memory leaks and potential null reference exceptions.
    private void OnDestroy()
    {
        moveAction.started -= MoveAction_started;
        moveAction.canceled -= MoveAction_canceled;
    }
}
