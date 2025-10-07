using UnityEngine;

/// <summary>
/// Manages all game settings with PlayerPrefs persistence.
/// Singleton pattern with DontDestroyOnLoad for cross-scene access.
/// Handles audio volumes, rotation speeds, control inversion, and global multiplier.
/// </summary>
public class SettingsManager : MonoBehaviour
{
    internal static SettingsManager Instance { get; private set; }
    
    [Header("Audio Settings")]
    [SerializeField] [Range(0f, 1f)] private float _bgmVolume = 1f;
    [SerializeField] [Range(0f, 1f)] private float _sfxVolume = 1f;
    
    [Header("Global Rotation Multiplier")]
    [Tooltip("Global multiplier affecting all rotation inputs (keyboard, mouse, touch)")]
    [SerializeField] [Range(0.1f, 5f)] private float _globalRotationMultiplier = 1f;
    
    [Header("Control Sensitivity (degrees/second multipliers)")]
    [Tooltip("Keyboard: 0.1-3.0 range for precise control")]
    [SerializeField] [Range(0.1f, 3f)] private float _keyboardSpeed = 1.1f;
    
    [Tooltip("Mouse: 0.1-100 range for drag sensitivity")]
    [SerializeField] [Range(0.1f, 100f)] private float _mouseSpeed = 50f;
    
    [Tooltip("Touch: 0.1-100 range for swipe sensitivity")]
    [SerializeField] [Range(0.1f, 100f)] private float _touchSpeed = 50f;
    
    [Header("Invert Controls")]
    [SerializeField] private bool _invertKeyboard = false;
    [SerializeField] private bool _invertMouse = true;
    [SerializeField] private bool _invertTouch = true;
    
    private void Awake()
    {
        // Singleton setup with scene persistence
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
        
        LoadSettings();
    }
    
    private void Start()
    {
        ApplyAudioSettings();
    }
    
