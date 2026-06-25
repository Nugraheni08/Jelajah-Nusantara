using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class StoryManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Image background;
    public GameObject kotakDialog;
    public TextMeshProUGUI teksDialog;
    public TextMeshProUGUI namaKarakter;
    public Button nextButton;

    [Header("Story Content")]
    public Sprite[] backgrounds;
    public string[] namaList;
    public string[] dialogList;

    [Header("Scene Berikutnya")]
    public string namaSceneBerikutnya = "GamePlayScene";

    private int currentIndex = 0;
    private bool isTyping = false;
    private string fullText = "";

    void Start()
    {
        nextButton.onClick.AddListener(OnNextClicked);
        TampilkanDialog(currentIndex);
    }

    void TampilkanDialog(int index)
    {
        if (index >= dialogList.Length)
        {
            SceneManager.LoadScene(namaSceneBerikutnya);
            return;
        }

        // Ganti background
        if (backgrounds != null && index < backgrounds.Length && backgrounds[index] != null)
            background.sprite = backgrounds[index];

        string dialog = dialogList[index];
        string nama = (namaList != null && index < namaList.Length) ? namaList[index] : "";

        // Sembunyikan kotak dialog kalau kosong
        if (string.IsNullOrEmpty(dialog) && string.IsNullOrEmpty(nama))
        {
            kotakDialog.SetActive(false);
            isTyping = false;
            fullText = "";
        }
        else
        {
            kotakDialog.SetActive(true);
            namaKarakter.text = nama;
            fullText = dialog;
            StopAllCoroutines();
            StartCoroutine(TypewriterEffect(fullText));
        }
    }

    System.Collections.IEnumerator TypewriterEffect(string text)
    {
        isTyping = true;
        teksDialog.text = "";
        foreach (char c in text)
        {
            teksDialog.text += c;
            yield return new WaitForSeconds(0.03f);
        }
        isTyping = false;
    }

    void OnNextClicked()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            teksDialog.text = fullText;
            isTyping = false;
        }
        else
        {
            currentIndex++;
            TampilkanDialog(currentIndex);
        }
    }
}