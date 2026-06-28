using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Source untuk Musik Background")]
    public AudioSource musicSource;

    [Header("Audio Source untuk SFX (klik tombol, dll)")]
    public AudioSource sfxSource;

    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";

    void Awake()
    {
        // Singleton pattern: hanya 1 AudioManager yang boleh ada
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadVolumeSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void LoadVolumeSettings()
    {
        float musicVol = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 0.7f);
        float sfxVol = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 0.7f);

        SetMusicVolume(musicVol);
        SetSFXVolume(sfxVol);
    }

    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
            musicSource.volume = volume;

        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float volume)
    {
        if (sfxSource != null)
            sfxSource.volume = volume;

        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, volume);
        PlayerPrefs.Save();
    }

    public float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 0.7f);
    }

    public float GetSFXVolume()
    {
        return PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 0.7f);
    }

    // Panggil ini untuk mainkan SFX sekali (misal pas klik tombol)
    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
            sfxSource.PlayOneShot(clip);
    }

    // Panggil ini untuk ganti musik background (misal pas pindah scene beda mood)
    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (musicSource == null) return;

        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }
}