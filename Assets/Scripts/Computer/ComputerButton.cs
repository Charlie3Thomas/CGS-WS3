using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum buttonType
{
    RESET,
    CONFIRM_ALLOCATION,
    CONFIRM_YEAR,
    JOURNAL_NOTEPAD
}

public class ComputerButton : MonoBehaviour
{
    public buttonType type = buttonType.RESET;

    public void Press()
    {
        switch (type)
        {
            case buttonType.RESET:
                // Reset
                Debug.Log("Reset");
                break;
            case buttonType.CONFIRM_ALLOCATION:
                // Confirm allocation
                Debug.Log("Confirm allocation");
                // Allocates the populations/factions and registers turn in a list as well as sorts the list in resource manager
                // Get index by name instead in future, im just super tired right now
                ResourceManager.instance.AllocatePopulation(ComputerController.Instance.pointSelectors[3].pointValue,
                    ComputerController.Instance.pointSelectors[0].pointValue, ComputerController.Instance.pointSelectors[1].pointValue, ComputerController.Instance.pointSelectors[2].pointValue);
                break;
            case buttonType.CONFIRM_YEAR:
                // Confirm year
                Debug.Log("Confirm year");
                break;
            case buttonType.JOURNAL_NOTEPAD:
                // Change between journal and notepad
                Debug.Log("Change screen between notepad and journal");
                break;
        }

        AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.buttonPressLEvent, null);
    }
}
