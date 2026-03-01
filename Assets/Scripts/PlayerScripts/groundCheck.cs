using UnityEngine;

public class groundCheck : MonoBehaviour
{
    public bool isGrounded;
    public float downwardRaycastDistance = 1f;
    public LayerMask groundLayerMask;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, 0.1f);
        // Check if the player is grounded using this collider
        if (hit != null && (hit.gameObject.CompareTag("Ground") || Physics2D.OverlapCircle(transform.position, 0.1f, groundLayerMask)))
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
