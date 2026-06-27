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

    [Header("Settings")]
    public int score = 0;
    public int combo = 0;
    public int lives = 5;

    private LevelData levelData;

    void Start()
    {
        string levelName = PlayerPrefs.GetString("SelectedLevel", "LevelData_Maluku");

        Debug.Log("Gameplay menerima = " + levelName);

        levelData = Resources.Load<LevelData>(levelName);

        if (levelData == null)
            Debug.LogError("LevelData gagal diload!");
        else
            Debug.Log("Berhasil load = " + levelData.name);
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
        if (lives <= 0) return;
        lives--;

        if (lives < hearts.Length)
            hearts[lives].color = new Color(0.2f, 0.2f, 0.2f, 1f);

        if (lives <= 0)
            GameOver();
    }

    void GameOver()
    {
        if (levelData != null)
            SceneManager.LoadScene(levelData.loseSceneName);
        else
            SceneManager.LoadScene("MalukuLoseScene");
    }

    public void SongFinished()
    {
        if (levelData != null)
            SceneManager.LoadScene(levelData.winSceneName);
        else
            SceneManager.LoadScene("MalukuWinScene");
    }
}