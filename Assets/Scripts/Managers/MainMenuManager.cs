using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages main menu UI and navigation.
/// Handles play, difficulty selection, settings, and quit.
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _difficultyPanel;
    [SerializeField] private GameObject _settingsPanel;
    
    private void Start()
    {
        // Show main menu, hide others
        ShowMainMenu();
    }
    
    /// <summary>
    /// Called by Play button.
    /// Shows difficulty selection screen.
    /// PUBLIC because Unity UI onClick requires public methods.
    /// </summary>
    public void OnPlayButton()
    {
        _mainMenuPanel.SetActive(false);
        _difficultyPanel.SetActive(true);
    }
    
    /// <summary>
    /// Called by difficulty buttons.
    /// Sets difficulty and starts game.
    /// PUBLIC because Unity UI onClick requires public methods.
    /// </summary>
    public void OnDifficultySelected(int difficultyIndex)
    {
        if (DifficultyManager.Instance != null)
        {
            DifficultyManager.Instance.SetDifficultyByIndex(difficultyIndex);
        }
        
        // Load MainGame scene
        SceneManager.LoadScene("MainGame");
    }
    
    /// <summary>
    /// Called by Back button on difficulty screen.
    /// PUBLIC because Unity UI onClick requires public methods.
    /// </summary>
    public void OnBackButton()
    {
        ShowMainMenu();
    }
    
    /// <summary>
    /// Shows main menu panel.
    /// </summary>
    private void ShowMainMenu()
    {
        _mainMenuPanel.SetActive(true);
        _difficultyPanel.SetActive(false);
        _settingsPanel.SetActive(false);
    }
    
    public void OnSettingsButton()
    {
        _mainMenuPanel.SetActive(false);
        _settingsPanel.SetActive(true);
    }

    public void OnBackFromSettings()
    {
        ShowMainMenu();
    }
    
    /// <summary>
    /// Quits the application.
    /// Called by Quit button.
    /// PUBLIC because Unity UI onClick requires public methods.
    /// </summary>
    public void OnQuitButton()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
