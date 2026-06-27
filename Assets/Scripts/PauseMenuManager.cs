using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [Header("Pause Menu Popup Object")]
    public GameObject pauseMenuPopup;

    [Header("Nama Scene Main Menu")]
    public string mainMenuSceneName = "MainMenuScene";

    private bool isPaused = false;

    void Start()
    {
        pauseMenuPopup.SetActive(false);
    }

    // Panggil dari tombol Pause (ikon pause di gameplay)
    public void TogglePause()
    {
        if (isPaused)
            Resume();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        pauseMenuPopup.SetActive(true);
        Time.timeScale = 0f; // Freeze game
        isPaused = true;
    }

    // Panggil dari tombol "Lanjutkan"
    public void Resume()
    {
        pauseMenuPopup.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    // Panggil dari tombol "Mulai Ulang"
    public void RestartLevel()
    {
        Time.timeScale = 1f; // PENTING: kembalikan time scale sebelum reload scene
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    // Panggil dari tombol "Keluar ke Menu"
    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; // PENTING: kembalikan time scale sebelum pindah scene
        SceneManager.LoadScene(mainMenuSceneName);
    }
}