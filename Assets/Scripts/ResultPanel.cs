using UnityEngine;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour
{
    [Header("Stars")]
    public Image star1;
    public Image star2;
    public Image star3;

    [Header("Sprites")]
    public Sprite starOn;   // bintang kuning
    public Sprite starOff;  // bintang abu/kosong

    void Start()
    {
        // Kalau mau test langsung, set score manual dulu
        // PlayerPrefs.SetInt("LastScore", 8000); // uncomment untuk test

        int score = PlayerPrefs.GetInt("LastScore", 0);
        Debug.Log("Score dibaca: " + score);
        UpdateStars(score);
    }

    void UpdateStars(int score)
    {
        // Atur threshold sesuai keinginan
        bool s1 = score >= 1000;
        bool s2 = score >= 5000;
        bool s3 = score >= 10000;

        star1.sprite = s1 ? starOn : starOff;
        star2.sprite = s2 ? starOn : starOff;
        star3.sprite = s3 ? starOn : starOff;
    }
}