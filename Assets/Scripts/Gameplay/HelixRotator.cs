using UnityEngine;

/// <summary>
/// Rotates the helix tower based on player input.
/// Controls the HelixContainer transform rotation around Y-axis.
/// </summary>
public class HelixRotator : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private Transform _helixContainer;
    
    [Header("Rotation Settings")]
    [Tooltip("Base rotation speed in degrees per second")]
    [SerializeField] private float _rotationSpeed = 200f;
    
    [Tooltip("Smoothing factor for rotation (0 = instant, 1 = very smooth)")]
    [SerializeField] [Range(0f, 1f)] private float _rotationSmoothing = 0.1f;
    
    private float _currentRotationVelocity = 0f;
    private bool _canRotate = true;
    
    private void Awake()
    {
        // Auto-find components if not assigned
        if (_inputHandler == null)
        {
            _inputHandler = FindAnyObjectByType<InputHandler>();
        }
        
        if (_helixContainer == null)
        {
            _helixContainer = GameObject.Find("HelixContainer")?.transform;
        }
    }
    
    private void Update()
    {
        if (_canRotate && _helixContainer != null && _inputHandler != null)
        {
            RotateHelix();
        }
    }
    
    /// <summary>
    /// Applies rotation to helix container based on input.
    /// </summary>
    private void RotateHelix()
    {
        float inputValue = _inputHandler.GetRotationInput();
        
        // Calculate target rotation speed
        float targetRotationSpeed = inputValue * _rotationSpeed;
        
        // Smooth rotation using Lerp
        _currentRotationVelocity = Mathf.Lerp(
            _currentRotationVelocity,
            targetRotationSpeed,
            1f - _rotationSmoothing
        );
        
        // Apply rotation around Y-axis
        float rotationAmount = _currentRotationVelocity * Time.deltaTime;
        _helixContainer.Rotate(0f, rotationAmount, 0f, Space.World);
    }
    
    /// <summary>
    /// Enables helix rotation (called on game start).
    /// </summary>
    internal void EnableRotation()
    {
        _canRotate = true;
    }
    
    /// <summary>
    /// Disables helix rotation (called on game over).
    /// </summary>
    internal void DisableRotation()
    {
        _canRotate = false;
        _currentRotationVelocity = 0f;
    }
}
