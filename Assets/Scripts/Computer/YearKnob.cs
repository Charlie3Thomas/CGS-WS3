using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YearKnob : MonoBehaviour
{
    public void YearUp()
    {
        ComputerController.Instance.desiredYear += 5;
        ComputerController.Instance.UpdateSlider();
        AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.dialRightEvent, null);
    }

    public void YearDown()
    {
        ComputerController.Instance.desiredYear -= 5;
        ComputerController.Instance.UpdateSlider();
        AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.dialRightEvent, null);
    }
}
