using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
    public int lane;
    public float speed;
    public float hitY;
    public GameplayManager gameplayManager;

    [Header("Note Sprites per Lane")]
    public Sprite[] laneSprites; // 4 sprite, index 0-3

    private RectTransform rt;
    private bool missed = false;

    void Start()
    {
        rt = GetComponent<RectTransform>();

        // Set sprite sesuai lane
        Image img = GetComponent<Image>();
        if (img != null && laneSprites != null && lane < laneSprites.Length && laneSprites[lane] != null)
            img.sprite = laneSprites[lane];
    }

    void Update()
    {
        rt.anchoredPosition += Vector2.down * speed * Time.deltaTime;

        if (rt.anchoredPosition.y < hitY - 100f && !missed)
        {
            missed = true;
            gameplayManager.ResetCombo();
            Destroy(gameObject);
        }
    }

    public void Hit()
    {
        Debug.Log("Hit dipanggil di lane: " + lane);
        gameplayManager.PlayNoteSound(lane);
        gameplayManager.AddScore(100);
        Destroy(gameObject);
    }
}