    /// <summary>
    /// Loads all settings from PlayerPrefs.
    /// If no saved settings exist, saves current defaults.
    /// </summary>
    private void LoadSettings()
    {
        bool settingsExist = PlayerPrefs.HasKey("BGMVolume");
        
        if (!settingsExist)
        {
            SaveSettings();
            return;
        }
        
        _bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);
        _sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        _globalRotationMultiplier = PlayerPrefs.GetFloat("GlobalRotationMultiplier", 1f);
        _keyboardSpeed = PlayerPrefs.GetFloat("KeyboardSpeed", 1.1f);
        _mouseSpeed = PlayerPrefs.GetFloat("MouseSpeed", 50f);
        _touchSpeed = PlayerPrefs.GetFloat("TouchSpeed", 50f);
        _invertKeyboard = PlayerPrefs.GetInt("InvertKeyboard", 0) == 1;
        _invertMouse = PlayerPrefs.GetInt("InvertMouse", 1) == 1;
        _invertTouch = PlayerPrefs.GetInt("InvertTouch", 1) == 1;
    }
    
    /// <summary>
    /// Saves all current settings to PlayerPrefs.
    /// Called automatically when any setting changes.
    /// </summary>
    private void SaveSettings()
    {
        PlayerPrefs.SetFloat("BGMVolume", _bgmVolume);
        PlayerPrefs.SetFloat("SFXVolume", _sfxVolume);
        PlayerPrefs.SetFloat("GlobalRotationMultiplier", _globalRotationMultiplier);
        PlayerPrefs.SetFloat("KeyboardSpeed", _keyboardSpeed);
        PlayerPrefs.SetFloat("MouseSpeed", _mouseSpeed);
        PlayerPrefs.SetFloat("TouchSpeed", _touchSpeed);
        PlayerPrefs.SetInt("InvertKeyboard", _invertKeyboard ? 1 : 0);
        PlayerPrefs.SetInt("InvertMouse", _invertMouse ? 1 : 0);
        PlayerPrefs.SetInt("InvertTouch", _invertTouch ? 1 : 0);
        PlayerPrefs.Save();
    }
    
    /// <summary>
    /// Applies current audio settings to AudioManager.
    /// Called on Start and after loading settings.
    /// </summary>
    private void ApplyAudioSettings()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetBGMVolume(_bgmVolume);
            AudioManager.Instance.SetSFXVolume(_sfxVolume);
        }
    }
    
    /// <summary>
    /// Sets background music volume and saves immediately.
    /// </summary>
    public void SetBGMVolume(float volume)
    {
        _bgmVolume = Mathf.Clamp01(volume);
        SaveSettings();
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetBGMVolume(_bgmVolume);
        }
    }
    
    /// <summary>
    /// Sets sound effects volume and saves immediately.
    /// </summary>
    public void SetSFXVolume(float volume)
    {
        _sfxVolume = Mathf.Clamp01(volume);
        SaveSettings();
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSFXVolume(_sfxVolume);
        }
    }
    
    /// <summary>
    /// Sets global rotation speed multiplier for all input types.
    /// Range: 0.1x (very slow) to 5.0x (very fast).
    /// Final rotation = InputSpeed × GlobalMultiplier × BaseMultiplier.
    /// </summary>
    public void SetGlobalRotationMultiplier(float multiplier)
    {
        _globalRotationMultiplier = Mathf.Clamp(multiplier, 0.1f, 5f);
        SaveSettings();
    }
    
    /// <summary>
    /// Sets keyboard rotation speed sensitivity.
    /// Range: 0.1 (slow) to 3.0 (fast).
    /// </summary>
    public void SetKeyboardSpeed(float speed)
    {
        _keyboardSpeed = Mathf.Clamp(speed, 0.1f, 3f);
        SaveSettings();
    }
    
    /// <summary>
    /// Sets mouse drag rotation sensitivity.
    /// Range: 0.1 (slow) to 100 (fast).
    /// </summary>
    public void SetMouseSpeed(float speed)
    {
        _mouseSpeed = Mathf.Clamp(speed, 0.1f, 100f);
        SaveSettings();
    }
    
    /// <summary>
    /// Sets touch swipe rotation sensitivity.
    /// Range: 0.1 (slow) to 100 (fast).
    /// </summary>
    public void SetTouchSpeed(float speed)
    {
        _touchSpeed = Mathf.Clamp(speed, 0.1f, 100f);
        SaveSettings();
    }
    
    /// <summary>
    /// Sets keyboard control inversion (left becomes right, right becomes left).
    /// </summary>
    public void SetInvertKeyboard(bool invert)
    {
        _invertKeyboard = invert;
        SaveSettings();
    }
    
    /// <summary>
    /// Sets mouse control inversion (drag left rotates right, drag right rotates left).
    /// </summary>
    public void SetInvertMouse(bool invert)
    {
        _invertMouse = invert;
        SaveSettings();
    }
    
    /// <summary>
    /// Sets touch control inversion (swipe left rotates right, swipe right rotates left).
    /// </summary>
    public void SetInvertTouch(bool invert)
    {
        _invertTouch = invert;
        SaveSettings();
    }
    
    /// <summary>
    /// Public getters for all settings.
    /// Accessed by other systems (AudioManager, HelixRotator, UI).
    /// </summary>
    internal float BGMVolume => _bgmVolume;
    internal float SFXVolume => _sfxVolume;
    internal float GlobalRotationMultiplier => _globalRotationMultiplier;
    internal float KeyboardSpeed => _keyboardSpeed;
    internal float MouseSpeed => _mouseSpeed;
    internal float TouchSpeed => _touchSpeed;
    internal bool InvertKeyboard => _invertKeyboard;
    internal bool InvertMouse => _invertMouse;
    internal bool InvertTouch => _invertTouch;
    
    /// <summary>
    /// Resets all settings to default values.
    /// Called by Reset button in settings panel.
    /// </summary>
    public void ResetToDefaults()
    {
        _bgmVolume = 1f;
        _sfxVolume = 1f;
        _globalRotationMultiplier = 1f;
        _keyboardSpeed = 1.1f;
        _mouseSpeed = 50f;
        _touchSpeed = 50f;
        _invertKeyboard = false;
        _invertMouse = true;
        _invertTouch = true;
        
        SaveSettings();
        ApplyAudioSettings();
    }
}