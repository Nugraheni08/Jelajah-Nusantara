using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DialogManager : MonoBehaviour
{
    [Header("UI References")]
    public Image background;
    public Image characterImage;
    public TextMeshProUGUI dialogText;
    public TextMeshProUGUI speakerName;
    public GameObject continuePrompt;
    public GameObject dialogBox;

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
        ShowLine(currentLine);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                skipTyping = true;
            }
            else
            {
                NextLine();
            }
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