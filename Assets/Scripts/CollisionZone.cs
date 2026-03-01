using UnityEngine;

public class CollisionZone : MonoBehaviour
{

    [Header("Collision Zone Config")]
    [SerializeField] int collisionThreshhold = 3;
    [SerializeField] BoxCollider2D boxCollider;
    public LayerMask excludeLayer;

    private int collisionCount = 0;

    public GameObject crammingStartPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collisionCount += 1;
        collision.GetComponent<BoxCollider2D>().excludeLayers = ~excludeLayer;
        if (collisionCount >= collisionThreshhold)
        {
            boxCollider.enabled = true;
        }
        Debug.Log("Collision Count: " + collisionCount);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collisionCount -= 1;
        collision.GetComponent<BoxCollider2D>().excludeLayers = 0;
        if (collisionCount < collisionThreshhold)
        {
            boxCollider.enabled = false;
        }
    }
}
