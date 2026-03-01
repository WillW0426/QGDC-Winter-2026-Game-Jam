using Unity.VisualScripting;
using UnityEngine;

public class PressurePlateScript : MonoBehaviour
{
    private Animator animator;

    [SerializeField] GameObject[] linkedDoors;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Magnet"))
        {
            animator.SetBool("IsPressed", true);
            foreach (GameObject door in linkedDoors)
            {
                door.SetActive(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Magnet"))
        {
            animator.SetBool("IsPressed", false);
            foreach (GameObject door in linkedDoors)
            {
                if (door != null) door.SetActive(true);
            }
        }
    }
}
