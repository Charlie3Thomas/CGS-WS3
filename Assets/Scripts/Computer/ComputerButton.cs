using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum buttonType
{
    GENERIC,
    RESET,
    SHOW_GRAPH,
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
            case buttonType.GENERIC:
                //Debug.Log("Generic button press");
                AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.keyboardEvent, null);
                break;
            case buttonType.RESET:
                // Reset
                Debug.Log("Reset");
                AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.buttonPressLEvent, null);

                break;
            case buttonType.SHOW_GRAPH:
                // Confirm allocation
                Debug.Log("Show Graph");
                // Allocates the populations/factions and registers turn in a list as well as sorts the list in resource manager
                // Get index by name instead in future, im just super tired right now
                ComputerController.Instance.showGraph = !ComputerController.Instance.showGraph;
                ComputerController.Instance.screen.SetActive(!ComputerController.Instance.showGraph);
                ComputerController.Instance.graph.SetActive(ComputerController.Instance.showGraph);
                AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.buttonPressLEvent, null);

                break;
            case buttonType.CONFIRM_YEAR:
                // Confirm year
                YearData._INSTANCE.current_year = ComputerController.Instance.desiredYear;

                // Allocate Resource Finalised
                ResourceManager.instance.AllocatePopulation(ComputerController.Instance.pointSelectors[3].pointValue,
                    ComputerController.Instance.pointSelectors[0].pointValue, ComputerController.Instance.pointSelectors[1].pointValue, ComputerController.Instance.pointSelectors[2].pointValue);

                Debug.Log("Year confirmed! The year is now: " + YearData._INSTANCE.current_year);
                ComputerController.Instance.UpdateSlider();

                AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.buttonPressLEvent, null);

                break;
            case buttonType.JOURNAL_NOTEPAD:
                // Change between journal and notepad
                Debug.Log("Change screen between notepad and journal");
                ComputerController.Instance.notepad.SetActive(!ComputerController.Instance.notepad.activeSelf);
                ComputerController.Instance.journal.SetActive(!ComputerController.Instance.journal.activeSelf);
                AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.buttonPressLEvent, null);

                break;
        }
    }

    void OnMouseOver()
    {
        if(type == buttonType.SHOW_GRAPH || type == buttonType.CONFIRM_YEAR)
            CustomCursor.Instance.OnHoverOverResourceSelector();
    }
    private void OnMouseExit()
    {
        if(type == buttonType.SHOW_GRAPH || type == buttonType.CONFIRM_YEAR)
            CustomCursor.Instance.SetDefaultCursor();
    }
}
