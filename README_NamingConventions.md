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
  public class GameManager { }
  public class BallController { }
  public class PlatformGenerator { }
  public interface IInputProvider { }
  public interface IScoreable { }
  ```

### Method Names
- **Format**: PascalCase
- **Rule**: Start with action verbs that describe what the method does
- **Boolean methods**: Phrase as questions (Is/Has/Can prefix)
- **Examples**:
  ```
  public void StartGame() { }
  public void IncrementScore() { }
  public void GeneratePlatforms() { }
  
  // Boolean methods - question format
  public bool IsGameOver() { }
  public bool HasPassedPlatform() { }
  public bool CanRotate() { }
  ```

### Variable & Field Names

#### Public Fields & Properties
- **Format**: PascalCase (no prefix)
- **Usage**: Only for fields/properties accessed by other classes
- **Examples**:
  ```
  public int CurrentScore;
  public float RotationSpeed;
  public GameObject BallPrefab;
  public Transform PlayerTransform;
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
  private Rigidbody _ballRigidbody;
  ```

#### SerializeField (Inspector-Exposed Private Fields)
- **Format**: camelCase with underscore prefix `_`
- **Always private**: Use `[SerializeField]` attribute to show in Inspector
- **Examples**:
  ```
  [SerializeField] private float _rotationSpeed = 150f;
  [SerializeField] private GameObject _ballPrefab;
  [SerializeField] private Material _safePlatformMaterial;
  [SerializeField] private int _maxPlatforms = 50;
  ```

#### Local Variables & Method Parameters
- **Format**: camelCase (no underscore)
- **Usage**: Variables declared inside methods
- **Examples**:
  ```
  void CalculateScore(int platformIndex, float timeBonus)
  {
      int finalScore = platformIndex * 10;
      float multiplier = 1.5f;
      bool isNewRecord = finalScore > _highScore;
  }
  ```

### Constants
- **Format**: ALL_CAPS with underscores separating words
- **Usage**: For values that never change
- **Examples**:
  ```
  private const int MAX_PLATFORMS = 50;
  private const float VERTICAL_SPACING = 2.0f;
  private const string HIGH_SCORE_KEY = "HighScore";
  private const int SEGMENTS_PER_PLATFORM = 12;
  ```

### Events & Delegates
- **Format**: PascalCase with "On" prefix
- **Usage**: For Unity events and custom delegates
- **Examples**:
  ```
  public event Action OnGameStart;
  public event Action OnGameOver;
  public event Action<int> OnScoreChanged;
  public event Action<float> OnTimeUpdated;
  ```

### Enumerations
- **Enum Type**: PascalCase
- **Enum Values**: PascalCase
- **Examples**:
  ```
  public enum GameState
  {
      MainMenu,
      Playing,
      Paused,
      GameOver
  }
  
  public enum PlatformType
  {
      Safe,
      Deadly,
      Bonus
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
  - `GameplayLevel1.unity`

### Prefabs
- **Format**: PascalCase, nouns describing the object
- **Convention**: No suffix needed (name describes what it is)
- **Examples**: 
  - `Ball.prefab`
  - `Platform.prefab`
  - `PlatformSegment.prefab`
  - `MainMenuCanvas.prefab`

### Materials
- **Format**: PascalCase with `_Mat` suffix
- **Convention**: Suffix helps identify asset type quickly
- **Examples**: 
  - `Ball_Mat.mat`
  - `Platform_Safe_Mat.mat`
  - `Platform_Deadly_Mat.mat`
  - `Background_Mat.mat`

### Scripts (C# Files)
- **Format**: PascalCase matching the class name exactly
- **Rule**: Filename MUST match the class name inside (Unity requirement)
- **Examples**: 
  - `GameManager.cs` → contains `public class GameManager`
  - `InputHandler.cs` → contains `public class InputHandler`
  - `BallController.cs` → contains `public class BallController`

### Folders
- **Format**: PascalCase for standard folders
- **Special Rule**: Prefix with underscore `_` for high-priority folders (sorts first)
- **Examples**: 
  - `_Scenes/` (underscore = appears at top of list)
  - `Scripts/`
  - `Prefabs/`
  - `Core/` (subfolder - no underscore)
  - `Gameplay/` (subfolder)

### 3D Models
- **Format**: PascalCase, descriptive nouns
- **Convention**: Name describes the object's function or appearance
- **Examples**: 
  - `PlatformSegment.fbx`
  - `Ball.fbx`
  - `HelixTower.fbx`

### Textures & Sprites
- **Format**: PascalCase with descriptive suffix
- **Examples**:
  - `Button_Play.png`
  - `Icon_Settings.png`
  - `Background_Gradient.png`

### Audio Files
- **Format**: PascalCase with descriptive prefix
- **Examples**:
  - `SFX_BallBounce.wav`
  - `SFX_PlatformHit.wav`
  - `Music_MainTheme.mp3`

---

## Code Style Guidelines

### Indentation & Spacing
- **Indentation**: 4 spaces per level (or 1 tab configured as 4 spaces)
- **Never mix**: Tabs and spaces in the same file
- **Blank lines**: Use single blank line to separate logical code blocks

### Braces Style
- **Opening brace**: New line (Allman/BSD style)
- **Closing brace**: Same indentation as opening statement
- **Example**:
  ```
  void StartGame()
  {
      _isGameRunning = true;
      _currentScore = 0;
      
      if (_isGameRunning)
      {
          StartTimer();
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
  UIManager.Instance.ShowGameOverScreen(
      _currentScore, 
      _currentTime, 
      _highScore, 
      _highScoreTime
  );
  ```

### Comments

#### XML Documentation (Required for public members)
```
/// <summary>
/// Increments the player's score and checks for new high score.
/// </summary>
/// <param name="points">Number of points to add to current score</param>
public void AddScore(int points)
{
    _currentScore += points;
    CheckHighScore();
}
```

#### Inline Comments (Use sparingly)
```
// Only comment complex logic that isn't self-explanatory
private void GeneratePlatform(int level)
{
    float yPosition = level * VERTICAL_SPACING;
    
    // Randomly skip 1-2 consecutive segments to create gap
    int gapStart = Random.Range(0, SEGMENTS_PER_PLATFORM);
    int gapSize = Random.Range(1, 3); // 1 or 2 segments
}
```

---

## Unity-Specific Conventions

### MonoBehaviour Methods
- **Order**: Keep Unity lifecycle methods at top of class in this order:
  ```
  public class ExampleClass : MonoBehaviour
  {
      // 1. Serialized fields
      [SerializeField] private float _speed;
      
      // 2. Private fields
      private int _counter;
      
      // 3. Unity lifecycle methods (in execution order)
      private void Awake() { }
      private void Start() { }
      private void Update() { }
      private void FixedUpdate() { }
      private void LateUpdate() { }
      
      // 4. Public methods
      public void PublicMethod() { }
      
      // 5. Private methods
      private void PrivateMethod() { }
  }
  ```

### SerializeField Usage
- **Always use** instead of making fields public unnecessarily
- **Example**:
  ```
  // ✅ CORRECT - Exposed to Inspector but encapsulated
  [SerializeField] private float _rotationSpeed = 150f;
  
  // ❌ WRONG - Breaks encapsulation
  public float rotationSpeed = 150f;
  ```

### Component References
- **Cache references** in Awake() or Start(), not Update()
- **Example**:
  ```
  private Rigidbody _rb;
  
  private void Awake()
  {
      _rb = GetComponent<Rigidbody>(); // Cache once
  }
  
  private void FixedUpdate()
  {
      _rb.AddForce(Vector3.forward); // Use cached reference
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
   - Correct: `temporaryScore`, `platformData`, `audioManager`, `helixTower`

6. **Public fields without justification**
   - Wrong: `public int score;` (other scripts can modify directly)
   - Correct: `[SerializeField] private int _score;` (controlled access)

7. **Magic numbers** (hardcoded values without explanation)
   - Wrong: `if (score > 1000)` (what is 1000?)
   - Correct: `const int SCORE_THRESHOLD = 1000; if (score > SCORE_THRESHOLD)`

---

## ✅ DO USE:

1. **Clear, self-documenting names**
   - `RotatePlatform()` instead of `Rot()`
   - `currentPlayerScore` instead of `cps`

2. **Consistent naming across project**
   - If you use `_Mat` suffix for materials, use it for ALL materials
   - If you prefix private fields with `_`, do it for ALL private fields

3. **Standard abbreviations only**
   - Common: `UI`, `ID`, `3D`, `2D`, `FPS`, `AI`
   - Acceptable in context: `Pos` (Position), `Rot` (Rotation), `Col` (Color)

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

### Complete Class Example
```
using UnityEngine;

/// <summary>
/// Manages the helix tower rotation based on player input.
/// </summary>
public class HelixRotator : MonoBehaviour
{
    // Constants
    private const float MAX_ROTATION_SPEED = 500f;
    
    // Serialized fields (Inspector-exposed)
    [SerializeField] private float _rotationSpeed = 200f;
    [SerializeField] private Transform _helixContainer;
    
    // Private fields
    private InputHandler _inputHandler;
    private float _currentRotation;
    private bool _canRotate;
    
    // Unity lifecycle methods
    private void Awake()
    {
        _inputHandler = GetComponent<InputHandler>();
        _canRotate = true;
    }
    
    private void Update()
    {
        if (_canRotate)
        {
            RotateHelix();
        }
    }
    
    // Public methods
    public void EnableRotation()
    {
        _canRotate = true;
    }
    
    public void DisableRotation()
    {
        _canRotate = false;
    }
    
    // Private methods
    private void RotateHelix()
    {
        float inputValue = _inputHandler.GetRotationInput();
        _currentRotation = inputValue * _rotationSpeed * Time.deltaTime;
        _helixContainer.Rotate(0f, _currentRotation, 0f);
    }
}
```

---

## Enforcement

These conventions are **mandatory** for all code submissions in this project.

**Benefits of following these standards**:
- Code is easier to read and maintain
- Team members (or reviewers) can understand code quickly
- Reduces bugs from naming confusion
- Professional presentation for portfolio/interviews

**Reference Sources**:
- Microsoft C# Naming Guidelines
- Unity Style Guide Best Practices
- Google C# Style Guide
- Industry-standard game development conventions

---

**Last Updated**: October 5, 2025  
**Project Phase**: Initial Setup  
**Version**: 1.0
```