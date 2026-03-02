using Unity.VectorGraphics;
using UnityEngine;

public class KillBox : MonoBehaviour
{

    [SerializeField] LevelManager levelManager;

    private void Awake()
    {
        levelManager = FindFirstObjectByType<LevelManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            levelManager.RespawnPlayer();
        }
    }
}
