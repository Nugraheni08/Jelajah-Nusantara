using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    [Header("UI References")]
    public Image background;
    public TextMeshProUGUI tutorialText;
    public GameObject continuePrompt;
    public RectTransform tutorialBox;

    [Header("Intro Panel")]
    public GameObject panelIntro;
    public Sprite bgIntro;

    [Header("Slides")]
    public Sprite[] slideBackgrounds;
    public string[] slideTexts = {
        "Tekan tombol sesuai jalur note.",
        "Pastikan kamu tidak kehabisan nyawa.",
        "Tekan saat note menyentuh garis.",
    };

    public Vector2[] slideBoxPositions = {
        new Vector2(0, -220),
        new Vector2(0, -220),
        new Vector2(0, -220),
    };

    [Header("Settings")]
    public string nextSceneName = "JabarStoryScene";
    public float typingSpeed = 0.03f;

    private int currentSlide = 0;
    private bool isTyping = false;
    private bool skipTyping = false;
    private bool introActive = true;

    void Start()
    {
        if (continuePrompt != null) continuePrompt.SetActive(false);
        if (tutorialBox != null) tutorialBox.gameObject.SetActive(false);

        // Set background intro
        if (background != null && bgIntro != null)
            background.sprite = bgIntro;

        // Tampilkan intro dulu
        if (panelIntro != null) panelIntro.SetActive(true);
    }

    void Update()
    {
        if (introActive) return;

        if (Mouse.current.leftButton.wasPressedThisFrame ||
            Keyboard.current.spaceKey.wasPressedThisFrame ||
            Keyboard.current.enterKey.wasPressedThisFrame)
        {
            if (isTyping)
                skipTyping = true;
            else
                NextSlide();
        }
    }

    public void OnMulaiTutorial()
    {
        introActive = false;

        if (panelIntro != null) panelIntro.SetActive(false);
        if (tutorialBox != null) tutorialBox.gameObject.SetActive(true);

        // Ganti background ke slide pertama
        if (background != null && slideBackgrounds != null && slideBackgrounds.Length > 0)
            background.sprite = slideBackgrounds[0];

        ShowSlide(currentSlide);
    }

    void ShowSlide(int index)
    {
        // Ganti background
        if (background != null && slideBackgrounds != null &&
            index < slideBackgrounds.Length)
            background.sprite = slideBackgrounds[index];

        // Pindah posisi textbox
        if (tutorialBox != null && index < slideBoxPositions.Length)
            tutorialBox.anchoredPosition = slideBoxPositions[index];

        // Tampilkan teks
        if (index < slideTexts.Length)
            StartCoroutine(TypeText(slideTexts[index]));
    }

    void NextSlide()
    {
        currentSlide++;

        if (currentSlide >= slideBackgrounds.Length)
        {
            SceneManager.LoadScene(nextSceneName);
            return;
        }

        ShowSlide(currentSlide);
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        skipTyping = false;
        if (continuePrompt != null) continuePrompt.SetActive(false);
        if (tutorialText != null) tutorialText.text = "";

        foreach (char c in text)
        {
            if (skipTyping)
            {
                if (tutorialText != null) tutorialText.text = text;
                break;
            }
            if (tutorialText != null) tutorialText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        if (continuePrompt != null) continuePrompt.SetActive(true);
    }
}