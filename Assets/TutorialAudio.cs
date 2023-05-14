using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class TutorialAudio : MonoBehaviour
{
    public static TutorialAudio Instance => m_instance;
    private static TutorialAudio m_instance;

    public EventReference tutorialSFX;

    void Start()
    {
        m_instance = this;
    }
   
}
