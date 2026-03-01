using UnityEngine;
using UnityEngine.InputSystem;

public class magnetTool : MonoBehaviour
{
    public InputActionAsset inputActions;


    [SerializeField] InputAction useMagnetAction;
    [SerializeField] InputAction aimMagnetAction;
    [SerializeField] InputAction prepareMagnetAction;
    private InputAction carryAction;

    [Header("Required Game Objects")]
    [SerializeField] GameObject magnetRangeObject;
    [SerializeField] GameObject magnetModel;
    [SerializeField] private GameObject playerModelHands;
    public bool isMagnetActive = false;

    private Vector2 aimMagnetPosition;
    private PlayerController playerController;
    private Collider2D magnetCollider;
    private SpriteRenderer magnetColliderSprite;

    private void OnEnable()
    {
        inputActions.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        inputActions.FindActionMap("Player").Disable();
    }

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();

        useMagnetAction = InputSystem.actions.FindAction("Player/UseMagnet");
        aimMagnetAction = InputSystem.actions.FindAction("Player/AimMagnet");
        prepareMagnetAction = InputSystem.actions.FindAction("Player/PrepareMagnet");
        carryAction = InputSystem.actions.FindAction("Player/Carry");
        magnetModel.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //magnetCollider = magnetRangeObject.GetComponent<Collider2D>();
        //magnetColliderSprite = magnetRangeObject.GetComponent<SpriteRenderer>();
        //magnetCollider.enabled = false;

        magnetRangeObject.SetActive(false);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (prepareMagnetAction.IsPressed() && !playerController.carrying)
        {

            // Get mouse angle
            float angle = CalculateAngleToMouse();

            // Apply the rotation
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            //Show the Magnet Model
            magnetModel.SetActive(true);
            playerModelHands.SetActive(false);

            // turn on / off magnet on button press by enabling and disabling the GameObject that contains the magnet collider and sprite renderer
            if (useMagnetAction.IsPressed())
            {
                //magnetColliderSprite.enabled = false;
                //magnetCollider.enabled = false;

                magnetRangeObject.SetActive(true);

                isMagnetActive = true;
            }
            else
            {
                //magnetColliderSprite.enabled = true;
                //magnetCollider.enabled = true;

                magnetRangeObject.SetActive(false);

                isMagnetActive = false;
            }
            

        }

        // if the player releases the prepare magnet button, turn off the magnet
        else
        {
            //magnetCollider.enabled = false;
            //magnetColliderSprite.enabled = false;

            magnetRangeObject.SetActive(false);

            isMagnetActive = false;

            magnetModel.SetActive(false);
            playerModelHands.SetActive(true);
        }
    }

    private float CalculateAngleToMouse()
    {
        // Get the mouse position using the new Input System
        aimMagnetPosition = aimMagnetAction.ReadValue<Vector2>();

        // Convert the mouse position to world space
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(
            new Vector3(aimMagnetPosition.x, aimMagnetPosition.y, 0f)
        );

        // Calculate the rotation angle
        Vector2 lookDirection = mouseWorldPosition - transform.position;
        return Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
    }
}
