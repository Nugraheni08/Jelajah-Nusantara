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
    public string winSceneName;          // contoh: "JawaBaratWinScene"
    public string loseSceneName;         // contoh: "JawaBaratLoseScene"
    public string nextStorySceneName;    // cutscene akhir level ini, contoh: "JabarCutsceneAkhir" (kosongkan kalau belum ada)

    [Header("Progression (dipakai saat MENANG)")]
    [Tooltip("Key PlayerPrefs yang akan di-set ke 1 saat level ini DIMENANGKAN. " +
             "Contoh: level Jawa Barat -> isi 'MalukuUnlocked'. Level terakhir (Papua) -> kosongkan.")]
    public string unlockKeyOnWin;
}

[System.Serializable]
public class BeatNote
{
    public float time;
    public int lane; // 0=Do, 1=Mi, 2=Sol, 3=La
}