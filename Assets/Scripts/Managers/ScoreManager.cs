using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Manages game scoring and platform tracking.
/// Tracks current score, high score, and platform passage.
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
    
    private int _currentScore;
    private int _highScore;
    private int _platformsPassed;
    private int _currentCombo;
    private HashSet<int> _passedPlatformIndices;
    
    // Events for UI updates
    internal event Action<int> OnScoreChanged;
    internal event Action<int> OnComboChanged;
    
    private void Awake()
    {
        _currentScore = 0;
        _highScore = 0;
        _platformsPassed = 0;
        _currentCombo = 0;
        _passedPlatformIndices = new HashSet<int>();
        
        // Singleton pattern
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
    }
    
    private void Start()
    {
        Debug.Log($"ScoreManager initialized. High Score: {_highScore}, Combo System: {(_useComboSystem ? "Enabled" : "Disabled")}");
    }
    
    /// <summary>
    /// Awards points when ball passes through platform gaps (position-based).
    /// Prevents duplicate scoring and edge-glitch exploits using HashSet.
    /// Called continuously to track Y position.
    /// </summary>
    internal void CheckPlatformPassed(float ballYPosition, out int currentPlatformIndex)
    {
        // Calculate which platform level ball is currently at or below
        // Platform_0 at Y=0, Platform_1 at Y=-4, Platform_2 at Y=-8, etc.
        currentPlatformIndex = Mathf.FloorToInt(-ballYPosition / 4f); // Assuming 4 unit spacing
    
        // Award points for all platforms that ball has passed but not yet scored
        for (int i = 1; i <= currentPlatformIndex; i++) // Start from 1 (skip Platform_0)
        {
            // Check if already scored this platform
            if (!_passedPlatformIndices.Contains(i))
            {
                // Mark as passed and award score
                _passedPlatformIndices.Add(i);
                _platformsPassed++;
            
                // Calculate score
                int pointsToAdd = _pointsPerPlatform;
            
                if (_useComboSystem)
                {
                    _currentCombo = Mathf.Min(_currentCombo + 1, _maxCombo);
                    int comboBonus = _currentCombo > 1 ? Mathf.RoundToInt(pointsToAdd * (_comboMultiplier - 1f) * (_currentCombo - 1)) : 0;
                    pointsToAdd += comboBonus;
                    OnComboChanged?.Invoke(_currentCombo);
                }
            
                _currentScore += pointsToAdd;
                OnScoreChanged?.Invoke(_currentScore);
            
                string comboText = _useComboSystem ? $", Combo: x{_currentCombo}" : "";
                Debug.Log($"Platform {i} passed! Score: {_currentScore} (+{pointsToAdd}){comboText}, Total: {_platformsPassed}");
            }
        }
    }
    
    /// <summary>
    /// Resets combo chain (called when ball bounces on platform).
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
    /// Called on game over to check and save high score.
    /// </summary>
    internal void OnGameOver()
    {
        if (_currentScore > _highScore)
        {
            _highScore = _currentScore;
            SaveHighScore();
            Debug.Log($"New High Score! {_highScore}");
        }
        else
        {
            Debug.Log($"Game Over. Score: {_currentScore}, High Score: {_highScore}");
        }
    }
    
    /// <summary>
    /// Called when player reaches finish platform (game completion).
    /// Saves high score and marks as victory.
    /// </summary>
    internal void OnGameComplete()
    {
        if (_currentScore > _highScore)
        {
            _highScore = _currentScore;
            SaveHighScore();
            Debug.Log($"<color=yellow>New High Score on completion! {_highScore}</color>");
        }
        else
        {
            Debug.Log($"<color=lime>Level Complete! Score: {_currentScore}, High Score: {_highScore}</color>");
        }
    
        // TODO: Trigger victory UI
        // Different from game over - this is success!
    }
    
    /// <summary>
    /// Resets current game score for restart.
    /// </summary>
    internal void ResetScore()
    {
        _currentScore = 0;
        _platformsPassed = 0;
        _currentCombo = 0;
        _passedPlatformIndices.Clear();
        
        OnScoreChanged?.Invoke(_currentScore);
        OnComboChanged?.Invoke(_currentCombo);
        
        Debug.Log("Score reset for new game");
    }
    
    /// <summary>
    /// Loads high score from PlayerPrefs.
    /// </summary>
    private void LoadHighScore()
    {
        _highScore = PlayerPrefs.GetInt("HighScore", 0);
    }
    
    /// <summary>
    /// Saves high score to PlayerPrefs.
    /// </summary>
    private void SaveHighScore()
    {
        PlayerPrefs.SetInt("HighScore", _highScore);
        PlayerPrefs.Save();
    }
    
    // Internal getters
    internal int CurrentScore => _currentScore;
    internal int HighScore => _highScore;
    internal int PlatformsPassed => _platformsPassed;
    internal int CurrentCombo => _currentCombo;
    internal bool UseComboSystem => _useComboSystem;
}
