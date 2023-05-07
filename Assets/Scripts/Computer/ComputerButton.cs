using CT;
using CT.Data;
using CT.Enumerations;
using CT.Lookup;
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
                {
                    //Debug.Log("Generic button press");
                    AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.keyboardEvent, null);
                    break;
                }

            case buttonType.RESET:
                {
                    // Reset
                    // Debug.Log("Reset");
                    AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.buttonPressLEvent, null);

                    break;
                }

            case buttonType.SHOW_GRAPH:
                {
                    // Confirm allocation
                    Debug.Log("Show Graph");
                    // Allocates the populations/factions and registers turn in a list as well as sorts the list in resource manager
                    // Get index by name instead in future, im just super tired right now

                    // Plot graph with necessary values when showing graph
                    ComputerController.Instance.RefreshGraph();

                    ComputerController.Instance.showGraph = !ComputerController.Instance.showGraph;
                    ComputerController.Instance.screen.SetActive(!ComputerController.Instance.showGraph);
                    ComputerController.Instance.graphGO.SetActive(ComputerController.Instance.showGraph);

                    if(ComputerController.Instance.showGraph)
                    {
                        // Set Colours
                        ComputerController.Instance.currencyText.color = DataSheet.WORKER_COLOUR;
                        ComputerController.Instance.rpText.color = DataSheet.SCIENTIST_COLOUR;
                        ComputerController.Instance.foodText.color = DataSheet.FARMER_COLOUR;
                        ComputerController.Instance.pointSelectors[0].pipMat.SetInt("_isGraph", 1);
                        ComputerController.Instance.pointSelectors[1].pipMat.SetInt("_isGraph", 1);
                        ComputerController.Instance.pointSelectors[2].pipMat.SetInt("_isGraph", 1);
                        ComputerController.Instance.pointSelectors[3].pipMat.SetInt("_isGraph", 1);
                    }
                    else
                    {
                        // Reset Colours
                        ComputerController.Instance.currencyText.color = DataSheet.DEFAULT_COLOR;
                        ComputerController.Instance.rpText.color = DataSheet.DEFAULT_COLOR;
                        ComputerController.Instance.foodText.color = DataSheet.DEFAULT_COLOR;
                        ComputerController.Instance.pointSelectors[0].pipMat.SetInt("_isGraph", 0);
                        ComputerController.Instance.pointSelectors[1].pipMat.SetInt("_isGraph", 0);
                        ComputerController.Instance.pointSelectors[2].pipMat.SetInt("_isGraph", 0);
                        ComputerController.Instance.pointSelectors[3].pipMat.SetInt("_isGraph", 0);

                        // Reset Pips

                        // Scientist
                        ComputerController.Instance.pointSelectors[0].pipMat.SetFloat("_FillAmount", GameManager._INSTANCE.GetFactionDistribution().y * 10);
                        // Plan
                        ComputerController.Instance.pointSelectors[1].pipMat.SetFloat("_FillAmount", GameManager._INSTANCE.GetFactionDistribution().w * 10);
                        // Farmer
                        ComputerController.Instance.pointSelectors[3].pipMat.SetFloat("_FillAmount", GameManager._INSTANCE.GetFactionDistribution().z * 10);
                        // Worker
                        ComputerController.Instance.pointSelectors[2].pipMat.SetFloat("_FillAmount", GameManager._INSTANCE.GetFactionDistribution().x * 10);

                        // Reset Counters
                        GameManager._INSTANCE.UpdateResourceCounters();

                    }

                    AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.buttonPressLEvent, null);

                    break;
                }

            case buttonType.CONFIRM_YEAR:
                {
                    // Confirm year
                    //YearData._INSTANCE.current_year = ComputerController.Instance.desiredYear;
                    //Debug.Log("Year confirmed! The year is now: " + YearData._INSTANCE.current_year);

                    uint turn = (uint)((ComputerController.Instance.desiredYear - DataSheet.STARTING_YEAR) / 5);

                    GameManager._INSTANCE.OnClickCheckoutYearButton(turn);

                    // Plot graph with necessary values when year changes in case graph is already showing and we need to update it
                    ComputerController.Instance.RefreshGraph();

                    // Refresh Policies
                    PolicyManager.instance.LoadPoliciesForTurn();

                    // Ensures colour of counter is correct on check out
                    ComputerController.Instance.UpdateSlider();

                    AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.buttonPressLEvent, null);

                    ComputerController.Instance.journal.GetComponent<Journal>().UpdateFactionProductionText();

                    break;
                }

            case buttonType.JOURNAL_NOTEPAD:
                {
                    // Change between journal and notepad
                    // Debug.Log("Change screen between notepad and journal");
                    ComputerController.Instance.notepad.SetActive(!ComputerController.Instance.notepad.activeSelf);
                    ComputerController.Instance.journal.SetActive(!ComputerController.Instance.journal.activeSelf);
                    AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.buttonPressLEvent, null);

                    break;
                }
        }
    }

    void OnMouseOver()
    {
        if (type == buttonType.SHOW_GRAPH || type == buttonType.CONFIRM_YEAR)
            CustomCursor.Instance.OnHoverOverResourceSelector();
    }
    private void OnMouseExit()
    {
        if (type == buttonType.SHOW_GRAPH || type == buttonType.CONFIRM_YEAR)
            CustomCursor.Instance.SetDefaultCursor();
    }
}
