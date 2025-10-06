using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    internal static UIManager Instance { get; private set; }
    
    [Header("HUD Elements")]
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _countdownText;
    [SerializeField] private TextMeshProUGUI _controlHintText;  // ADD THIS
    
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
        HideHUD();
        
        // Show control hints
        ShowControlHints();
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
    
    /// <summary>
    /// Shows device-specific control hints.
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
    /// Hides control hints.
    /// </summary>
    private void HideControlHints()
    {
        if (_controlHintText != null)
        {
            _controlHintText.gameObject.SetActive(false);
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
    
    internal void ShowGoMessage()
    {
        if (_countdownText != null)
        {
            _countdownText.text = "GO!";
            _countdownText.gameObject.SetActive(true);
            
            // Hide control hints when "GO!" appears
            HideControlHints();
            
            Invoke(nameof(HideCountdownAndShowHUD), 1f);
        }
    }
    
    private void HideCountdownAndShowHUD()
    {
        if (_countdownText != null)
        {
            _countdownText.gameObject.SetActive(false);
        }
        
        ShowHUD();
        
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.StartTimer();
        }
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMainGameBGM();
        }
    }
    
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
    
    internal void ShowGameOverScreen()
    {
        if (_gameOverPanel == null) return;
        
        CancelInvoke(nameof(HideCountdownAndShowHUD));
        HideHUD();
        
        _gameOverPanel.SetActive(true);
        
        if (ScoreManager.Instance != null)
        {
            int finalScore = ScoreManager.Instance.CurrentScore;
            int highScore = ScoreManager.Instance.HighScore;
            float finalTime = ScoreManager.Instance.CurrentSessionTime;
            float highScoreTime = ScoreManager.Instance.HighScoreTime;
            
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
                string highScoreTimeText = ScoreManager.Instance.FormatTime(highScoreTime);
                _gameOverBestTime.text = $"Best Time: {highScoreTimeText}";
            }
        }
    }
    
    internal void ShowVictoryScreen()
    {
        if (_victoryPanel == null) return;
        
        CancelInvoke(nameof(HideCountdownAndShowHUD));
        HideHUD();
        
        _victoryPanel.SetActive(true);
        
        if (ScoreManager.Instance != null)
        {
            int finalScore = ScoreManager.Instance.CurrentScore;
            int highScore = ScoreManager.Instance.HighScore;
            float finalTime = ScoreManager.Instance.CurrentSessionTime;
            float highScoreTime = ScoreManager.Instance.HighScoreTime;
            
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
                string highScoreTimeText = ScoreManager.Instance.FormatTime(highScoreTime);
                
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
    }
    
    private void HideAllScreens()
    {
        if (_gameOverPanel != null) _gameOverPanel.SetActive(false);
        if (_victoryPanel != null) _victoryPanel.SetActive(false);
    }
    
    public void RestartGame()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void ExitToMainMenu()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        Time.timeScale = 1f;
        
        if (Application.CanStreamedLevelBeLoaded("MainMenu"))
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}