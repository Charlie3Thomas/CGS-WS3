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
    private bool journal = false;
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

        if (journal)
        {
            // Tech tree
            Debug.Log("Pan Down");
        }

        if (techTree)
        {
            // Tech tree
            Debug.Log("Pan Up");
        }

        if (confirmYear)
        {
            // Confirm year
            Debug.Log("Confirm year");
        }
    }
}
