using UnityEngine;

/// <summary>
/// Generates helix tower with procedurally created platforms.
/// Platform_0 is empty (starting point), actual platforms start from Platform_1.
/// Platforms generate downward from parent position.
/// </summary>
public class PlatformGenerator : MonoBehaviour
{
    [Header("Platform Configuration")]
    [SerializeField] private GameObject _platformSegmentPrefab;
    [SerializeField] private int _platformCount = 50;
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
    
    [Header("Finish Platform")]
    [Tooltip("Material for final finish platform (no gaps, no deadly segments)")]
    [SerializeField] private Material _finishPlatformMaterial;
    
    [Header("Central Cylinder")]
    [SerializeField] private Material _centralCylinderMaterial;
    [SerializeField] private float _cylinderRadius = 0.3f;
    
    [Header("Organization")]
    [SerializeField] private Transform _helixContainer;
    
    private float _degreesPerSegment;
    private int _actualPlatformCount;
    
    private void Awake()
    {
        _degreesPerSegment = 360f / _segmentsPerPlatform;
        _actualPlatformCount = _platformCount + 1;
    }
    
    private void Start()
    {
        // Apply difficulty settings (updates existing _deadlySegmentsRange Vector2Int)
        if (DifficultyManager.Instance != null)
        {
            _deadlySegmentsRange.x = DifficultyManager.Instance.MinDeadlySegments;
            _deadlySegmentsRange.y = DifficultyManager.Instance.MaxDeadlySegments;
            Debug.Log($"Applied difficulty: Deadly Segments {_deadlySegmentsRange.x}-{_deadlySegmentsRange.y}");
        }
    
        // Generate level
        GenerateCentralCylinder();
        GenerateAllPlatforms();
    }
    
    private void GenerateCentralCylinder()
    {
        if (_helixContainer == null)
        {
            Debug.LogError("PlatformGenerator: Helix Container not assigned!");
            return;
        }
    
        GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        cylinder.name = "CentralPole";
        cylinder.transform.SetParent(_helixContainer);
    
        // Include finish platform in height calculation
        float totalHeight = _actualPlatformCount * _platformSpacing; // +1 for finish platform
        float cylinderY = -totalHeight / 2f;
        cylinder.transform.localPosition = new Vector3(0f, cylinderY, 0f);
    
        float cylinderScaleY = totalHeight / 2f;
        cylinder.transform.localScale = new Vector3(_cylinderRadius * 2f, cylinderScaleY, _cylinderRadius * 2f);
    
        if (_centralCylinderMaterial != null)
        {
            cylinder.GetComponent<MeshRenderer>().material = _centralCylinderMaterial;
        }
    
        Destroy(cylinder.GetComponent<Collider>());
    
        Debug.Log($"Cylinder: Height={totalHeight:F2}, Y={cylinderY:F2} (includes finish platform)");
    }

    
    private void GenerateAllPlatforms()
    {
        if (_platformSegmentPrefab == null || _helixContainer == null)
        {
            Debug.LogError("PlatformGenerator: Missing prefab or container!");
            return;
        }
    
        // Generate all platforms including empty Platform_0
        for (int i = 0; i < _actualPlatformCount; i++)
        {
            GenerateSinglePlatform(i);
        }
    
        // Generate final finish platform (after all normal platforms)
        GenerateFinishPlatform();
    
        Debug.Log($"Generated {_actualPlatformCount} platforms ({_platformCount} visible + 1 empty) + 1 finish platform.");
    }
    
