using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{

    // This script consists of button press events for changing scenes, enabling disabling canvas objects and to trigger Audio events based on it

    // All button functionality to be added to this script and removed from the Inspector, once all UI elements are finalized.

    #region MAIN MENU BUTTONS AND AUDIO EVENT TRIGGERS
    public void OnSelectStartGame()
    {
        SceneManager.LoadScene(1);
        AudioPlayback.PlayOneShot(MenuAudioManager.Instance.mainMenuRefs.menuButtonSelectEvent, null);
        MenuAudioManager.Instance.ReleaseMenuMusic();
    }

    public void OnSelectLoadGame()
    {
        AudioPlayback.PlayOneShot(MenuAudioManager.Instance.mainMenuRefs.menuButtonSelectEvent, null);
    }

    public void OnSelectSettings()
    {
        AudioPlayback.PlayOneShot(MenuAudioManager.Instance.mainMenuRefs.menuButtonSelectEvent, null);
    }
    public void OnSelectQuitGame()
    {
        Application.Quit();
    }

    public void OnSelectBackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void OnSelectBack() 
    {
        AudioPlayback.PlayOneShot(MenuAudioManager.Instance.mainMenuRefs.menuButtonSelectEvent, null);
    }

    public void OnSelectAudioSetttings()
    {
        AudioPlayback.PlayOneShot(MenuAudioManager.Instance.mainMenuRefs.menuButtonSelectEvent, null);
    }

    public void OnSelectExitAudioSettings()
    {
        AudioPlayback.PlayOneShot(MenuAudioManager.Instance.mainMenuRefs.menuButtonSelectEvent, null);
    }
}

#endregion