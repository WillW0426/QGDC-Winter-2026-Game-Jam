using Unity.VisualScripting;
using UnityEngine;

public class PressurePlateScript : MonoBehaviour
{

    [SerializeField] GameObject[] linkedDoors;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Magnet"))
        {
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
            foreach (GameObject door in linkedDoors)
            {
                door.SetActive(true);
            }
        }
    }
}
