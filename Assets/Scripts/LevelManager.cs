using UnityEngine;

public class LevelManager : MonoBehaviour
{

    [SerializeField] SpawnPoint[] spawnPoints;
    public SpawnPoint activatedSpawnPoint;

    [SerializeField] GameObject[] MagneticObjects;
    private Vector3[] magneticObjectSpawnPositions;

    [SerializeField] private GameObject[] benchObjects;
    private Vector3[] benchObjectSpawnPositions;

    [SerializeField] GlitchEventTrigger[] glitchEventTriggers;
    [SerializeField] GameObject PlayerPrefab;

    private Vector3 playerRestartLocation;

    private void Awake()
    {
        if (PlayerPrefab == null) PlayerPrefab = GameObject.FindGameObjectWithTag("Player");

        playerRestartLocation = PlayerPrefab.transform.position;

        magneticObjectSpawnPositions = new Vector3[MagneticObjects.Length];

        benchObjectSpawnPositions = new Vector3[benchObjects.Length];

        for (int i = 0; i < MagneticObjects.Length; i++)
        {
            magneticObjectSpawnPositions[i] = MagneticObjects[i].transform.position;
        }

        for (int i = 0; i < benchObjects.Length; i++)
        {
            benchObjectSpawnPositions[i] = benchObjects[i].transform.position;
        }

    }

    public void RespawnPlayer()
    {
        PlayerPrefab.transform.position = activatedSpawnPoint.transform.position;
        PlayerPrefab.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
    }

    public void Restart()
    {
        PlayerPrefab.transform.position = playerRestartLocation;

        for (int i = 0; i < MagneticObjects.Length; i++)
        {
            MagneticObjects[i].GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            MagneticObjects[i].GetComponent<Rigidbody2D>().angularVelocity = 0f;

            MagneticObjects[i].transform.rotation = Quaternion.identity;
            MagneticObjects[i].transform.position = magneticObjectSpawnPositions[i];
        }

        for (int i = 0; i < benchObjects.Length; i++)
        {
            benchObjects[i].GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            benchObjects[i].GetComponent<Rigidbody2D>().angularVelocity = 0f;

            benchObjects[i].transform.rotation = Quaternion.identity;
            benchObjects[i].transform.position = benchObjectSpawnPositions[i];
        }

        foreach (GlitchEventTrigger glitchController in glitchEventTriggers)
        {
            glitchController.ResetTrigger();
        }

        foreach (SpawnPoint spawn in spawnPoints)
        {
            spawn.ResetSpawnPoint();
        }

    }
}
