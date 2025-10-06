# Helix Jump - Naming Conventions Guide

## Project: AnusheelSoni-Assignment-Helix_Jump_Proto

This document defines all naming standards for code, assets, and files in this project.

---

## C# Script Naming Standards

### Class & Interface Names
- **Format**: PascalCase
- **Rule**: Use nouns or noun phrases that describe the entity
- **Examples**:
  ```
  public class AudioManager { }
  public class BallController { }
  public class PlatformGenerator { }
  public interface IInputProvider { }
  public interface IPoolable { }
  ```

### Method Names
- **Format**: PascalCase
- **Rule**: Start with action verbs that describe what the method does
- **Boolean methods**: Phrase as questions (Is/Has/Can prefix)
- **Examples**:
  ```
  public void StartTimer() { }
  public void AddScore(int points) { }
  public void GeneratePlatformSegments() { }
  public void TransferTopCylinderToBottom() { }
  
  // Boolean methods - question format
  public bool IsGameOver() { }
  public bool IsNewHighScore(int score, float time) { }
  public bool CanRotate() { }
  ```

### Variable & Field Names

#### Internal Fields (internal keyword)
- **Format**: PascalCase (no prefix)
- **Usage**: For singleton instance access and controlled exposure
- **Examples**:
  ```
  internal static AudioManager Instance { get; private set; }
  internal int CurrentScore => _currentScore;
  internal float HighScoreTime => _highScoreTime;
  internal bool IsTimerRunning => _isTimerRunning;
  ```

#### Private Fields
- **Format**: camelCase with underscore prefix `_`
- **Usage**: For all internal class data
- **Examples**:
  ```
  private int _currentScore;
  private float _elapsedTime;
  private bool _isGameRunning;
  private Transform _ballTransform;
  private Rigidbody _rigidbody;
  private List<GameObject> _platformPool;
  private Dictionary<int, GameObject> _platformsByIndex;
  ```

#### SerializeField (Inspector-Exposed Private Fields)
- **Format**: camelCase with underscore prefix `_`
- **Always private**: Use `[SerializeField]` attribute to show in Inspector
- **Examples**:
  ```
  [SerializeField] private float _rotationSpeed = 150f;
  [SerializeField] private GameObject _platformSegmentPrefab;
  [SerializeField] private Material _safePlatformMaterial;
  [SerializeField] private int _initialPlatformCount = 20;
  [SerializeField] private AudioClip _ballBounceSFX;
  [SerializeField] private Vector2Int _gapSizeRange = new Vector2Int(1, 3);
  ```

#### Local Variables & Method Parameters
- **Format**: camelCase (no underscore)
- **Usage**: Variables declared inside methods
- **Examples**:
  ```
  void CheckPlatformPassed(float ballY, out int currentPlatformIndex)
  {
      int poolIndex = platformIndex % _platformPool.Count;
      float randomYRotation = Random.Range(0f, 360f);
      bool isNewRecord = score > _highScore;
  }
  ```

### Constants
- **Format**: ALL_CAPS with underscores separating words (unused in project)
- **Alternative Format**: PascalCase for const fields (used in project)
- **Usage**: For values that never change
- **Examples**:
  ```
  private const int MAX_PLATFORMS = 50;
  private const float VERTICAL_SPACING = 2.0f;
  private const string HIGH_SCORE_KEY = "HighScore";
  ```

### Events & Delegates
- **Format**: PascalCase with "On" prefix
- **Usage**: For Unity events and custom delegates
- **Examples**:
  ```
  internal event Action<int> OnScoreChanged;
  internal event Action<int> OnComboChanged;
  internal event Action<float> OnTimeChanged;
  ```

### Enumerations
- **Enum Type**: PascalCase
- **Enum Values**: PascalCase
- **Examples**:
  ```
  public enum Difficulty
  {
      Easy,
      Medium,
      Hard,
      Expert
  }
  
  public enum GameState
  {
      MainMenu,
      Playing,
      Paused,
      GameOver
  }
  ```

---

## Unity Asset Naming

### Scenes
- **Format**: PascalCase, descriptive nouns
- **Convention**: Action or location-based names
- **Examples**: 
  - `MainMenu.unity`
  - `MainGame.unity`

### Prefabs
- **Format**: PascalCase, nouns describing the object
- **Convention**: No suffix needed (name describes what it is)
- **Examples**: 
  - `Ball.prefab`
  - `PlatformSegment.prefab`

### Materials
- **Format**: PascalCase with `_Mat` suffix
- **Convention**: Suffix helps identify asset type quickly
- **Examples**: 
  - `Ball_Mat.mat`
  - `BallTrail_Mat.mat`
  - `Platform_Safe_Mat.mat`
  - `Platform_Deadly_Mat.mat`
  - `CentralPole_Mat.mat`

