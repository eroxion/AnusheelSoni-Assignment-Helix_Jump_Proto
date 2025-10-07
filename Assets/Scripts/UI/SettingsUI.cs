using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Handles settings UI interactions and updates.
/// Syncs UI elements (sliders, toggles, text) with SettingsManager values.
/// Initializes on Start to show default values immediately.
/// </summary>
public class SettingsUI : MonoBehaviour
{
    [Header("Audio UI")]
    [SerializeField] private Slider _bgmVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;
    [SerializeField] private TextMeshProUGUI _bgmValueText;
    [SerializeField] private TextMeshProUGUI _sfxValueText;
    
    [Header("Global Rotation Multiplier UI")]
    [SerializeField] private Slider _globalRotationMultiplierSlider;
    [SerializeField] private TextMeshProUGUI _globalRotationMultiplierValueText;
    
    [Header("Control UI")]
    [SerializeField] private Slider _keyboardSpeedSlider;
    [SerializeField] private Slider _mouseSpeedSlider;
    [SerializeField] private Slider _touchSpeedSlider;
    [SerializeField] private TextMeshProUGUI _keyboardValueText;
    [SerializeField] private TextMeshProUGUI _mouseValueText;
    [SerializeField] private TextMeshProUGUI _touchValueText;
    
    [Header("Invert UI")]
    [SerializeField] private Toggle _invertKeyboardToggle;
    [SerializeField] private Toggle _invertMouseToggle;
    [SerializeField] private Toggle _invertTouchToggle;
    
    private bool _isInitialized = false;
    
    /// <summary>
    /// Initialize on Start to load settings immediately.
    /// This ensures UI shows correct values even before panel is opened.
    /// </summary>
    private void Start()
    {
        InitializeUI();
    }
    
