using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolicyCard : MonoBehaviour
{
    [HideInInspector]
    public Policy policy;

    public void PlayShowSound()
    {
        Debug.Log("Play show sound");
    }

    public void PlayHideSound()
    {
        Debug.Log("Play hide sound");
    }
}
