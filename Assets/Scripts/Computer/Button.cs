using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField]
    private bool reset = true;
    [SerializeField]
    private bool techTree = false;
    [SerializeField]
    private bool confirmYear = false;

    public void Press()
    {
        AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.buttonPressEvent, null);
        if (reset)
        {
            // Reset
            Debug.Log("Reset");
        }

        if(techTree)
        {
            // Tech tree
            Debug.Log("Open tech tree");
        }

        if(confirmYear)
        {
            // Confirm year
            Debug.Log("Confirm year");
        }
    }
}
