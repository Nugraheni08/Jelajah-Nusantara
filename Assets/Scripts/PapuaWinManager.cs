using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

// Versi Papua dari MalukuWinManager -- strukturnya disamakan persis:
// Panel Result -> Dialog Noise kalah -> Panel reveal Tifa -> Panel Peta (dialog
// penutup panjang Aksa & Ranu) -> lanjut scene.
//
// BEDA dari Maluku: karena Papua daerah TERAKHIR, tidak ada "unlock daerah baru".
// Setelah dialog penutup selesai, langsung lanjut ke "EndingScene" (TAMAT).
// Scene "PapuaCutsceneAkhir" yang lama TIDAK dipakai lagi -- isinya sudah
// dipindah semua ke panel "Peta" di script ini.
public class PapuaWinManager : MonoBehaviour
{
    [Header("Shared UI (dialog Noise)")]
    public Image background;
    public Image characterImageLeft;
    public Image characterImageRight;
    public TextMeshProUGUI dialogText;
    public TextMeshProUGUI speakerName;
    public GameObject continuePrompt;
    public GameObject dialogBox;

    [Header("Panel Result")]
    public GameObject panelResult;
    public GameObject btnLanjut;
    public GameObject btnUlang;

    [Header("Panel Noise Dialog")]
    public Sprite bgSunset;     // isi: "Latar Papua.png"
    public Sprite noiseSprite;  // isi: "Noise Kalah.png"

    [Header("Panel Tifa (reveal, sama pola dgn Panel Totobuang Maluku)")]
    public GameObject panelTifa;
    // tombol di panel ini -> OnLanjutTifa()

    [Header("Panel Peta (dialog penutup panjang)")]
    public GameObject panelPeta;
    public Image wargaImagePeta;
    public Image aksaImagePeta;
    public Image ranuImagePeta;
    public TextMeshProUGUI dialogTextPeta;
    public TextMeshProUGUI speakerNamePeta;
    public GameObject continuePromptPeta;
    public GameObject dialogBoxPeta;
    public Sprite wargaSprite; // isi: "13_NPC_Papua_Bicara.png"
    public Sprite aksaSprite;  // isi: "14_Aksa_Bicara_2.png" / "Aksa Antusias.png"
    public Sprite ranuSprite;  // isi: "03_Ranu_Bicara.png" / "21_Ranu_Senang.png"

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip dingSFX;
    public AudioClip tifaMusic;

    [Header("Settings")]
    public string nextSceneName = "EndingScene"; // BUKAN PapuaCutsceneAkhir lagi
    public float typingSpeed = 0.04f;

    private int currentLine = 0;
    private bool isTyping = false;
    private bool skipTyping = false;
    private bool inputBlocked = false;
    private bool isInPetaDialog = false;

    private List<WinDialogLine> noiseDialogLines;
    private List<WinDialogLine> petaDialogLines;

    void Start()
    {
        if (panelResult != null) panelResult.SetActive(false);
        if (panelTifa != null) panelTifa.SetActive(false);
        if (panelPeta != null) panelPeta.SetActive(false);
        if (dialogBox != null) dialogBox.SetActive(false);
        if (continuePrompt != null) continuePrompt.SetActive(false);

        if (panelResult != null) panelResult.SetActive(true);
        inputBlocked = true;
    }

    void Update()
    {
        if (inputBlocked) return;

        if (Mouse.current.leftButton.wasPressedThisFrame ||
            Keyboard.current.spaceKey.wasPressedThisFrame ||
            Keyboard.current.enterKey.wasPressedThisFrame)
        {
            if (isTyping)
                skipTyping = true;
            else
            {
                if (isInPetaDialog)
                    NextPetaLine();
                else
                    NextLine();
            }
        }
    }

    // ===== PANEL 1 - RESULT =====
    public void OnLanjutResult()
    {
        if (panelResult != null) panelResult.SetActive(false);
        StartCoroutine(StartNoiseDialog());
    }

    public void OnUlangLevel()
    {
        SceneManager.LoadScene("GamePlayScene");
    }

