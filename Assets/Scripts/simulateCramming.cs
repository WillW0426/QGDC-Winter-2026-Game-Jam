using UnityEngine;

public class simulateCramming : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public LayerMask crammableObjects;
    [SerializeField] private int cramNumber = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        cramNumber += 1;

        if(cramNumber > 1)
        {
            collision.gameObject.GetComponentInChildren<oscilate>().enabled = true;
            collision.gameObject.GetComponent<Rigidbody2D>().mass = 5;
            collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0f, 0f);
        }
    }
}
