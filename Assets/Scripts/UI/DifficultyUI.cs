using UnityEngine;
using TMPro;

/// <summary>
/// Manages difficulty selection UI including high score and time display.
/// Updates high scores when panel is opened.
/// </summary>
public class DifficultyUI : MonoBehaviour
{
    [Header("High Score Texts")]
    [SerializeField] private TextMeshProUGUI _easyHighScoreText;
    [SerializeField] private TextMeshProUGUI _mediumHighScoreText;
    [SerializeField] private TextMeshProUGUI _hardHighScoreText;
    [SerializeField] private TextMeshProUGUI _expertHighScoreText;
    
    [Header("Best Time Texts")]
    [SerializeField] private TextMeshProUGUI _easyBestTimeText;
    [SerializeField] private TextMeshProUGUI _mediumBestTimeText;
    [SerializeField] private TextMeshProUGUI _hardBestTimeText;
    [SerializeField] private TextMeshProUGUI _expertBestTimeText;
    
    private void OnEnable()
    {
        UpdateAllHighScores();
    }
    
    /// <summary>
    /// Updates high score and time display for all difficulties.
    /// </summary>
    private void UpdateAllHighScores()
    {
        if (DifficultyManager.Instance == null) return;
        
        UpdateDifficultyDisplay(_easyHighScoreText, _easyBestTimeText, DifficultyManager.Difficulty.Easy);
        UpdateDifficultyDisplay(_mediumHighScoreText, _mediumBestTimeText, DifficultyManager.Difficulty.Medium);
        UpdateDifficultyDisplay(_hardHighScoreText, _hardBestTimeText, DifficultyManager.Difficulty.Hard);
        UpdateDifficultyDisplay(_expertHighScoreText, _expertBestTimeText, DifficultyManager.Difficulty.Expert);
    }
    
    /// <summary>
    /// Updates a single difficulty's high score and time.
    /// </summary>
    private void UpdateDifficultyDisplay(TextMeshProUGUI scoreText, TextMeshProUGUI timeText, DifficultyManager.Difficulty difficulty)
    {
        int highScore = DifficultyManager.Instance.GetHighScore(difficulty);
        float highScoreTime = DifficultyManager.Instance.GetHighScoreTime(difficulty);
        
        // Update score text
        if (scoreText != null)
        {
            if (highScore > 0)
            {
                scoreText.text = $"Best: {highScore}";
            }
            else
            {
                scoreText.text = "Best: ---";
            }
        }
        
        // Update time text
        if (timeText != null)
        {
            if (highScoreTime > 0)
            {
                timeText.text = $"Time: {FormatTime(highScoreTime)}";
            }
            else
            {
                timeText.text = "Time: --:--";
            }
        }
    }
    
    /// <summary>
    /// Formats time in MM:SS format.
    /// </summary>
    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        return $"{minutes:00}:{seconds:00}";
    }
}