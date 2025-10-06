using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    internal static SettingsManager Instance { get; private set; }
    
    [Header("Audio Settings")]
    [SerializeField] [Range(0f, 1f)] private float _bgmVolume = 1f;
    [SerializeField] [Range(0f, 1f)] private float _sfxVolume = 1f;
    
    [Header("Control Sensitivity (degrees/second multipliers)")]
    [Tooltip("Keyboard: 0.1-3.0 = 50-300 deg/s")]
    [SerializeField] [Range(0.1f, 3f)] private float _keyboardSpeed = 1.1f;
    
    [Tooltip("Mouse: 0.1-100 = 500-10000 deg/s")]
    [SerializeField] [Range(0.1f, 100f)] private float _mouseSpeed = 50f;
    
    [Tooltip("Touch: 0.1-100 = 500-10000 deg/s")]
    [SerializeField] [Range(0.1f, 100f)] private float _touchSpeed = 50f;
    
    [Header("Invert Controls")]
    [SerializeField] private bool _invertKeyboard = false;
    [SerializeField] private bool _invertMouse = true;
    [SerializeField] private bool _invertTouch = true;
    
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
        
        LoadSettings();
    }
    
    private void Start()
    {
        ApplyAudioSettings();
    }
    
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
        _keyboardSpeed = PlayerPrefs.GetFloat("KeyboardSpeed", 1.1f);
        _mouseSpeed = PlayerPrefs.GetFloat("MouseSpeed", 50f);
        _touchSpeed = PlayerPrefs.GetFloat("TouchSpeed", 50f);
        _invertKeyboard = PlayerPrefs.GetInt("InvertKeyboard", 0) == 1;
        _invertMouse = PlayerPrefs.GetInt("InvertMouse", 1) == 1;
        _invertTouch = PlayerPrefs.GetInt("InvertTouch", 1) == 1;
    }
    
    private void SaveSettings()
    {
        PlayerPrefs.SetFloat("BGMVolume", _bgmVolume);
        PlayerPrefs.SetFloat("SFXVolume", _sfxVolume);
        PlayerPrefs.SetFloat("KeyboardSpeed", _keyboardSpeed);
        PlayerPrefs.SetFloat("MouseSpeed", _mouseSpeed);
        PlayerPrefs.SetFloat("TouchSpeed", _touchSpeed);
        PlayerPrefs.SetInt("InvertKeyboard", _invertKeyboard ? 1 : 0);
        PlayerPrefs.SetInt("InvertMouse", _invertMouse ? 1 : 0);
        PlayerPrefs.SetInt("InvertTouch", _invertTouch ? 1 : 0);
        PlayerPrefs.Save();
    }
    
    private void ApplyAudioSettings()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetBGMVolume(_bgmVolume);
            AudioManager.Instance.SetSFXVolume(_sfxVolume);
        }
    }
    
    // Audio settings
    public void SetBGMVolume(float volume)
    {
        _bgmVolume = Mathf.Clamp01(volume);
        SaveSettings();
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetBGMVolume(_bgmVolume);
        }
    }
    
    public void SetSFXVolume(float volume)
    {
        _sfxVolume = Mathf.Clamp01(volume);
        SaveSettings();
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSFXVolume(_sfxVolume);
        }
    }
    
    // Control settings
    public void SetKeyboardSpeed(float speed)
    {
        _keyboardSpeed = Mathf.Clamp(speed, 0.1f, 3f);
        SaveSettings();
    }
    
    public void SetMouseSpeed(float speed)
    {
        _mouseSpeed = Mathf.Clamp(speed, 0.1f, 100f);
        SaveSettings();
    }
    
    public void SetTouchSpeed(float speed)
    {
        _touchSpeed = Mathf.Clamp(speed, 0.1f, 100f);
        SaveSettings();
    }
    
    // Invert settings
    public void SetInvertKeyboard(bool invert)
    {
        _invertKeyboard = invert;
        SaveSettings();
    }
    
    public void SetInvertMouse(bool invert)
    {
        _invertMouse = invert;
        SaveSettings();
    }
    
    public void SetInvertTouch(bool invert)
    {
        _invertTouch = invert;
        SaveSettings();
    }
    
    // Getters
    internal float BGMVolume => _bgmVolume;
    internal float SFXVolume => _sfxVolume;
    internal float KeyboardSpeed => _keyboardSpeed;
    internal float MouseSpeed => _mouseSpeed;
    internal float TouchSpeed => _touchSpeed;
    internal bool InvertKeyboard => _invertKeyboard;
    internal bool InvertMouse => _invertMouse;
    internal bool InvertTouch => _invertTouch;
    
    public void ResetToDefaults()
    {
        _bgmVolume = 1f;
        _sfxVolume = 1f;
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