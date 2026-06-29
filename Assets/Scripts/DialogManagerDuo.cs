using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

public class DialogManagerDuo : MonoBehaviour
{
    [Header("UI References")]
    public Image background;
    public Image characterImageLeft;
    public Image characterImageRight;
    public TextMeshProUGUI dialogText;
    public TextMeshProUGUI speakerName;
    public GameObject continuePrompt;
    public GameObject dialogBox;
    public Image flashOverlay;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip flashSFX;

    [Header("Flash Settings")]
    public int flashAtLine = 0;
    public float flashDuration = 2f;

    [Header("Dialog Data")]
    public List<DialogLineDuo> dialogLines;

    [Header("Settings")]
    public float typingSpeed = 0.03f;
    public string nextSceneName = "JabarStoryScene";

    private int currentLine = 0;
    private bool isTyping = false;
    private bool skipTyping = false;

    void Start()
    {
        continuePrompt.SetActive(false);
        if (AudioManager.Instance != null)
            AudioManager.Instance.RegisterSFXSource(audioSource);
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

        DialogLineDuo line = dialogLines[index];

        if (background != null && line.background != null)
            background.sprite = line.background;

        if (characterImageLeft != null)
        {
            if (line.characterSpriteLeft != null)
            {
                characterImageLeft.gameObject.SetActive(true);
                characterImageLeft.sprite = line.characterSpriteLeft;
            }
            else
            {
                characterImageLeft.gameObject.SetActive(false);
            }
        }

        if (characterImageRight != null)
        {
            if (line.characterSpriteRight != null)
            {
                characterImageRight.gameObject.SetActive(true);
                characterImageRight.sprite = line.characterSpriteRight;
            }
            else
            {
                characterImageRight.gameObject.SetActive(false);
            }
        }

        if (speakerName != null)
        {
            if (line.speakerName != "")
            {
                speakerName.gameObject.SetActive(true);
                speakerName.text = line.speakerName;
            }
            else
            {
                speakerName.gameObject.SetActive(false);
            }
        }

        if (flashOverlay != null && index == flashAtLine)
            StartCoroutine(FlashEffect());

        StartCoroutine(TypeText(line.dialogText));
    }

    IEnumerator FlashEffect()
    {
        if (audioSource != null && flashSFX != null)
            audioSource.PlayOneShot(flashSFX);

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
public class DialogLineDuo
{
    public string speakerName = "";
    public Sprite background;
    public Sprite characterSpriteLeft;
    public Sprite characterSpriteRight;
    [TextArea(3, 6)]
    public string dialogText;
}