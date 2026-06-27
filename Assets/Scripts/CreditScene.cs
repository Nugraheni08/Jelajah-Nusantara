using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditScroller : MonoBehaviour
{
    [Header("Content yang berisi semua text credit (Title, Matkul, Dosen, dst)")]
    public RectTransform content;

    [Header("Kecepatan scroll (pixel per detik)")]
    public float scrollSpeed = 60f;

    [Header("Posisi Y akhir (saat semua teks sudah lewat ke atas, scroll berhenti)")]
    public float endPosY = 1200f;

    [Header("Nama Scene Main Menu (untuk tombol Back)")]
    public string mainMenuSceneName = "MainMenuScene";

    private bool isScrolling = true;

    void Update()
    {
        if (!isScrolling) return;

        // Geser Content ke atas (Y makin besar = makin naik di UI Canvas)
        Vector2 pos = content.anchoredPosition;
        pos.y += scrollSpeed * Time.deltaTime;
        content.anchoredPosition = pos;

        // Berhenti kalau sudah lewat batas akhir
        if (pos.y >= endPosY)
        {
            pos.y = endPosY;
            content.anchoredPosition = pos;
            isScrolling = false;
        }
    }

    // Panggil fungsi ini dari OnClick() tombol "Back"
    public void OnBackButtonClicked()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}