using UnityEngine;
using UnityEngine.InputSystem;

public class BenchScript : MonoBehaviour
{
    public GameObject exitPosition;
    [SerializeField] private bool playerNear = false;
    [SerializeField] private bool sitting = false;
    [SerializeField] private bool beingCarried = false;
    public GameObject playerObject;
    public InputActionAsset inputActions;
    private InputAction interactAction;
    private InputAction carryAction;
    public float carryOffset = 1;

    private bool isPlayerCarrying;
    private bool isPlayerJumping;

    private Transform spawnPositon;
    [SerializeField] private float bottomBound = -100f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
        interactAction = InputSystem.actions.FindAction("Player/Interact");
        carryAction = InputSystem.actions.FindAction("Player/Carry");
    }
    void Start()
    {
        spawnPositon = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (interactAction.WasPressedThisFrame() && playerNear && !beingCarried)
        {
            if (!sitting)
            {
                sit();
            }
            else
            {   
                stand();
            }
        }

        if (playerObject != null)
        {
            isPlayerCarrying = playerObject.GetComponent<PlayerController>().carrying;
            isPlayerJumping = playerObject.GetComponent<Rigidbody2D>().linearVelocity.y > 0.1f;
        }

        if (carryAction.WasPressedThisFrame() && playerNear)
        {
            if (!beingCarried && !isPlayerCarrying && !isPlayerJumping && !sitting)
            {
                beingCarried = true;
                isPlayerCarrying = true;
            }
            else
            {
                beingCarried = false;
                isPlayerCarrying = false;
                gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            }
        }

        if (beingCarried)
        {
            gameObject.transform.position = playerObject.transform.position + new Vector3(0f, 1 * carryOffset, 0f);
        }


        if (gameObject.transform.position.y <= bottomBound)
        {
            gameObject.transform.position = spawnPositon.position;
            gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
            playerObject = other.gameObject;
            Debug.Log(playerObject.GetComponent<PlayerController>().carrying);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !beingCarried)
        {
            playerNear = false;
            playerObject = null;
        }
    }
    private void sit()
    {
        playerObject.transform.position = gameObject.transform.position + new Vector3(0f, 0.5f, 0f);
        Debug.Log("Fuck");
        playerObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        Debug.Log("Fuck2");
        sitting = true;
        Debug.Log("Fuck3");
    }
    
    private void stand()
    {
        playerObject.transform.position = exitPosition.transform.position;
        Debug.Log("Fuck4");
        playerObject.GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        Debug.Log("Fuck5");
        sitting = false;
        Debug.Log("Fuck6");
    }
}
