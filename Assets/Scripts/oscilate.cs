using UnityEngine;

public class oscilate : MonoBehaviour
{
    public float distance = 3f;   // How far it moves left/right
    public float speed = 2f;      // Movement speed

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        startPos = transform.parent.position;
        float offset = Mathf.PingPong(Time.time * speed, distance * 2f) - distance;
        transform.position = new Vector3(startPos.x + offset, startPos.y, startPos.z);
    }
}