    /// <summary>
    /// Generates single platform with random gap and deadly segments.
    /// Platform_0 is empty (no segments).
    /// </summary>
    private void GenerateSinglePlatform(int platformIndex)
    {
        float yPosition = -platformIndex * _platformSpacing;
        
        GameObject platformParent = new GameObject($"Platform_{platformIndex}");
        platformParent.transform.SetParent(_helixContainer);
        platformParent.transform.localPosition = new Vector3(0f, yPosition, 0f);
        
        // Platform_0 is empty (starting point)
        if (platformIndex == 0)
        {
            return;
        }
        
        // Randomly determine gap size within range
        int gapSize = Random.Range(_gapSizeRange.x, _gapSizeRange.y + 1);
        int gapStart = Random.Range(0, _segmentsPerPlatform);
        
        // Randomly determine number of deadly segments within range
        int deadlySegmentCount = Random.Range(_deadlySegmentsRange.x, _deadlySegmentsRange.y + 1);
        
        // Randomly select which segments will be deadly (excluding gap segments)
        System.Collections.Generic.HashSet<int> deadlyIndices = new System.Collections.Generic.HashSet<int>();
        int attempts = 0;
        while (deadlyIndices.Count < deadlySegmentCount && attempts < _segmentsPerPlatform * 2)
        {
            int randomIndex = Random.Range(0, _segmentsPerPlatform);
            
            // Don't make gap segments deadly (they don't exist)
            if (!IsSegmentInGap(randomIndex, gapStart, gapSize))
            {
                deadlyIndices.Add(randomIndex);
            }
            attempts++;
        }
        
        // Generate segments
        for (int seg = 0; seg < _segmentsPerPlatform; seg++)
        {
            if (IsSegmentInGap(seg, gapStart, gapSize))
            {
                continue;
            }
            
            bool isDeadly = deadlyIndices.Contains(seg);
            CreatePlatformSegment(seg, platformParent.transform, platformIndex, isDeadly);
        }
    }
    
    /// <summary>
    /// Creates single segment with specified material (safe or deadly).
    /// </summary>
    private void CreatePlatformSegment(int segmentIndex, Transform parent, int platformIndex, bool isDeadly)
    {
        float angle = segmentIndex * _degreesPerSegment;
        Quaternion rotation = Quaternion.Euler(0f, angle, 0f);
        
        GameObject segment = Instantiate(_platformSegmentPrefab, parent.position, rotation, parent);
        segment.name = $"Segment_{segmentIndex}";
        
        // Assign material and tag
        MeshRenderer renderer = segment.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            renderer.material = isDeadly ? _deadlyPlatformMaterial : _safePlatformMaterial;
            segment.tag = isDeadly ? "Deadly" : "Safe";
        }
    }
    
    /// <summary>
    /// Checks if segment is within gap range (handles circular wrapping).
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
    /// Generates the final finish platform with no gaps and no deadly segments.
    /// All segments tagged as "Finish" to trigger game completion.
    /// </summary>
    private void GenerateFinishPlatform()
    {
        int finishPlatformIndex = _actualPlatformCount;
        float yPosition = -finishPlatformIndex * _platformSpacing;
    
        GameObject platformParent = new GameObject($"Platform_Finish");
        platformParent.transform.SetParent(_helixContainer);
        platformParent.transform.localPosition = new Vector3(0f, yPosition, 0f);
    
        // Generate all segments (no gaps, no deadly zones)
        for (int seg = 0; seg < _segmentsPerPlatform; seg++)
        {
            CreateFinishSegment(seg, platformParent.transform);
        }
    
        Debug.Log($"<color=green>Finish platform generated at index {finishPlatformIndex}, Y={yPosition:F2}</color>");
    }

    /// <summary>
    /// Creates a finish platform segment with special material and "Finish" tag.
    /// </summary>
    private void CreateFinishSegment(int segmentIndex, Transform parent)
    {
        float angle = segmentIndex * _degreesPerSegment;
        Quaternion rotation = Quaternion.Euler(0f, angle, 0f);
    
        GameObject segment = Instantiate(_platformSegmentPrefab, parent.position, rotation, parent);
        segment.name = $"Segment_Finish_{segmentIndex}";
    
        // Assign finish material and tag
        MeshRenderer renderer = segment.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            if (_finishPlatformMaterial != null)
            {
                renderer.material = _finishPlatformMaterial;
            }
        
            // Tag as Finish
            segment.tag = "Finish";
        }
    }
}