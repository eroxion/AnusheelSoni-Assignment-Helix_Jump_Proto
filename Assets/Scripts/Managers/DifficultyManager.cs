using UnityEngine;

/// <summary>
/// Manages game difficulty settings with per-difficulty high scores.
/// Persists across scenes using DontDestroyOnLoad.
/// Each difficulty has separate high score tracking.
/// </summary>
public class DifficultyManager : MonoBehaviour
{
    internal static DifficultyManager Instance { get; private set; }
    
    internal enum Difficulty
    {
        Easy,
        Medium,
        Hard,
        Expert
    }
    
    [Header("Current Difficulty")]
    [SerializeField] private Difficulty _currentDifficulty = Difficulty.Medium;
    
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
        
        LoadDifficulty();
    }
    
    private void LoadDifficulty()
    {
        int savedDifficulty = PlayerPrefs.GetInt("Difficulty", (int)Difficulty.Medium);
        _currentDifficulty = (Difficulty)savedDifficulty;
        Debug.Log($"Loaded difficulty: {_currentDifficulty}");
    }
    
    private void SaveDifficulty()
    {
        PlayerPrefs.SetInt("Difficulty", (int)_currentDifficulty);
        PlayerPrefs.Save();
    }
    
    /// <summary>
    /// Sets difficulty by index (for UI buttons).
    /// PUBLIC for Unity onClick events.
    /// </summary>
    public void SetDifficultyByIndex(int index)
    {
        if (index >= 0 && index < System.Enum.GetValues(typeof(Difficulty)).Length)
        {
            SetDifficulty((Difficulty)index);
        }
    }
    
    /// <summary>
    /// Sets difficulty and saves to PlayerPrefs.
    /// </summary>
    internal void SetDifficulty(Difficulty difficulty)
    {
        _currentDifficulty = difficulty;
        SaveDifficulty();
        
        Debug.Log($"Difficulty set to: {difficulty} | Bounce Freq: {BounceFrequency}, Deadly Segments: {MinDeadlySegments}-{MaxDeadlySegments}");
    }
    
    // ===== DIFFICULTY PARAMETERS =====
    
    internal Difficulty CurrentDifficulty => _currentDifficulty;
    
    internal float BounceFrequency
    {
        get
        {
            return _currentDifficulty switch
            {
                Difficulty.Easy => 1.75f,
                Difficulty.Medium => 2.5f,
                Difficulty.Hard => 3.25f,
                Difficulty.Expert => 4.0f,
                _ => 2.5f
            };
        }
    }
    
    internal int MinDeadlySegments
    {
        get
        {
            return _currentDifficulty switch
            {
                Difficulty.Easy => 1,
                Difficulty.Medium => 1,
                Difficulty.Hard => 2,
                Difficulty.Expert => 3,
                _ => 1
            };
        }
    }
    
    internal int MaxDeadlySegments
    {
        get
        {
            return _currentDifficulty switch
            {
                Difficulty.Easy => 2,
                Difficulty.Medium => 3,
                Difficulty.Hard => 4,
                Difficulty.Expert => 4,
                _ => 3
            };
        }
    }
    
    // ===== PER-DIFFICULTY HIGH SCORES =====
    
    /// <summary>
    /// Gets high score for specific difficulty.
    /// </summary>
    internal int GetHighScore(Difficulty difficulty)
    {
        string key = $"HighScore_{difficulty}";
        return PlayerPrefs.GetInt(key, 0);
    }
    
    /// <summary>
    /// Gets high score for current difficulty.
    /// </summary>
    internal int GetCurrentDifficultyHighScore()
    {
        return GetHighScore(_currentDifficulty);
    }
    
    /// <summary>
    /// Saves high score for specific difficulty.
    /// </summary>
    internal void SaveHighScore(Difficulty difficulty, int score)
    {
        string key = $"HighScore_{difficulty}";
        PlayerPrefs.SetInt(key, score);
        PlayerPrefs.Save();
    }
    
    /// <summary>
    /// Saves high score for current difficulty.
    /// </summary>
    internal void SaveCurrentDifficultyHighScore(int score)
    {
        SaveHighScore(_currentDifficulty, score);
    }
    
    /// <summary>
    /// Gets high score time for specific difficulty.
    /// </summary>
    internal float GetHighScoreTime(Difficulty difficulty)
    {
        string key = $"HighScoreTime_{difficulty}";
        return PlayerPrefs.GetFloat(key, 0f);
    }
    
    /// <summary>
    /// Gets high score time for current difficulty.
    /// </summary>
    internal float GetCurrentDifficultyHighScoreTime()
    {
        return GetHighScoreTime(_currentDifficulty);
    }
    
    /// <summary>
    /// Saves high score time for specific difficulty.
    /// </summary>
    internal void SaveHighScoreTime(Difficulty difficulty, float time)
    {
        string key = $"HighScoreTime_{difficulty}";
        PlayerPrefs.SetFloat(key, time);
        PlayerPrefs.Save();
    }
    
    /// <summary>
    /// Saves high score time for current difficulty.
    /// </summary>
    internal void SaveCurrentDifficultyHighScoreTime(float time)
    {
        SaveHighScoreTime(_currentDifficulty, time);
    }
    
    /// <summary>
    /// Gets difficulty name as display string.
    /// </summary>
    internal string GetDifficultyName(Difficulty difficulty)
    {
        return difficulty.ToString();
    }
}