using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

public class JabarDialogManager : MonoBehaviour
{
    [Header("UI References")]
    public Image background;
    public Image characterImageLeft;
    public Image characterImageRight;
    public TextMeshProUGUI dialogText;
    public TextMeshProUGUI speakerName;
    public GameObject continuePrompt;
    public GameObject dialogBox;
    public GameObject skipButton;
    public Image flashOverlay;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip noiseSFX;
    public AudioClip dingSFX;
    public AudioClip clickSFX;
    public int noiseStartLine = 999;
    public int noiseEndLine = 999;

    [Header("Flash Settings")]
    public int flashAtLine = 999;
    public float flashDuration = 1.5f;

    [Header("Dialog Data")]
    public List<JabarDialogLine> dialogLines;

    [Header("Settings")]
    public float typingSpeed = 0.03f;
    public string nextSceneName = "TutorialScene";

    private int currentLine = 0;
    private bool isTyping = false;
    private bool skipTyping = false;

    void Start()
    {
        continuePrompt.SetActive(false);

        if (flashOverlay != null)
            flashOverlay.color = new Color(1, 1, 1, 0);

        if (audioSource != null && dingSFX != null)
            audioSource.PlayOneShot(dingSFX);

        ShowLine(currentLine);
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame ||
            Keyboard.current.spaceKey.wasPressedThisFrame ||
            Keyboard.current.enterKey.wasPressedThisFrame)
        {
            Debug.Log("Klik! audioSource=" + (audioSource != null ? "ada" : "NULL") + " clickSFX=" + (clickSFX != null ? clickSFX.name : "NULL"));
            
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
            SceneManager.LoadScene(nextSceneName);
            return;
        }

        JabarDialogLine line = dialogLines[index];

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

        if (audioSource != null && noiseSFX != null)
        {
            if (index >= noiseStartLine && index <= noiseEndLine)
            {
                if (audioSource.clip != noiseSFX)
                {
                    audioSource.clip = noiseSFX;
                    audioSource.loop = true;
                    audioSource.Play();
                }
            }
            else
            {
                if (audioSource.clip == noiseSFX && audioSource.isPlaying)
                {
                    audioSource.Stop();
                    audioSource.clip = null;
                }
            }
        }

        StartCoroutine(TypeText(line.dialogText));
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

    public void OnSkipClicked()
    {
        StopAllCoroutines();
        if (skipButton != null) skipButton.SetActive(false);
        ShowLine(dialogLines.Count);
    }
}

[System.Serializable]
public class JabarDialogLine
{
    public string speakerName = "";
    public Sprite background;
    public Sprite characterSpriteLeft;
    public Sprite characterSpriteRight;
    [TextArea(3, 6)]
    public string dialogText;
}