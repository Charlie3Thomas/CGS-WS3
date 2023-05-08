using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YearKnob : MonoBehaviour
{
    public void YearUp()
    {
        // if 
        if (ComputerController.Instance.desiredYear < 2100)
            ComputerController.Instance.desiredYear += 5;

        ComputerController.Instance.UpdateSlider();
        AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.dialRightEvent, null);
    }

    public void YearDown()
    {
        if(ComputerController.Instance.desiredYear > 1900)
            ComputerController.Instance.desiredYear -= 5;

        ComputerController.Instance.UpdateSlider();
        AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.dialRightEvent, null);
    }

    void OnMouseOver()
    {
        CustomCursor.Instance.OnHoverOverKnobSelector();
    }
    private void OnMouseExit()
    {
        CustomCursor.Instance.SetDefaultCursor();
    }
}
