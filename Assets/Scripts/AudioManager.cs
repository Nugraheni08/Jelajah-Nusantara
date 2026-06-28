using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource bgmSource;   // buat lagu Sajojo
    public AudioSource sfxSource;   // buat bunyi tifa
    public AudioClip tifaHitSound;  // sample tifa pendek

    void Awake()
    {
        Instance = this;
    }

    public void PlayTifaHit()
    {
        sfxSource.pitch = Random.Range(0.97f, 1.03f);
        sfxSource.PlayOneShot(tifaHitSound);
    }
}
