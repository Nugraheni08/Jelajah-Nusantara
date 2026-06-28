using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingGame : MonoBehaviour
{
    public GameObject settingsPanel;

    public void Resume()
    {
        settingsPanel.SetActive(false);
        Time.timeScale = 1f;
        
        // Resume semua audio
        AudioSource[] allAudio = FindObjectsByType<AudioSource>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (AudioSource a in allAudio)
            a.UnPause();
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        Time.timeScale = 0f;
        
        // Pause semua audio
        AudioSource[] allAudio = FindObjectsByType<AudioSource>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (AudioSource a in allAudio)
            a.Pause();
    }
}