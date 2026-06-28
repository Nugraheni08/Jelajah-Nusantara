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

    [Header("Navigasi (fallback kalau LevelData tidak ketemu)")]
    public string fallbackWinScene = "MalukuWinScene";
    public string fallbackLoseScene = "MalukuLoseScene";
    public string mainMenuSceneName = "LevelSelectScene";

    [Header("Settings")]
    public int score = 0;
    public int combo = 0;
    public int lives;

    private bool levelEnded = false;
    private LevelData levelData;

    void Start()
    {
        string levelName = PlayerPrefs.GetString("SelectedLevel", "LevelData_Maluku");
        levelData = Resources.Load<LevelData>(levelName);

        if (levelData == null)
            Debug.LogError("LevelData gagal diload: " + levelName);
        else
            Debug.Log("Berhasil load = " + levelData.name);

        // Jumlah nyawa ikut jumlah heart icon yang dipasang di scene,
        // jadi tinggal nambah/kurang object Heart di Inspector tanpa ubah kode.
        lives = hearts != null ? hearts.Length : 3;

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

    // Satu-satunya sumber deteksi "lagu selesai" supaya tidak dobel sama NoteSpawner.
    IEnumerator WatchSongEnd(float durasiLagu)
    {
        yield return new WaitForSeconds(durasiLagu);

        if (!levelEnded && lives > 0)
            LevelSelesai();
    }

    void LevelSelesai()
    {
        if (levelEnded) return;
        levelEnded = true;

        if (musicSource != null) musicSource.Stop();

        PlayerPrefs.SetInt("LastScore", score);
        PlayerPrefs.Save();

        // Buka level berikutnya begitu level ini DIMENANGKAN.
        if (levelData != null && !string.IsNullOrEmpty(levelData.unlockKeyOnWin))
            PlayerPrefs.SetInt(levelData.unlockKeyOnWin, 1);

        Time.timeScale = 1f;

        string target = (levelData != null && !string.IsNullOrEmpty(levelData.winSceneName))
            ? levelData.winSceneName
            : fallbackWinScene;

        SceneManager.LoadScene(target);
    }

    void GameOver()
    {
        if (levelEnded) return;
        levelEnded = true;

        if (musicSource != null) musicSource.Stop();
        PlayerPrefs.SetInt("LastScore", score);
        PlayerPrefs.Save();
        Time.timeScale = 1f;

        string target = (levelData != null && !string.IsNullOrEmpty(levelData.loseSceneName))
            ? levelData.loseSceneName
            : fallbackLoseScene;

        SceneManager.LoadScene(target);
    }
}