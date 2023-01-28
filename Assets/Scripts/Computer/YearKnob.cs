using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YearKnob : MonoBehaviour
{
    public void YearUp()
    {
        YearData._INSTANCE.YearUp();
        ComputerController.Instance.UpdateSlider();
        AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.dialRightEvent, null);
    }

    public void YearDown()
    {
        YearData._INSTANCE.YearDown();
        ComputerController.Instance.UpdateSlider();
        AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.dialLeftEvent, null);
    }
}
