using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField]
    private bool reset = true;
    [SerializeField]
    private bool panUp = false;
    [SerializeField]
    private bool panDown = false;
    [SerializeField]
    private bool confirmYear = false;
    [SerializeField]
    private bool journalNotepad = false;

    public void Press()
    {
        AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.buttonPressEvent, null);
        if (reset)
        {
            // Reset
            Debug.Log("Reset");
        }

        if (panDown)
        {
            // Pan down to journal
            Debug.Log("Pan Down");
        }

        if (panUp)
        {
            // Pan up to tech tree screen
            Debug.Log("Pan Up");
        }

        if (confirmYear)
        {
            // Confirm year
            Debug.Log("Confirm year");
        }

        if (journalNotepad)
        {
            // Change between journal and notepad
            Debug.Log("Change screen between notepad and journal");
        }
    }
}
