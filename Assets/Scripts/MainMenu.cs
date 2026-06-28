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

        if (bgMusic != null && !bgMusic.isPlaying)
            bgMusic.Play();
    }

    void StopAllAudio()
    {
        AudioSource[] allAudio = FindObjectsByType<AudioSource>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (AudioSource a in allAudio)
            a.Stop();
    }

    public void OnMulaiPermainan()
    {
        StopAllAudio();
        SceneManager.LoadScene("OpeningScene");
    }

    public void OnPilihLevel()
    {
        StopAllAudio();
        SceneManager.LoadScene("LevelSelectScene");
    }

    public void OnCredits()
    {
        StopAllAudio();
        SceneManager.LoadScene("CreditScene");
    }

    public void OnPengaturan()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    public void OnKeluar()
    {
        StopAllAudio();
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