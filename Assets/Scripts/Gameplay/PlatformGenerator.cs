using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Pure object pooling for infinite gameplay.
/// Platforms generated once with random rotations, then only repositioned.
/// Cylinders managed separately and transfer sequentially from top to bottom.
/// Zero instantiation or destruction during gameplay.
/// </summary>
public class PlatformGenerator : MonoBehaviour
{
    [Header("Platform Configuration")]
    [SerializeField] private GameObject _platformSegmentPrefab;
    [SerializeField] private int _initialPlatformCount = 20;
    [SerializeField] private float _platformSpacing = 4.0f;
    
    [Header("Segment Configuration")]
    [SerializeField] private int _segmentsPerPlatform = 12;
    
    [Header("Gap Size Range")]
    [Tooltip("Gap size range: [Min, Max] segments missing per platform")]
    [SerializeField] private Vector2Int _gapSizeRange = new Vector2Int(1, 3);
    
    [Header("Deadly Segments Range")]
    [Tooltip("Deadly segments range: [Min, Max] deadly segments per platform")]
    [SerializeField] private Vector2Int _deadlySegmentsRange = new Vector2Int(0, 3);
    
    [Header("Materials")]
    [SerializeField] private Material _safePlatformMaterial;
    [SerializeField] private Material _deadlyPlatformMaterial;
    
    [Header("Central Cylinder")]
    [SerializeField] private Material _centralCylinderMaterial;
    [SerializeField] private float _cylinderRadius = 0.3f;
    [SerializeField] private float _cylinderHeightPerSegment = 4.0f;
    [SerializeField] private int _cylindersAboveStart = 3;
    [SerializeField] private int _platformsBeforeCylinderTransfer = 4;
    
    [Header("Organization")]
    [SerializeField] private Transform _helixContainer;
    
    [Header("Ball Reference")]
    [SerializeField] private Transform _ball;
    
    // Object pooling
    private List<GameObject> _platformPool = new List<GameObject>();
    private List<GameObject> _cylinders = new List<GameObject>();
    
    private int _highestPassedIndex = -1;
    private float _lowestPlatformY = 0f;
    private float _lowestCylinderY = 0f;
    
    private float _degreesPerSegment;
    private int _recycleCheckFrequency = 0;
    
    private void Awake()
    {
        _degreesPerSegment = 360f / _segmentsPerPlatform;
    }
    
    private void Start()
    {
        // Find ball reference
        if (_ball == null)
        {
            BallController ballController = FindAnyObjectByType<BallController>();
            if (ballController != null)
            {
                _ball = ballController.transform;
            }
        }
        
        // Apply difficulty settings
        if (DifficultyManager.Instance != null)
        {
            _deadlySegmentsRange.x = DifficultyManager.Instance.MinDeadlySegments;
            _deadlySegmentsRange.y = DifficultyManager.Instance.MaxDeadlySegments;
        }
        
        // Generate level
        GenerateCylinders();
        GenerateInitialPlatforms();
    }
    
    private void Update()
    {
        // Optimize: Check recycling every 3 frames
        _recycleCheckFrequency++;
        if (_recycleCheckFrequency >= 3)
        {
            _recycleCheckFrequency = 0;
            CheckPlatformsToRecycle();
        }
    }
    
    /// <summary>
    /// Generates cylinders separately from platforms.
    /// </summary>
    private void GenerateCylinders()
    {
        if (_helixContainer == null) return;
        
        int totalCylinders = _initialPlatformCount + _cylindersAboveStart;
        
        for (int i = 0; i < totalCylinders; i++)
        {
            float yPosition = (_cylindersAboveStart - i) * _cylinderHeightPerSegment;
            
            GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            cylinder.name = $"Cylinder_{i}";
            cylinder.transform.SetParent(_helixContainer);
            cylinder.transform.localPosition = new Vector3(0f, yPosition, 0f);
            
            float cylinderScaleY = _cylinderHeightPerSegment / 2f;
            cylinder.transform.localScale = new Vector3(_cylinderRadius * 2f, cylinderScaleY, _cylinderRadius * 2f);
            
            if (_centralCylinderMaterial != null)
            {
                cylinder.GetComponent<MeshRenderer>().material = _centralCylinderMaterial;
            }
            
            Destroy(cylinder.GetComponent<Collider>());
            
            _cylinders.Add(cylinder);
            
            if (yPosition < _lowestCylinderY)
            {
                _lowestCylinderY = yPosition;
            }
        }
        
        Debug.Log($"Generated {totalCylinders} cylinders");
    }
    
    /// <summary>
    /// Generates initial platform pool with random Y rotations.
    /// These exact platforms will be reused forever with only repositioning.
    /// </summary>
    private void GenerateInitialPlatforms()
    {
        if (_platformSegmentPrefab == null || _helixContainer == null)
        {
            Debug.LogError("PlatformGenerator: Missing prefab or container!");
            return;
        }
        
        for (int i = 0; i < _initialPlatformCount; i++)
        {
            float yPosition = -i * _platformSpacing;
            
            GameObject platformParent = new GameObject($"Platform_{i}");
            platformParent.transform.SetParent(_helixContainer);
            platformParent.transform.localPosition = new Vector3(0f, yPosition, 0f);
            
            // Apply random Y rotation to every platform
            float randomYRotation = Random.Range(0f, 360f);
            platformParent.transform.localRotation = Quaternion.Euler(0f, randomYRotation, 0f);
            
            // Generate segments once
            GeneratePlatformSegments(platformParent.transform);
            
            _platformPool.Add(platformParent);
            
            if (yPosition < _lowestPlatformY)
            {
                _lowestPlatformY = yPosition;
            }
        }
        
        Debug.Log($"Generated {_initialPlatformCount} platforms with random rotations");
    }
    
