using Unity.VisualScripting;
using UnityEngine;

public class GoalLocation : MonoBehaviour
{

    [Header("Goal Config")]
    [SerializeField] Loader.Scene NextScene;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player reached the goal!");
            // Here you can add code to handle what happens when the player reaches the goal,
            // such as loading the next level, showing a victory screen, etc.
            Loader.Load(NextScene);
        }
    }
}
