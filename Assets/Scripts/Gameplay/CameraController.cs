using UnityEngine;

/// <summary>
/// Smooth camera follow system that tracks the ball's position.
/// Uses LateUpdate for smooth camera movement after all other updates.
/// Implements Vector3.Lerp for gradual position interpolation.
/// </summary>
public class CameraController : MonoBehaviour
{
    [Header("Follow Target")]
    [Tooltip("Transform to follow (usually the Ball)")]
    [SerializeField] private Transform _target;
    
    [Header("Follow Settings")]
    [Tooltip("How smoothly camera follows target (lower = smoother but slower)")]
    [SerializeField] [Range(0.01f, 1f)] private float _followSpeed = 0.125f;
    
    [Tooltip("Camera offset from target position")]
    [SerializeField] private Vector3 _offset = new Vector3(0f, 5f, -8f);
    
    private void Start()
    {
        // Auto-find ball if target not assigned
        if (_target == null)
        {
            GameObject ball = GameObject.FindGameObjectWithTag("Player");
            if (ball != null)
            {
                _target = ball.transform;
                Debug.Log("CameraController: Auto-found target (Ball)");
            }
            else
            {
                Debug.LogWarning("CameraController: No target assigned and couldn't find Ball!");
            }
        }
        
        // Set initial camera position
        if (_target != null)
        {
            transform.position = _target.position + _offset;
        }
    }
    
    /// <summary>
    /// LateUpdate is called after all Update functions.
    /// Ensures camera moves after target has moved, preventing jitter.
    /// </summary>
    private void LateUpdate()
    {
        if (_target == null) return;
        
        FollowTarget();
    }
    
    /// <summary>
    /// Smoothly follows target position with offset.
    /// </summary>
    private void FollowTarget()
    {
        // Calculate desired position
        Vector3 desiredPosition = _target.position + _offset;
        
        // Smooth movement using Lerp
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _followSpeed);
        
        // Apply position
        transform.position = smoothedPosition;
    }
    
    /// <summary>
    /// Sets new follow target at runtime.
    /// </summary>
    internal void SetTarget(Transform newTarget)
    {
        _target = newTarget;
    }
    
    /// <summary>
    /// Instantly moves camera to target position (no smoothing).
    /// </summary>
    internal void SnapToTarget()
    {
        if (_target != null)
        {
            transform.position = _target.position + _offset;
        }
    }
    
    /// <summary>
    /// Updates camera offset at runtime.
    /// </summary>
    internal void SetOffset(Vector3 newOffset)
    {
        _offset = newOffset;
    }
}