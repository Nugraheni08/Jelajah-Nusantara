using UnityEngine;

public class QuitGame : MonoBehaviour
{
    // Panggil fungsi ini dari OnClick() tombol "Keluar"
    public void OnQuitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}