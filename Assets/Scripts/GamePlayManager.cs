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

    [Header("Panel Hasil")]
    public GameObject panelLevelSelesai;
    public TextMeshProUGUI scoreTextSelesai;
    public GameObject panelMisiGagal;
    public TextMeshProUGUI scoreTextGagal;

    [Header("Navigasi")]
    public string mainMenuSceneName = "MainMenuScene";

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

        if (AudioManager.Instance != null)
            AudioManager.Instance.RegisterSFXSource(noteAudioSource);

        string levelName = PlayerPrefs.GetString("SelectedLevel", "LevelData_Maluku");
        Debug.Log("Gameplay menerima = " + levelName);

        levelData = Resources.Load<LevelData>(levelName);

        if (levelData == null)
            Debug.LogError("LevelData gagal diload!");
        else
            Debug.Log("Berhasil load = " + levelData.name);
    }

    // Dipanggil oleh NoteSpawner setelah musik diplay
    public void StartWatchSong(float durasi)
    {
        Debug.Log("StartWatchSong dipanggil, durasi: " + durasi);
        StartCoroutine(WatchSongEnd(durasi));
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

    IEnumerator WatchSongEnd(float durasiLagu)
    {
        Debug.Log("WatchSongEnd mulai, durasi: " + durasiLagu);
        yield return new WaitForSeconds(durasiLagu);
        Debug.Log("WatchSongEnd selesai! levelEnded=" + levelEnded + " lives=" + lives);

        if (!levelEnded && lives > 0)
            LevelSelesai();
        else if (!levelEnded)
            GameOver();
    }

    void LevelSelesai()
    {
        if (levelEnded) return;
        levelEnded = true;

        Time.timeScale = 1f;
        PlayerPrefs.SetInt("LastScore", score);
        PlayerPrefs.Save();

        if (levelData != null && levelData.winSceneName != "")
            SceneManager.LoadScene(levelData.winSceneName);
        else
            SceneManager.LoadScene("MalukuWinScene");
    }

    void GameOver()
    {
        if (levelEnded) return;
        levelEnded = true;

        Time.timeScale = 1f;

        if (levelData != null && levelData.loseSceneName != "")
            SceneManager.LoadScene(levelData.loseSceneName);
        else
            SceneManager.LoadScene("MalukuLoseScene");
    }

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
        if (levelData != null && levelData.winSceneName != "")
            SceneManager.LoadScene(levelData.winSceneName);
        else
            SceneManager.LoadScene("MalukuWinScene");
    }
}