    // ===== PANEL 2 - NOISE DIALOG (kalah final, lebih dramatis) =====
    IEnumerator StartNoiseDialog()
    {
        inputBlocked = false;

        if (background != null && bgSunset != null)
            background.sprite = bgSunset;

        if (characterImageRight != null && noiseSprite != null)
        {
            characterImageRight.gameObject.SetActive(true);
            characterImageRight.sprite = noiseSprite;
            characterImageRight.color = Color.white;
        }
        if (characterImageLeft != null)
            characterImageLeft.gameObject.SetActive(false);

        if (dialogBox != null) dialogBox.SetActive(true);

        noiseDialogLines = new List<WinDialogLine>
        {
            new WinDialogLine("Noise", "Aku..."),
            new WinDialogLine("Noise", "Tidak bisa..."),
        };

        currentLine = 0;
        ShowNoiseLine(currentLine);
        yield return null;
    }

    void ShowNoiseLine(int index)
    {
        if (index >= noiseDialogLines.Count)
        {
            StartCoroutine(FadeOutNoiseThenTifa());
            return;
        }

        var line = noiseDialogLines[index];
        if (speakerName != null)
        {
            speakerName.gameObject.SetActive(true);
            speakerName.text = line.speaker;
        }
        StartCoroutine(TypeText(line.text));
    }

    void NextLine()
    {
        currentLine++;
        ShowNoiseLine(currentLine);
    }

    IEnumerator FadeOutNoiseThenTifa()
    {
        inputBlocked = true;
        if (continuePrompt != null) continuePrompt.SetActive(false);

        if (characterImageRight != null)
        {
            float t = 0f;
            Color c = characterImageRight.color;
            while (t < 1f)
            {
                t += Time.deltaTime;
                characterImageRight.color = new Color(c.r, c.g, c.b, 1f - t);
                yield return null;
            }
            characterImageRight.gameObject.SetActive(false);
            characterImageRight.color = c;
        }

        if (dialogBox != null) dialogBox.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        ShowTifaPanel();
    }

    // ===== PANEL 3 - TIFA (reveal, statis, sama pola dgn Panel Totobuang) =====
    void ShowTifaPanel()
    {
        if (panelTifa != null) panelTifa.SetActive(true);

        if (audioSource != null && tifaMusic != null)
        {
            audioSource.clip = tifaMusic;
            audioSource.Play();
        }

        if (audioSource != null && dingSFX != null)
            audioSource.PlayOneShot(dingSFX);

        inputBlocked = true;
    }

    public void OnLanjutTifa()
    {
        if (audioSource != null) audioSource.Stop();
        if (panelTifa != null) panelTifa.SetActive(false);
        StartCoroutine(StartPetaSequence());
    }

    // ===== PANEL 4 - PETA: dialog penutup panjang =====
    IEnumerator StartPetaSequence()
    {
        if (panelPeta != null) panelPeta.SetActive(true);

        yield return null; // tunggu 1 frame biar SetActive sempat jalan

        if (wargaImagePeta != null) wargaImagePeta.gameObject.SetActive(false);
        if (aksaImagePeta != null) aksaImagePeta.gameObject.SetActive(false);
        if (ranuImagePeta != null) ranuImagePeta.gameObject.SetActive(false);
        if (dialogBoxPeta != null) dialogBoxPeta.SetActive(true);

        petaDialogLines = new List<WinDialogLine>
        {
            new WinDialogLine("",      "Suara Tifa kembali menggema."),
            new WinDialogLine("Warga", "Aku ingat suara itu!"),
            new WinDialogLine("Warga", "Ini Tifa!"),
            new WinDialogLine("",      "Angklung, Totobuang, dan Tifa kembali terdengar."),
            new WinDialogLine("",      "Harmoni Nusantara telah pulih."),
            new WinDialogLine("Aksa",  "Jadi... semuanya sudah berakhir?"),
            new WinDialogLine("Ranu",  "Ya."),
            new WinDialogLine("Ranu",  "Noise telah menghilang."),
            new WinDialogLine("Aksa",  "Aku tidak menyangka perjalanan ini membawaku sejauh ini."),
            new WinDialogLine("Ranu",  "Dan semua itu berkat keberanianmu."),
            new WinDialogLine("Aksa",  "Tapi budaya tidak akan bertahan hanya karena aku seorang."),
            new WinDialogLine("Ranu",  "Benar."),
            new WinDialogLine("Ranu",  "Budaya akan tetap hidup selama masih ada yang mau mengenal dan melestarikannya."),
            new WinDialogLine("",      "Budaya bukan sekadar warisan masa lalu."),
            new WinDialogLine("",      "Budaya adalah jati diri yang harus dijaga oleh setiap generasi."),
            new WinDialogLine("Ranu",  "Terima kasih, Aksa."),
            new WinDialogLine("Aksa",  "Aku hanya mengingatkan orang-orang tentang apa yang mereka miliki."),
            new WinDialogLine("Ranu",  "Dan itu sudah lebih dari cukup."),
        };

        isInPetaDialog = true;
        inputBlocked = false;
        currentLine = 0;
        ShowPetaLine(currentLine);
    }

