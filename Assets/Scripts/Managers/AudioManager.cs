using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Manages all audio in the game including BGM and SFX.
/// Uses logarithmic volume conversion for accurate perceived volume control.
/// Singleton with DontDestroyOnLoad for persistence across scenes.
/// </summary>
public class AudioManager : MonoBehaviour
{
    internal static AudioManager Instance { get; private set; }
    
    [Header("Audio Sources")]
    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioSource _sfxSource;
    
    [Header("Music Clips")]
    [SerializeField] private AudioClip _mainMenuBGM;
    [SerializeField] private AudioClip _mainGameBGM;
    
    [Header("SFX Clips")]
    [SerializeField] private AudioClip _ballBounceSFX;
    [SerializeField] private AudioClip _buttonClickSFX;
    [SerializeField] private AudioClip _levelWinSFX;
    [SerializeField] private AudioClip _deathSFX;
    [SerializeField] private AudioClip _newHighScoreSFX;
    [SerializeField] private AudioClip _platformClearSFX;
    
    private void Awake()
    {
        // Singleton with DontDestroyOnLoad
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // Create AudioSources if not assigned
        if (_bgmSource == null)
        {
            _bgmSource = gameObject.AddComponent<AudioSource>();
            _bgmSource.loop = true;
            _bgmSource.playOnAwake = false;
        }
        
        if (_sfxSource == null)
        {
            _sfxSource = gameObject.AddComponent<AudioSource>();
            _sfxSource.loop = false;
            _sfxSource.playOnAwake = false;
        }
    }
    
    private void Start()
    {
        // Apply saved volume settings
        if (SettingsManager.Instance != null)
        {
            SetBGMVolume(SettingsManager.Instance.BGMVolume);
            SetSFXVolume(SettingsManager.Instance.SFXVolume);
        }
        
        Debug.Log("AudioManager initialized");
    }
    
    /// <summary>
    /// Converts linear volume (0-1) to logarithmic decibels.
    /// Human hearing is logarithmic, so this provides more accurate volume perception.
    /// Formula: dB = 20 * log10(linear)
    /// Range: -80dB (silence) to 0dB (full volume)
    /// </summary>
    private float LinearToLogarithmic(float linearVolume)
    {
        // Clamp to prevent log(0)
        linearVolume = Mathf.Clamp(linearVolume, 0.0001f, 1f);
        
        // Convert to decibels
        float db = 20f * Mathf.Log10(linearVolume);
        
        // Clamp to reasonable range (-80dB to 0dB)
        return Mathf.Clamp(db, -80f, 0f);
    }
    
    /// <summary>
    /// Sets BGM volume using logarithmic conversion.
    /// </summary>
    internal void SetBGMVolume(float linearVolume)
    {
        if (_bgmSource != null)
        {
            // Convert linear (0-1) to logarithmic volume
            float volumeDB = LinearToLogarithmic(linearVolume);
            
            // Unity AudioSource.volume expects linear (0-1)
            // So we convert: volume = 10^(dB/20)
            _bgmSource.volume = Mathf.Pow(10f, volumeDB / 20f);
            
            Debug.Log($"BGM Volume: {linearVolume:F2} → {volumeDB:F1}dB → Unity Volume: {_bgmSource.volume:F3}");
        }
    }
    
    /// <summary>
    /// Sets SFX volume using logarithmic conversion.
    /// </summary>
    internal void SetSFXVolume(float linearVolume)
    {
        if (_sfxSource != null)
        {
            float volumeDB = LinearToLogarithmic(linearVolume);
            _sfxSource.volume = Mathf.Pow(10f, volumeDB / 20f);
            
            Debug.Log($"SFX Volume: {linearVolume:F2} → {volumeDB:F1}dB → Unity Volume: {_sfxSource.volume:F3}");
        }
    }
    
    // ===== MUSIC PLAYBACK =====
    
    /// <summary>
    /// Plays Main Menu BGM.
    /// </summary>
    internal void PlayMainMenuBGM()
    {
        PlayBGM(_mainMenuBGM);
    }
    
    /// <summary>
    /// Plays Main Game BGM.
    /// </summary>
    internal void PlayMainGameBGM()
    {
        PlayBGM(_mainGameBGM);
    }
    
    /// <summary>
    /// Plays the specified BGM clip.
    /// </summary>
    private void PlayBGM(AudioClip clip)
    {
        if (_bgmSource == null || clip == null) return;
        
        // Only restart if different clip
        if (_bgmSource.clip != clip)
        {
            _bgmSource.clip = clip;
            _bgmSource.Play();
            Debug.Log($"Playing BGM: {clip.name}");
        }
        else if (!_bgmSource.isPlaying)
        {
            _bgmSource.Play();
        }
    }
    
    /// <summary>
    /// Stops currently playing BGM.
    /// </summary>
    internal void StopBGM()
    {
        if (_bgmSource != null)
        {
            _bgmSource.Stop();
        }
    }
    
    /// <summary>
    /// Pauses BGM.
    /// </summary>
    internal void PauseBGM()
    {
        if (_bgmSource != null)
        {
            _bgmSource.Pause();
        }
    }
    
    /// <summary>
    /// Resumes BGM.
    /// </summary>
    internal void ResumeBGM()
    {
        if (_bgmSource != null)
        {
            _bgmSource.UnPause();
        }
    }
    
    // ===== SFX PLAYBACK =====
    
    /// <summary>
    /// Plays ball bounce sound effect.
    /// </summary>
    internal void PlayBallBounce()
    {
        PlaySFX(_ballBounceSFX);
    }
    
    /// <summary>
    /// Plays button click sound effect.
    /// </summary>
    internal void PlayButtonClick()
    {
        PlaySFX(_buttonClickSFX);
    }
    
    /// <summary>
    /// Plays level win sound effect.
    /// </summary>
    internal void PlayLevelWin()
    {
        PlaySFX(_levelWinSFX);
    }
    
    /// <summary>
    /// Plays death sound effect.
    /// </summary>
    internal void PlayDeath()
    {
        PlaySFX(_deathSFX);
    }
    
    /// <summary>
    /// Plays new high score sound effect.
    /// </summary>
    internal void PlayNewHighScore()
    {
        PlaySFX(_newHighScoreSFX);
    }
    
    /// <summary>
    /// Plays platform clear sound effect.
    /// </summary>
    internal void PlayPlatformClear()
    {
        PlaySFX(_platformClearSFX);
    }
    
    /// <summary>
    /// Plays the specified SFX clip.
    /// </summary>
    private void PlaySFX(AudioClip clip)
    {
        if (_sfxSource == null || clip == null) return;
        
        _sfxSource.PlayOneShot(clip);
    }
    
    /// <summary>
    /// Plays a generic SFX clip (for UI or other uses).
    /// </summary>
    internal void PlaySFX(AudioClip clip, float volumeScale = 1f)
    {
        if (_sfxSource == null || clip == null) return;
        
        _sfxSource.PlayOneShot(clip, volumeScale);
    }
}
