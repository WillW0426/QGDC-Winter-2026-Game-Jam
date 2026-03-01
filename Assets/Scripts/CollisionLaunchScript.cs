using UnityEngine;

public class CollisionLaunchScript : MonoBehaviour
{
    [Header("Collision Launch Config")]
    [SerializeField] GameObject launchDirectionObject;
    [SerializeField] float launchForce = 10f;
    [SerializeField] GameObject[] launchableObjects;

    private Vector3 launchAngle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        launchAngle = launchDirectionObject.transform.right.normalized;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Collision Detected with: " + collision.gameObject.name);
        //Debug.Log("launchable: " + IsLaunchableObject(collision.gameObject));
        if (IsLaunchableObject(collision.gameObject))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(launchAngle * launchForce, ForceMode2D.Impulse);
            }
        }
    }

    private bool IsLaunchableObject(GameObject obj)
    {
        foreach (GameObject launchable in launchableObjects)
        {
            if (launchable == obj)
            {
                return true;
            }
        }
        return false;
    }


}
