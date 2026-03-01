using UnityEngine;
using UnityEngine.WSA;

public class CollisionZone : MonoBehaviour
{

    [Header("Collision Zone Config")]
    [SerializeField] int collisionThreshhold = 3;
    [SerializeField] BoxCollider2D boxCollider;

    private int collisionCount = 0;

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
        if (collisionCount >= collisionThreshhold)
        {
            boxCollider.enabled = true;
        }
        Debug.Log("Collision Count: " + collisionCount);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collisionCount -= 1;
        if (collisionCount < collisionThreshhold)
        {
            boxCollider.enabled = false;
        }
    }
}
