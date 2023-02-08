using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notepad : MonoBehaviour
{
    public void PlayShowSound()
    {
        AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.disasterUIShowEvent, null);
    }

    public void PlayHideSound()
    {
        AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.disasterUIHideEvent, null);
    }
}
