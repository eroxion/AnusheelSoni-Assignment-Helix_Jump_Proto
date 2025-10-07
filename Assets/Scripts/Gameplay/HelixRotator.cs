using UnityEngine;

/// <summary>
/// Controls helix rotation based on player input with smooth damping.
/// Supports keyboard, mouse, and touch input with individual sensitivities.
/// Applies global rotation multiplier for unified speed control.
/// Uses Lerp-based smoothing for natural acceleration/deceleration feel.
/// </summary>
public class HelixRotator : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform _helixTransform;
    
    [Header("Rotation Speed")]
    [Tooltip("Base rotation multiplier for all inputs.\nFinal formula: InputSpeed × GlobalMultiplier × BaseMultiplier = degrees/second\nExample: KeyboardSpeed 1.1 × Global 2.0 × Base 200 = 440 deg/s")]
    [SerializeField] private float _rotationMultiplier = 200f;
    
    [Header("Smoothing")]
    [Tooltip("Smoothing factor (0.1-1.0): Higher = smoother but less responsive")]
    [SerializeField] [Range(0.1f, 1f)] private float _smoothingFactor = 0.65f;
    
    private bool _isRotationEnabled = true;
    private bool _isDragging = false;
    private Vector2 _lastInputPosition;
    
    // Smoothing variables for Lerp interpolation
    private float _currentRotationVelocity = 0f;
    private float _targetRotationSpeed = 0f;
    
    private void Awake()
    {
        if (_helixTransform == null)
        {
            _helixTransform = transform;
        }
    }
    
    private void Update()
    {
        if (!_isRotationEnabled)
        {
            // Smoothly stop rotation when disabled (game over, pause)
            _targetRotationSpeed = 0f;
            _currentRotationVelocity = Mathf.Lerp(_currentRotationVelocity, 0f, Time.deltaTime * 10f);
            return;
        }
        
        // Get target rotation speed from input sources
        _targetRotationSpeed = GetInputDelta();
        
        // Smooth the rotation using Lerp for natural feel
        _currentRotationVelocity = Mathf.Lerp(
            _currentRotationVelocity, 
            _targetRotationSpeed, 
            1f - _smoothingFactor
        );
        
        // Apply smoothed rotation with Time.deltaTime for framerate independence
        if (Mathf.Abs(_currentRotationVelocity) > 0.001f)
        {
            _helixTransform.Rotate(Vector3.up, _currentRotationVelocity * Time.deltaTime);
        }
    }
    
    /// <summary>
    /// Gets rotation input from all sources with priority system.
    /// Priority: Keyboard > Touch > Mouse.
    /// Returns degrees per second to rotate.
    /// </summary>
    private float GetInputDelta()
    {
        // Priority 1: Keyboard (most precise control)
        float keyboardInput = GetKeyboardInput();
        if (Mathf.Abs(keyboardInput) > 0.001f)
        {
            return keyboardInput;
        }
        
        // Priority 2: Touch (mobile devices)
        if (Input.touchCount > 0)
        {
            return GetTouchInput();
        }
        
        // Priority 3: Mouse (desktop fallback)
        return GetMouseInput();
    }
    
    /// <summary>
    /// Keyboard input handler with global multiplier.
    /// Formula: KeyboardSpeed × GlobalMultiplier × RotationMultiplier = degrees/second.
    /// Supports A/D and Arrow keys with inversion option.
    /// </summary>
    private float GetKeyboardInput()
    {
        float settingsSpeed = 1.1f;
        float globalMultiplier = 1f;
        bool invert = false;
        
        if (SettingsManager.Instance != null)
        {
            settingsSpeed = SettingsManager.Instance.KeyboardSpeed;
            globalMultiplier = SettingsManager.Instance.GlobalRotationMultiplier;
            invert = SettingsManager.Instance.InvertKeyboard;
        }
        
        // Calculate final rotation speed with global multiplier
        float degreesPerSecond = settingsSpeed * globalMultiplier * _rotationMultiplier;
        float horizontal = 0f;
        
        // Check keyboard input (A/D or Arrow keys)
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            horizontal = -1f;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            horizontal = 1f;
        }
        
        if (Mathf.Abs(horizontal) > 0.001f)
        {
            float direction = invert ? -1f : 1f;
            return horizontal * degreesPerSecond * direction;
        }
        
        return 0f;
    }
    
    /// <summary>
    /// Mouse drag input handler with global multiplier.
    /// Formula: MouseSpeed × GlobalMultiplier × RotationMultiplier × normalizedDelta = degrees/second.
    /// Delta normalized by screen width for resolution independence.
    /// </summary>
    private float GetMouseInput()
    {
        float settingsSpeed = 50f;
        float globalMultiplier = 1f;
        bool invert = true;
        
        if (SettingsManager.Instance != null)
        {
            settingsSpeed = SettingsManager.Instance.MouseSpeed;
            globalMultiplier = SettingsManager.Instance.GlobalRotationMultiplier;
            invert = SettingsManager.Instance.InvertMouse;
        }
        
        float degreesPerSecond = settingsSpeed * globalMultiplier * _rotationMultiplier;
        
        // Start dragging
        if (Input.GetMouseButtonDown(0))
        {
            _isDragging = true;
            _lastInputPosition = Input.mousePosition;
            return 0f;
        }
        
        // Continue dragging
        if (Input.GetMouseButton(0) && _isDragging)
        {
            Vector2 currentPosition = Input.mousePosition;
            float deltaX = currentPosition.x - _lastInputPosition.x;
            _lastInputPosition = currentPosition;
            
            // Normalize delta by screen width for consistent feel across resolutions
            float normalizedDelta = deltaX / Screen.width;
            float direction = invert ? -1f : 1f;
            
            return normalizedDelta * degreesPerSecond * direction;
        }
        
        // Stop dragging
        if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
        }
        
        return 0f;
    }
    
    /// <summary>
    /// Touch input handler with global multiplier.
    /// Formula: TouchSpeed × GlobalMultiplier × RotationMultiplier × normalizedDelta = degrees/second.
    /// Uses first touch only (single-finger swipe).
    /// </summary>
    private float GetTouchInput()
    {
        float settingsSpeed = 50f;
        float globalMultiplier = 1f;
        bool invert = true;
        
        if (SettingsManager.Instance != null)
        {
            settingsSpeed = SettingsManager.Instance.TouchSpeed;
            globalMultiplier = SettingsManager.Instance.GlobalRotationMultiplier;
            invert = SettingsManager.Instance.InvertTouch;
        }
        
        float degreesPerSecond = settingsSpeed * globalMultiplier * _rotationMultiplier;
        
        Touch touch = Input.GetTouch(0);
        
        // Only process when touch is moving
        if (touch.phase == TouchPhase.Moved)
        {
            // Normalize delta by screen width
            float normalizedDelta = touch.deltaPosition.x / Screen.width;
            float direction = invert ? -1f : 1f;
            
            return normalizedDelta * degreesPerSecond * direction;
        }
        
        return 0f;
    }
    
    /// <summary>
    /// Disables rotation (called on game over or pause).
    /// Smoothly stops rotation using Lerp.
    /// </summary>
    internal void DisableRotation()
    {
        _isRotationEnabled = false;
    }
    
    /// <summary>
    /// Enables rotation (called on game start or resume).
    /// Resets velocity to zero for clean start.
    /// </summary>
    internal void EnableRotation()
    {
        _isRotationEnabled = true;
        _currentRotationVelocity = 0f;
        _targetRotationSpeed = 0f;
    }
}