using UnityEngine;
using UnityEngine.Audio;
using System;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Source untuk Musik Background")]
    public AudioSource musicSource;

    [Header("Audio Source untuk SFX (klik tombol, dll)")]
    public AudioSource sfxSource;

    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";

    // Dipanggil setiap kali volume musik/SFX berubah (dari slider mana pun, scene mana pun).
    // Semua AudioSource musik/SFX di game bisa subscribe ke ini supaya volumenya selalu sinkron.
    public static event Action<float> OnMusicVolumeChanged;
    public static event Action<float> OnSFXVolumeChanged;

    // Daftar AudioSource musik/SFX yang aktif saat ini, supaya bisa langsung di-update volumenya
    // tanpa perlu tiap script subscribe/unsubscribe manual ke event di atas.
    private readonly List<AudioSource> musicSources = new List<AudioSource>();
    private readonly List<AudioSource> sfxSources = new List<AudioSource>();

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

        musicSources.RemoveAll(src => src == null);
        foreach (AudioSource src in musicSources)
            src.volume = volume;

        OnMusicVolumeChanged?.Invoke(volume);
    }

    public void SetSFXVolume(float volume)
    {
        if (sfxSource != null)
            sfxSource.volume = volume;

        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, volume);
        PlayerPrefs.Save();

        sfxSources.RemoveAll(src => src == null);
        foreach (AudioSource src in sfxSources)
            src.volume = volume;

        OnSFXVolumeChanged?.Invoke(volume);
    }

    // Panggil ini sekali di Start() pada AudioSource yang berfungsi sebagai MUSIK
    // (musik gameplay, musik story, musik win/lose, dst). Otomatis langsung diset
    // ke volume tersimpan, dan akan ikut ter-update kalau slider musik digeser nanti.
    public void RegisterMusicSource(AudioSource source)
    {
        if (source == null) return;
        if (!musicSources.Contains(source))
            musicSources.Add(source);
        source.volume = GetMusicVolume();
    }

    // Sama seperti RegisterMusicSource, tapi untuk AudioSource yang berfungsi sebagai SFX
    // (ketuk note, klik tombol, dst).
    public void RegisterSFXSource(AudioSource source)
    {
        if (source == null) return;
        if (!sfxSources.Contains(source))
            sfxSources.Add(source);
        source.volume = GetSFXVolume();
    }

    // Panggil di OnDestroy() pada AudioSource yang sudah Register, supaya nggak nyangkut
    // di daftar setelah objeknya hancur (opsional, list juga otomatis dibersihkan saat set volume).
    public void UnregisterSource(AudioSource source)
    {
        musicSources.Remove(source);
        sfxSources.Remove(source);
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
    public void StopMusic()
    {
        if (musicSource != null)
            musicSource.Stop();
    }
}