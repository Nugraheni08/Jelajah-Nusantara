using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// Pasang script ini di SEMUA scene Win (JawaBaratWinScene, MalukuWinScene, PapuaWinScene)
// dan SEMUA scene Lose (JawaBaratLoseScene, MalukuLoseScene, PapuaLoseScene).
// Cukup centang "isWinScene" yang sesuai di Inspector, sisanya otomatis.
public class LevelResultManager : MonoBehaviour
{
    [Header("Tipe Scene Ini")]
    public bool isWinScene = true; // true = Win Scene, false = Lose Scene

    [Header("UI References")]
    public TextMeshProUGUI scoreText;     // contoh: "SCORE: 1200"
    public TextMeshProUGUI levelNameText; // contoh: "JAWA BARAT" - opsional, boleh dikosongkan

    [Header("Scene Tujuan")]
    public string gameplaySceneName = "GamePlayScene"; // dipakai tombol "Coba Lagi"
    public string levelSelectSceneName = "LevelSelectScene"; // dipakai tombol "Kembali ke Menu"

    private LevelData levelData;

    void Start()
    {
        string levelKey = PlayerPrefs.GetString("SelectedLevel", "");
        if (!string.IsNullOrEmpty(levelKey))
            levelData = Resources.Load<LevelData>(levelKey);

        int lastScore = PlayerPrefs.GetInt("LastScore", 0);

        if (scoreText != null)
            scoreText.text = "SCORE: " + lastScore;

        if (levelNameText != null && levelData != null)
            levelNameText.text = levelData.levelName;
    }

    // ----- Dipanggil dari tombol di Inspector (OnClick) -----

    // Tombol "Lanjut" (hanya ada di Win Scene)
    public void OnLanjutClicked()
    {
        if (!isWinScene) return;

        // Kalau level ini punya cutscene akhir, mampir dulu ke sana.
        if (levelData != null && !string.IsNullOrEmpty(levelData.nextStorySceneName))
        {
            SceneManager.LoadScene(levelData.nextStorySceneName);
        }
        else
        {
            SceneManager.LoadScene(levelSelectSceneName);
        }
    }

    // Tombol "Coba Lagi" (hanya ada di Lose Scene)
    public void OnCobaLagiClicked()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }

    // Tombol "Kembali ke Menu" (ada di Win & Lose Scene)
    public void OnKembaliMenuClicked()
    {
        SceneManager.LoadScene(levelSelectSceneName);
    }
}