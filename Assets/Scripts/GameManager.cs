using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject GameOverPanel;
    public GameObject LevelCompletionPanel;
    public GameObject PauseMenuPanel;
    public GameObject MainMenuPanel;
    public GameObject AboutPanel;
    public Button RestartButton;
    public Button NextLevelButton;
    public Button PauseButton;
    public Button ResumeButton;
    public Button HomeButton;
    public Button PlayButton;
    public Button AboutButton;
    public Button BackButton;
    public Button ExitButton;
    public Button PlayMusicButton;
    public Button MuteButton;
    public AudioSource backgroundMusic;

    // New variable to specify the desired scene index for the Exit button
    public int exitSceneIndex;

    private void Start()
    {
        // Initializing Panels
        GameOverPanel.SetActive(false);
        LevelCompletionPanel.SetActive(false);
        PauseMenuPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
        AboutPanel.SetActive(false);

        // Initializing Buttons
        RestartButton.onClick.AddListener(RestartGame);
        NextLevelButton.onClick.AddListener(NextLevel);
        PauseButton.onClick.AddListener(ShowPauseMenu);
        ResumeButton.onClick.AddListener(ResumeGame);
        HomeButton.onClick.AddListener(ReturnToMainMenu);
        PlayButton.onClick.AddListener(StartGame);
        AboutButton.onClick.AddListener(ShowAboutPanel);
        BackButton.onClick.AddListener(ReturnToMainMenu);
        ExitButton.onClick.AddListener(ExitToDesiredScene);
        PlayMusicButton.onClick.AddListener(PlayMusic);
        MuteButton.onClick.AddListener(MuteMusic);

        // Start with music playing
        PlayMusic();

        // Pause the game initially
        Time.timeScale = 0f;
    }

    public void ShowGameOverPanel()
    {
        Debug.Log("Showing game over panel.");
        Time.timeScale = 0f; // Pause the game
        GameOverPanel.SetActive(true);
    }

    public void ShowLevelCompletionPanel()
    {
        Debug.Log("Showing level completion panel.");
        Time.timeScale = 0f; // Pause the game
        LevelCompletionPanel.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("All levels completed!");
            SceneManager.LoadScene(0); // Restart from level 1, or replace 0 with your "You Win" scene index.
        }
    }

    public void ShowPauseMenu()
    {
        Debug.Log("Showing pause menu.");
        Time.timeScale = 0f; // Pause the game
        PauseMenuPanel.SetActive(true);
    }

    public void ResumeGame()
    {
        Debug.Log("Resuming game.");
        Time.timeScale = 1f;
        PauseMenuPanel.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        Debug.Log("Returning to main menu.");
        Time.timeScale = 1f; // Resume time scale in case it's paused
        SceneManager.LoadScene(0); // Assuming the main menu is the first scene
    }

    public void StartGame()
    {
        Debug.Log("Starting game from level 1.");
        MainMenuPanel.SetActive(false);
        Time.timeScale = 1f; // Resume the game
        SceneManager.LoadScene(1); // Load the first level
    }

    public void ShowAboutPanel()
    {
        Debug.Log("Showing about panel.");
        AboutPanel.SetActive(true);
        MainMenuPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game.");
        Application.Quit();
    }

    public void PlayMusic()
    {
        backgroundMusic.Play();
        PlayMusicButton.gameObject.SetActive(false);
        MuteButton.gameObject.SetActive(true);
    }

    public void MuteMusic()
    {
        backgroundMusic.Pause();
        PlayMusicButton.gameObject.SetActive(true);
        MuteButton.gameObject.SetActive(false);
    }

    // New method to exit to the desired scene based on the Inspector setting
    public void ExitToDesiredScene()
    {
        Debug.Log($"Exiting to scene index: {exitSceneIndex}");
        Time.timeScale = 1f;
        SceneManager.LoadScene(exitSceneIndex);
    }
}
