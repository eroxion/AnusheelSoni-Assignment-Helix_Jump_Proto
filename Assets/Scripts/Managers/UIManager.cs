using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages all UI elements and screen transitions.
/// Handles score display, time display, game over screen, victory screen, and countdown.
/// </summary>
public class UIManager : MonoBehaviour
{
    internal static UIManager Instance { get; private set; }
    
    [Header("HUD Elements")]
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _countdownText;
    
    [Header("Game Over Screen")]
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private TextMeshProUGUI _gameOverFinalScore;
    [SerializeField] private TextMeshProUGUI _gameOverFinalTime;
    [SerializeField] private TextMeshProUGUI _gameOverHighScore;
    [SerializeField] private TextMeshProUGUI _gameOverBestTime;
    
    [Header("Victory Screen")]
    [SerializeField] private GameObject _victoryPanel;
    [SerializeField] private TextMeshProUGUI _victoryFinalScore;
    [SerializeField] private TextMeshProUGUI _victoryFinalTime;
    [SerializeField] private TextMeshProUGUI _victoryHighScore;
    [SerializeField] private TextMeshProUGUI _victoryBestTime;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.OnScoreChanged += UpdateScoreDisplay;
            ScoreManager.Instance.OnTimeChanged += UpdateTimeDisplay;
        }
        
        UpdateScoreDisplay(0);
        UpdateTimeDisplay(0f);
        HideAllScreens();
        HideHUD(); // Start with HUD hidden
    }
    
    private void OnDestroy()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.OnScoreChanged -= UpdateScoreDisplay;
            ScoreManager.Instance.OnTimeChanged -= UpdateTimeDisplay;
        }
    }
    
    private void UpdateScoreDisplay(int score)
    {
        if (_scoreText != null)
        {
            _scoreText.text = $"Score: {score}";
        }
    }
    
    private void UpdateTimeDisplay(float timeInSeconds)
    {
        if (_timeText != null && ScoreManager.Instance != null)
        {
            _timeText.text = $"Time: {ScoreManager.Instance.FormatTime(timeInSeconds)}";
        }
    }
    
    internal void UpdateCountdown(int seconds)
    {
        if (_countdownText != null)
        {
            if (seconds > 0)
            {
                _countdownText.text = seconds.ToString();
                _countdownText.gameObject.SetActive(true);
                
                // Keep score and time hidden during countdown
                if (_scoreText != null)
                {
                    _scoreText.gameObject.SetActive(false);
                }
                
                if (_timeText != null)
                {
                    _timeText.gameObject.SetActive(false);
                }
            }
            else
            {
                _countdownText.gameObject.SetActive(false);
            }
        }
    }
    
    internal void ShowGoMessage()
    {
        if (_countdownText != null)
        {
            _countdownText.text = "GO!";
            _countdownText.gameObject.SetActive(true);
            
            // Schedule HUD to appear after 1 second
            Invoke(nameof(HideCountdownAndShowHUD), 1f);
        }
    }
    
    private void HideCountdownAndShowHUD()
    {
        if (_countdownText != null)
        {
            _countdownText.gameObject.SetActive(false);
        }
        
        // Show score and time
        if (_scoreText != null)
        {
            _scoreText.gameObject.SetActive(true);
        }
        
        if (_timeText != null)
        {
            _timeText.gameObject.SetActive(true);
        }
        
        // Start timer
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.StartTimer();
        }
    }
    
    /// <summary>
    /// Shows game over screen with final scores and time.
    /// </summary>
    internal void ShowGameOverScreen()
    {
        if (_gameOverPanel == null) return;
        
        // Cancel any pending HUD show
        CancelInvoke(nameof(HideCountdownAndShowHUD));
        HideHUD();
        
        _gameOverPanel.SetActive(true);
        
        if (ScoreManager.Instance != null)
        {
            int finalScore = ScoreManager.Instance.CurrentScore;
            int highScore = ScoreManager.Instance.HighScore;
            float finalTime = ScoreManager.Instance.CurrentSessionTime;
            float highScoreTime = ScoreManager.Instance.HighScoreTime; // CHANGED: Use high score time
            
            if (_gameOverFinalScore != null)
            {
                _gameOverFinalScore.text = $"Score: {finalScore}";
            }
            
            if (_gameOverFinalTime != null)
            {
                _gameOverFinalTime.text = $"Time: {ScoreManager.Instance.FormatTime(finalTime)}";
            }
            
            if (_gameOverHighScore != null)
            {
                _gameOverHighScore.text = $"High Score: {highScore}";
            }
            
            if (_gameOverBestTime != null)
            {
                // CHANGED: Show time associated with high score
                string highScoreTimeText = ScoreManager.Instance.FormatTime(highScoreTime);
                _gameOverBestTime.text = $"Best Time: {highScoreTimeText}";
            }
        }
        
        Debug.Log("Game Over screen displayed");
    }

    /// <summary>
    /// Shows victory screen with final scores and time.
    /// </summary>
    internal void ShowVictoryScreen()
    {
        if (_victoryPanel == null) return;
        
        // Cancel any pending HUD show
        CancelInvoke(nameof(HideCountdownAndShowHUD));
        HideHUD();
        
        _victoryPanel.SetActive(true);
        
        if (ScoreManager.Instance != null)
        {
            int finalScore = ScoreManager.Instance.CurrentScore;
            int highScore = ScoreManager.Instance.HighScore;
            float finalTime = ScoreManager.Instance.CurrentSessionTime;
            float highScoreTime = ScoreManager.Instance.HighScoreTime; // CHANGED: Use high score time
            
            if (_victoryFinalScore != null)
            {
                _victoryFinalScore.text = $"Score: {finalScore}";
            }
            
            if (_victoryFinalTime != null)
            {
                _victoryFinalTime.text = $"Time: {ScoreManager.Instance.FormatTime(finalTime)}";
            }
            
            if (_victoryHighScore != null)
            {
                _victoryHighScore.text = $"High Score: {highScore}";
            }
            
            if (_victoryBestTime != null)
            {
                // CHANGED: Show time associated with high score
                string highScoreTimeText = ScoreManager.Instance.FormatTime(highScoreTime);
                
                // Highlight if this run achieved new high score
                bool isNewHighScore = (finalScore > highScore) || 
                                     (finalScore == highScore && finalTime < highScoreTime);
                
                if (isNewHighScore && Mathf.Abs(finalTime - highScoreTime) < 0.1f)
                {
                    _victoryBestTime.text = $"<color=yellow>Best Time: {highScoreTimeText} ⭐ NEW!</color>";
                }
                else
                {
                    _victoryBestTime.text = $"Best Time: {highScoreTimeText}";
                }
            }
        }
        
        Debug.Log("Victory screen displayed");
    }
    
    /// <summary>
    /// Hides all HUD elements (score, time, countdown).
    /// </summary>
    private void HideHUD()
    {
        if (_scoreText != null)
        {
            _scoreText.gameObject.SetActive(false);
        }
    
        if (_timeText != null)
        {
            _timeText.gameObject.SetActive(false);
        }
    
        if (_countdownText != null)
        {
            _countdownText.gameObject.SetActive(false);
        }
    }
    
    private void HideAllScreens()
    {
        if (_gameOverPanel != null) _gameOverPanel.SetActive(false);
        if (_victoryPanel != null) _victoryPanel.SetActive(false);
    }
    
    public void RestartGame()
    {
        Debug.Log("Restarting game...");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void ExitToMainMenu()
    {
        Debug.Log("Exiting to Main Menu...");
        Time.timeScale = 1f;
        
        if (Application.CanStreamedLevelBeLoaded("MainMenu"))
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            Debug.LogWarning("MainMenu scene not found in Build Settings!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
