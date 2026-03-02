using UnityEngine;

public class CollisionZone : MonoBehaviour
{

    [Header("Collision Zone Config")]
    [SerializeField] int collisionThreshhold = 3;
    [SerializeField] BoxCollider2D boxCollider;
    public LayerMask excludeLayer;

    private int collisionCount = 0;

    public GameObject crammingStartPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BoxCollider2D result;
        if (collision.TryGetComponent<BoxCollider2D>(out result))
        {
            collisionCount += 1;
            result.excludeLayers = ~excludeLayer;

            if (collisionCount >= collisionThreshhold)
            {
                boxCollider.enabled = true;
            }
            Debug.Log("Collision Count: " + collisionCount);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        BoxCollider2D result;
        if (collision.TryGetComponent<BoxCollider2D>(out result))
        {
            collisionCount -= 1;
            result.excludeLayers = 0;

            if (collisionCount < collisionThreshhold)
            {
                boxCollider.enabled = false;
            }
        }
        
    }
}
