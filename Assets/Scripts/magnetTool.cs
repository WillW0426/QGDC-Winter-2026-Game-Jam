using UnityEngine;
using UnityEngine.InputSystem;

public class magnetTool : MonoBehaviour
{
    public InputActionAsset inputActions;

    [SerializeField] InputAction useMagnetAction;
    [SerializeField] InputAction aimMagnetAction;
    [SerializeField] InputAction prepareMagnetAction;
    [SerializeField] GameObject magnetRangeObject;

    private Vector2 aimMagnetPosition;
    private Collider2D magnetCollider;
    private bool isMagnetActive = false;

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
        useMagnetAction = InputSystem.actions.FindAction("Player/UseMagnet");
        aimMagnetAction = InputSystem.actions.FindAction("Player/AimMagnet");
        prepareMagnetAction = InputSystem.actions.FindAction("Player/PrepareMagnet");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        magnetCollider = magnetRangeObject.GetComponent<Collider2D>();
        magnetCollider.enabled = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (prepareMagnetAction.IsPressed())
        {

            // Get mouse angle
            float angle = CalculateAngleToMouse();

            // Apply the rotation
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // turn on / off magnet on button press by enabling and disabling the collider
            if (useMagnetAction.WasPressedThisFrame())
            {
                if (isMagnetActive)
                {
                    magnetCollider.enabled = false;
                    isMagnetActive = false;
                }
                else
                {
                    magnetCollider.enabled = true;
                    isMagnetActive = true;
                }
            }
            
        }

        // if the player releases the prepare magnet button, turn off the magnet
        else
        {
            magnetCollider.enabled = false;
            isMagnetActive = false;
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