### Scripts (C# Files)
- **Format**: PascalCase matching the class name exactly
- **Rule**: Filename MUST match the class name inside (Unity requirement)
- **Examples**: 
  - `AudioManager.cs` → contains `public class AudioManager`
  - `InputHandler.cs` → contains `public class InputHandler`
  - `BallController.cs` → contains `public class BallController`
  - `PlatformGenerator.cs` → contains `public class PlatformGenerator`

### Folders
- **Format**: PascalCase for standard folders
- **Special Rule**: Prefix with underscore `_` for high-priority folders (sorts first)
- **Examples**: 
  - `_Scenes/` (underscore = appears at top of list)
  - `Scripts/`
  - `Prefabs/`
  - `Managers/` (subfolder - no underscore)
  - `Gameplay/` (subfolder)
  - `Input/` (subfolder)
  - `UI/` (subfolder)

### 3D Models
- **Format**: PascalCase, descriptive nouns with `.fbx` extension
- **Convention**: Name describes the object's function or appearance
- **Examples**: 
  - `PlatformSegment.fbx`

### Audio Files
- **Format**: PascalCase with `_BGM` or `_SFX` suffix
- **Extension**: `.ogg` (Vorbis format for WebGL)
- **Examples**:
  - `MainMenu_BGM.ogg`
  - `MainGame_BGM.ogg`
  - `Ball_Bounce_SFX.ogg`
  - `Button_Click_SFX.ogg`
  - `Death_SFX.ogg`
  - `Level_Win_SFX.ogg`
  - `New_HighScore_SFX.ogg`
  - `Platform_Clear_SFX.ogg`

---

## Code Style Guidelines

### Indentation & Spacing
- **Indentation**: 4 spaces per level (configured in JetBrains Rider)
- **Never mix**: Tabs and spaces in the same file
- **Blank lines**: Use single blank line to separate logical code blocks

### Braces Style
- **Opening brace**: New line (Allman/BSD style)
- **Closing brace**: Same indentation as opening statement
- **Example**:
  ```
  void StartTimer()
  {
      _sessionStartTime = Time.time;
      _isTimerRunning = true;
      
      if (_isTimerRunning)
      {
          Debug.Log("Session timer started");
      }
  }
  ```

### Spacing Around Operators
- **Binary operators**: Space on both sides
- **Commas**: Space after, not before
- **Parentheses**: No space inside
- **Example**:
  ```
  int total = score + bonus;
  float result = CalculateScore(10, 5.5f);
  if (score > 0 && time < maxTime)
  {
      // logic
  }
  ```

### Line Length
- **Recommended**: Keep lines under 120 characters
- **Break long lines**: At logical points (after commas, operators)
- **Example**:
  ```
  // Good - broken at logical point
  GameObject segment = Instantiate(
      _platformSegmentPrefab, 
      parent.position, 
      rotation, 
      parent
  );
  ```

### Comments

#### XML Documentation (Required for internal/public members)
```
/// <summary>
/// Checks if ball has passed a platform and awards score.
/// Called every frame by BallController.
/// </summary>
/// <param name="ballY">Current Y position of the ball</param>
/// <param name="currentPlatformIndex">Output parameter for current platform index</param>
internal void CheckPlatformPassed(float ballY, out int currentPlatformIndex)
{
    currentPlatformIndex = Mathf.FloorToInt(-ballY / 4f);
    // Implementation...
}
```

#### Inline Comments (Use for complex logic)
```
// Only comment non-obvious logic
private void RepositionPlatform(GameObject platform)
{
    _lowestPlatformY -= _platformSpacing;
    platform.transform.localPosition = new Vector3(0f, _lowestPlatformY, 0f);
    
    // Apply new random Y rotation for visual variety
    float randomYRotation = Random.Range(0f, 360f);
    platform.transform.localRotation = Quaternion.Euler(0f, randomYRotation, 0f);
}
```

---

## Unity-Specific Conventions

### MonoBehaviour Methods
- **Order**: Keep Unity lifecycle methods at top of class in execution order:
  ```
  public class ExampleClass : MonoBehaviour
  {
      // 1. Serialized fields with headers
      [Header("Configuration")]
      [SerializeField] private float _speed;
      
      // 2. Private fields
      private int _counter;
      
      // 3. Unity lifecycle methods (in execution order)
      private void Awake() { }
      private void Start() { }
      private void Update() { }
      private void FixedUpdate() { }
      private void LateUpdate() { }
      
      // 4. Internal methods (singleton access, getters)
      internal void PublicMethod() { }
      
      // 5. Private methods
      private void PrivateMethod() { }
  }
  ```

### SerializeField Usage
- **Always use** instead of making fields public unnecessarily
- **Include [Header] and [Tooltip]** for organization in Inspector
- **Example**:
  ```
  // ✅ CORRECT - Exposed to Inspector but encapsulated
  [Header("Rotation Settings")]
  [Tooltip("Base rotation speed in degrees per second")]
  [SerializeField] private float _rotationSpeed = 150f;
  
  // ❌ WRONG - Breaks encapsulation
  public float rotationSpeed = 150f;
  ```

