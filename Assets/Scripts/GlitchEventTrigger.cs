using UnityEngine;

public class GlitchEventTrigger : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToDestroy;

    private Collider2D triggerCollider;


    private void Start()
    {
        triggerCollider = GetComponent<Collider2D>();
        if (triggerCollider == null)
        {
            Debug.LogError("GlitchEventTrigger requires a Collider2D component.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (GlitchController.Instance != null) GlitchController.Instance.TriggerBurst(0.5f,  10f, 0.2f);

            foreach (GameObject obj in objectsToDestroy)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }

            gameObject.SetActive(false);
        }
    }
}
