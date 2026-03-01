using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{

    public float Health, Maxhealth, Width, Height;

    [SerializeField] private RectTransform healthBar;
    [SerializeField] private  GameObject healthBarTextObject;

    public void SetMaxHealth(float maxhealth)
    {
        Maxhealth = maxhealth;
    }

    public void SetHealth(float health)
    {
        Health = health;
        float healthPercent = (Health / Maxhealth);
        healthBar.sizeDelta = new Vector2(Width * healthPercent, Height);
        healthPercent = Mathf.Clamp(healthPercent, 0, 1);
        healthPercent = Mathf.Round(healthPercent * 100);
        healthBarTextObject.GetComponent<HealthBarTextUI>().SetHealthText(healthPercent.ToString());
    }
}
