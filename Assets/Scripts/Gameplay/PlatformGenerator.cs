using UnityEngine;

/// <summary>
/// Generates helix tower with procedurally created platforms.
/// 
/// IMPORTANT: Platform count logic
/// - Inspector value = number of VISIBLE platforms user sees (e.g., 50)
/// - Actual generated = Inspector value + 1 (e.g., 51 total)
/// - Reason: Platform_0 is empty (starting point), so visual count starts from Platform_1
/// - Game design: Cleaner start, pole top visible, better player orientation
/// 
/// Generation: Downward from Y=0 (Platform_0 at origin, others below)
/// </summary>
public class PlatformGenerator : MonoBehaviour
{
    [Header("Platform Configuration")]
    [Tooltip("Number of VISIBLE platforms (actual generated = this + 1, because Platform_0 is empty)")]
    [SerializeField] private GameObject _platformSegmentPrefab;
    [SerializeField] private int _platformCount = 50;
    [SerializeField] private float _platformSpacing = 4.0f;
    
    [Header("Segment Configuration")]
    [SerializeField] private int _segmentsPerPlatform = 12;
    [SerializeField] private int _minGapSize = 1;
    [SerializeField] private int _maxGapSize = 3;
    
    [Header("Materials")]
    [SerializeField] private Material _safePlatformMaterial;
    [SerializeField] private Material _deadlyPlatformMaterial;
    [SerializeField] [Range(0f, 1f)] private float _deadlySegmentChance = 0.2f;
    
    [Header("Central Cylinder")]
    [SerializeField] private Material _centralCylinderMaterial;
    [SerializeField] private float _cylinderRadius = 0.3f;
    
    [Header("Organization")]
    [SerializeField] private Transform _helixContainer;
    
    private float _degreesPerSegment;
    private int _actualPlatformCount; // Visual platforms + 1 empty starting platform
    
    private void Awake()
    {
        _degreesPerSegment = 360f / _segmentsPerPlatform;
        
        // Add 1 to platform count for empty starting platform (Platform_0)
        // Visual platforms: 50 (Platform_1 to Platform_50 have segments)
        _actualPlatformCount = _platformCount + 1;
    }
    
    private void Start()
    {
        GenerateCentralCylinder();
        GenerateAllPlatforms();
    }
    
    /// <summary>
    /// Creates central pole spanning from Platform_0 (Y=0) to bottom platform.
    /// Positioned at center of tower for proper visual alignment.
    /// </summary>
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
        
        // Calculate tower height from Platform_0 (Y=0) to last platform
        float totalHeight = (_actualPlatformCount - 1) * _platformSpacing;
        
        // Position cylinder at center of tower
        // Tower spans from Y=0 to Y=-totalHeight
        // Center = -totalHeight/2
        float cylinderY = -totalHeight / 2f;
        cylinder.transform.localPosition = new Vector3(0f, cylinderY, 0f);
        
        // Scale cylinder (Unity cylinder default height = 2, so divide by 2)
        float cylinderScaleY = totalHeight / 2f;
        cylinder.transform.localScale = new Vector3(
            _cylinderRadius * 2f,  // Diameter = radius * 2
            cylinderScaleY,
            _cylinderRadius * 2f
        );
        
        // Apply material
        if (_centralCylinderMaterial != null)
        {
            cylinder.GetComponent<MeshRenderer>().material = _centralCylinderMaterial;
        }
        
        // Remove collider (visual only, no physics interaction)
        Destroy(cylinder.GetComponent<Collider>());
        
        Debug.Log($"Cylinder: Height={totalHeight:F2}, Position Y={cylinderY:F2}, " +
                  $"Spans from Y=0 (top) to Y={-totalHeight:F2} (bottom)");
    }
    
    /// <summary>
    /// Generates all platform levels including empty starting platform.
    /// </summary>
    private void GenerateAllPlatforms()
    {
        if (_platformSegmentPrefab == null || _helixContainer == null)
        {
            Debug.LogError("PlatformGenerator: Missing prefab or container!");
            return;
        }
        
        // Generate actual platform count (includes empty Platform_0)
        for (int i = 0; i < _actualPlatformCount; i++)
        {
            GenerateSinglePlatform(i);
        }
        
        Debug.Log($"Generated {_actualPlatformCount} total platforms " +
                  $"({_platformCount} visible + 1 empty starting platform).");
    }
    
    /// <summary>
    /// Generates single platform at index position.
    /// Platform_0 is empty (no segments) for clean starting point.
    /// Other platforms have segments with random gaps.
    /// </summary>
    private void GenerateSinglePlatform(int platformIndex)
    {
        // Calculate Y position downward from origin
        float yPosition = -platformIndex * _platformSpacing;
        
        // Create platform parent GameObject
        GameObject platformParent = new GameObject($"Platform_{platformIndex}");
        platformParent.transform.SetParent(_helixContainer);
        platformParent.transform.localPosition = new Vector3(0f, yPosition, 0f);
        
        // Platform_0 is empty (starting point) - no segments generated
        // Provides clean visual start and clear view of pole top
        if (platformIndex == 0)
        {
            return;
        }
        
        // Generate segments for all other platforms
        int gapStart = Random.Range(0, _segmentsPerPlatform);
        int gapSize = Random.Range(_minGapSize, _maxGapSize);
        
        for (int seg = 0; seg < _segmentsPerPlatform; seg++)
        {
            if (IsSegmentInGap(seg, gapStart, gapSize))
            {
                continue;
            }
            
            CreatePlatformSegment(seg, platformParent.transform, platformIndex);
        }
    }
    
    /// <summary>
    /// Creates single segment at specified rotation angle.
    /// </summary>
    private void CreatePlatformSegment(int segmentIndex, Transform parent, int platformIndex)
    {
        float angle = segmentIndex * _degreesPerSegment;
        Quaternion rotation = Quaternion.Euler(0f, angle, 0f);
        
        GameObject segment = Instantiate(_platformSegmentPrefab, parent.position, rotation, parent);
        segment.name = $"Segment_{segmentIndex}";
        
        AssignSegmentMaterial(segment);
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
    /// Randomly assigns safe or deadly material to segment.
    /// </summary>
    private void AssignSegmentMaterial(GameObject segment)
    {
        MeshRenderer renderer = segment.GetComponent<MeshRenderer>();
        if (renderer == null) return;
        
        bool isDeadly = Random.value < _deadlySegmentChance;
        
        renderer.material = isDeadly ? _deadlyPlatformMaterial : _safePlatformMaterial;
        segment.tag = isDeadly ? "Deadly" : "Safe";
    }
}