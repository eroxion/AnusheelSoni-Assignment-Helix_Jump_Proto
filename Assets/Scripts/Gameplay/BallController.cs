using UnityEngine;

/// <summary>
/// Controls ball physics with custom bounce system.
/// Independent bounce height and bounce frequency control.
/// Bounce height is always achieved regardless of gravity.
/// </summary>
public class BallController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private TrailRenderer _trailRenderer;
    
    [Header("Game Start Settings")]
    [Tooltip("Delay before gravity starts (gives player time to prepare)")]
    [SerializeField] private float _gravityStartDelay = 3f;

    [Tooltip("Show countdown in console")]
    [SerializeField] private bool _showCountdown = true;
    
    [Header("Bounce Settings")]
    [Tooltip("Target height ball reaches after each bounce (always achieved)")]
    [SerializeField] private float _targetBounceHeight = 3f;
    
    [Tooltip("Bounce frequency: How fast ball bounces (affects gravity and speed)")]
    [SerializeField] [Range(0.1f, 10f)] private float _bounceFrequency = 3f;
    
    [Header("Advanced Settings")]
    [Tooltip("Show physics calculations in console")]
    [SerializeField] private bool _showDebugInfo = false;
    
    private bool _gravityEnabled;
    private float _gravityStartTime;
    private bool _isGameOver;
    private float _calculatedGravity;
    private float _calculatedBounceVelocity;
    private int _currentPlatformIndex;
    
    private void Awake()
    {
        _gravityEnabled = false;
        _isGameOver = false;
        _currentPlatformIndex = -1;
        
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
        if (_rigidbody != null)
        {
            _rigidbody.useGravity = false;
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        }
    
        if (_trailRenderer != null)
        {
            _trailRenderer.emitting = true;
        }
    
        // Apply difficulty settings BEFORE calculating physics
        if (DifficultyManager.Instance != null)
        {
            _bounceFrequency = DifficultyManager.Instance.BounceFrequency;
            Debug.Log($"Applied difficulty: Bounce Frequency {_bounceFrequency}");
        }
        
        // Calculate physics with (possibly updated) bounce frequency
        RecalculatePhysics();
        
        // CRITICAL FIX: Initialize gravity start time
        _gravityStartTime = Time.time + _gravityStartDelay;
        _gravityEnabled = false;
        
        if (_showCountdown)
        {
            Debug.Log($"<color=yellow>Game starting in {_gravityStartDelay:F1} seconds...</color>");
        }
    }

    
    private void Update()
    {
        CheckPlatformPassage();
    
        // Show countdown UI
        if (!_gravityEnabled && UIManager.Instance != null)
        {
            float timeRemaining = _gravityStartTime - Time.time;
        
            if (timeRemaining > 0)
            {
                int secondsRemaining = Mathf.CeilToInt(timeRemaining);
                UIManager.Instance.UpdateCountdown(secondsRemaining);
            }
        }
    }
    
    private void FixedUpdate()
    {
        if (_isGameOver) return;
    
        // Check if gravity should start
        if (!_gravityEnabled && Time.time >= _gravityStartTime)
        {
            _gravityEnabled = true;
            
            if (UIManager.Instance != null)
            {
                UIManager.Instance.HideCountdownAndShowHUD();
            }
        
            if (_showCountdown)
            {
                Debug.Log("<color=lime>GO! Gravity enabled!</color>");
            }
        }
    
        // Only apply gravity if enabled
        if (_gravityEnabled)
        {
            ApplyGravity();
        }
    }
    
    /// <summary>
    /// Calculates gravity and bounce velocity to achieve target height with desired frequency.
    /// 
    /// Physics explanation:
    /// - Bounce frequency controls how fast the ball cycle completes
    /// - Higher frequency = stronger gravity + higher velocity = faster bouncing
    /// - Lower frequency = weaker gravity + lower velocity = slower bouncing
    /// - Height remains constant regardless of frequency
    /// 
    /// Formulas:
    /// Time to reach peak: t = sqrt(2h/g)
    /// But we want to control frequency, so we invert:
    /// g = 2h * (frequency)²
    /// v = sqrt(2gh) = sqrt(2 * 2h * frequency² * h) = 2h * frequency
    /// </summary>
    private void RecalculatePhysics()
    {
        // Calculate gravity based on bounce frequency
        // Higher frequency = stronger gravity = faster bouncing
        _calculatedGravity = 2f * _targetBounceHeight * (_bounceFrequency * _bounceFrequency);
        
        // Calculate bounce velocity to reach target height with calculated gravity
        // v = sqrt(2 * g * h)
        _calculatedBounceVelocity = Mathf.Sqrt(2f * _calculatedGravity * _targetBounceHeight);
        
        if (_showDebugInfo)
        {
            float timeToReachPeak = _calculatedBounceVelocity / _calculatedGravity;
            float totalBounceTime = timeToReachPeak * 2f;
            
            Debug.Log($"<color=cyan>Physics Recalculated:</color>\n" +
                      $"Target Height: {_targetBounceHeight:F2}m\n" +
                      $"Bounce Frequency: {_bounceFrequency:F2}x\n" +
                      $"Calculated Gravity: {_calculatedGravity:F2} m/s²\n" +
                      $"Bounce Velocity: {_calculatedBounceVelocity:F2} m/s\n" +
                      $"Time to Peak: {timeToReachPeak:F2}s\n" +
                      $"Full Bounce Cycle: {totalBounceTime:F2}s");
        }
    }
    
    /// <summary>
    /// Applies calculated gravity force.
    /// </summary>
    private void ApplyGravity()
    {
        Vector3 gravity = Vector3.down * _calculatedGravity;
        _rigidbody.AddForce(gravity, ForceMode.Acceleration);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (_isGameOver) return;
    
        if (collision.gameObject.CompareTag("Deadly"))
        {
            HandleDeadlyCollision();
        }
        else if (collision.gameObject.CompareTag("Finish"))
        {
            HandleFinishCollision();
        }
        else if (collision.gameObject.CompareTag("Safe"))
        {
            HandleSafeCollision(collision);
        }
    }
    
    /// <summary>
    /// Handles safe platform collision with consistent bounce.
    /// </summary>
    private void HandleSafeCollision(Collision collision)
    {
        // Apply calculated bounce velocity
        _rigidbody.linearVelocity = Vector3.up * _calculatedBounceVelocity;
        
        // Play bounce sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayBallBounce();
        }
        
        // Break combo on bounce
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetCombo();
        }
    }

    /// <summary>
    /// Handles deadly platform collision - triggers game over.
    /// </summary>
    private void HandleDeadlyCollision()
    {
        _isGameOver = true;
        
        // Play death sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayDeath();
        }
        
        // Stop physics
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.isKinematic = true;
        
        if (_trailRenderer != null)
        {
            _trailRenderer.emitting = false;
        }
        
        HelixRotator rotator = FindAnyObjectByType<HelixRotator>();
        if (rotator != null)
        {
            rotator.DisableRotation();
        }
        
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.StopTimer();
            ScoreManager.Instance.OnGameOver();
        }
        
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowGameOverScreen();
        }
    }

    /// <summary>
    /// Handles collision with finish platform - triggers game completion.
    /// </summary>
    private void HandleFinishCollision()
    {
        _isGameOver = true;
        
        // Play level win sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayLevelWin();
        }
        
        // Stop physics
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.isKinematic = true;
        
        if (_trailRenderer != null)
        {
            _trailRenderer.emitting = false;
        }
        
        HelixRotator rotator = FindAnyObjectByType<HelixRotator>();
        if (rotator != null)
        {
            rotator.DisableRotation();
        }
        
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.StopTimer();
            ScoreManager.Instance.OnGameComplete();
        }
        
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowVictoryScreen();
        }
    }
    
    /// <summary>
    /// Tracks ball position and awards score when passing platforms.
    /// Position-based to trigger immediately when gap is passed.
    /// </summary>
    private void CheckPlatformPassage()
    {
        float ballY = transform.position.y;
    
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.CheckPlatformPassed(ballY, out _currentPlatformIndex);
        }
        else
        {
            // Fallback if ScoreManager not available
            _currentPlatformIndex = Mathf.FloorToInt(-ballY / 4f);
        }
    }
    
    /// <summary>
    /// Resets ball to initial state for restart.
    /// </summary>
    internal void ResetBall(Vector3 startPosition)
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
    
        // Reset gravity delay
        _gravityStartTime = Time.time + _gravityStartDelay;
        _gravityEnabled = false;
    
        if (_showCountdown)
        {
            Debug.Log($"<color=yellow>Game restarting in {_gravityStartDelay:F1} seconds...</color>");
        }
    }

    
    /// <summary>
    /// Updates target bounce height at runtime and recalculates physics.
    /// </summary>
    internal void SetBounceHeight(float height)
    {
        _targetBounceHeight = Mathf.Max(0.1f, height);
        RecalculatePhysics();
    }
    
    /// <summary>
    /// Updates bounce frequency at runtime and recalculates physics.
    /// </summary>
    internal void SetBounceFrequency(float frequency)
    {
        _bounceFrequency = Mathf.Clamp(frequency, 0.1f, 10f);
        RecalculatePhysics();
    }
    
    /// <summary>
    /// Called by Inspector when values change (Editor only).
    /// </summary>
    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            RecalculatePhysics();
        }
    }
}