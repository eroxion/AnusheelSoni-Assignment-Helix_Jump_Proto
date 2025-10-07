using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages all UI elements and screen transitions.
/// Singleton pattern for global access.
/// Handles HUD (score/timer), countdown, control hints, and end-game screens.
/// Subscribes to ScoreManager events for real-time UI updates.
/// </summary>
public class UIManager : MonoBehaviour
{
    internal static UIManager Instance { get; private set; }
    
    [Header("HUD Elements")]
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _countdownText;
    [SerializeField] private TextMeshProUGUI _controlHintText;
    
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
        // Singleton setup (no DontDestroyOnLoad - scene-specific)
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
        // Subscribe to ScoreManager events for real-time updates
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.OnScoreChanged += UpdateScoreDisplay;
            ScoreManager.Instance.OnTimeChanged += UpdateTimeDisplay;
        }
        
        // Initialize UI state
        UpdateScoreDisplay(0);
        UpdateTimeDisplay(0f);
        HideAllScreens();
        HideHUD();
        
        // Show device-specific control hints during countdown
        ShowControlHints();
    }
    
    private void OnDestroy()
    {
        // Unsubscribe from events to prevent memory leaks
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.OnScoreChanged -= UpdateScoreDisplay;
            ScoreManager.Instance.OnTimeChanged -= UpdateTimeDisplay;
        }
    }
    
    /// <summary>
    /// Updates score display text when score changes.
    /// Called automatically via ScoreManager event subscription.
    /// </summary>
    private void UpdateScoreDisplay(int score)
    {
        if (_scoreText != null)
        {
            _scoreText.text = $"Score: {score}";
        }
    }
    
    /// <summary>
    /// Updates time display text when timer updates.
    /// Called automatically via ScoreManager event subscription.
    /// Uses ScoreManager.FormatTime for consistent MM:SS.ms format.
    /// </summary>
    private void UpdateTimeDisplay(float timeInSeconds)
    {
        if (_timeText != null && ScoreManager.Instance != null)
        {
            _timeText.text = $"Time: {ScoreManager.Instance.FormatTime(timeInSeconds)}";
        }
    }
    
    /// <summary>
    /// Shows device-specific control hints during countdown.
    /// Retrieves hint text from InputHandler based on detected device type.
    /// </summary>
    private void ShowControlHints()
    {
        if (_controlHintText == null) return;
        
        InputHandler inputHandler = FindAnyObjectByType<InputHandler>();
        
        if (inputHandler != null)
        {
            _controlHintText.text = inputHandler.GetControlHint();
            _controlHintText.gameObject.SetActive(true);
        }
    }
    
    /// <summary>
    /// Hides control hints (called when gameplay starts).
    /// </summary>
    private void HideControlHints()
    {
        if (_controlHintText != null)
        {
            _controlHintText.gameObject.SetActive(false);
        }
    }
    
    /// <summary>
    /// Updates countdown display during pre-game timer.
    /// Shows countdown number (3, 2, 1) while hiding HUD elements.
    /// Keeps control hints visible to help player prepare.
    /// </summary>
    internal void UpdateCountdown(int seconds)
    {
        if (_countdownText != null)
        {
            if (seconds > 0)
            {
                _countdownText.text = seconds.ToString();
                _countdownText.gameObject.SetActive(true);
                
                // Hide HUD during countdown
                if (_scoreText != null)
                {
                    _scoreText.gameObject.SetActive(false);
                }
                
                if (_timeText != null)
                {
                    _timeText.gameObject.SetActive(false);
                }
                
                // Keep control hints visible during countdown
                ShowControlHints();
            }
            else
            {
                _countdownText.gameObject.SetActive(false);
            }
        }
    }
    
    /// <summary>
    /// Hides countdown and shows HUD immediately when gameplay begins.
    /// Starts the timer and begins playing background music.
    /// Called by BallController when gravity is enabled.
    /// </summary>
    internal void HideCountdownAndShowHUD()
    {
        // Hide countdown and control hints
        if (_countdownText != null)
        {
            _countdownText.gameObject.SetActive(false);
        }

        if (_controlHintText != null)
        {
            _controlHintText.gameObject.SetActive(false);
        }
        
        // Show gameplay HUD
        ShowHUD();
        
        // Start game systems
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.StartTimer();
        }
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMainGameBGM();
        }
    }
    
    /// <summary>
    /// Shows HUD elements (score and timer).
    /// Called when gameplay begins.
    /// </summary>
    private void ShowHUD()
    {
        if (_scoreText != null)
        {
            _scoreText.gameObject.SetActive(true);
        }
        
        if (_timeText != null)
        {
            _timeText.gameObject.SetActive(true);
        }
    }
    
    /// <summary>
    /// Hides all HUD elements (score, timer, countdown, hints).
    /// Called on game initialization or when showing end-game screens.
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
        
        if (_controlHintText != null)
        {
            _controlHintText.gameObject.SetActive(false);
        }
    }
    
    /// <summary>
    /// Shows game over screen with final statistics.
    /// Displays final score, session time, high score, and best time.
    /// Called by BallController on deadly platform collision.
    /// </summary>
    internal void ShowGameOverScreen()
    {
        if (_gameOverPanel == null) return;
        
        // Cancel any pending HUD show if countdown was running
        CancelInvoke(nameof(HideCountdownAndShowHUD));
        HideHUD();
        
        _gameOverPanel.SetActive(true);
        
        if (ScoreManager.Instance != null)
        {
            int finalScore = ScoreManager.Instance.CurrentScore;
            int highScore = ScoreManager.Instance.HighScore;
            float finalTime = ScoreManager.Instance.CurrentSessionTime;
            float highScoreTime = ScoreManager.Instance.HighScoreTime;
            
            // Display final score
            if (_gameOverFinalScore != null)
            {
                _gameOverFinalScore.text = $"Score: {finalScore}";
            }
            
            // Display final time
            if (_gameOverFinalTime != null)
            {
                _gameOverFinalTime.text = $"Time: {ScoreManager.Instance.FormatTime(finalTime)}";
            }
            
            // Display high score for current difficulty
            if (_gameOverHighScore != null)
            {
                _gameOverHighScore.text = $"High Score: {highScore}";
            }
            
            // Display best time for high score
            if (_gameOverBestTime != null)
            {
                string highScoreTimeText = ScoreManager.Instance.FormatTime(highScoreTime);
                _gameOverBestTime.text = $"Best Time: {highScoreTimeText}";
            }
        }
    }
    
    /// <summary>
    /// Shows victory screen with final statistics.
    /// Displays final score, session time, high score, and best time.
    /// Highlights new high score achievements with visual feedback.
    /// Called by BallController on finish platform collision (if implemented).
    /// </summary>
    internal void ShowVictoryScreen()
    {
        if (_victoryPanel == null) return;
        
        // Cancel any pending HUD show if countdown was running
        CancelInvoke(nameof(HideCountdownAndShowHUD));
        HideHUD();
        
        _victoryPanel.SetActive(true);
        
        if (ScoreManager.Instance != null)
        {
            int finalScore = ScoreManager.Instance.CurrentScore;
            int highScore = ScoreManager.Instance.HighScore;
            float finalTime = ScoreManager.Instance.CurrentSessionTime;
            float highScoreTime = ScoreManager.Instance.HighScoreTime;
            
            // Display final score
            if (_victoryFinalScore != null)
            {
                _victoryFinalScore.text = $"Score: {finalScore}";
            }
            
            // Display final time
            if (_victoryFinalTime != null)
            {
                _victoryFinalTime.text = $"Time: {ScoreManager.Instance.FormatTime(finalTime)}";
            }
            
            // Display high score for current difficulty
            if (_victoryHighScore != null)
            {
                _victoryHighScore.text = $"High Score: {highScore}";
            }
            
            // Display best time with new record highlight if applicable
            if (_victoryBestTime != null)
            {
                string highScoreTimeText = ScoreManager.Instance.FormatTime(highScoreTime);
                
                // Check if this is a new high score
                bool isNewHighScore = (finalScore > highScore) || 
                                     (finalScore == highScore && finalTime < highScoreTime);
                
                // Highlight if new high score with time tiebreaker
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
    }
    
    /// <summary>
    /// Hides all end-game screens (game over and victory).
    /// Called during initialization to ensure clean UI state.
    /// </summary>
    private void HideAllScreens()
    {
        if (_gameOverPanel != null) _gameOverPanel.SetActive(false);
        if (_victoryPanel != null) _victoryPanel.SetActive(false);
    }
    
    /// <summary>
    /// Restarts the current game scene.
    /// PUBLIC for Unity UI onClick events.
    /// Resets time scale and reloads active scene.
    /// </summary>
    public void RestartGame()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        // Ensure time scale is normal (in case paused)
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    /// <summary>
    /// Returns to main menu scene.
    /// PUBLIC for Unity UI onClick events.
    /// Checks if MainMenu scene exists before loading.
    /// </summary>
    public void ExitToMainMenu()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        // Ensure time scale is normal (in case paused)
        Time.timeScale = 1f;
        
        // Check if MainMenu scene exists in build settings
        if (Application.CanStreamedLevelBeLoaded("MainMenu"))
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            // Fallback: reload current scene if MainMenu not found
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}