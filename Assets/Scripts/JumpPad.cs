using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public GameObject direction;
    public float forceAmount = 5f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.collider.GetComponent<Rigidbody2D>();
        
        Vector2 forceDir = direction.transform.right.normalized;

            
        rb.AddForce(forceDir * forceAmount, ForceMode2D.Impulse);
        
    }
}