### Component References
- **Cache references** in Awake() or Start(), not Update()
- **Example**:
  ```
  private Rigidbody _rigidbody;
  
  private void Awake()
  {
      _rigidbody = GetComponent<Rigidbody>(); // Cache once
  }
  
  private void FixedUpdate()
  {
      _rigidbody.AddForce(Vector3.forward); // Use cached reference
  }
  ```

### Singleton Pattern (Used for Managers)
```
public class AudioManager : MonoBehaviour
{
    internal static AudioManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // Initialize...
    }
}
```

---

## Prohibited Patterns

### ❌ DO NOT USE:

1. **Spaces in file/folder names**
   - Wrong: `Platform Segment.prefab`
   - Correct: `PlatformSegment.prefab`

2. **Hungarian notation** (type prefixes)
   - Wrong: `int intScore`, `string strName`
   - Correct: `int score`, `string playerName`

3. **Unclear abbreviations**
   - Wrong: `PltfrmGen.cs`, `BllCtrl.cs`
   - Correct: `PlatformGenerator.cs`, `BallController.cs`

4. **Single-letter variables** (except loop counters)
   - Wrong: `int x = 5;`, `float t;`
   - Correct: `int score = 5;`, `float elapsedTime;`
   - Exception: `for (int i = 0; i < count; i++)` ✅

5. **Ambiguous generic names**
   - Wrong: `temp`, `data`, `manager2`, `thing`
   - Correct: `temporaryScore`, `platformData`, `audioManager`, `helixContainer`

6. **Public fields without justification**
   - Wrong: `public int score;` (other scripts can modify directly)
   - Correct: `[SerializeField] private int _score;` (controlled access)
   - Use `internal` properties for controlled external access

7. **Magic numbers** (hardcoded values without explanation)
   - Wrong: `if (score > 1000)` (what is 1000?)
   - Correct: Store in serialized field or add comment explaining significance

---

## ✅ DO USE:

1. **Clear, self-documenting names**
   - `RepositionPlatform()` instead of `Repos()`
   - `currentPlatformIndex` instead of `cpi`

2. **Consistent naming across project**
   - If you use `_Mat` suffix for materials, use it for ALL materials
   - If you prefix private fields with `_`, do it for ALL private fields

3. **Standard abbreviations only**
   - Common: `UI`, `ID`, `3D`, `2D`, `FPS`, `BGM`, `SFX`
   - Acceptable in context: `Pos` (Position), `Rot` (Rotation), `SFX` (Sound Effects)

4. **Meaningful loop variables**
   - Simple loops: `for (int i = 0; i < count; i++)` ✅
   - Nested loops: Use descriptive names
     ```
     for (int platformIndex = 0; platformIndex < platforms.Length; platformIndex++)
     {
         for (int segmentIndex = 0; segmentIndex < segments.Length; segmentIndex++)
         {
             // Clear what each index represents
         }
     }
     ```

---

## Examples of Good Naming

### Complete Class Example (from project)
```
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Pure object pooling for infinite gameplay.
/// Platforms generated once with random rotations, then only repositioned.
/// </summary>
public class PlatformGenerator : MonoBehaviour
{
    [Header("Platform Configuration")]
    [SerializeField] private GameObject _platformSegmentPrefab;
    [SerializeField] private int _initialPlatformCount = 20;
    [SerializeField] private float _platformSpacing = 4.0f;
    
    [Header("Central Cylinder")]
    [SerializeField] private Material _centralCylinderMaterial;
    [SerializeField] private float _cylinderRadius = 0.3f;
    
    // Object pooling
    private List<GameObject> _platformPool = new List<GameObject>();
    private int _highestPassedIndex = -1;
    private float _lowestPlatformY = 0f;
    
    private void Start()
    {
        GenerateCylinders();
        GenerateInitialPlatforms();
    }
    
    private void Update()
    {
        CheckPlatformsToRecycle();
    }
    
    /// <summary>
    /// Repositions platform to bottom with new random Y rotation.
    /// </summary>
    private void RepositionPlatform(GameObject platform)
    {
        _lowestPlatformY -= _platformSpacing;
        platform.transform.localPosition = new Vector3(0f, _lowestPlatformY, 0f);
        
        float randomYRotation = Random.Range(0f, 360f);
        platform.transform.localRotation = Quaternion.Euler(0f, randomYRotation, 0f);
    }
}
```

---

## Enforcement

These conventions are **mandatory** for all code in this project.

**Benefits of following these standards**:
- Code is easier to read and maintain
- Reviewers can understand implementation quickly
- Reduces bugs from naming confusion
- Professional presentation for portfolio

**Reference Sources**:
- Microsoft C# Naming Guidelines
- Unity Style Guide Best Practices
- JetBrains Rider code style recommendations
- Industry-standard game development conventions

---

**Last Updated**: October 7, 2025  
**Project Status**: Complete - Ready for Submission  
**Development Time**: 3 days