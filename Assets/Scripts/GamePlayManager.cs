using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameplayManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;
    public Image[] hearts;

    [Header("Audio")]
    public AudioClip[] noteClips;
    public AudioSource noteAudioSource;

    [Header("Lagu Utama")]
    public AudioSource musicSource; // AudioSource yang muter lagu (Sajojo, dst)

    [Header("Panel Hasil")]
    public GameObject panelLevelSelesai;
    public TextMeshProUGUI scoreTextSelesai; // teks score di dalam PanelLevelSelesai
    public GameObject panelMisiGagal;
    public TextMeshProUGUI scoreTextGagal;   // teks score di dalam PanelMisiGagal

    [Header("Navigasi")]
    public string sceneLanjutan = "EndingScenes";   // dipanggil tombol "Lanjut" pas menang
    public string mainMenuSceneName = "MainMenuScene"; // dipanggil tombol "Kembali ke Menu"

    [Header("Settings")]
    public int score = 0;
    public int combo = 0;
    public int lives = 3;

    private bool levelEnded = false;
    private LevelData levelData;

    void Start()
    {
        if (panelLevelSelesai != null) panelLevelSelesai.SetActive(false);
        if (panelMisiGagal != null) panelMisiGagal.SetActive(false);

        string levelName = PlayerPrefs.GetString("SelectedLevel", "LevelData_Maluku");

        Debug.Log("Gameplay menerima = " + levelName);

        levelData = Resources.Load<LevelData>(levelName);

        if (levelData == null)
            Debug.LogError("LevelData gagal diload!");
        else
            Debug.Log("Berhasil load = " + levelData.name);

        if (musicSource != null && musicSource.clip != null)
            StartCoroutine(WatchSongEnd(musicSource.clip.length));
    }


    void Update()
    {
        scoreText.text = "SCORE: " + score;
        comboText.text = "COMBO: x" + combo;
    }

    public void AddScore(int points)
    {
        combo++;
        score += points * combo;
    }

    public void PlayNoteSound(int lane)
    {
        Debug.Log("PlayNoteSound dipanggil! lane=" + lane + 
                " | audioSource=" + (noteAudioSource != null ? "ada" : "NULL") +
                " | clips length=" + (noteClips != null ? noteClips.Length.ToString() : "NULL") +
                " | clip=" + (noteClips != null && lane < noteClips.Length && noteClips[lane] != null ? noteClips[lane].name : "NULL"));

        if (noteClips != null && lane < noteClips.Length && noteClips[lane] != null)
            noteAudioSource.PlayOneShot(noteClips[lane]);
    }

    public void ResetCombo()
    {
        combo = 0;
        LoseLife();
    }

    void LoseLife()
    {
        if (levelEnded || lives <= 0) return;
        lives--;

        if (lives < hearts.Length)
            hearts[lives].color = new Color(0.2f, 0.2f, 0.2f, 1f);

        if (lives <= 0)
            GameOver();
    }

    // Nunggu lagu sampai habis, lalu cek menang/kalah
    IEnumerator WatchSongEnd(float durasiLagu)
    {
        yield return new WaitForSeconds(durasiLagu);

        if (!levelEnded && lives > 0)
        {
            LevelSelesai();
        }
    }

    void LevelSelesai()
    {
        if (levelEnded) return;
        levelEnded = true;

        if (musicSource != null) musicSource.Stop();
        Time.timeScale = 1f;

        // Simpan score dulu
        PlayerPrefs.SetInt("LastScore", score);
        PlayerPrefs.Save();

        // Langsung pindah ke MalukuWinScene
        SceneManager.LoadScene("MalukuWinScene");
    }

    void GameOver()
    {
        if (levelEnded) return;
        levelEnded = true;

        if (musicSource != null) musicSource.Stop();
        Time.timeScale = 1f;

        // Langsung pindah ke MalukuLoseScene
        SceneManager.LoadScene("MalukuLoseScene");
    }

    public void SongFinished()
    {
        // Simpan score dulu sebelum pindah
        PlayerPrefs.SetInt("LastScore", score);
        PlayerPrefs.Save();
        Debug.Log("Score tersimpan: " + score);

        if (levelData != null)
            SceneManager.LoadScene(levelData.winSceneName);
        else
            SceneManager.LoadScene("MalukuWinScene");
    }

    // ----- Dipanggil dari tombol di Panel (OnClick di Inspector) -----

    public void OnCobaLagiClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnKembaliMenuClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void OnLanjutClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneLanjutan);
    }
}