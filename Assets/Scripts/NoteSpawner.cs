using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class NoteSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject notePrefab;
    public Transform canvas;
    public GameplayManager gameplayManager;
    public AudioSource musicSource;
    public Image backgroundImage;

    [Header("Lane Positions")]
    public float[] laneXPositions = { -270f, -90f, 90f, 270f };

    [Header("Settings")]
    public float spawnY = 400f;
    public float noteSpeed = 300f;
    public float hitY = -280f;

    private LevelData levelData;
    private int currentBeatIndex = 0;
    private float songTimer = 0f;
    private bool songStarted = false;

    void Start()
    {
        // Load level data
        string levelName = PlayerPrefs.GetString("SelectedLevel", "LevelData_Maluku");
        levelData = Resources.Load<LevelData>(levelName);

        // Kalau masih null coba load langsung
        if (levelData == null)
            levelData = Resources.Load<LevelData>("LevelData_Maluku");

        if (levelData == null)
        {
            Debug.LogError("LevelData tidak ditemukan: " + levelName);
            return;
        }

        // Set background
        if (backgroundImage != null && levelData.background != null)
        {
            backgroundImage.sprite = levelData.background;
        }

        // Play musik
        if (musicSource != null && levelData.music != null)
        {
            musicSource.clip = levelData.music;
            musicSource.Play();
            songStarted = true;
        }
        else
        {
            Debug.LogWarning("Music source atau musik kosong!");
            songStarted = true; // tetap start biar note spawn
        }

        // Load note sounds ke GameplayManager
        if (gameplayManager != null && levelData.noteClips != null)
        {
            gameplayManager.noteClips = levelData.noteClips;
            Debug.Log("Note clips loaded: " + levelData.noteClips.Length);
        }
        else
        {
            Debug.LogWarning("noteClips kosong di LevelData!");
        }
    }

    void Update()
    {
        if (!songStarted || levelData == null) return;

        songTimer += Time.deltaTime;

        while (currentBeatIndex < levelData.beatMap.Length &&
               songTimer >= levelData.beatMap[currentBeatIndex].time)
        {
            SpawnNote(levelData.beatMap[currentBeatIndex].lane);
            currentBeatIndex++;
        }

        if (currentBeatIndex >= levelData.beatMap.Length &&
            musicSource != null && !musicSource.isPlaying)
        {
            gameplayManager.SongFinished();
        }
    }

    void SpawnNote(int lane)
    {
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