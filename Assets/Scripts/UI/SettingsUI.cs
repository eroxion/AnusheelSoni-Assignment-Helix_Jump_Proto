using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Handles settings UI interactions and updates.
/// Syncs UI elements with SettingsManager values.
/// Initializes on Start to show default values immediately.
/// </summary>
public class SettingsUI : MonoBehaviour
{
    [Header("Audio UI")]
    [SerializeField] private Slider _bgmVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;
    [SerializeField] private TextMeshProUGUI _bgmValueText;
    [SerializeField] private TextMeshProUGUI _sfxValueText;
    
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
        
        // Subscribe to events
        SubscribeToEvents();
    }
    
    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        UnsubscribeFromEvents();
    }
    
    /// <summary>
    /// Initializes UI with current settings.
    /// Called once on Start.
    /// </summary>
    private void InitializeUI()
    {
        // Set slider ranges
        _bgmVolumeSlider.minValue = 0f;
        _bgmVolumeSlider.maxValue = 1f;
        
        _sfxVolumeSlider.minValue = 0f;
        _sfxVolumeSlider.maxValue = 1f;
        
        _keyboardSpeedSlider.minValue = 0.1f;
        _keyboardSpeedSlider.maxValue = 3f;  // CHANGED: max 3
        
        _mouseSpeedSlider.minValue = 0.1f;
        _mouseSpeedSlider.maxValue = 100f;
        
        _touchSpeedSlider.minValue = 0.1f;
        _touchSpeedSlider.maxValue = 100f;
        
        // Load current settings to UI
        LoadSettingsToUI();
        
        _isInitialized = true;
        Debug.Log("SettingsUI initialized with default values");
    }
    
    /// <summary>
    /// Loads settings from SettingsManager to UI elements.
    /// </summary>
    private void LoadSettingsToUI()
    {
        if (SettingsManager.Instance == null) return;
        
        // Audio - without triggering events
        _bgmVolumeSlider.SetValueWithoutNotify(SettingsManager.Instance.BGMVolume);
        _sfxVolumeSlider.SetValueWithoutNotify(SettingsManager.Instance.SFXVolume);
        UpdateBGMValueText(SettingsManager.Instance.BGMVolume);
        UpdateSFXValueText(SettingsManager.Instance.SFXVolume);
        
        // Controls - without triggering events
        _keyboardSpeedSlider.SetValueWithoutNotify(SettingsManager.Instance.KeyboardSpeed);
        _mouseSpeedSlider.SetValueWithoutNotify(SettingsManager.Instance.MouseSpeed);
        _touchSpeedSlider.SetValueWithoutNotify(SettingsManager.Instance.TouchSpeed);
        UpdateKeyboardValueText(SettingsManager.Instance.KeyboardSpeed);
        UpdateMouseValueText(SettingsManager.Instance.MouseSpeed);
        UpdateTouchValueText(SettingsManager.Instance.TouchSpeed);
        
        // Invert - without triggering events
        _invertKeyboardToggle.SetIsOnWithoutNotify(SettingsManager.Instance.InvertKeyboard);
        _invertMouseToggle.SetIsOnWithoutNotify(SettingsManager.Instance.InvertMouse);
        _invertTouchToggle.SetIsOnWithoutNotify(SettingsManager.Instance.InvertTouch);
    }
    
    private void SubscribeToEvents()
    {
        _bgmVolumeSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        _sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        _keyboardSpeedSlider.onValueChanged.AddListener(OnKeyboardSpeedChanged);
        _mouseSpeedSlider.onValueChanged.AddListener(OnMouseSpeedChanged);
        _touchSpeedSlider.onValueChanged.AddListener(OnTouchSpeedChanged);
        _invertKeyboardToggle.onValueChanged.AddListener(OnInvertKeyboardChanged);
        _invertMouseToggle.onValueChanged.AddListener(OnInvertMouseChanged);
        _invertTouchToggle.onValueChanged.AddListener(OnInvertTouchChanged);
    }
    
    private void UnsubscribeFromEvents()
    {
        _bgmVolumeSlider.onValueChanged.RemoveListener(OnBGMVolumeChanged);
        _sfxVolumeSlider.onValueChanged.RemoveListener(OnSFXVolumeChanged);
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
    
    private void UpdateBGMValueText(float value)
    {
        if (_bgmValueText != null)
        {
            _bgmValueText.text = $"{Mathf.RoundToInt(value * 100)}%";
        }
    }
    
    private void UpdateSFXValueText(float value)
    {
        if (_sfxValueText != null)
        {
            _sfxValueText.text = $"{Mathf.RoundToInt(value * 100)}%";
        }
    }
    
    private void UpdateKeyboardValueText(float value)
    {
        if (_keyboardValueText != null)
        {
            _keyboardValueText.text = value.ToString("F1");
        }
    }
    
    private void UpdateMouseValueText(float value)
    {
        if (_mouseValueText != null)
        {
            _mouseValueText.text = value.ToString("F0");
        }
    }
    
    private void UpdateTouchValueText(float value)
    {
        if (_touchValueText != null)
        {
            _touchValueText.text = value.ToString("F0");
        }
    }
    
    /// <summary>
    /// Called by Reset button.
    /// PUBLIC for Unity UI onClick.
    /// </summary>
    public void OnResetButtonClicked()
    {
        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.ResetToDefaults();
            LoadSettingsToUI(); // Refresh UI with new defaults
        }
    }
}
