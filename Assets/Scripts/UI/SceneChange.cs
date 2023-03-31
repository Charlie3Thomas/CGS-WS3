using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
  

    public void OnSelectStartGame()
    {
        SceneManager.LoadScene(1);
        AudioPlayback.PlayOneShot(MenuAudioManager.instance.mainMenuRefs.menuButtonSelectEvent, null);
        MenuAudioManager.instance.ReleaseMenuMusic();
    }

    public void OnSelectLoadGame()
    {
        AudioPlayback.PlayOneShot(MenuAudioManager.instance.mainMenuRefs.menuButtonSelectEvent, null);
    }

    public void OnSelectSettings()
    {
        AudioPlayback.PlayOneShot(MenuAudioManager.instance.mainMenuRefs.menuButtonSelectEvent, null);
    }
    public void OnSelectQuitGame()
    {
        Application.Quit();
    }


}
