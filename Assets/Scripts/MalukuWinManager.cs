using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

public class MalukuWinManager : MonoBehaviour
{
    [Header("Shared UI")]
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
    public Sprite bgSunset;
    public Sprite noiseSprite;
    public Sprite wargaSprite;
    public Sprite ranuSprite;

    [Header("Panel Totobuang")]
    public GameObject panelTotobuang;

    [Header("Panel Peta")]
    public GameObject panelPeta;
    public Image ranuImagePeta;
    public Image wargaImagePeta;
    public TextMeshProUGUI dialogTextPeta;
    public TextMeshProUGUI speakerNamePeta;
    public GameObject continuePromptPeta;
    public GameObject dialogBoxPeta;
    public TextMeshProUGUI unlockText;
    public Sprite petaKosong;
    public Sprite petaUnlock;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip dingSFX;
    public AudioClip totobuangMusic;

    [Header("Settings")]
    public string nextSceneName = "PapuaStoryScene";
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
        if (panelTotobuang != null) panelTotobuang.SetActive(false);
        if (panelPeta != null) panelPeta.SetActive(false);
        if (dialogBox != null) dialogBox.SetActive(false);
        if (continuePrompt != null) continuePrompt.SetActive(false);

        if (AudioManager.Instance != null)
            AudioManager.Instance.RegisterSFXSource(audioSource);

        // Paksa sembunyikan unlockText terlepas dari parent-nya
        if (unlockText != null)
        {
            unlockText.gameObject.SetActive(false);
            Debug.Log("UnlockText disembunyikan paksa");
        }

        if (panelResult != null) panelResult.SetActive(true);
        inputBlocked = true;;
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

    // =====================
    // PANEL 1 - RESULT
    // =====================
    public void OnLanjutResult()
    {
        if (panelResult != null) panelResult.SetActive(false);
        StartCoroutine(StartNoiseDialog());
    }

    public void OnUlangLevel()
    {
        SceneManager.LoadScene("GamePlayScene");
    }

    // =====================
    // PANEL 2 - NOISE DIALOG
    // =====================
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
            new WinDialogLine("Noise", "Tidak mungkin..."),
            new WinDialogLine("Noise", "Aku dikalahkan lagi..."),
        };

        currentLine = 0;
        ShowNoiseLine(currentLine);
        yield return null;
    }

    void ShowNoiseLine(int index)
    {
        if (index >= noiseDialogLines.Count)
        {
            StartCoroutine(FadeOutNoiseThenTotobuang());
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

    IEnumerator FadeOutNoiseThenTotobuang()
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

        ShowTotobuangPanel();
    }

    // =====================
    // PANEL 3 - TOTOBUANG
    // =====================
    void ShowTotobuangPanel()
    {
        if (panelTotobuang != null) panelTotobuang.SetActive(true);

        if (audioSource != null && totobuangMusic != null)
        {
            audioSource.clip = totobuangMusic;
            audioSource.Play();
        }

        if (audioSource != null && dingSFX != null)
            audioSource.PlayOneShot(dingSFX);

        inputBlocked = true;
    }

    public void OnLanjutTotobuang()
    {
        if (audioSource != null) audioSource.Stop();
        if (panelTotobuang != null) panelTotobuang.SetActive(false);
        StartCoroutine(StartPetaSequence());
    }

    // =====================
    // PANEL 4 - PETA + DIALOG
    // =====================
    IEnumerator StartPetaSequence()
    {
        // Aktifkan panel peta dulu
        if (panelPeta != null) panelPeta.SetActive(true);

        // Setelah panel aktif, baru sembunyikan child objects
        yield return null; // tunggu 1 frame biar SetActive sempat jalan

        if (unlockText != null) unlockText.gameObject.SetActive(false);
        if (wargaImagePeta != null) wargaImagePeta.gameObject.SetActive(false);
        if (ranuImagePeta != null) ranuImagePeta.gameObject.SetActive(false);
        if (dialogBoxPeta != null) dialogBoxPeta.SetActive(true);

        petaDialogLines = new List<WinDialogLine>
        {
            new WinDialogLine("Narasi", "Suara Totobuang kembali menggema."),
            new WinDialogLine("Warga",  "Aku ingat suara itu!"),
            new WinDialogLine("Warga",  "Ini Totobuang!"),
            new WinDialogLine("Narasi", "Harmoni Maluku telah pulih."),
            new WinDialogLine("Ranu",   "Tujuan berikutnya: Papua."),
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
            StartCoroutine(ShowUnlockSequence());
            return;
        }

        var line = petaDialogLines[index];

        if (line.speaker == "Warga")
        {
            if (wargaImagePeta != null)
            {
                wargaImagePeta.gameObject.SetActive(true);
                if (wargaSprite != null) wargaImagePeta.sprite = wargaSprite;
            }
            if (ranuImagePeta != null) ranuImagePeta.gameObject.SetActive(false);
        }
        else if (line.speaker == "Ranu")
        {
            if (ranuImagePeta != null)
            {
                ranuImagePeta.gameObject.SetActive(true);
                if (ranuSprite != null) ranuImagePeta.sprite = ranuSprite;
            }
            if (wargaImagePeta != null) wargaImagePeta.gameObject.SetActive(false);
        }
        else
        {
            if (wargaImagePeta != null) wargaImagePeta.gameObject.SetActive(false);
            if (ranuImagePeta != null) ranuImagePeta.gameObject.SetActive(false);
        }

        if (speakerNamePeta != null)
        {
            if (line.speaker != "Narasi")
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

    void NextPetaLine()
    {
        currentLine++;
        ShowPetaLine(currentLine);
    }

    IEnumerator ShowUnlockSequence()
    {
        inputBlocked = true;
        isInPetaDialog = false;
        if (continuePromptPeta != null) continuePromptPeta.SetActive(false);
        if (dialogBoxPeta != null) dialogBoxPeta.SetActive(false);
        if (wargaImagePeta != null) wargaImagePeta.gameObject.SetActive(false);
        if (ranuImagePeta != null) ranuImagePeta.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        if (background != null && petaKosong != null)
            background.sprite = petaKosong;

        yield return new WaitForSeconds(0.5f);

        if (audioSource != null && dingSFX != null)
            audioSource.PlayOneShot(dingSFX);

        if (background != null && petaUnlock != null)
            background.sprite = petaUnlock;

        if (unlockText != null)
        {
            unlockText.gameObject.SetActive(true);
            // DEBUG - cek teks sebelum diubah
            Debug.Log("UnlockText sebelum diubah: " + unlockText.text);
            unlockText.text = "Daerah Baru Terbuka!\nPapua";
            Debug.Log("UnlockText sesudah diubah: " + unlockText.text);
            
            unlockText.color = new Color(1f, 0.84f, 0f, 0f);
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime;
                unlockText.color = new Color(1f, 0.84f, 0f, t);
                yield return null;
            }
        }

        yield return new WaitForSeconds(2f);
        PlayerPrefs.SetInt("PapuaUnlocked", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene(nextSceneName);
    }

    // =====================
    // UTILITIES
    // =====================
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

public class WinDialogLine
{
    public string speaker;
    public string text;

    public WinDialogLine(string speaker, string text)
    {
        this.speaker = speaker;
        this.text = text;
    }
}