using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BenchSFX : MonoBehaviour
{
    // ─────────────────────────────────────────────
    //  Inspector: Audio Clips
    // ─────────────────────────────────────────────
    [Header("Master Volume")]
    [Range(0f, 1f)][SerializeField] private float masterVolume = 1f;

    [Header("Sit SFX")]
    [SerializeField] private AudioClip sitClip;
    [SerializeField] private bool sitPitchRandomize = true;
    [SerializeField] private Vector2 sitPitchRange = new Vector2(0.92f, 1.08f);
    [Range(0f, 1f)][SerializeField] private float sitVolume = 1f;

    [Header("Stand SFX")]
    [SerializeField] private AudioClip standClip;
    [SerializeField] private bool standPitchRandomize = true;
    [SerializeField] private Vector2 standPitchRange = new Vector2(0.92f, 1.08f);
    [Range(0f, 1f)][SerializeField] private float standVolume = 1f;

    [Header("Pickup SFX")]
    [SerializeField] private AudioClip pickupClip;
    [SerializeField] private bool pickupPitchRandomize = true;
    [SerializeField] private Vector2 pickupPitchRange = new Vector2(0.9f, 1.1f);
    [Range(0f, 1f)][SerializeField] private float pickupVolume = 1f;

    [Header("Put Down SFX")]
    [SerializeField] private AudioClip putDownClip;
    [SerializeField] private bool putDownPitchRandomize = true;
    [SerializeField] private Vector2 putDownPitchRange = new Vector2(0.88f, 1.05f);
    [Range(0f, 1f)][SerializeField] private float putDownVolume = 1f;

    [Header("Carry Loop SFX")]
    [SerializeField] private AudioClip carryLoopClip;           // optional ambient creak while carried
    [SerializeField] private bool carryLoopPitchRandomize = false;
    [SerializeField] private Vector2 carryLoopPitchRange = new Vector2(0.95f, 1.05f);
    [Range(0f, 1f)][SerializeField] private float carryLoopVolume = 1f;

    // ─────────────────────────────────────────────
    //  Private references / state
    // ─────────────────────────────────────────────
    private AudioSource sfxSource;
    private AudioSource carryLoopSource;

    private BenchScript benchScript;

    // Shadow the BenchScript private fields by tracking last-known state
    private bool wasSitting = false;
    private bool wasBeingCarried = false;

    // ─────────────────────────────────────────────
    //  Unity Lifecycle
    // ─────────────────────────────────────────────
    private void Awake()
    {
        sfxSource = GetComponent<AudioSource>();
        sfxSource.playOnAwake = false;
        sfxSource.loop = false;

        // Dedicated source for the optional carry loop
        carryLoopSource = gameObject.AddComponent<AudioSource>();
        carryLoopSource.playOnAwake = false;
        carryLoopSource.loop = true;
        carryLoopSource.clip = carryLoopClip;

        benchScript = GetComponent<BenchScript>();
        if (benchScript == null)
            Debug.LogWarning("[BenchSFX] No BenchScript found on this GameObject.");
    }

    private void Update()
    {
        if (benchScript == null) return;

        // Read current state via the public-facing serialized fields.
        // Because BenchScript marks them [SerializeField] they are readable
        // here through the component reference via reflection-free property access.
        // We use a small helper to extract private field values safely.
        bool currentlySitting = GetBenchBool("sitting");
        bool currentlyCarried = GetBenchBool("beingCarried");

        // ── Sit / Stand detection ────────────────
        if (currentlySitting && !wasSitting)
            PlayOneShot(sitClip, sitPitchRandomize, sitPitchRange, sitVolume);

        if (!currentlySitting && wasSitting)
            PlayOneShot(standClip, standPitchRandomize, standPitchRange, standVolume);

        // ── Pickup / Put-down detection ──────────
        if (currentlyCarried && !wasBeingCarried)
        {
            PlayOneShot(pickupClip, pickupPitchRandomize, pickupPitchRange, pickupVolume);
            StartCarryLoop();
        }

        if (!currentlyCarried && wasBeingCarried)
        {
            StopCarryLoop();
            PlayOneShot(putDownClip, putDownPitchRandomize, putDownPitchRange, putDownVolume);
        }

        // Store for next frame
        wasSitting = currentlySitting;
        wasBeingCarried = currentlyCarried;
    }

    // ─────────────────────────────────────────────
    //  Public trigger methods
    //  Call these directly from BenchScript if you
    //  prefer explicit calls over state-polling.
    // ─────────────────────────────────────────────

    /// <summary>Trigger the sit sound manually from BenchScript.</summary>
    public void PlaySitSFX() => PlayOneShot(sitClip, sitPitchRandomize, sitPitchRange, sitVolume);

    /// <summary>Trigger the stand sound manually from BenchScript.</summary>
    public void PlayStandSFX() => PlayOneShot(standClip, standPitchRandomize, standPitchRange, standVolume);

    /// <summary>Trigger the pickup sound manually from BenchScript.</summary>
    public void PlayPickupSFX()
    {
        PlayOneShot(pickupClip, pickupPitchRandomize, pickupPitchRange, pickupVolume);
        StartCarryLoop();
    }

    /// <summary>Trigger the put-down sound manually from BenchScript.</summary>
    public void PlayPutDownSFX()
    {
        StopCarryLoop();
        PlayOneShot(putDownClip, putDownPitchRandomize, putDownPitchRange, putDownVolume);
    }

    // ─────────────────────────────────────────────
    //  Private helpers
    // ─────────────────────────────────────────────
    private void PlayOneShot(AudioClip clip, bool randomizePitch, Vector2 pitchRange, float volume = 1f)
    {
        if (clip == null) return;

        sfxSource.pitch = randomizePitch
            ? Random.Range(pitchRange.x, pitchRange.y)
            : 1f;

        sfxSource.PlayOneShot(clip, masterVolume * volume);
    }

    private void StartCarryLoop()
    {
        if (carryLoopClip == null) return;

        carryLoopSource.pitch = carryLoopPitchRandomize
            ? Random.Range(carryLoopPitchRange.x, carryLoopPitchRange.y)
            : 1f;

        carryLoopSource.volume = masterVolume * carryLoopVolume;

        if (!carryLoopSource.isPlaying)
            carryLoopSource.Play();
    }

    private void StopCarryLoop()
    {
        if (carryLoopSource.isPlaying)
            carryLoopSource.Stop();
    }

    /// <summary>
    /// Reads a private bool field from BenchScript via reflection.
    /// This avoids modifying BenchScript while keeping state-polling clean.
    /// </summary>
    private bool GetBenchBool(string fieldName)
    {
        var field = typeof(BenchScript).GetField(
            fieldName,
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
        );
        return field != null && (bool)field.GetValue(benchScript);
    }
}