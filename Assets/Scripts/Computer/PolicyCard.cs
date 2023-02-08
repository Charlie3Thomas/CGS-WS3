using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolicyCard : MonoBehaviour
{
    [HideInInspector]
    public Policy policy;

    public void PlayShowSound()
    {
        AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.policyCardShowEvent, null);
    }

    public void PlayHideSound()
    {
        Debug.Log("Play hide sound");
    }
}
