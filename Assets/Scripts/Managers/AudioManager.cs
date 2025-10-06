using UnityEngine;

/// <summary>
/// Ultra-optimized audio manager with zero-overhead SFX playback.
/// Simple round-robin pool with no availability checking.
/// </summary>
public class AudioManager : MonoBehaviour
{
    internal static AudioManager Instance { get; private set; }
    
    [Header("Audio Sources")]
    [SerializeField] private AudioSource _bgmSource;
    
    [Header("SFX Sources Pool (Simple Round-Robin)")]
    [Tooltip("Uses simple round-robin, no checking = zero overhead")]
    [SerializeField] private int _sfxSourceCount = 8;
    private AudioSource[] _sfxSources;
    private int _nextSfxIndex = 0;
    
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
        
        InitializeAudioSources();
    }
    
    private void Start()
    {
        if (SettingsManager.Instance != null)
        {
            SetBGMVolume(SettingsManager.Instance.BGMVolume);
            SetSFXVolume(SettingsManager.Instance.SFXVolume);
        }
    }
    
    /// <summary>
    /// Creates BGM and SFX source pool with optimal settings.
    /// </summary>
    private void InitializeAudioSources()
    {
        // BGM Source
        if (_bgmSource == null)
        {
            _bgmSource = gameObject.AddComponent<AudioSource>();
        }
        
        _bgmSource.loop = true;
        _bgmSource.playOnAwake = false;
        _bgmSource.priority = 128;
        
        // SFX Source Pool - Pre-allocated array
        _sfxSources = new AudioSource[_sfxSourceCount];
        
        for (int i = 0; i < _sfxSourceCount; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.loop = false;
            source.playOnAwake = false;
            source.priority = 0;
            source.spatialBlend = 0f;
            _sfxSources[i] = source;
        }
    }
    
    /// <summary>
    /// Converts linear volume to squared curve for better control.
    /// </summary>
    private float LinearToVolumeCurve(float linearVolume)
    {
        linearVolume = Mathf.Clamp01(linearVolume);
        return linearVolume * linearVolume;
    }
    
    internal void SetBGMVolume(float linearVolume)
    {
        if (_bgmSource != null)
        {
            _bgmSource.volume = LinearToVolumeCurve(linearVolume);
        }
    }
    
    internal void SetSFXVolume(float linearVolume)
    {
        if (_sfxSources == null) return;
        
        float curvedVolume = LinearToVolumeCurve(linearVolume);
        
        // Direct array access, no foreach overhead
        for (int i = 0; i < _sfxSources.Length; i++)
        {
            _sfxSources[i].volume = curvedVolume;
        }
    }
    
    // ===== MUSIC PLAYBACK =====
    
    internal void PlayMainMenuBGM()
    {
        PlayBGM(_mainMenuBGM);
    }
    
    internal void PlayMainGameBGM()
    {
        PlayBGM(_mainGameBGM);
    }
    
    private void PlayBGM(AudioClip clip)
    {
        if (_bgmSource == null || clip == null) return;
        
        if (_bgmSource.clip != clip)
        {
            _bgmSource.clip = clip;
            _bgmSource.Play();
        }
        else if (!_bgmSource.isPlaying)
        {
            _bgmSource.Play();
        }
    }
    
    internal void StopBGM()
    {
        if (_bgmSource != null)
        {
            _bgmSource.Stop();
        }
    }
    
    internal void PauseBGM()
    {
        if (_bgmSource != null)
        {
            _bgmSource.Pause();
        }
    }
    
    internal void ResumeBGM()
    {
        if (_bgmSource != null)
        {
            _bgmSource.UnPause();
        }
    }
    
    // ===== ULTRA-FAST SFX PLAYBACK =====
    
    /// <summary>
    /// Plays SFX with zero-overhead round-robin.
    /// No loops, no checks, just instant playback.
    /// </summary>
    private void PlaySFX(AudioClip clip)
    {
        if (clip == null || _sfxSources == null) return;
        
        // Simple round-robin: Use next source, wrap around
        _sfxSources[_nextSfxIndex].PlayOneShot(clip);
        
        // Increment with wrap (no modulo needed, faster)
        _nextSfxIndex++;
        if (_nextSfxIndex >= _sfxSourceCount)
        {
            _nextSfxIndex = 0;
        }
    }
    
    internal void PlayBallBounce()
    {
        PlaySFX(_ballBounceSFX);
    }
    
    internal void PlayButtonClick()
    {
        PlaySFX(_buttonClickSFX);
    }
    
    internal void PlayLevelWin()
    {
        PlaySFX(_levelWinSFX);
    }
    
    internal void PlayDeath()
    {
        PlaySFX(_deathSFX);
    }
    
    internal void PlayNewHighScore()
    {
        PlaySFX(_newHighScoreSFX);
    }
    
    internal void PlayPlatformClear()
    {
        PlaySFX(_platformClearSFX);
    }
}