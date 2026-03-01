using UnityEngine;

public class groundCheck : MonoBehaviour
{
    public bool isGrounded;
    public float downwardRaycastDistance = 1f;
    public LayerMask groundLayerMask;
    public LayerMask crammableObjects;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        groundLayerMask = LayerMask.GetMask("Ground");
        crammableObjects = LayerMask.GetMask("Crammable");
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is grounded using this collider
        if (Physics2D.OverlapCircle(transform.position, 0.1f, crammableObjects) || Physics2D.OverlapCircle(transform.position, 0.1f, groundLayerMask))
        {
            isGrounded = true;
        } else
        {
            isGrounded = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.1f);
    }

}
