using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

public class JabarLoseManager : MonoBehaviour
{
    [Header("UI References")]
    public Image background;
    public Image characterImageLeft;
    public Image characterImageRight;
    public TextMeshProUGUI dialogText;
    public TextMeshProUGUI speakerName;
    public GameObject continuePrompt;
    public GameObject dialogBox;

    [Header("Tombol")]
    public GameObject buttonPanel;
    public GameObject btnCobaLagi;
    public GameObject btnKembaliMenu;

    [Header("Sprites")]
    public Sprite bgDialog;
    public Sprite bgGagal;
    public Sprite aksaSprite;
    public Sprite ranuSprite;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip bzzzztSFX;
    public AudioClip clickSFX;

    [Header("Settings")]
    public float typingSpeed = 0.04f;
    public string gameplaySceneName = "GamePlayScene";
    public string menuSceneName = "MainMenuScene";

    private int currentLine = 0;
    private bool isTyping = false;
    private bool skipTyping = false;
    private bool inputBlocked = false;

    private List<LoseDialogLine> dialogLines;

    void Start()
    {
        if (buttonPanel != null) buttonPanel.SetActive(false);
        if (continuePrompt != null) continuePrompt.SetActive(false);

        if (audioSource != null && bzzzztSFX != null)
            audioSource.PlayOneShot(bzzzztSFX);

        if (background != null && bgDialog != null)
            background.sprite = bgDialog;

        if (characterImageLeft != null && aksaSprite != null)
        {
            characterImageLeft.gameObject.SetActive(true);
            characterImageLeft.sprite = aksaSprite;
        }
        if (characterImageRight != null)
            characterImageRight.gameObject.SetActive(false);

        dialogLines = new List<LoseDialogLine>
        {
            new LoseDialogLine("Aksa", "kiri",  "Aku gagal…"),
            new LoseDialogLine("Ranu", "kanan", "Jangan menyerah."),
            new LoseDialogLine("Ranu", "kanan", "Coba ikuti iramanya sekali lagi."),
        };

        currentLine = 0;
        ShowLine(currentLine);
    }

    void Update()
    {
        if (inputBlocked) return;

        if (Mouse.current.leftButton.wasPressedThisFrame ||
            Keyboard.current.spaceKey.wasPressedThisFrame ||
            Keyboard.current.enterKey.wasPressedThisFrame)
        {
            if (audioSource != null && clickSFX != null)
                audioSource.PlayOneShot(clickSFX);

            if (isTyping)
                skipTyping = true;
            else
                NextLine();
        }
    }

    void ShowLine(int index)
    {
        if (index >= dialogLines.Count)
        {
            ShowButtons();
            return;
        }

        var line = dialogLines[index];

        if (line.posisi == "kiri")
        {
            if (characterImageLeft != null)
            {
                characterImageLeft.gameObject.SetActive(true);
                if (aksaSprite != null) characterImageLeft.sprite = aksaSprite;
            }
            if (characterImageRight != null)
                characterImageRight.gameObject.SetActive(false);
        }
        else if (line.posisi == "kanan")
        {
            if (characterImageRight != null)
            {
                characterImageRight.gameObject.SetActive(true);
                if (ranuSprite != null) characterImageRight.sprite = ranuSprite;
            }
            if (characterImageLeft != null)
                characterImageLeft.gameObject.SetActive(false);
        }

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
        ShowLine(currentLine);
    }

    void ShowButtons()
    {
        inputBlocked = true;
        if (continuePrompt != null) continuePrompt.SetActive(false);
        if (dialogBox != null) dialogBox.SetActive(false);
        if (characterImageLeft != null) characterImageLeft.gameObject.SetActive(false);
        if (characterImageRight != null) characterImageRight.gameObject.SetActive(false);

        if (background != null && bgGagal != null)
            background.sprite = bgGagal;

        if (buttonPanel != null) buttonPanel.SetActive(true);
    }

    public void OnCobaLagi()
    {
        PlayerPrefs.SetString("SelectedLevel", "LevelData_JawaBarat");
        PlayerPrefs.Save();
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void OnKembaliMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }

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
}