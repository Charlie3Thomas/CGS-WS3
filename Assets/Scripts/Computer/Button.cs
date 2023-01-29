using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum buttonType
{
    RESET,
    PAN_UP,
    PAN_DOWN,
    CONFIRM_YEAR,
    JOURNAL_NOTEPAD
}

public class Button : MonoBehaviour
{
    public buttonType type = buttonType.RESET;

    public void Press()
    {
        switch(type)
        {
            case buttonType.RESET:
                // Reset
                Debug.Log("Reset");
                break;
            case buttonType.PAN_DOWN:
                // Pan down to journal
                Debug.Log("Pan Down");
                AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.buttonPressLEvent, null);
                break;
            case buttonType.PAN_UP:
                // Pan up to tech tree screen
                Debug.Log("Pan Up");
                AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.buttonPressLEvent, null);
                break;
            case buttonType.CONFIRM_YEAR:
                // Confirm year
                Debug.Log("Confirm year");
                AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.buttonPressREvent, null);
                break;
            case buttonType.JOURNAL_NOTEPAD:
                // Change between journal and notepad
                Debug.Log("Change screen between notepad and journal");
                break;
        }
    }
}
