using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioSource))]
public class PlayerSFX : MonoBehaviour
{
    // ─────────────────────────────────────────────
    //  Inspector: Audio Clips
    // ─────────────────────────────────────────────
    [Header("Master Volume")]
    [Range(0f, 1f)][SerializeField] private float masterVolume = 1f;

    [Header("Walking SFX")]
    [SerializeField] private AudioClip walkingClip;
    [SerializeField] private bool walkPitchRandomize = true;
    [SerializeField] private Vector2 walkPitchRange = new Vector2(0.9f, 1.1f);
    [SerializeField] private float walkStepInterval = 0.35f;   // seconds between footstep sounds
    [Range(0f, 1f)][SerializeField] private float walkVolume = 1f;

    [Header("Jumping SFX")]
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private bool jumpPitchRandomize = true;
    [SerializeField] private Vector2 jumpPitchRange = new Vector2(0.95f, 1.05f);
    [Range(0f, 1f)][SerializeField] private float jumpVolume = 1f;

    [Header("Landing SFX")]
    [SerializeField] private AudioClip landClip;
    [SerializeField] private bool landPitchRandomize = true;
    [SerializeField] private Vector2 landPitchRange = new Vector2(0.9f, 1.1f);
    [Range(0f, 1f)][SerializeField] private float landVolume = 1f;

    [Header("Magnet SFX")]
    [SerializeField] private AudioClip magnetActivateClip;    // plays once when magnet turns on
    [SerializeField] private AudioClip magnetLoopClip;        // loops while magnet is active
    [SerializeField] private AudioClip magnetDeactivateClip;  // plays once when magnet turns off
    [SerializeField] private bool magnetPitchRandomize = false;
    [SerializeField] private Vector2 magnetPitchRange = new Vector2(0.95f, 1.05f);
    [Range(0f, 1f)][SerializeField] private float magnetVolume = 1f;

    // ─────────────────────────────────────────────
    //  Private references / state
    // ─────────────────────────────────────────────
    private AudioSource sfxSource;         // one-shot sounds (walk, jump, land)
    private AudioSource magnetLoopSource;  // dedicated looping source for the magnet hum

    private PlayerController playerController;
    private magnetTool magnetToolComponent;
    private groundCheck groundCheckComponent;

    private bool wasGrounded = true;
    private bool wasMagnetActive = false;
    private float walkStepTimer = 0f;
    private Rigidbody2D rb;

    // ─────────────────────────────────────────────
    //  Unity Lifecycle
    // ─────────────────────────────────────────────
    private void Awake()
    {
        // Grab / create the main AudioSource (one-shots)
        sfxSource = GetComponent<AudioSource>();
        sfxSource.playOnAwake = false;
        sfxSource.loop = false;

        // Create a second AudioSource dedicated to the magnet loop
        magnetLoopSource = gameObject.AddComponent<AudioSource>();
        magnetLoopSource.playOnAwake = false;
        magnetLoopSource.loop = true;
        magnetLoopSource.clip = magnetLoopClip;

        // Find sibling / parent components
        playerController = GetComponent<PlayerController>();
        if (playerController == null)
            playerController = GetComponentInParent<PlayerController>();

        magnetToolComponent = GetComponentInChildren<magnetTool>();
        if (magnetToolComponent == null)
            magnetToolComponent = GetComponentInParent<magnetTool>();

        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            rb = GetComponentInParent<Rigidbody2D>();
    }

    private void Start()
    {
        // Resolve groundCheck so we can track landing events
        if (playerController != null && playerController.groundCheck != null)
            groundCheckComponent = playerController.groundCheck.GetComponent<groundCheck>();

        wasGrounded = IsGrounded();
    }

    private void Update()
    {
        bool grounded = IsGrounded();

        HandleWalkingSFX(grounded);
        HandleLandingSFX(grounded);
        HandleMagnetSFX();

        wasGrounded = grounded;
    }

    // ─────────────────────────────────────────────
    //  Public method — called by PlayerController
    //  when the jump is actually executed.
    // ─────────────────────────────────────────────
    public void PlayJumpSFX()
    {
        PlayOneShot(jumpClip, jumpPitchRandomize, jumpPitchRange, jumpVolume);
    }

    // ─────────────────────────────────────────────
    //  Private helpers
    // ─────────────────────────────────────────────
    private void HandleWalkingSFX(bool grounded)
    {
        bool isMoving = rb != null && Mathf.Abs(rb.linearVelocity.x) > 0.1f;

        if (grounded && isMoving && walkingClip != null)
        {
            walkStepTimer -= Time.deltaTime;
            if (walkStepTimer <= 0f)
            {
                PlayOneShot(walkingClip, walkPitchRandomize, walkPitchRange, walkVolume);
                walkStepTimer = walkStepInterval;
            }
        }
        else
        {
            // Reset timer so the first step plays immediately when walking resumes
            walkStepTimer = 0f;
        }
    }

    private void HandleLandingSFX(bool grounded)
    {
        // Detect the frame we touch the ground after being airborne
        if (!wasGrounded && grounded)
        {
            PlayOneShot(landClip, landPitchRandomize, landPitchRange, landVolume);
        }
    }

    private void HandleMagnetSFX()
    {
        if (magnetToolComponent == null) return;

        bool magnetActive = magnetToolComponent.isMagnetActive;

        // Magnet just turned ON
        if (magnetActive && !wasMagnetActive)
        {
            PlayOneShot(magnetActivateClip, magnetPitchRandomize, magnetPitchRange, magnetVolume);

            if (magnetLoopClip != null)
            {
                magnetLoopSource.pitch = magnetPitchRandomize
                    ? Random.Range(magnetPitchRange.x, magnetPitchRange.y)
                    : 1f;
                magnetLoopSource.volume = masterVolume * magnetVolume;
                magnetLoopSource.Play();
            }
        }

        // Magnet just turned OFF
        if (!magnetActive && wasMagnetActive)
        {
            magnetLoopSource.Stop();
            PlayOneShot(magnetDeactivateClip, magnetPitchRandomize, magnetPitchRange, magnetVolume);
        }

        wasMagnetActive = magnetActive;
    }

    /// <summary>
    /// Plays a clip once through the shared sfxSource with optional pitch randomization and volume control.
    /// </summary>
    private void PlayOneShot(AudioClip clip, bool randomizePitch, Vector2 pitchRange, float volume = 1f)
    {
        if (clip == null) return;

        sfxSource.pitch = randomizePitch
            ? Random.Range(pitchRange.x, pitchRange.y)
            : 1f;

        sfxSource.PlayOneShot(clip, masterVolume * volume);
    }

    private bool IsGrounded()
    {
        return groundCheckComponent != null && groundCheckComponent.isGrounded;
    }
}