    /// <summary>
    /// Generates segments for a platform (ONLY called during initial generation).
    /// </summary>
    private void GeneratePlatformSegments(Transform platformParent)
    {
        int gapSize = Random.Range(_gapSizeRange.x, _gapSizeRange.y + 1);
        int gapStart = Random.Range(0, _segmentsPerPlatform);
        
        int deadlySegmentCount = Random.Range(_deadlySegmentsRange.x, _deadlySegmentsRange.y + 1);
        
        HashSet<int> deadlyIndices = new HashSet<int>();
        int attempts = 0;
        while (deadlyIndices.Count < deadlySegmentCount && attempts < _segmentsPerPlatform * 2)
        {
            int randomIndex = Random.Range(0, _segmentsPerPlatform);
            
            if (!IsSegmentInGap(randomIndex, gapStart, gapSize))
            {
                deadlyIndices.Add(randomIndex);
            }
            attempts++;
        }
        
        for (int seg = 0; seg < _segmentsPerPlatform; seg++)
        {
            if (IsSegmentInGap(seg, gapStart, gapSize))
            {
                continue;
            }
            
            bool isDeadly = deadlyIndices.Contains(seg);
            CreatePlatformSegment(seg, platformParent, isDeadly);
        }
    }
    
    /// <summary>
    /// Creates a single segment (ONLY during initial generation).
    /// </summary>
    private void CreatePlatformSegment(int segmentIndex, Transform parent, bool isDeadly)
    {
        float angle = segmentIndex * _degreesPerSegment;
        Quaternion rotation = Quaternion.Euler(0f, angle, 0f);
        
        GameObject segment = Instantiate(_platformSegmentPrefab, parent.position, rotation, parent);
        segment.name = $"Segment_{segmentIndex}";
        
        MeshRenderer renderer = segment.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            renderer.material = isDeadly ? _deadlyPlatformMaterial : _safePlatformMaterial;
            segment.tag = isDeadly ? "Deadly" : "Safe";
        }
    }
    
    /// <summary>
    /// Checks if segment is in gap range.
    /// </summary>
    private bool IsSegmentInGap(int segmentIndex, int gapStart, int gapSize)
    {
        for (int i = 0; i < gapSize; i++)
        {
            if (segmentIndex == (gapStart + i) % _segmentsPerPlatform)
            {
                return true;
            }
        }
        return false;
    }
    
    /// <summary>
    /// Checks platforms to recycle based on ball position.
    /// </summary>
    private void CheckPlatformsToRecycle()
    {
        if (_ball == null) return;
        
        float ballY = _ball.position.y;
        int currentPlatformIndex = Mathf.FloorToInt(-ballY / _platformSpacing);
        
        // Process each platform passed since last check
        for (int i = _highestPassedIndex + 1; i <= currentPlatformIndex; i++)
        {
            // Get platform from pool by wrapping index
            int poolIndex = i % _platformPool.Count;
            GameObject platformToRecycle = _platformPool[poolIndex];
            
            // Reposition platform
            RepositionPlatform(platformToRecycle);
            
            // Transfer cylinder for every platform after 4th
            if (i >= _platformsBeforeCylinderTransfer)
            {
                TransferTopCylinderToBottom();
            }
        }
        
        _highestPassedIndex = currentPlatformIndex;
    }
    
    /// <summary>
    /// Repositions platform to bottom with new random Y rotation.
    /// Platform keeps same segments but gets new rotation for variety.
    /// </summary>
    private void RepositionPlatform(GameObject platform)
    {
        if (platform == null) return;
    
        // Move to bottom
        _lowestPlatformY -= _platformSpacing;
        platform.transform.localPosition = new Vector3(0f, _lowestPlatformY, 0f);
    
        // Apply new random Y rotation
        float randomYRotation = Random.Range(0f, 360f);
        platform.transform.localRotation = Quaternion.Euler(0f, randomYRotation, 0f);
    }

    
    /// <summary>
    /// Transfers the topmost cylinder to the bottom.
    /// Called for every platform passed after the 4th.
    /// </summary>
    private void TransferTopCylinderToBottom()
    {
        if (_cylinders.Count == 0) return;
        
        // Find topmost cylinder
        GameObject topCylinder = null;
        float highestY = float.MinValue;
        
        foreach (GameObject cylinder in _cylinders)
        {
            float cylinderY = cylinder.transform.localPosition.y;
            if (cylinderY > highestY)
            {
                highestY = cylinderY;
                topCylinder = cylinder;
            }
        }
        
        if (topCylinder == null) return;
        
        // Move to bottom
        _lowestCylinderY -= _cylinderHeightPerSegment;
        topCylinder.transform.localPosition = new Vector3(0f, _lowestCylinderY, 0f);
    }
}