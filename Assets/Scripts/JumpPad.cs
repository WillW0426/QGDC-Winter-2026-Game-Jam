using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public GameObject direction;
    public float forceAmount = 5f;

    private AudioSource audioSource;

    [SerializeField] AudioClip audioClip;

    private void Start()
    {
        audioSource = GetComponentInParent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.collider.GetComponent<Rigidbody2D>();
        
        Vector2 forceDir = direction.transform.right.normalized;

        audioSource.PlayOneShot(audioClip);
        //rb.AddForce(forceDir * forceAmount, ForceMode2D.Impulse);

        rb.linearVelocityY = forceAmount;
        
    }
}
