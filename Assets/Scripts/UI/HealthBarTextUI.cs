using TMPro;
using UnityEngine;

public class HealthBarTextUI : MonoBehaviour
{

    public TMP_Text healthPercent;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthPercent.SetText("100%");
    }

    public void SetHealthText(string text)
    {
        healthPercent.SetText(text + "%");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
