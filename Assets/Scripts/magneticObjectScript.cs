using UnityEngine;

public class magneticObjectScript : MonoBehaviour
{

    private Vector3 magnetPosition;
    private Rigidbody2D rb;
    private bool isBeingPulled = false;
    [SerializeField] float magneticPullStrengthY = 2;
    [SerializeField] float magneticPullStrengthX = 2;

    public GameObject magnetOrigin;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isBeingPulled)
        {
            magnetPosition = magnetOrigin.transform.position;
            Vector3 movementDirection = (magnetPosition - transform.position).normalized;
            rb.linearVelocity = new Vector2(movementDirection.x * magneticPullStrengthX, movementDirection.y * magneticPullStrengthY);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Magnet") && magnetOrigin.GetComponent<magnetTool>().isMagnetActive)
        {
            isBeingPulled = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Magnet"))
        {
            isBeingPulled = false;
        }
    }
}
