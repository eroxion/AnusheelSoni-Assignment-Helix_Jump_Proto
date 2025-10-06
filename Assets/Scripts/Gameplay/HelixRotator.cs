using UnityEngine;

/// <summary>
/// Controls helix rotation based on player input with smooth damping.
/// Each input type has its own sensitivity from SettingsManager.
/// Smoothing provides better feel and prevents jerky rotation.
/// </summary>
public class HelixRotator : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform _helixTransform;
    
    [Header("Rotation Speed")]
    [Tooltip("Global rotation multiplier for all inputs.\nFormula: InputSpeed × Multiplier = degrees/second\nExample: KeyboardSpeed 1.1 × 200 = 220 deg/s")]
    [SerializeField] private float _rotationMultiplier = 200f;
    
    [Header("Smoothing")]
    [Tooltip("Smoothing factor: Higher = smoother but less responsive")]
    [SerializeField] [Range(0.1f, 1f)] private float _smoothingFactor = 0.65f;
    
    private bool _isRotationEnabled = true;
    private bool _isDragging = false;
    private Vector2 _lastInputPosition;
    
    // Smoothing variables
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
            // Smoothly stop rotation when disabled
            _targetRotationSpeed = 0f;
            _currentRotationVelocity = Mathf.Lerp(_currentRotationVelocity, 0f, Time.deltaTime * 10f);
            return;
        }
        
        // Get target rotation speed from input
        _targetRotationSpeed = GetInputDelta();
        
        // Smooth the rotation using Lerp
        _currentRotationVelocity = Mathf.Lerp(
            _currentRotationVelocity, 
            _targetRotationSpeed, 
            1f - _smoothingFactor
        );
        
        // Apply smoothed rotation
        if (Mathf.Abs(_currentRotationVelocity) > 0.001f)
        {
            _helixTransform.Rotate(Vector3.up, _currentRotationVelocity * Time.deltaTime);
        }
    }
    
    /// <summary>
    /// Gets rotation input from all sources.
    /// Returns degrees per second to rotate.
    /// </summary>
    private float GetInputDelta()
    {
        // Priority 1: Keyboard
        float keyboardInput = GetKeyboardInput();
        if (Mathf.Abs(keyboardInput) > 0.001f)
        {
            return keyboardInput;
        }
        
        // Priority 2: Touch
        if (Input.touchCount > 0)
        {
            return GetTouchInput();
        }
        
        // Priority 3: Mouse
        return GetMouseInput();
    }
    
    /// <summary>
    /// Keyboard input: Returns degrees per second.
    /// Formula: KeyboardSpeed × RotationMultiplier = degrees/second.
    /// </summary>
    private float GetKeyboardInput()
    {
        float settingsSpeed = 1.1f;
        bool invert = false;
        
        if (SettingsManager.Instance != null)
        {
            settingsSpeed = SettingsManager.Instance.KeyboardSpeed;
            invert = SettingsManager.Instance.InvertKeyboard;
        }
        
        float degreesPerSecond = settingsSpeed * _rotationMultiplier;
        float horizontal = 0f;
        
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
    /// Mouse drag input: Returns degrees per second.
    /// Formula: MouseSpeed × RotationMultiplier × normalizedDelta = degrees/second.
    /// </summary>
    private float GetMouseInput()
    {
        float settingsSpeed = 50f;
        bool invert = true;
        
        if (SettingsManager.Instance != null)
        {
            settingsSpeed = SettingsManager.Instance.MouseSpeed;
            invert = SettingsManager.Instance.InvertMouse;
        }
        
        float degreesPerSecond = settingsSpeed * _rotationMultiplier;
        
        if (Input.GetMouseButtonDown(0))
        {
            _isDragging = true;
            _lastInputPosition = Input.mousePosition;
            return 0f;
        }
        
        if (Input.GetMouseButton(0) && _isDragging)
        {
            Vector2 currentPosition = Input.mousePosition;
            float deltaX = currentPosition.x - _lastInputPosition.x;
            _lastInputPosition = currentPosition;
            
            float normalizedDelta = deltaX / Screen.width;
            float direction = invert ? -1f : 1f;
            
            return normalizedDelta * degreesPerSecond * direction;
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
        }
        
        return 0f;
    }
    
    /// <summary>
    /// Touch input: Returns degrees per second.
    /// Formula: TouchSpeed × RotationMultiplier × normalizedDelta = degrees/second.
    /// </summary>
    private float GetTouchInput()
    {
        float settingsSpeed = 50f;
        bool invert = true;
        
        if (SettingsManager.Instance != null)
        {
            settingsSpeed = SettingsManager.Instance.TouchSpeed;
            invert = SettingsManager.Instance.InvertTouch;
        }
        
        float degreesPerSecond = settingsSpeed * _rotationMultiplier;
        
        Touch touch = Input.GetTouch(0);
        
        if (touch.phase == TouchPhase.Moved)
        {
            float normalizedDelta = touch.deltaPosition.x / Screen.width;
            float direction = invert ? -1f : 1f;
            
            return normalizedDelta * degreesPerSecond * direction;
        }
        
        return 0f;
    }
    
    internal void DisableRotation()
    {
        _isRotationEnabled = false;
    }
    
    internal void EnableRotation()
    {
        _isRotationEnabled = true;
        _currentRotationVelocity = 0f;
        _targetRotationSpeed = 0f;
    }
}