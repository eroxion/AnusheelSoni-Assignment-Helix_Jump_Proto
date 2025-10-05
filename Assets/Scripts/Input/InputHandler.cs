using UnityEngine;

/// <summary>
/// Universal input handler supporting keyboard, mouse, and touch controls.
/// Automatically detects device type and enables appropriate input methods.
/// Supports: A/D keys, Arrow keys, Mouse drag, and Touch drag.
/// </summary>
public class InputHandler : MonoBehaviour
{
    [Header("Input Enable Settings")]
    [Tooltip("Enable/disable keyboard controls (A/D and Arrow keys)")]
    [SerializeField] private bool _keyboardEnabled = true;
    
    [Tooltip("Enable/disable mouse drag controls")]
    [SerializeField] private bool _mouseEnabled = true;
    
    [Tooltip("Enable/disable touch controls (mobile/tablet)")]
    [SerializeField] private bool _touchEnabled = true;
    
    [Header("Input Sensitivity")]
    [Tooltip("Master sensitivity multiplier for all inputs")]
    [SerializeField] private float _inputSensitivity = 1.0f;
    
    [Header("Input Speed Settings")]
    [Tooltip("Keyboard input speed (for A/D and Arrow keys)")]
    [SerializeField] private float _keyboardSpeed = 0.5f;
    
    [Tooltip("Mouse drag speed multiplier")]
    [SerializeField] private float _mouseSpeed = 1.5f;
    
    [Tooltip("Touch drag speed multiplier (mobile/tablet)")]
    [SerializeField] private float _touchSpeed = 1.5f;
    
    [Header("Invert Controls")]
    [Tooltip("Invert keyboard controls (swap left/right)")]
    [SerializeField] private bool _invertKeyboard = false;
    
    [Tooltip("Invert mouse controls (swap drag direction)")]
    [SerializeField] private bool _invertMouse = false;
    
    [Tooltip("Invert touch controls (swap swipe direction)")]
    [SerializeField] private bool _invertTouch = false;
    
    [Header("Debug Info")]
    [SerializeField] private bool _showDebugInfo = false;
    
    private bool _isDragging = false;
    private float _currentInputDelta = 0f;
    private Vector2 _lastInputPosition;
    private DeviceType _detectedDevice;
    
    private enum DeviceType
    {
        Desktop,
        Mobile,
        Tablet
    }
    
    private void Start()
    {
        DetectDevice();
    }
    
    /// <summary>
    /// Detects device type for optimal input handling.
    /// </summary>
    private void DetectDevice()
    {
        if (Application.isMobilePlatform)
        {
            _detectedDevice = DeviceType.Mobile;
            Debug.Log("Device detected: Mobile");
        }
        else if (Input.touchSupported)
        {
            _detectedDevice = DeviceType.Tablet;
            Debug.Log("Device detected: Tablet/Touchscreen");
        }
        else
        {
            _detectedDevice = DeviceType.Desktop;
            Debug.Log("Device detected: Desktop");
        }
    }
    
    /// <summary>
    /// Returns normalized input delta for rotation (-1 to 1 range).
    /// Positive = rotate right, Negative = rotate left.
    /// </summary>
    internal float GetRotationInput()
    {
        return _currentInputDelta * _inputSensitivity;
    }
    
    private void Update()
    {
        HandleInput();
        
        if (_showDebugInfo)
        {
            ShowDebugInfo();
        }
    }
    
    /// <summary>
    /// Master input handler - processes all input types.
    /// Priority: Keyboard > Touch > Mouse.
    /// </summary>
    private void HandleInput()
    {
        _currentInputDelta = 0f;
        
        // Priority 1: Keyboard (if enabled)
        if (_keyboardEnabled && HandleKeyboardInput())
        {
            return;
        }
        
        // Priority 2: Touch (if enabled)
        if (_touchEnabled && Input.touchCount > 0)
        {
            HandleTouchInput();
            return;
        }
        
        // Priority 3: Mouse (if enabled)
        if (_mouseEnabled && Input.mousePresent)
        {
            HandleMouseInput();
        }
    }
    
    /// <summary>
    /// Handles keyboard input: A/D keys and Arrow keys.
    /// </summary>
    private bool HandleKeyboardInput()
    {
        bool keyPressed = false;
        float direction = _invertKeyboard ? 1f : -1f;
        
        // Left rotation
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _currentInputDelta = direction * _keyboardSpeed;
            keyPressed = true;
        }
        // Right rotation
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _currentInputDelta = -direction * _keyboardSpeed;
            keyPressed = true;
        }
        
        return keyPressed;
    }
    
    /// <summary>
    /// Handles touch input for mobile/tablet devices.
    /// </summary>
    private void HandleTouchInput()
    {
        Touch touch = Input.GetTouch(0);
        
        switch (touch.phase)
        {
            case TouchPhase.Began:
                _isDragging = true;
                _lastInputPosition = touch.position;
                break;
                
            case TouchPhase.Moved:
                if (_isDragging)
                {
                    Vector2 currentPosition = touch.position;
                    float deltaX = currentPosition.x - _lastInputPosition.x;
                    
                    float direction = _invertTouch ? -1f : 1f;
                    _currentInputDelta = (deltaX / Screen.width) * _touchSpeed * direction;
                    
                    _lastInputPosition = currentPosition;
                }
                break;
                
            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                _isDragging = false;
                break;
        }
    }
    
    /// <summary>
    /// Handles mouse input for desktop.
    /// </summary>
    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _isDragging = true;
            _lastInputPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0) && _isDragging)
        {
            Vector2 currentPosition = Input.mousePosition;
            float deltaX = currentPosition.x - _lastInputPosition.x;
            
            float direction = _invertMouse ? -1f : 1f;
            _currentInputDelta = (deltaX / Screen.width) * _mouseSpeed * direction;
            
            _lastInputPosition = currentPosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
        }
    }
    
    /// <summary>
    /// Shows debug information (for testing).
    /// </summary>
    private void ShowDebugInfo()
    {
        if (_currentInputDelta != 0f)
        {
            string inputSource = "None";
            
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || 
                Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            {
                inputSource = "Keyboard";
            }
            else if (Input.touchCount > 0)
            {
                inputSource = "Touch";
            }
            else if (_isDragging)
            {
                inputSource = "Mouse";
            }
            
            Debug.Log($"Input: {_currentInputDelta:F3} | Source: {inputSource} | Device: {_detectedDevice}");
        }
    }
    
    /// <summary>
    /// Returns current device type.
    /// </summary>
    internal string GetDeviceType()
    {
        return _detectedDevice.ToString();
    }
    
    /// <summary>
    /// Checks if touch input is available.
    /// </summary>
    internal bool IsTouchSupported()
    {
        return Input.touchSupported || Application.isMobilePlatform;
    }
}