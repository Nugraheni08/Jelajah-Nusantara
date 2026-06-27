using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;


public class MalukuDialogManager : MonoBehaviour
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

    [Header("Misi Panel")]
    public GameObject misiPanel;
    public TextMeshProUGUI misiTitle;
    public TextMeshProUGUI misiDesc;
    public GameObject misiButton;
    public GameObject darkOverlay;

    [Header("Countdown Panel")]
    public GameObject countdownPanel;
    public TextMeshProUGUI countdownText;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip noiseSFX;
    public int noiseStartLine = 3;
    public int noiseEndLine = 4;

    [Header("Flash Settings")]
    public int flashAtLine = 999;
    public float flashDuration = 1.5f;

    [Header("Dialog Data")]
    public List<MalukuDialogLine> dialogLines;

    [Header("Settings")]
    public float typingSpeed = 0.03f;
    public string gameplaySceneName = "GameplayScene";

    private int currentLine = 0;
    private bool isTyping = false;
    private bool skipTyping = false;
    private bool waitingForMisi = false;

    void Start()
    {
        continuePrompt.SetActive(false);

        if (flashOverlay != null)
            flashOverlay.color = new Color(1, 1, 1, 0);

        if (misiPanel != null)
            misiPanel.SetActive(false);

        if (darkOverlay != null)
            darkOverlay.SetActive(false);

        if (countdownPanel != null)
            countdownPanel.SetActive(false);

        ShowLine(currentLine);
    }

    void Update()
    {
        if (waitingForMisi) return;

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
            ShowMisiPanel();
            return;
        }

        MalukuDialogLine line = dialogLines[index];

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

        if (audioSource != null && noiseSFX != null)
        {
            if (index >= noiseStartLine && index <= noiseEndLine)
            {
                if (!audioSource.isPlaying)
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

    void ShowMisiPanel()
    {
        Debug.Log("SHOW MISI PANEL");

        waitingForMisi = true;

        continuePrompt.SetActive(false);
        dialogBox.SetActive(false);

        darkOverlay.SetActive(true);
        misiPanel.SetActive(true);
    }

    public void OnMisiButtonClick()
    {
        PlayerPrefs.SetString("SelectedLevel", "LevelData_Maluku");
        PlayerPrefs.Save();

        Debug.Log("Disimpan = " + PlayerPrefs.GetString("SelectedLevel"));

        if (misiPanel != null)
            misiPanel.SetActive(false);

        if (darkOverlay != null)
            darkOverlay.SetActive(false);

        StartCoroutine(CountdownAndStart());
    }

    IEnumerator CountdownAndStart()
    {
        Debug.Log("Countdown dimulai");

        if (countdownPanel != null)
        {
            countdownPanel.SetActive(true);
            string[] counts = { "3", "2", "1", "START!" };

            foreach (string c in counts)
            {
                if (countdownText != null)
                    countdownText.text = c;

                yield return new WaitForSeconds(1f);
            }

            countdownPanel.SetActive(false);
        }

        Debug.Log("Load Scene: " + gameplaySceneName);

        SceneManager.LoadScene(gameplaySceneName);
    }

    IEnumerator FlashEffect()
    {
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
public class MalukuDialogLine
{
    public string speakerName = "";
    public Sprite background;
    public Sprite characterSpriteLeft;
    public Sprite characterSpriteRight;
    [TextArea(3, 6)]
    public string dialogText;
}