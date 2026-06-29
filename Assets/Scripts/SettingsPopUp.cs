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
        // Listener selalu dipasang duluan, terlepas dari AudioManager sudah siap atau belum,
        // supaya geser slider tetap selalu berfungsi.
        musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);

        if (AudioManager.Instance != null)
        {
            musicSlider.value = AudioManager.Instance.GetMusicVolume();
            sfxSlider.value = AudioManager.Instance.GetSFXVolume();
        }
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
        Debug.Log("OnMusicVolumeChanged dipanggil, value=" + value + ", AudioManager.Instance=" + (AudioManager.Instance != null));
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetMusicVolume(value);
    }

    public void OnSFXVolumeChanged(float value)
    {
        Debug.Log("OnSFXVolumeChanged dipanggil, value=" + value + ", AudioManager.Instance=" + (AudioManager.Instance != null));
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetSFXVolume(value);
    }
}