using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Manages game scoring, platform tracking, and session time.
/// Tracks current score, high score (with time tiebreaker), and platform passage.
/// High score logic: Higher score wins. If scores equal, lower time wins.
/// Uses singleton pattern for global access.
/// </summary>
public class ScoreManager : MonoBehaviour
{
    internal static ScoreManager Instance { get; private set; }
    
    [Header("Score Settings")]
    [Tooltip("Points awarded per platform passed")]
    [SerializeField] private int _pointsPerPlatform = 1;
    
    [Header("Combo Settings (Optional)")]
    [Tooltip("Enable combo system for bonus points")]
    [SerializeField] private bool _useComboSystem = false;
    
    [Tooltip("Bonus multiplier for consecutive platforms (combo)")]
    [SerializeField] private float _comboMultiplier = 1.5f;
    
    [Tooltip("Max combo chain length")]
    [SerializeField] private int _maxCombo = 5;
    
    private int _currentScore = 0;
    private int _highScore = 0;
    private float _highScoreTime = 999999f; // Time associated with high score
    private int _platformsPassed = 0;
    private int _currentCombo = 0;
    private HashSet<int> _passedPlatformIndices = new HashSet<int>();
    private float _platformSpacing = 4f;
    
    // Time tracking
    private float _sessionStartTime = 0f;
    private float _currentSessionTime = 0f;
    private bool _isTimerRunning = false;
    
    // Events for UI updates
    internal event Action<int> OnScoreChanged;
    internal event Action<int> OnComboChanged;
    internal event Action<float> OnTimeChanged;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    
        LoadHighScore();
        LoadHighScoreTime();
    
