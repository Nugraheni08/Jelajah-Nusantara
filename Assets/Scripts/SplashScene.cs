using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SplashScene : MonoBehaviour
{
    [Header("UI References")]
    public Image loadingBarFill;
    public Image loadingBarBackground;
    public CanvasGroup logoGroup;
    public CanvasGroup subtitleGroup;

    [Header("Loading Bar Colors")]
    public Color barColorStart = new Color(0.85f, 0.55f, 0.10f, 1f); 
    public Color barColorEnd = new Color(0.55f, 0.28f, 0.05f, 1f);   
    public Color barBgColor = new Color(0.96f, 0.90f, 0.78f, 1f);    

    [Header("Settings")]
    public float fadeDuration = 1.5f;
    public float loadingDuration = 3f;
    public string nextSceneName = "MainMenu";

    void Start()
    {
       
        if (loadingBarBackground != null)
            loadingBarBackground.color = barBgColor;

        StartCoroutine(PlaySplash());
    }

    IEnumerator PlaySplash()
    {
        logoGroup.alpha = 0f;
        subtitleGroup.alpha = 0f;
        loadingBarFill.fillAmount = 0f;
        loadingBarFill.color = barColorStart;

        yield return StartCoroutine(FadeIn(logoGroup, fadeDuration));
        yield return StartCoroutine(FadeIn(subtitleGroup, fadeDuration * 0.5f));

        // Loading bar dengan gradient color
        float elapsed = 0f;
        while (elapsed < loadingDuration)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / loadingDuration);
            loadingBarFill.fillAmount = progress;

            // Warna berubah dari emas ke coklat sesuai progress
            loadingBarFill.color = Color.Lerp(barColorStart, barColorEnd, progress);

            yield return null;
        }

        loadingBarFill.fillAmount = 1f;
        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene(nextSceneName);
    }

    IEnumerator FadeIn(CanvasGroup cg, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Clamp01(t / duration);
            yield return null;
        }
        cg.alpha = 1f;
    }
}