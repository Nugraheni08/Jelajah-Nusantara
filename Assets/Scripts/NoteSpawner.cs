using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject notePrefab;
    public Transform canvas;
    public GameplayManager gameplayManager;

    [Header("Lane Positions")]
    public float[] laneXPositions = { -270f, -90f, 90f, 270f };

    [Header("Settings")]
    public float spawnY = 400f;
    public float noteSpeed = 300f;
    public float spawnInterval = 1f;
    public float hitY = -280f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnNote();
            timer = 0f;
        }
    }

    void SpawnNote()
{
    int lane = Random.Range(0, 4);
    GameObject note = Instantiate(notePrefab, canvas);
    RectTransform rt = note.GetComponent<RectTransform>();
    rt.anchoredPosition = new Vector2(laneXPositions[lane], spawnY);

    Note noteScript = note.GetComponent<Note>();
    noteScript.lane = lane;
    noteScript.speed = noteSpeed;
    noteScript.hitY = hitY;
    noteScript.gameplayManager = gameplayManager;
}
}