    void ShowPetaLine(int index)
    {
        if (index >= petaDialogLines.Count)
        {
            StartCoroutine(GoToEnding());
            return;
        }

        var line = petaDialogLines[index];

        if (line.speaker == "Warga")
        {
            SetActiveOnly(wargaImagePeta);
            if (wargaImagePeta != null && wargaSprite != null) wargaImagePeta.sprite = wargaSprite;
        }
        else if (line.speaker == "Aksa")
        {
            SetActiveOnly(aksaImagePeta);
            if (aksaImagePeta != null && aksaSprite != null) aksaImagePeta.sprite = aksaSprite;
        }
        else if (line.speaker == "Ranu")
        {
            SetActiveOnly(ranuImagePeta);
            if (ranuImagePeta != null && ranuSprite != null) ranuImagePeta.sprite = ranuSprite;
        }
        else
        {
            SetActiveOnly(null); // narasi -- tidak ada karakter muncul
        }

        if (speakerNamePeta != null)
        {
            if (line.speaker != "")
            {
                speakerNamePeta.gameObject.SetActive(true);
                speakerNamePeta.text = line.speaker;
            }
            else
            {
                speakerNamePeta.gameObject.SetActive(false);
            }
        }

        StartCoroutine(TypeTextPeta(line.text));
    }

    void SetActiveOnly(Image target)
    {
        if (wargaImagePeta != null) wargaImagePeta.gameObject.SetActive(wargaImagePeta == target);
        if (aksaImagePeta != null) aksaImagePeta.gameObject.SetActive(aksaImagePeta == target);
        if (ranuImagePeta != null) ranuImagePeta.gameObject.SetActive(ranuImagePeta == target);
    }

    void NextPetaLine()
    {
        currentLine++;
        ShowPetaLine(currentLine);
    }

    IEnumerator GoToEnding()
    {
        inputBlocked = true;
        isInPetaDialog = false;
        if (continuePromptPeta != null) continuePromptPeta.SetActive(false);
        if (dialogBoxPeta != null) dialogBoxPeta.SetActive(false);
        SetActiveOnly(null);

        yield return new WaitForSeconds(1f);

        // Tidak ada unlock daerah baru di sini -- Papua adalah daerah terakhir.
        // Langsung tutup dengan layar TAMAT (sudah dibangun di scene EndingScene).
        SceneManager.LoadScene(nextSceneName);
    }

    // ===== UTILITIES =====
    IEnumerator TypeText(string text)
    {
        isTyping = true;
        skipTyping = false;
        if (continuePrompt != null) continuePrompt.SetActive(false);
        if (dialogText != null) dialogText.text = "";

        foreach (char c in text)
        {
            if (skipTyping)
            {
                if (dialogText != null) dialogText.text = text;
                break;
            }
            if (dialogText != null) dialogText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        if (continuePrompt != null) continuePrompt.SetActive(true);
    }

    IEnumerator TypeTextPeta(string text)
    {
        isTyping = true;
        skipTyping = false;
        if (continuePromptPeta != null) continuePromptPeta.SetActive(false);
        if (dialogTextPeta != null) dialogTextPeta.text = "";

        foreach (char c in text)
        {
            if (skipTyping)
            {
                if (dialogTextPeta != null) dialogTextPeta.text = text;
                break;
            }
            if (dialogTextPeta != null) dialogTextPeta.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        if (continuePromptPeta != null) continuePromptPeta.SetActive(true);
    }
}
