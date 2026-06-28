using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// GenericResultManager — pasang di satu scene Win ATAU Lose.
/// Script ini membaca data dari PlayerPrefs yang sudah di-set
/// oleh GameplayManager (LastScore, SelectedLevel) dan menampilkan
/// hasil dengan animasi masuk, lalu menyediakan tombol navigasi.
///
/// Cara pakai:
///   1. Buat scene baru (misal "JawaBaratWinScene" atau "JawaBaratLoseScene").
///   2. Pasang script ini ke sebuah GameObject di scene tersebut.
///   3. Hubungkan semua referensi UI di Inspector.
///   4. Centang "isWinScene" untuk Win, biarkan false untuk Lose.
///   5. Isi "levelDataKey" sesuai LevelData yang dipakai
///      (contoh: "LevelData_JawaBarat", "LevelData_Maluku", "LevelData_Papua").
///   6. Buat tombol OnClick() → panggil OnLanjutClicked / OnCobaLagiClicked / OnKembaliMenuClicked.
/// </summary>
public class GenericResultManager : MonoBehaviour
{
    // ─────────────────────────────────────────────────────────────────────────
    // INSPECTOR FIELDS
    // ─────────────────────────────────────────────────────────────────────────

    [Header("Tipe Scene Ini")]
    [Tooltip("Centang kalau ini Win Scene. Biarkan false untuk Lose Scene.")]
    public bool isWinScene = true;

    [Header("Level yang Sesuai")]
    [Tooltip("Nama LevelData di Resources/ yang cocok dengan level ini.\n" +
             "Contoh: LevelData_JawaBarat | LevelData_Maluku | LevelData_Papua")]
    public string levelDataKey = "LevelData_JawaBarat";

    [Header("UI – Teks")]
    [Tooltip("Label judul level, misal 'JAWA BARAT'. Opsional.")]
    public TextMeshProUGUI levelNameText;

    [Tooltip("Menampilkan skor akhir, misal 'SCORE: 1200'.")]
    public TextMeshProUGUI scoreText;

    [Tooltip("Teks reaksi/komentar Aksa yang dinamis berdasarkan skor. Opsional.")]
    public TextMeshProUGUI commentText;

    [Header("UI – Karakter")]
    [Tooltip("Sprite Aksa yang berbeda tergantung Win/Lose dan rentang skor.")]
    public Image characterImage;

    [Tooltip("Sprite Aksa saat menang dengan skor tinggi (>= scoreThresholdHigh).")]
    public Sprite aksaWinHigh;

    [Tooltip("Sprite Aksa saat menang dengan skor sedang (>= scoreThresholdMid).")]
    public Sprite aksaWinMid;

    [Tooltip("Sprite Aksa saat menang dengan skor rendah (di bawah scoreThresholdMid).")]
    public Sprite aksaWinLow;

    [Tooltip("Sprite Aksa saat kalah.")]
    public Sprite aksaLose;

    [Header("UI – Tombol")]
    [Tooltip("Tombol 'Lanjut' (hanya tampil di Win Scene → pindah ke cutscene/level select).")]
    public GameObject buttonLanjut;

    [Tooltip("Tombol 'Coba Lagi' (hanya tampil di Lose Scene → ulang gameplay).")]
    public GameObject buttonCobaLagi;

    [Tooltip("Tombol 'Kembali ke Menu' (tampil di Win & Lose → ke LevelSelect).")]
    public GameObject buttonKembaliMenu;

    [Header("UI – Panel Utama")]
    [Tooltip("Panel/GameObject yang berisi semua elemen result. Akan di-animasi masuk.")]
    public RectTransform resultPanel;

    [Tooltip("Overlay gelap di belakang panel. Opsional.")]
    public Image backgroundOverlay;

    [Header("Threshold Skor (untuk variasi komentar & sprite Aksa)")]
    public int scoreThresholdHigh = 10000;
    public int scoreThresholdMid  = 5000;

    [Header("Animasi Panel")]
    [Tooltip("Durasi animasi slide-in panel result (detik).")]
    public float panelAnimDuration = 0.5f;

    [Tooltip("Panel muncul dari bawah sebesar nilai ini (pixel).")]
    public float panelSlideOffset = 300f;

    [Tooltip("Delay sebelum tombol-tombol muncul setelah panel tiba (detik).")]
    public float buttonAppearDelay = 0.4f;

    [Header("Scene Navigasi")]
    [Tooltip("Nama scene Gameplay – dipakai tombol 'Coba Lagi'.")]
    public string gameplaySceneName = "GamePlayScene";

    [Tooltip("Nama scene Level Select – dipakai tombol 'Kembali ke Menu'.")]
    public string levelSelectSceneName = "LevelSelectScene";

    // ─────────────────────────────────────────────────────────────────────────
    // PRIVATE STATE
    // ─────────────────────────────────────────────────────────────────────────

    private LevelData levelData;
    private int lastScore;

    // ─────────────────────────────────────────────────────────────────────────
    // UNITY LIFECYCLE
    // ─────────────────────────────────────────────────────────────────────────

