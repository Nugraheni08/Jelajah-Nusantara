using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject settingsPanel;

    // CATATAN: jangan drag AudioSource manual ke field ini di Inspector.
    // Source musik diambil otomatis dari AudioManager.Instance saat runtime,
    // supaya selalu merujuk ke instance yang benar (yang DontDestroyOnLoad),
    // bukan ke GameObject sementara yang bisa berubah jadi Missing tiap
    // scene MainMenuScene di-reload.
    private AudioSource bgMusic => AudioManager.Instance != null ? AudioManager.Instance.musicSource : null;

    void Start()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);

        StartCoroutine(PlayMusicNextFrame());
    }

    // Tunggu 1 frame dulu sebelum Play(). Ini buat hindari kasus AudioSource
    // sempat ke-disable sesaat (misal saat objek DontDestroyOnLoad lagi
    // "berpindah" antar scene), yang bikin Play() gagal dengan warning
    // "Can not play a disabled audio source" dan musiknya jadi nggak kedengeran.
    IEnumerator PlayMusicNextFrame()
    {
        yield return null;

        if (AudioManager.Instance != null)
            AudioManager.Instance.EnsureMusicPlaying();
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