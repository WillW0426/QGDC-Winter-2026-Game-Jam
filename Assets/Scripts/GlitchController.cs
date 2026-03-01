using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class GlitchController : MonoBehaviour
{
    public static GlitchController Instance { get; private set; }

    [SerializeField] Material glitchMaterial;
    [SerializeField] Renderer2DData rendererData; // drag your 2D Renderer Data asset here

    static readonly int IntensityID = Shader.PropertyToID("_GlitchIntensity");
    static readonly int SpeedID = Shader.PropertyToID("_GlitchSpeed");

    FullScreenPassRendererFeature glitchFeature;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Find the feature by name
        foreach (var feature in rendererData.rendererFeatures)
        {
            if (feature.name == "Glitch")
            {
                glitchFeature = feature as FullScreenPassRendererFeature;
                break;
            }
        }

        SetGlitch(0f);
        glitchFeature.SetActive(false); // start disabled
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public void SetGlitch(float intensity, float speed = 5f)
    {
        glitchMaterial.SetFloat(IntensityID, intensity);
        glitchMaterial.SetFloat(SpeedID, speed);
    }

    public void TriggerBurst(float intensity = 0.9f, float speed = 12f, float duration = 0.5f)
    {
        StartCoroutine(GlitchBurst(intensity, speed, duration));
    }

    IEnumerator GlitchBurst(float intensity, float speed, float duration)
    {
        glitchFeature.SetActive(true);
        SetGlitch(intensity, speed);
        yield return new WaitForSeconds(duration);
        SetGlitch(0f);
        glitchFeature.SetActive(false);
    }
}