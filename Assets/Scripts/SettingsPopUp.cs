using UnityEngine;
using UnityEngine.UI;

public class SettingsPopupManager : MonoBehaviour
{
    [Header("Popup Object (drag GameObject 'SettingsPopup' di sini)")]
    public GameObject settingsPopup;

    [Header("Audio")]
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        settingsPopup.SetActive(false);
        LoadAllSettings();
    }

    void LoadAllSettings()
    {
        if (AudioManager.Instance != null)
        {
            musicSlider.value = AudioManager.Instance.GetMusicVolume();
            sfxSlider.value = AudioManager.Instance.GetSFXVolume();
        }

        musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
    }

    // Panggil dari tombol "Pengaturan" di Main Menu
    public void OpenSettingsPopup()
    {
        settingsPopup.SetActive(true);
    }

    // Panggil dari tombol "Tutup" di dalam popup
    public void CloseSettingsPopup()
    {
        settingsPopup.SetActive(false);
    }

    public void OnMusicVolumeChanged(float value)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetMusicVolume(value);
    }

    public void OnSFXVolumeChanged(float value)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetSFXVolume(value);
    }
}