    private void OnEnable()
    {
        // Reload settings when panel opens (in case they changed elsewhere)
        if (_isInitialized)
        {
            LoadSettingsToUI();
        }
        
        // Subscribe to UI events
        SubscribeToEvents();
    }
    
    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        UnsubscribeFromEvents();
    }
    
    /// <summary>
    /// Initializes all UI elements with proper ranges and current values.
    /// Called once on Start.
    /// </summary>
    private void InitializeUI()
    {
        // Audio volume sliders (0-100%)
        _bgmVolumeSlider.minValue = 0f;
        _bgmVolumeSlider.maxValue = 1f;
        
        _sfxVolumeSlider.minValue = 0f;
        _sfxVolumeSlider.maxValue = 1f;
        
        // Global rotation multiplier (0.1x - 5.0x)
        _globalRotationMultiplierSlider.minValue = 0.1f;
        _globalRotationMultiplierSlider.maxValue = 5f;
        
        // Keyboard speed (0.1 - 3.0)
        _keyboardSpeedSlider.minValue = 0.1f;
        _keyboardSpeedSlider.maxValue = 3f;
        
        // Mouse speed (0.1 - 100)
        _mouseSpeedSlider.minValue = 0.1f;
        _mouseSpeedSlider.maxValue = 100f;
        
        // Touch speed (0.1 - 100)
        _touchSpeedSlider.minValue = 0.1f;
        _touchSpeedSlider.maxValue = 100f;
        
        // Load current settings into UI
        LoadSettingsToUI();
        
        _isInitialized = true;
        Debug.Log("SettingsUI initialized with current values");
    }
    
    /// <summary>
    /// Loads settings from SettingsManager to UI elements.
    /// Uses SetValueWithoutNotify to avoid triggering change events.
    /// </summary>
    private void LoadSettingsToUI()
    {
        if (SettingsManager.Instance == null) return;
        
        // Audio settings
        _bgmVolumeSlider.SetValueWithoutNotify(SettingsManager.Instance.BGMVolume);
        _sfxVolumeSlider.SetValueWithoutNotify(SettingsManager.Instance.SFXVolume);
        UpdateBGMValueText(SettingsManager.Instance.BGMVolume);
        UpdateSFXValueText(SettingsManager.Instance.SFXVolume);
        
        // Global rotation multiplier
        _globalRotationMultiplierSlider.SetValueWithoutNotify(SettingsManager.Instance.GlobalRotationMultiplier);
        UpdateGlobalRotationMultiplierValueText(SettingsManager.Instance.GlobalRotationMultiplier);
        
        // Control sensitivity settings
        _keyboardSpeedSlider.SetValueWithoutNotify(SettingsManager.Instance.KeyboardSpeed);
        _mouseSpeedSlider.SetValueWithoutNotify(SettingsManager.Instance.MouseSpeed);
        _touchSpeedSlider.SetValueWithoutNotify(SettingsManager.Instance.TouchSpeed);
        UpdateKeyboardValueText(SettingsManager.Instance.KeyboardSpeed);
        UpdateMouseValueText(SettingsManager.Instance.MouseSpeed);
        UpdateTouchValueText(SettingsManager.Instance.TouchSpeed);
        
        // Invert control settings
        _invertKeyboardToggle.SetIsOnWithoutNotify(SettingsManager.Instance.InvertKeyboard);
        _invertMouseToggle.SetIsOnWithoutNotify(SettingsManager.Instance.InvertMouse);
        _invertTouchToggle.SetIsOnWithoutNotify(SettingsManager.Instance.InvertTouch);
    }
    
    /// <summary>
    /// Subscribes to all UI element change events.
    /// Called when panel is enabled.
    /// </summary>
    private void SubscribeToEvents()
    {
        _bgmVolumeSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        _sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        _globalRotationMultiplierSlider.onValueChanged.AddListener(OnGlobalRotationMultiplierChanged);
        _keyboardSpeedSlider.onValueChanged.AddListener(OnKeyboardSpeedChanged);
        _mouseSpeedSlider.onValueChanged.AddListener(OnMouseSpeedChanged);
        _touchSpeedSlider.onValueChanged.AddListener(OnTouchSpeedChanged);
        _invertKeyboardToggle.onValueChanged.AddListener(OnInvertKeyboardChanged);
        _invertMouseToggle.onValueChanged.AddListener(OnInvertMouseChanged);
        _invertTouchToggle.onValueChanged.AddListener(OnInvertTouchChanged);
    }
    
    /// <summary>
    /// Unsubscribes from all UI element change events.
    /// Called when panel is disabled to prevent memory leaks.
    /// </summary>
    private void UnsubscribeFromEvents()
    {
        _bgmVolumeSlider.onValueChanged.RemoveListener(OnBGMVolumeChanged);
        _sfxVolumeSlider.onValueChanged.RemoveListener(OnSFXVolumeChanged);
        _globalRotationMultiplierSlider.onValueChanged.RemoveListener(OnGlobalRotationMultiplierChanged);
        _keyboardSpeedSlider.onValueChanged.RemoveListener(OnKeyboardSpeedChanged);
        _mouseSpeedSlider.onValueChanged.RemoveListener(OnMouseSpeedChanged);
        _touchSpeedSlider.onValueChanged.RemoveListener(OnTouchSpeedChanged);
        _invertKeyboardToggle.onValueChanged.RemoveListener(OnInvertKeyboardChanged);
        _invertMouseToggle.onValueChanged.RemoveListener(OnInvertMouseChanged);
        _invertTouchToggle.onValueChanged.RemoveListener(OnInvertTouchChanged);
    }
    
    // ===== AUDIO CALLBACKS =====
    
    private void OnBGMVolumeChanged(float value)
    {
        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.SetBGMVolume(value);
        }
        UpdateBGMValueText(value);
    }
    
    private void OnSFXVolumeChanged(float value)
    {
        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.SetSFXVolume(value);
        }
        UpdateSFXValueText(value);
    }
    
    // ===== GLOBAL MULTIPLIER CALLBACK =====
    
    private void OnGlobalRotationMultiplierChanged(float value)
    {
        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.SetGlobalRotationMultiplier(value);
        }
        UpdateGlobalRotationMultiplierValueText(value);
    }
    
    // ===== CONTROL CALLBACKS =====
    
    private void OnKeyboardSpeedChanged(float value)
    {
        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.SetKeyboardSpeed(value);
        }
        UpdateKeyboardValueText(value);
    }
    
    private void OnMouseSpeedChanged(float value)
    {
        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.SetMouseSpeed(value);
        }
        UpdateMouseValueText(value);
    }
    
    private void OnTouchSpeedChanged(float value)
    {
        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.SetTouchSpeed(value);
        }
        UpdateTouchValueText(value);
    }
    
    // ===== INVERT CALLBACKS =====
    
    private void OnInvertKeyboardChanged(bool value)
    {
        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.SetInvertKeyboard(value);
        }
    }
    
    private void OnInvertMouseChanged(bool value)
    {
        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.SetInvertMouse(value);
        }
    }
    
    private void OnInvertTouchChanged(bool value)
    {
        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.SetInvertTouch(value);
        }
    }
    
    // ===== VALUE TEXT UPDATES =====
    
    /// <summary>
    /// Updates BGM volume text display (0-100%).
    /// </summary>
    private void UpdateBGMValueText(float value)
    {
        if (_bgmValueText != null)
        {
            _bgmValueText.text = $"{Mathf.RoundToInt(value * 100)}%";
        }
    }
    
    /// <summary>
    /// Updates SFX volume text display (0-100%).
    /// </summary>
    private void UpdateSFXValueText(float value)
    {
        if (_sfxValueText != null)
        {
            _sfxValueText.text = $"{Mathf.RoundToInt(value * 100)}%";
        }
    }
    
    /// <summary>
    /// Updates global rotation multiplier text display (e.g., "2.5x").
    /// </summary>
    private void UpdateGlobalRotationMultiplierValueText(float value)
    {
        if (_globalRotationMultiplierValueText != null)
        {
            _globalRotationMultiplierValueText.text = value.ToString("F1") + "x";
        }
    }
    
    /// <summary>
    /// Updates keyboard speed text display (one decimal place).
    /// </summary>
    private void UpdateKeyboardValueText(float value)
    {
        if (_keyboardValueText != null)
        {
            _keyboardValueText.text = value.ToString("F1");
        }
    }
    
    /// <summary>
    /// Updates mouse speed text display (whole number).
    /// </summary>
    private void UpdateMouseValueText(float value)
    {
        if (_mouseValueText != null)
        {
            _mouseValueText.text = value.ToString("F0");
        }
    }
    
    /// <summary>
    /// Updates touch speed text display (whole number).
    /// </summary>
    private void UpdateTouchValueText(float value)
    {
        if (_touchValueText != null)
        {
            _touchValueText.text = value.ToString("F0");
        }
    }
    
    /// <summary>
    /// Called by Reset button in settings panel.
    /// PUBLIC for Unity UI onClick event.
    /// </summary>
    public void OnResetButtonClicked()
    {
        // Play button click sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
    
        // Reset all settings to defaults
        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.ResetToDefaults();
            LoadSettingsToUI();
        }
    }
}