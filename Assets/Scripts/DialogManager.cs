using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

public class DialogManager : MonoBehaviour
{
    [Header("UI References")]
    public Image background;
    public Image characterImage;
    public TextMeshProUGUI dialogText;
    public TextMeshProUGUI speakerName;
    public GameObject continuePrompt;
    public GameObject dialogBox;
    public Image flashOverlay;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip whooshSFX;

    [Header("Flash Settings")]
    public int flashAtLine = 15;
    public float flashDuration = 1.5f;

    [Header("Dialog Data")]
    public List<DialogLine> dialogLines;

    [Header("Settings")]
    public float typingSpeed = 0.03f;
    public string nextSceneName = "MainMenu";

    private int currentLine = 0;
    private bool isTyping = false;
    private bool skipTyping = false;

    void Start()
    {
        continuePrompt.SetActive(false);
        if (flashOverlay != null)
            flashOverlay.color = new Color(1, 1, 1, 0);
        ShowLine(currentLine);
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame ||
            Keyboard.current.spaceKey.wasPressedThisFrame ||
            Keyboard.current.enterKey.wasPressedThisFrame)
        {
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
            SceneManager.LoadScene(nextSceneName);
            return;
        }

        DialogLine line = dialogLines[index];

        if (line.background != null)
            background.sprite = line.background;

        if (line.characterSprite != null)
        {
            characterImage.gameObject.SetActive(true);
            characterImage.sprite = line.characterSprite;
        }
        else
        {
            characterImage.gameObject.SetActive(false);
        }

        if (line.speakerName != "")
        {
            speakerName.gameObject.SetActive(true);
            speakerName.text = line.speakerName;
        }
        else
        {
            speakerName.gameObject.SetActive(false);
        }

        if (index == flashAtLine)
        {
            StartCoroutine(FlashEffect());
        }

        StartCoroutine(TypeText(line.dialogText));
    }

    IEnumerator FlashEffect()
    {
        Debug.Log("FlashEffect dipanggil! Line: " + currentLine);
        
        if (audioSource != null && whooshSFX != null)
            audioSource.PlayOneShot(whooshSFX);

        float t = 0f;
        while (t < flashDuration * 0.3f)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Clamp01(t / (flashDuration * 0.3f));
            flashOverlay.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        yield return new WaitForSeconds(flashDuration * 0.4f);

        t = 0f;
        while (t < flashDuration * 0.3f)
        {
            t += Time.deltaTime;
            float alpha = 1f - Mathf.Clamp01(t / (flashDuration * 0.3f));
            flashOverlay.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        flashOverlay.color = new Color(1, 1, 1, 0);
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        skipTyping = false;
        continuePrompt.SetActive(false);
        dialogText.text = "";

        foreach (char c in text)
        {
            if (skipTyping)
            {
                dialogText.text = text;
                break;
            }
            dialogText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        continuePrompt.SetActive(true);
    }

    void NextLine()
    {
        currentLine++;
        ShowLine(currentLine);
    }
}

[System.Serializable]
public class DialogLine
{
    public string speakerName = "";
    public Sprite background;
    public Sprite characterSprite;
    [TextArea(3, 6)]
    public string dialogText;
}