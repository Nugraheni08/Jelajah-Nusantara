using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
    [Header("Level Info")]
    public string levelName;
    public Sprite background;
    public AudioClip music;

    [Header("Note Sounds")]
    public AudioClip[] noteClips; // 4 suara: Do, Mi, Sol, La

    [Header("Beat Map")]
    // Format: time, lane
    public BeatNote[] beatMap;

    [Header("Scene Names")]
    public string winSceneName;
    public string loseSceneName;
    public string nextStorySceneName;
}

[System.Serializable]
public class BeatNote
{
    public float time;
    public int lane; // 0=Do, 1=Mi, 2=Sol, 3=La
}