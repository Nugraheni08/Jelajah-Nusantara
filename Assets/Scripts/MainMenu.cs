using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject settingsPanel;
    public GameObject creditsPanel;

    [Header("Audio")]
    public AudioSource bgMusic;

    void Start()
    {
        // Pastikan panel tersembunyi saat start
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(false);
    }

    public void OnMulaiPermainan()
    {
        SceneManager.LoadScene("GameScene"); // sesuaikan nama scene
    }

    public void OnPilihLevel()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void OnCredits()
    {
        if (creditsPanel != null)
            creditsPanel.SetActive(true);
    }

    public void OnPengaturan()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    public void OnKeluar()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void TutupPanel()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(false);
    }
}