using UnityEngine;

public class Note : MonoBehaviour
{
    public int lane;
    public float speed;
    public float hitY;
    public GameplayManager gameplayManager;

    private RectTransform rt;
    private bool missed = false;

    void Start()
    {
        rt = GetComponent<RectTransform>();
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
        gameplayManager.AddScore(100);
        Destroy(gameObject);
    }
}