using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages main menu navigation and panel transitions.
/// Handles Play, Settings, Difficulty selection, and Quit functionality.
/// No singleton pattern - scene-specific lifecycle.
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _difficultyPanel;
    [SerializeField] private GameObject _settingsPanel;
    
    private void Start()
    {
        // Initialize menu state
        ShowMainMenu();
        
        // Start main menu background music
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMainMenuBGM();
        }
    }
    
    /// <summary>
    /// Handles Play button click.
    /// PUBLIC for Unity UI onClick events.
    /// Transitions from main menu to difficulty selection panel.
    /// </summary>
    public void OnPlayButton()
    {
        // Play button click sound feedback
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        // Hide main menu, show difficulty selection
        _mainMenuPanel.SetActive(false);
        _difficultyPanel.SetActive(true);
    }
    
    /// <summary>
    /// Handles difficulty selection button clicks.
    /// PUBLIC for Unity UI onClick events.
    /// Accepts difficulty index: 0=Easy, 1=Medium, 2=Hard, 3=Expert.
    /// Saves difficulty selection and loads gameplay scene.
    /// </summary>
    public void OnDifficultySelected(int difficultyIndex)
    {
        // Play button click sound feedback
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        // Save selected difficulty to DifficultyManager (persists across scenes)
        if (DifficultyManager.Instance != null)
        {
            DifficultyManager.Instance.SetDifficultyByIndex(difficultyIndex);
        }
        
        // Load gameplay scene
        SceneManager.LoadScene("MainGame");
    }
    
    /// <summary>
    /// Handles Settings button click.
    /// PUBLIC for Unity UI onClick events.
    /// Transitions from main menu to settings panel.
    /// </summary>
    public void OnSettingsButton()
    {
        // Play button click sound feedback
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        // Hide main menu, show settings panel
        _mainMenuPanel.SetActive(false);
        _settingsPanel.SetActive(true);
    }
    
    /// <summary>
    /// Handles Back button click from difficulty panel.
    /// PUBLIC for Unity UI onClick events.
    /// Returns to main menu from difficulty selection.
    /// </summary>
    public void OnBackButton()
    {
        // Play button click sound feedback
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        // Return to main menu
        ShowMainMenu();
    }
    
    /// <summary>
    /// Handles Back button click from settings panel.
    /// PUBLIC for Unity UI onClick events.
    /// Returns to main menu from settings.
    /// </summary>
    public void OnBackFromSettings()
    {
        // Play button click sound feedback
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        // Return to main menu
        ShowMainMenu();
    }
    
    /// <summary>
    /// Shows main menu panel and hides all other panels.
    /// Used for initialization and back button navigation.
    /// </summary>
    private void ShowMainMenu()
    {
        _mainMenuPanel.SetActive(true);
        _difficultyPanel.SetActive(false);
        _settingsPanel.SetActive(false);
    }
    
    /// <summary>
    /// Handles Quit button click.
    /// PUBLIC for Unity UI onClick events.
    /// Quits application in builds, stops play mode in editor.
    /// </summary>
    public void OnQuitButton()
    {
        // Play button click sound feedback
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        // Quit application (works in builds)
        Application.Quit();
        
        // Stop play mode in Unity Editor (for testing)
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}