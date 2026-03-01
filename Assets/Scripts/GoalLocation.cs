using Unity.VisualScripting;
using UnityEngine;

public class GoalLocation : MonoBehaviour
{

    [Header("Goal Config")]
    [SerializeField] Loader.Scene NextScene;
    [SerializeField] float maxHealth;
    [SerializeField] HealthBarUI healthBar;

    private bool isBeingMagnetized = false;
    private float health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthBar.SetMaxHealth(maxHealth);
        health = maxHealth;
    }

    void FixedUpdate()
    {
        if (isBeingMagnetized)
        {
            setHealth(-20f);
            GlitchController.Instance.TriggerBurst(0.4f, 5f, 0.1f);
            Debug.Log("Health: " + health);
            if (health <= 0)
            {
                Loader.Load(NextScene);
            }
        }
    }

    private void setHealth(float healthChange)
    {
        health += healthChange * Time.deltaTime;
        health = Mathf.Clamp(health, 0, maxHealth);
        healthBar.SetHealth(health);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Magnet") && collision.GetComponentInParent<magnetTool>().isMagnetActive)
        {
            isBeingMagnetized = true;
            
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Magnet"))
        {
            isBeingMagnetized = false;
        }
    }
}
