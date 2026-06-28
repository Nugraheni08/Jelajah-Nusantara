using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject settingsPanel;

    [Header("Audio")]
    public AudioSource bgMusic;

    void Start()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    public void OnMulaiPermainan()
    {
        SceneManager.LoadScene("OpeningScene");
    }

    public void OnPilihLevel()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    // Diubah: pindah ke scene CreditScene, bukan buka panel popup
    public void OnCredits()
    {
        SceneManager.LoadScene("CreditScene");
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
    }
}