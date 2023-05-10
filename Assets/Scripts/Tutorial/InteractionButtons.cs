using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionButtons : MonoBehaviour
{

    public void pipinteraction()
    {

       // make the graph static screen appear

       // disable any existing UI elements that are currently active in the scene
        TutorialManager.PolicyUIText.SetActive(false);
        TutorialManager.AwarenessUIText.SetActive(false);
        TutorialManager.GraphUIText.SetActive(false);
        TutorialManager.TimeSliderUIText.SetActive(false);
        TutorialManager.YearSliderUIText.SetActive(false);
        TutorialManager.TimePipUIText.SetActive(false);
        TutorialManager.ConfirmChangeUIText.SetActive(false);

       // enable the UI elements
        TutorialManager.PipsUIText.SetActive(true);
        AudioPlayback.PlayOneShot(TutorialAudio.Instance.tutorialSFX, null);
        Debug.Log("Sound");
       
    }
    public void policyinteraction()
    {
        // make the graph static screen appear

       // disable any existing UI elements that are currently active in the scene
        TutorialManager.PipsUIText.SetActive(false);
        TutorialManager.PolicyUIText.SetActive(false);
        TutorialManager.AwarenessUIText.SetActive(false);
        TutorialManager.GraphUIText.SetActive(false);
        TutorialManager.TimeSliderUIText.SetActive(false);
        TutorialManager.YearSliderUIText.SetActive(false);
        TutorialManager.TimePipUIText.SetActive(false);
        TutorialManager.ConfirmChangeUIText.SetActive(false);
       // enable the UI elements
        TutorialManager.PolicyUIText.SetActive(true);
        AudioPlayback.PlayOneShot(TutorialAudio.Instance.tutorialSFX, null);
        Debug.Log("Sound");
    }
    public void awarnessInteraction()
    {
       // make the graph static screen appear

       // disable any existing UI elements that are currently active in the scene
        TutorialManager.PipsUIText.SetActive(false);
        TutorialManager.PolicyUIText.SetActive(false);
        TutorialManager.GraphUIText.SetActive(false);
        TutorialManager.TimeSliderUIText.SetActive(false);
        TutorialManager.YearSliderUIText.SetActive(false);
        TutorialManager.TimePipUIText.SetActive(false);
        TutorialManager.ConfirmChangeUIText.SetActive(false);
       // enable the UI elements
        TutorialManager.AwarenessUIText.SetActive(true);
        AudioPlayback.PlayOneShot(TutorialAudio.Instance.tutorialSFX, null);
        Debug.Log("Sound");
    }
    public void graphinteraction()
    {
       // make the graph static screen appear

       // disable any existing UI elements that are currently active in the scene
        TutorialManager.PipsUIText.SetActive(false);
        TutorialManager.PolicyUIText.SetActive(false);
        TutorialManager.AwarenessUIText.SetActive(false);
        TutorialManager.TimeSliderUIText.SetActive(false);
        TutorialManager.YearSliderUIText.SetActive(false);
        TutorialManager.TimePipUIText.SetActive(false);
        TutorialManager.ConfirmChangeUIText.SetActive(false);
       // enable the UI elements
        TutorialManager.GraphUIText.SetActive(true);
        AudioPlayback.PlayOneShot(TutorialAudio.Instance.tutorialSFX, null);
        Debug.Log("Sound");
    }
    public void timeSliderhinteraction()
    {
       // make the graph static screen appear

       // disable any existing UI elements that are currently active in the scene
        TutorialManager.PipsUIText.SetActive(false);
        TutorialManager.PolicyUIText.SetActive(false);
        TutorialManager.AwarenessUIText.SetActive(false);
        TutorialManager.GraphUIText.SetActive(false);
        TutorialManager.YearSliderUIText.SetActive(false);
        TutorialManager.TimePipUIText.SetActive(false);
        TutorialManager.ConfirmChangeUIText.SetActive(false);
       // enable the UI elements
       TutorialManager.TimeSliderUIText.SetActive(true);
       AudioPlayback.PlayOneShot(TutorialAudio.Instance.tutorialSFX, null);
       Debug.Log("Sound");
    }
    public void yearinteraction()
    {
       // make the graph static screen appear

       // disable any existing UI elements that are currently active in the scene
        TutorialManager.PipsUIText.SetActive(false);
        TutorialManager.PolicyUIText.SetActive(false);
        TutorialManager.AwarenessUIText.SetActive(false);
        TutorialManager.GraphUIText.SetActive(false);
        TutorialManager.TimeSliderUIText.SetActive(false);
        TutorialManager.TimePipUIText.SetActive(false);
        TutorialManager.ConfirmChangeUIText.SetActive(false);
       // enable the UI elements
        TutorialManager.YearSliderUIText.SetActive(true);
        AudioPlayback.PlayOneShot(TutorialAudio.Instance.tutorialSFX, null);
        Debug.Log("Sound");
    }
    public void yearKnobinteraction()
    {
       // make the graph static screen appear

       // disable any existing UI elements that are currently active in the scene
        TutorialManager.PipsUIText.SetActive(false);
        TutorialManager.PolicyUIText.SetActive(false);
        TutorialManager.AwarenessUIText.SetActive(false);
        TutorialManager.GraphUIText.SetActive(false);
        TutorialManager.TimeSliderUIText.SetActive(false);
        TutorialManager.YearSliderUIText.SetActive(false);
        TutorialManager.ConfirmChangeUIText.SetActive(false);
       // enable the UI elements
        TutorialManager.TimePipUIText.SetActive(true);
        AudioPlayback.PlayOneShot(TutorialAudio.Instance.tutorialSFX, null);
        Debug.Log("Sound");
    }
    public void confirmChangesInteraction()
    {
       // make the graph static screen appear

       // disable any existing UI elements that are currently active in the scene
        TutorialManager.PipsUIText.SetActive(false);
        TutorialManager.PolicyUIText.SetActive(false);
        TutorialManager.AwarenessUIText.SetActive(false);
        TutorialManager.GraphUIText.SetActive(false);
        TutorialManager.TimeSliderUIText.SetActive(false);
        TutorialManager.YearSliderUIText.SetActive(false);
        TutorialManager.TimePipUIText.SetActive(false);
       // enable the UI elements
        TutorialManager.ConfirmChangeUIText.SetActive(true);
        AudioPlayback.PlayOneShot(TutorialAudio.Instance.tutorialSFX, null);
        Debug.Log("Sound");
    }
}
