using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        TutorialManager.Index++;
        AudioPlayback.PlayOneShot(TutorialAudio.Instance.tutorialSFX, null);
       
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
        TutorialManager.Index++;
        AudioPlayback.PlayOneShot(TutorialAudio.Instance.tutorialSFX, null);
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
        TutorialManager.Index++;
        AudioPlayback.PlayOneShot(TutorialAudio.Instance.tutorialSFX, null);
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
        TutorialManager.Index++;
        AudioPlayback.PlayOneShot(TutorialAudio.Instance.tutorialSFX,null);
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
        TutorialManager.Index++;
        AudioPlayback.PlayOneShot(TutorialAudio.Instance.tutorialSFX, null);
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
        TutorialManager.Index++;
        AudioPlayback.PlayOneShot(TutorialAudio.Instance.tutorialSFX, null);
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
        TutorialManager.Index++;
        AudioPlayback.PlayOneShot(TutorialAudio.Instance.tutorialSFX, null);
    }
    public void confirmChangesInteraction()
    {

        TutorialManager.PipsUIText.SetActive(false);
        TutorialManager.PolicyUIText.SetActive(false);
        TutorialManager.AwarenessUIText.SetActive(false);
        TutorialManager.GraphUIText.SetActive(false);
        TutorialManager.TimeSliderUIText.SetActive(false);
        TutorialManager.YearSliderUIText.SetActive(false);
        TutorialManager.TimePipUIText.SetActive(false);

       // enable the UI elements
        TutorialManager.ConfirmChangeUIText.SetActive(true);
        TutorialManager.Index++;
        AudioPlayback.PlayOneShot(TutorialAudio.Instance.tutorialSFX, null);
    }

    public void NextButton()
    {
        TutorialManager.gameState = TutorialManager.GameState.PanningUPFromScreen;
        AudioPlayback.PlayOneShot(TutorialAudio.Instance.tutorialSFX, null);
    }

    public void TechTree()
    {
        TutorialManager.TechTreeUIText.SetActive(true);
        TutorialManager.TechTreeButton.SetActive(false);
        TutorialManager.PanDownFromUpTechTreeObject.SetActive(true);
        AudioPlayback.PlayOneShot(TutorialAudio.Instance.tutorialSFX, null);
    }

    public void BigRedButton()
    {
        // disable any existing UI elements that are currently active in the scene


        // enable the UI elements
        TutorialManager.RedButtonText.SetActive(true);
        TutorialManager.TypeButtonText.SetActive(false);
        TutorialManager.BreakDownText.SetActive(false);
        TutorialManager.KeyboardIndex++;
        AudioPlayback.PlayOneShot(TutorialAudio.Instance.tutorialSFX, null);

    }
    public void TypeButton()
    {
        // disable any existing UI elements that are currently active in the scene


        // enable the UI elements
        TutorialManager.RedButtonText.SetActive(false);
        TutorialManager.TypeButtonText.SetActive(true);
        TutorialManager.BreakDownText.SetActive(false);
        TutorialManager.KeyboardIndex++;
        AudioPlayback.PlayOneShot(TutorialAudio.Instance.tutorialSFX, null);

    }
    public void BreakDownButton()
    {
        // disable any existing UI elements that are currently active in the scene


        // enable the UI elements
        TutorialManager.RedButtonText.SetActive(false);
        TutorialManager.TypeButtonText.SetActive(false);
        TutorialManager.BreakDownText.SetActive(true);
        TutorialManager.KeyboardIndex++;
        AudioPlayback.PlayOneShot(TutorialAudio.Instance.tutorialSFX, null);

    }
    public void FinishTut(int Index)
    {
        SceneManager.LoadScene(Index);
        AudioPlayback.PlayOneShot(TutorialAudio.Instance.tutorialSFX, null);

    }
}
