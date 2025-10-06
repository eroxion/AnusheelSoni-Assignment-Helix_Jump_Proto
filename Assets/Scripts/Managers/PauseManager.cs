using UnityEngine;

/// <summary>
/// Manages game pause functionality with pause menu.
/// Handles ESC key, time scale, and pause UI.
/// </summary>
public class PauseManager : MonoBehaviour
{
    [Header("Pause UI")]
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _settingsPanel;
    
    private bool _isPaused = false;
    
    private void Update()
    {
        // ESC key to toggle pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_settingsPanel != null && _settingsPanel.activeSelf)
            {
                // If settings open, close settings instead of pausing
                CloseSettings();
            }
            else
            {
                TogglePause();
            }
        }
    }
    
    /// <summary>
    /// Toggles pause state.
    /// </summary>
    private void TogglePause()
    {
        if (_isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }
    
    /// <summary>
    /// Pauses the game.
    /// PUBLIC for UI button onClick.
    /// </summary>
    public void PauseGame()
    {
        _isPaused = true;
        Time.timeScale = 0f;
        
        if (_pausePanel != null)
        {
            _pausePanel.SetActive(true);
        }
        
        // Play button click sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        // Pause BGM
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PauseBGM();
        }
    }
    
    /// <summary>
    /// Resumes the game.
    /// PUBLIC for UI button onClick.
    /// </summary>
    public void ResumeGame()
    {
        _isPaused = false;
        Time.timeScale = 1f;
        
        if (_pausePanel != null)
        {
            _pausePanel.SetActive(false);
        }
        
        // Ensure settings panel is closed
        if (_settingsPanel != null)
        {
            _settingsPanel.SetActive(false);
        }
        
        // Play button click sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        // Resume BGM
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ResumeBGM();
        }
    }
    
    /// <summary>
    /// Opens settings panel from pause menu.
    /// PUBLIC for UI button onClick.
    /// </summary>
    public void OpenSettings()
    {
        if (_settingsPanel != null)
        {
            _settingsPanel.SetActive(true);
        }
        
        if (_pausePanel != null)
        {
            _pausePanel.SetActive(false);
        }
        
        // Play button click sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
    }
    
    /// <summary>
    /// Closes settings and returns to pause menu.
    /// PUBLIC for UI button onClick.
    /// </summary>
    public void CloseSettings()
    {
        if (_settingsPanel != null)
        {
            _settingsPanel.SetActive(false);
        }
        
        if (_pausePanel != null)
        {
            _pausePanel.SetActive(true);
        }
        
        // Play button click sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
    }
    
    /// <summary>
    /// Exits to main menu.
    /// PUBLIC for UI button onClick.
    /// </summary>
    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        
        // Play button click sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        // Use UIManager's exit method (handles scene loading properly)
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ExitToMainMenu();
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
    }
}