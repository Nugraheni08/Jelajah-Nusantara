using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    [Header("Card Buttons")]
    public Button cardJabar;
    public Button cardMaluku;
    public Button cardPapua;

    [Header("Card Sprites - Unlocked")]
    public Sprite jabarUnlocked;
    public Sprite malukuUnlocked;
    public Sprite papuaUnlocked;

    [Header("Card Sprites - Locked")]
    public Sprite malukuLocked;
    public Sprite papuaLocked;

    [Header("Overlay Peta (warna merah)")]
    public GameObject overlayJabar;
    public GameObject overlayMaluku;
    public GameObject overlayPapua;

    [Header("Scene Tujuan")]
    public string sceneJabar = "JawaBaratStory";
    public string sceneMaluku = "MalukuStoryScene";
    public string scenePapua = "PapuaStoryScene";

    void Start()
    {
        UpdateTampilan();

        cardJabar.onClick.AddListener(() => SceneManager.LoadScene(sceneJabar));
        cardMaluku.onClick.AddListener(() => SceneManager.LoadScene(sceneMaluku));
        cardPapua.onClick.AddListener(() => SceneManager.LoadScene(scenePapua));
    }

    void UpdateTampilan()
    {
        bool malukuUnlock = PlayerPrefs.GetInt("MalukuUnlocked", 0) == 1;
        bool papuaUnlock = PlayerPrefs.GetInt("PapuaUnlocked", 0) == 1;

        // Jawa Barat selalu unlocked
        cardJabar.interactable = true;
        cardJabar.image.sprite = jabarUnlocked;
        overlayJabar.SetActive(true);

        // Maluku
        cardMaluku.interactable = malukuUnlock;
        cardMaluku.image.sprite = malukuUnlock ? malukuUnlocked : malukuLocked;
        overlayMaluku.SetActive(malukuUnlock);

        // Papua
        cardPapua.interactable = papuaUnlock;
        cardPapua.image.sprite = papuaUnlock ? papuaUnlocked : papuaLocked;
        overlayPapua.SetActive(papuaUnlock);
    }
}