    void Start()
    {
        // Ambil data dari PlayerPrefs (di-set oleh GameplayManager sebelum pindah scene)
        lastScore = PlayerPrefs.GetInt("LastScore", 0);

        string storedKey = PlayerPrefs.GetString("SelectedLevel", levelDataKey);
        levelData = Resources.Load<LevelData>(storedKey);

        // Sembunyikan tombol dulu, akan muncul setelah animasi
        SetButtonsVisible(false);

        // Setup UI sebelum animasi
        SetupUI();

        // Mainkan animasi masuk
        StartCoroutine(AnimatePanelIn());
    }

    // ─────────────────────────────────────────────────────────────────────────
    // SETUP UI
    // ─────────────────────────────────────────────────────────────────────────

    void SetupUI()
    {
        // --- Nama Level ---
        if (levelNameText != null)
            levelNameText.text = (levelData != null) ? levelData.levelName.ToUpper() : "";

        // --- Skor ---
        if (scoreText != null)
            scoreText.text = "SCORE: " + lastScore.ToString("N0");

        // --- Sprite Karakter & Komentar ---
        if (isWinScene)
            SetupWinUI();
        else
            SetupLoseUI();

        // --- Tombol: tampilkan sesuai tipe scene ---
        if (buttonLanjut  != null) buttonLanjut.SetActive(isWinScene);
        if (buttonCobaLagi != null) buttonCobaLagi.SetActive(!isWinScene);
        // buttonKembaliMenu selalu ada, akan diaktifkan setelah animasi bersama tombol lain
    }

    void SetupWinUI()
    {
        // Pilih sprite & komentar berdasarkan rentang skor
        if (lastScore >= scoreThresholdHigh)
        {
            SetCharacterSprite(aksaWinHigh);
            SetComment("Sempurna! Musik Nusantara mengalir dari hatimu!");
        }
        else if (lastScore >= scoreThresholdMid)
        {
            SetCharacterSprite(aksaWinMid);
            SetComment("Bagus! Kamu berhasil membawa harmoni ke kampung ini.");
        }
        else
        {
            SetCharacterSprite(aksaWinLow);
            SetComment("Berhasil! Masih bisa lebih baik, tapi misinya selesai!");
        }
    }

    void SetupLoseUI()
    {
        SetCharacterSprite(aksaLose);
        SetComment("Noise terlalu keras... Tapi Aksa tidak akan menyerah!");
    }

    void SetCharacterSprite(Sprite sprite)
    {
        if (characterImage == null || sprite == null) return;
        characterImage.sprite = sprite;
    }

    void SetComment(string comment)
    {
        if (commentText == null) return;
        commentText.text = comment;
    }

    // ─────────────────────────────────────────────────────────────────────────
    // ANIMASI
    // ─────────────────────────────────────────────────────────────────────────

    IEnumerator AnimatePanelIn()
    {
        if (resultPanel == null)
        {
            // Tidak ada panel untuk di-animasi, langsung tampilkan tombol
            SetButtonsVisible(true);
            yield break;
        }

        // Posisi awal: geser ke bawah
        Vector2 targetPos  = resultPanel.anchoredPosition;
        Vector2 startPos   = targetPos + Vector2.down * panelSlideOffset;
        resultPanel.anchoredPosition = startPos;

        // Fade in overlay
        if (backgroundOverlay != null)
        {
            Color c = backgroundOverlay.color;
            backgroundOverlay.color = new Color(c.r, c.g, c.b, 0f);
        }

        float elapsed = 0f;
        while (elapsed < panelAnimDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / panelAnimDuration);

            resultPanel.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);

            if (backgroundOverlay != null)
            {
                Color c = backgroundOverlay.color;
                backgroundOverlay.color = new Color(c.r, c.g, c.b, t * 0.6f);
            }

            yield return null;
        }

        resultPanel.anchoredPosition = targetPos;

        // Tunggu sebentar lalu munculkan tombol
        yield return new WaitForSeconds(buttonAppearDelay);
        SetButtonsVisible(true);
    }

    void SetButtonsVisible(bool visible)
    {
        if (buttonLanjut    != null && isWinScene)  buttonLanjut.SetActive(visible);
        if (buttonCobaLagi  != null && !isWinScene) buttonCobaLagi.SetActive(visible);
        if (buttonKembaliMenu != null)              buttonKembaliMenu.SetActive(visible);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // TOMBOL CALLBACKS — hubungkan lewat Inspector OnClick()
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Tombol "Lanjut" — hanya untuk Win Scene.
    /// Jika LevelData punya nextStorySceneName, mampir ke sana dulu
    /// (cutscene pasca-level). Kalau kosong, langsung ke Level Select.
    /// </summary>
    public void OnLanjutClicked()
    {
        if (!isWinScene) return;

        if (levelData != null && !string.IsNullOrEmpty(levelData.nextStorySceneName))
            SceneManager.LoadScene(levelData.nextStorySceneName);
        else
            SceneManager.LoadScene(levelSelectSceneName);
    }

    /// <summary>
    /// Tombol "Coba Lagi" — hanya untuk Lose Scene.
    /// Kembali ke GamePlayScene dengan level yang sama (PlayerPrefs.SelectedLevel masih tersimpan).
    /// </summary>
    public void OnCobaLagiClicked()
    {
        if (isWinScene) return;
        SceneManager.LoadScene(gameplaySceneName);
    }

    /// <summary>
    /// Tombol "Kembali ke Menu" — tersedia di Win & Lose Scene.
    /// </summary>
    public void OnKembaliMenuClicked()
    {
        SceneManager.LoadScene(levelSelectSceneName);
    }
}