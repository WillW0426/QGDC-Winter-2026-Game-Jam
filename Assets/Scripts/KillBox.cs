using Unity.VectorGraphics;
using UnityEngine;

public class KillBox : MonoBehaviour
{

    [SerializeField] Loader.Scene respawnScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Loader.Load(respawnScene);
        }
    }
}
