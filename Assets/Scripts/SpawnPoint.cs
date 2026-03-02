using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private LevelManager levelManager;
    [SerializeField] private GameObject particlePrefab;

    public bool Activated = false;

    private void Awake()
    {
        levelManager = FindFirstObjectByType<LevelManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Activated) return;

        Activated = true;
        levelManager.activatedSpawnPoint = this;
        particlePrefab.SetActive(true);
    }

    public void ResetSpawnPoint()
    {
        Activated = false;
        particlePrefab.SetActive(false);
    }
}
