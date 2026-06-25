using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public GameplayManager gameplayManager;
    public float hitRange = 300f;

    void Update()
    {
        if (Keyboard.current.dKey.wasPressedThisFrame) CheckHit(0);
        if (Keyboard.current.fKey.wasPressedThisFrame) CheckHit(1);
        if (Keyboard.current.jKey.wasPressedThisFrame) CheckHit(2);
        if (Keyboard.current.kKey.wasPressedThisFrame) CheckHit(3);
    }

    void CheckHit(int lane)
    {
        Note[] notes = FindObjectsByType<Note>(FindObjectsSortMode.None);
        Note closestNote = null;
        float closestDist = hitRange;

        foreach (Note note in notes)
        {
            if (note.lane == lane)
            {
                RectTransform rt = note.GetComponent<RectTransform>();
                float dist = Mathf.Abs(rt.anchoredPosition.y - (-280f));
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closestNote = note;
                }
            }
        }

        if (closestNote != null)
            closestNote.Hit();
        else
            gameplayManager.ResetCombo();
    }
}