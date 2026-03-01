using UnityEngine;

/// <summary>
/// A simple music player that loops a single AudioClip.
/// Attach this script to any GameObject in your scene.
/// Assign an AudioClip in the Inspector.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    [Header("Music Settings")]
    [Tooltip("The audio clip to loop.")]
    public AudioClip musicClip;

    [Range(0f, 1f)]
    [Tooltip("Playback volume (0 = silent, 1 = full).")]
    public float volume = 1f;

    [Tooltip("Start playing automatically when the scene loads.")]
    public bool playOnAwake = true;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        // Configure the AudioSource
        _audioSource.clip = musicClip;
        _audioSource.volume = volume;
        _audioSource.loop = true;          // <-- This is what makes it loop
        _audioSource.playOnAwake = false;    // We handle play timing ourselves
    }

    private void Start()
    {
        if (playOnAwake)
            Play();
    }

    // ── Public Controls ──────────────────────────────────────────────────────

    /// <summary>Start (or resume) playback.</summary>
    public void Play()
    {
        if (musicClip == null)
        {
            Debug.LogWarning("[MusicPlayer] No AudioClip assigned!");
            return;
        }

        if (!_audioSource.isPlaying)
            _audioSource.Play();
    }

    /// <summary>Pause playback (keeps the current position).</summary>
    public void Pause()
    {
        if (_audioSource.isPlaying)
            _audioSource.Pause();
    }

    /// <summary>Stop playback and reset to the beginning.</summary>
    public void Stop()
    {
        _audioSource.Stop();
    }

    /// <summary>Set volume at runtime (0–1).</summary>
    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp01(newVolume);
        _audioSource.volume = volume;
    }

    /// <summary>Swap the clip and immediately start playing it on loop.</summary>
    public void ChangeClip(AudioClip newClip)
    {
        if (newClip == null) return;

        _audioSource.Stop();
        musicClip = newClip;
        _audioSource.clip = newClip;
        _audioSource.Play();
    }
}
