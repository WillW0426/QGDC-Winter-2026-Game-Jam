using UnityEngine;

public class MagnetOscilate : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject startPoint;
    public GameObject endPoint;
    [SerializeField] private bool magnetized = false;
    public float speed = 5f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Transform target = magnetized ? endPoint.transform : startPoint.transform;

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Magnet"))
        {
            magnetized = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Magnet"))
        {
            magnetized = false;
        }
    }
}
