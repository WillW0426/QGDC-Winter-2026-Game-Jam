using UnityEngine;

public class WinScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GlitchController.Instance != null) GlitchController.Instance.TriggerBurst(0.5f, 10f, 999f);
    }
}
