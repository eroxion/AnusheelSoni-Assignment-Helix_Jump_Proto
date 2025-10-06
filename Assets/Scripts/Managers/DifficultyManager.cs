using UnityEngine;

/// <summary>
/// Manages game difficulty settings and persists across scenes.
/// Singleton with DontDestroyOnLoad to carry difficulty from MainMenu to MainGame.
/// </summary>
public class DifficultyManager : MonoBehaviour
{
    internal static DifficultyManager Instance { get; private set; }
    
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard,
        Expert
    }
    
    [Header("Current Difficulty")]
    [SerializeField] private Difficulty _currentDifficulty = Difficulty.Medium;
    
    // Difficulty parameters
    private float _bounceFrequency;
    private Vector2Int _deadlySegmentsRange;
    
    private void Awake()
    {
        // Singleton with DontDestroyOnLoad
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
        
        // Load saved difficulty
        LoadDifficulty();
        ApplyDifficultySettings();
    }
    
    /// <summary>
    /// Sets difficulty by index (for UI buttons).
    /// PUBLIC because called by Unity UI onClick events.
    /// 0=Easy, 1=Medium, 2=Hard, 3=Expert
    /// </summary>
    public void SetDifficultyByIndex(int index)  // ← MUST BE PUBLIC
    {
        Difficulty difficulty = (Difficulty)index;
        SetDifficulty(difficulty);
    }
    
    /// <summary>
    /// Sets the difficulty level.
    /// INTERNAL because called from code, not UI.
    /// </summary>
    internal void SetDifficulty(Difficulty difficulty)
    {
        _currentDifficulty = difficulty;
        ApplyDifficultySettings();
        SaveDifficulty();
        
        Debug.Log($"Difficulty set to: {difficulty} | Bounce Freq: {_bounceFrequency}, Deadly Segments: {_deadlySegmentsRange.x}-{_deadlySegmentsRange.y}");
    }
    
    /// <summary>
    /// Applies difficulty parameters based on selected difficulty.
    /// </summary>
    private void ApplyDifficultySettings()
    {
        switch (_currentDifficulty)
        {
            case Difficulty.Easy:
                _bounceFrequency = 1.75f;
                _deadlySegmentsRange = new Vector2Int(1, 2);
                break;
                
            case Difficulty.Medium:
                _bounceFrequency = 2.5f;
                _deadlySegmentsRange = new Vector2Int(1, 3);
                break;
                
            case Difficulty.Hard:
                _bounceFrequency = 3.25f;
                _deadlySegmentsRange = new Vector2Int(2, 4);
                break;
                
            case Difficulty.Expert:
                _bounceFrequency = 4.0f;
                _deadlySegmentsRange = new Vector2Int(3, 4);
                break;
        }
    }
    
    private void LoadDifficulty()
    {
        int savedDifficulty = PlayerPrefs.GetInt("Difficulty", 1);
        _currentDifficulty = (Difficulty)savedDifficulty;
        Debug.Log($"Loaded difficulty: {_currentDifficulty}");
    }
    
    private void SaveDifficulty()
    {
        PlayerPrefs.SetInt("Difficulty", (int)_currentDifficulty);
        PlayerPrefs.Save();
    }
    
    // Internal getters
    internal Difficulty CurrentDifficulty => _currentDifficulty;
    internal float BounceFrequency => _bounceFrequency;
    internal Vector2Int DeadlySegmentsRange => _deadlySegmentsRange;
    internal string GetDifficultyName() => _currentDifficulty.ToString();
}