using UnityEngine;

/// <summary>
/// Controls ball physics with custom bounce system.
/// Uses FixedUpdate for physics calculations to ensure consistent behavior.
/// Implements consistent bounce height using kinematic equations: v = √(2gh)
/// </summary>
public class BallController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private TrailRenderer _trailRenderer;
    
    [Header("Bounce Settings")]
    [Tooltip("Target height ball reaches after each bounce (in units)")]
    [SerializeField] private float _bounceHeight = 3f;
    
    [Tooltip("Gravity magnitude applied to ball (positive value)")]
    [SerializeField] private float _gravityForce = 20f;
    
    private bool _isGameOver = false;
    private Vector3 _customGravity;
    
    private void Awake()
    {
        // Cache component references
        if (_rigidbody == null)
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        if (_trailRenderer == null)
        {
            _trailRenderer = GetComponent<TrailRenderer>();
        }
    }
    
    private void Start()
    {
        // Configure Rigidbody for manual physics control
        if (_rigidbody != null)
        {
            _rigidbody.useGravity = false; // Disable Unity gravity
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        }
        
        // Calculate custom gravity vector
        _customGravity = Vector3.down * _gravityForce;
        
        // Enable trail emission
        if (_trailRenderer != null)
        {
            _trailRenderer.emitting = true;
        }
    }
    
    /// <summary>
    /// FixedUpdate: Called at fixed intervals for consistent physics.
    /// Proper method for all Rigidbody physics manipulation.
    /// </summary>
    private void FixedUpdate()
    {
        if (_isGameOver) return;
        
        ApplyGravity();
    }
    
    /// <summary>
    /// Applies custom gravity force using ForceMode.Acceleration.
    /// </summary>
    private void ApplyGravity()
    {
        _rigidbody.AddForce(_customGravity, ForceMode.Acceleration);
    }
    
    /// <summary>
    /// Detects collisions with platform segments.
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        if (_isGameOver) return;
        
        if (collision.gameObject.CompareTag("Deadly"))
        {
            HandleDeadlyCollision();
        }
        else if (collision.gameObject.CompareTag("Safe"))
        {
            HandleSafeCollision(collision);
        }
    }
    
    /// <summary>
    /// Handles safe platform collision with consistent bounce.
    /// Uses physics formula: v = √(2gh) for consistent bounce height.
    /// </summary>
    private void HandleSafeCollision(Collision collision)
    {
        // Calculate required velocity to reach target bounce height
        float bounceVelocity = Mathf.Sqrt(2f * _gravityForce * _bounceHeight);
        
        // Set upward velocity directly for consistent bounce
        _rigidbody.linearVelocity = Vector3.up * bounceVelocity;
    }
    
    /// <summary>
    /// Handles deadly platform collision - triggers game over.
    /// </summary>
    private void HandleDeadlyCollision()
    {
        _isGameOver = true;
        
        // Stop all motion
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.isKinematic = true;
        
        // Disable trail
        if (_trailRenderer != null)
        {
            _trailRenderer.emitting = false;
        }
        Debug.Log("Game Over! Ball hit deadly platform.");
    }
    
    /// <summary>
    /// Resets ball to initial state for restart.
    /// </summary>
    public void ResetBall(Vector3 startPosition)
    {
        _isGameOver = false;
        
        _rigidbody.isKinematic = false;
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        
        transform.position = startPosition;
        
        if (_trailRenderer != null)
        {
            _trailRenderer.Clear();
            _trailRenderer.emitting = true;
        }
    }
    
    /// <summary>
    /// Adjusts bounce height at runtime.
    /// </summary>
    public void SetBounceHeight(float height)
    {
        _bounceHeight = Mathf.Max(0.1f, height);
    }
    
    /// <summary>
    /// Adjusts gravity at runtime.
    /// </summary>
    public void SetGravity(float gravity)
    {
        _gravityForce = Mathf.Max(1f, gravity);
        _customGravity = Vector3.down * _gravityForce;
    }
}