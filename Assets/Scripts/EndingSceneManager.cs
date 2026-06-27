using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingSceneManager : MonoBehaviour
{
    [Header("Panel-panel Ending (urutan sesuai tampil)")]
    public GameObject panel1; // Tamat
    public GameObject panel2; // Kutipan
    public GameObject panel3; // Terima Kasih + tombol Kembali

    [Header("Waktu tampil tiap panel (detik)")]
    public float delayPerPanel = 5f;

    [Header("Nama Scene Main Menu")]
    public string mainMenuSceneName = "MainMenuScene";

    void Start()
    {
        // Pastikan cuma Panel1 yang aktif di awal
        panel1.SetActive(true);
        panel2.SetActive(false);
        panel3.SetActive(false);

        StartCoroutine(PlayEndingSequence());
    }

    IEnumerator PlayEndingSequence()
    {
        // Tampilkan Panel1 selama delayPerPanel detik
        yield return new WaitForSeconds(delayPerPanel);

        panel1.SetActive(false);
        panel2.SetActive(true);

        // Tampilkan Panel2 selama delayPerPanel detik
        yield return new WaitForSeconds(delayPerPanel);

        panel2.SetActive(false);
        panel3.SetActive(true);

        // Panel3 tetap tampil, menunggu user klik tombol "Kembali"
    }

    // Panggil fungsi ini dari OnClick() tombol "Kembali" di Panel3
    public void OnKembaliButtonClicked()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}