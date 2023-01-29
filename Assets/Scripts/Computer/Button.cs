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
        
        if (reset)
        {
            // Reset
            Debug.Log("Reset");
        }

        if (panDown)
        {
            // Pan down to journal
            Debug.Log("Pan Down");
            AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.buttonPressLEvent, null);
        }

        if (panUp)
        {
            // Pan up to tech tree screen
            Debug.Log("Pan Up");
            AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.buttonPressLEvent, null);
        }

        if (confirmYear)
        {
            // Confirm year
            Debug.Log("Confirm year");
            AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.buttonPressREvent, null);
            
        }

        if (journalNotepad)
        {
            // Change between journal and notepad
            Debug.Log("Change screen between notepad and journal");
        }
    }
}