        // Initialize current session time
        _currentSessionTime = 0f;
    }
    
    private void Start()
    {
        // Get platform spacing from PlatformGenerator
        PlatformGenerator generator = FindAnyObjectByType<PlatformGenerator>();
        if (generator != null)
        {
            _platformSpacing = generator.PlatformSpacing;
            Debug.Log($"ScoreManager: Using platform spacing {_platformSpacing} from PlatformGenerator");
        }
        else
        {
            Debug.LogWarning("ScoreManager: PlatformGenerator not found, using default spacing 4.0");
        }
        
        string highScoreDisplay = _highScore > 0 
            ? $"High Score: {_highScore} (Time: {FormatTime(_highScoreTime)})" 
            : "High Score: 0";
        Debug.Log($"ScoreManager initialized. {highScoreDisplay}, Combo System: {(_useComboSystem ? "Enabled" : "Disabled")}");
    }
    
    private void Update()
    {
        // Update session time
        if (_isTimerRunning)
        {
            _currentSessionTime = Time.time - _sessionStartTime;
            OnTimeChanged?.Invoke(_currentSessionTime);
        }
    }
    
    /// <summary>
    /// Starts the session timer.
    /// </summary>
    internal void StartTimer()
    {
        _sessionStartTime = Time.time;
        _isTimerRunning = true;
        Debug.Log("Session timer started");
    }
    
    /// <summary>
    /// Stops the session timer.
    /// </summary>
    internal void StopTimer()
    {
        _isTimerRunning = false;
        Debug.Log($"Session timer stopped. Final time: {FormatTime(_currentSessionTime)}");
    }
    
    /// <summary>
    /// Tracks ball position and awards score when passing platforms.
    /// </summary>
    internal void CheckPlatformPassed(float ballY, out int currentPlatformIndex)
    {
        // Use a half-spacing safety offset to avoid off-by-one near boundaries
        float offsetY = -ballY - (_platformSpacing * 0.5f);
        currentPlatformIndex = Mathf.FloorToInt(offsetY / _platformSpacing);

        for (int i = 0; i <= currentPlatformIndex; i++)
        {
            if (_passedPlatformIndices.Add(i))
            {
                _platformsPassed++;

                // Award points for ALL platforms including 0
                int pointsToAdd = _pointsPerPlatform;

                if (_useComboSystem)
                {
                    _currentCombo = Mathf.Min(_currentCombo + 1, _maxCombo);
                    int comboBonus = _currentCombo > 1
                        ? Mathf.RoundToInt(pointsToAdd * (_comboMultiplier - 1f) * (_currentCombo - 1))
                        : 0;
                    pointsToAdd += comboBonus;
                    OnComboChanged?.Invoke(_currentCombo);
                }

                _currentScore += pointsToAdd;
                OnScoreChanged?.Invoke(_currentScore);

                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlayPlatformClear();
                }
            }
        }
    }
    
    /// <summary>
    /// Resets combo chain.
    /// </summary>
    internal void ResetCombo()
    {
        if (!_useComboSystem) return;
        
        if (_currentCombo > 0)
        {
            Debug.Log($"Combo broken! Was at x{_currentCombo}");
            _currentCombo = 0;
            OnComboChanged?.Invoke(_currentCombo);
        }
    }
    
    /// <summary>
    /// Checks if current performance beats high score.
    /// Logic: Higher score wins. If equal scores, lower time wins.
    /// </summary>
    private bool IsNewHighScore(int score, float time)
    {
        // Case 1: Higher score always wins
        if (score > _highScore)
        {
            return true;
        }
        
        // Case 2: Equal score, check time tiebreaker
        if (score == _highScore && time < _highScoreTime)
        {
            return true;
        }
        
        // Case 3: Lower score or equal score but slower time
        return false;
    }
    
    /// <summary>
    /// Called on game over to check and save high score with time.
    /// </summary>
    internal void OnGameOver()
    {
        StopTimer();

        if (IsNewHighScore(_currentScore, _currentSessionTime))
        {
            bool wasScoreIncrease = _currentScore > _highScore;
            bool wasTimeTiebreaker = _currentScore == _highScore && _currentSessionTime < _highScoreTime;
        
            _highScore = _currentScore;
            _highScoreTime = _currentSessionTime;
            SaveHighScore();
            SaveHighScoreTime();
        
            // Play new high score sound
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayNewHighScore();
            }
        
            if (wasScoreIncrease)
            {
                Debug.Log($"<color=yellow>🏆 New High Score! {_highScore} (Time: {FormatTime(_highScoreTime)})</color>");
            }
            else if (wasTimeTiebreaker)
            {
                Debug.Log($"<color=yellow>🏆 New High Score! Same score ({_highScore}) but faster time: {FormatTime(_highScoreTime)}</color>");
            }
        }
    }
    
    /// <summary>
    /// Called when player reaches finish platform (game completion).
    /// </summary>
    internal void OnGameComplete()
    {
        StopTimer();

        if (IsNewHighScore(_currentScore, _currentSessionTime))
        {
            bool wasScoreIncrease = _currentScore > _highScore;
            bool wasTimeTiebreaker = _currentScore == _highScore && _currentSessionTime < _highScoreTime;
        
            _highScore = _currentScore;
            _highScoreTime = _currentSessionTime;
            SaveHighScore();
            SaveHighScoreTime();
        
            // Play new high score sound
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayNewHighScore();
            }
        
            if (wasScoreIncrease)
            {
                Debug.Log($"<color=lime>🎉 Level Complete! 🏆 New High Score! {_highScore} (Time: {FormatTime(_highScoreTime)})</color>");
            }
            else if (wasTimeTiebreaker)
            {
                Debug.Log($"<color=lime>🎉 Level Complete! 🏆 New High Score! Same score ({_highScore}) but faster time: {FormatTime(_highScoreTime)}</color>");
            }
        }
    }
    
    /// <summary>
    /// Resets current game score and time for restart.
    /// </summary>
    internal void ResetScore()
    {
        _currentScore = 0;
        _platformsPassed = 0;
        _currentCombo = 0;
        _passedPlatformIndices.Clear();
        _currentSessionTime = 0f;
        _isTimerRunning = false;
        
        OnScoreChanged?.Invoke(_currentScore);
        OnComboChanged?.Invoke(_currentCombo);
        OnTimeChanged?.Invoke(_currentSessionTime);
        
        Debug.Log("Score and timer reset for new game");
    }
    
    /// <summary>
    /// Loads high score from PlayerPrefs for current difficulty.
    /// </summary>
    private void LoadHighScore()
    {
        if (DifficultyManager.Instance != null)
        {
            _highScore = DifficultyManager.Instance.GetCurrentDifficultyHighScore();
            Debug.Log($"Loaded high score for {DifficultyManager.Instance.CurrentDifficulty}: {_highScore}");
        }
        else
        {
            _highScore = 0;
            Debug.Log("No high score recorded yet");
        }
    }
    
    /// <summary>
    /// Saves high score to PlayerPrefs for current difficulty.
    /// </summary>
    private void SaveHighScore()
    {
        if (DifficultyManager.Instance != null)
        {
            DifficultyManager.Instance.SaveCurrentDifficultyHighScore(_highScore);
            Debug.Log($"High score saved for {DifficultyManager.Instance.CurrentDifficulty}: {_highScore}");
        }
    }
    
    /// <summary>
    /// Loads high score time for current difficulty.
    /// </summary>
    private void LoadHighScoreTime()
    {
        if (DifficultyManager.Instance != null)
        {
            _highScoreTime = DifficultyManager.Instance.GetCurrentDifficultyHighScoreTime();
        
            if (_highScoreTime > 0)
            {
                Debug.Log($"Loaded high score time for {DifficultyManager.Instance.CurrentDifficulty}: {FormatTime(_highScoreTime)}");
            }
            else
            {
                Debug.Log($"No high score time recorded yet for {DifficultyManager.Instance.CurrentDifficulty}");
            }
        }
        else
        {
            _highScoreTime = 0f;
            Debug.Log("No high score time recorded yet");
        }
    }

    /// <summary>
    /// Saves high score time for current difficulty.
    /// </summary>
    private void SaveHighScoreTime()
    {
        if (DifficultyManager.Instance != null)
        {
            DifficultyManager.Instance.SaveCurrentDifficultyHighScoreTime(_highScoreTime);
            Debug.Log($"High score time saved for {DifficultyManager.Instance.CurrentDifficulty}: {FormatTime(_highScoreTime)}");
        }
    }

    /// <summary>
    /// Formats time in seconds to readable string (MM:SS.ss).
    /// </summary>
    internal string FormatTime(float timeInSeconds)
    {
        if (timeInSeconds >= 999999f || timeInSeconds < 0f)
        {
            return "--:--";
        }
    
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        int milliseconds = Mathf.FloorToInt((timeInSeconds * 100f) % 100f);
    
        return $"{minutes:00}:{seconds:00}.{milliseconds:00}";
    }
    
    // Internal getters
    internal int CurrentScore => _currentScore;
    internal int HighScore => _highScore;
    internal float HighScoreTime => _highScoreTime;
    internal int PlatformsPassed => _platformsPassed;
    internal int CurrentCombo => _currentCombo;
    internal bool UseComboSystem => _useComboSystem;
    internal float CurrentSessionTime => _currentSessionTime;
    internal bool IsTimerRunning => _isTimerRunning;
}