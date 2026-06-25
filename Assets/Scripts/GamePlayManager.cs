using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;
    public Image[] hearts;

    [Header("Settings")]
    public int score = 0;
    public int combo = 0;
    public int lives = 5;

    void Update()
    {
        scoreText.text = "SCORE: " + score;
        comboText.text = "COMBO: " + combo;
    }

    public void AddScore(int points)
    {
        combo++;
        score += points * combo;
    }

    public void ResetCombo()
    {
        combo = 0;
        LoseLife();
    }

    void LoseLife()
    {
        if (lives <= 0) return;
        lives--;
        hearts[lives].color = new Color(0.2f, 0.2f, 0.2f, 1f);

        if (lives <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Debug.Log("GAME OVER!");
        // nanti bisa tambah scene game over
    }
}