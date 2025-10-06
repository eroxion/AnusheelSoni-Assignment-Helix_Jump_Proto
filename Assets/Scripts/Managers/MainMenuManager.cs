using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _difficultyPanel;
    [SerializeField] private GameObject _settingsPanel;
    
    private void Start()
    {
        ShowMainMenu();
        
        // Play main menu BGM
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMainMenuBGM();
        }
    }
    
    public void OnPlayButton()
    {
        // Play button click sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        _mainMenuPanel.SetActive(false);
        _difficultyPanel.SetActive(true);
    }
    
    public void OnDifficultySelected(int difficultyIndex)
    {
        // Play button click sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        if (DifficultyManager.Instance != null)
        {
            DifficultyManager.Instance.SetDifficultyByIndex(difficultyIndex);
        }
        
        SceneManager.LoadScene("MainGame");
    }
    
    public void OnSettingsButton()
    {
        // Play button click sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        _mainMenuPanel.SetActive(false);
        _settingsPanel.SetActive(true);
    }
    
    public void OnBackButton()
    {
        // Play button click sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        ShowMainMenu();
    }
    
    public void OnBackFromSettings()
    {
        // Play button click sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        ShowMainMenu();
    }
    
    private void ShowMainMenu()
    {
        _mainMenuPanel.SetActive(true);
        _difficultyPanel.SetActive(false);
        _settingsPanel.SetActive(false);
    }
    
    public void OnQuitButton()
    {
        // Play button click sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}