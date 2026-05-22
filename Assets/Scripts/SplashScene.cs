using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SplashScene : MonoBehaviour
{
    [Header("UI References")]
    public Image loadingBarFill;
    public CanvasGroup logoGroup;
    public CanvasGroup subtitleGroup;

    [Header("Settings")]
    public float fadeDuration = 1.5f;
    public float loadingDuration = 3f;
    public string nextSceneName = "MainMenu";

    void Start()
    {
        StartCoroutine(PlaySplash());
    }

    IEnumerator PlaySplash()
    {
        // Fade in logo
        logoGroup.alpha = 0f;
        subtitleGroup.alpha = 0f;
        loadingBarFill.fillAmount = 0f;

        yield return StartCoroutine(FadeIn(logoGroup, fadeDuration));
        yield return StartCoroutine(FadeIn(subtitleGroup, fadeDuration * 0.5f));

        // Loading bar progress
        float elapsed = 0f;
        while (elapsed < loadingDuration)
        {
            elapsed += Time.deltaTime;
            loadingBarFill.fillAmount = Mathf.Clamp01(elapsed / loadingDuration);
            yield return null;
        }

        loadingBarFill.fillAmount = 1f;
        yield return new WaitForSeconds(0.5f);

        // Load next scene
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