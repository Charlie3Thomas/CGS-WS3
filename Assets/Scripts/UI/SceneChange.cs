using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneChange : MonoBehaviour
{
    public Animator transition;

    // This script consists of button press events for changing scenes, enabling disabling canvas objects and to trigger Audio events based on it

    // All button functionality to be added to this script and removed from the Inspector, once all UI elements are finalized.

    public RectTransform audioSettingsRect;
    public CanvasGroup audioSettingsCanvas;
    public float fadetime = 1f;

    #region MAIN MENU BUTTONS AND AUDIO EVENT TRIGGERS
    public void OnSelectStartGame()
    {
        LoadMainGame();  
    }



    private void LoadMainGame()
    {
        StartCoroutine(LoadGameTransition(1));
    }

    IEnumerator LoadGameTransition(int sceneindex)

    {
        transition.SetTrigger("Start");
        AudioPlayback.PlayOneShot(MenuAudioManager.Instance.mainMenuRefs.menuButtonSelectEvent, null);
        MenuAudioManager.Instance.ReleaseMenuMusic();
        yield return new WaitForSeconds(0);

        SceneManager.LoadScene(sceneindex);
    }

    // Load/Save game feature removed 

    //public void OnSelectLoadGame()
    //{
    //    AudioPlayback.PlayOneShot(MenuAudioManager.Instance.mainMenuRefs.menuButtonSelectEvent, null);
    //}

    //public void OnSelectSettings()
    //{
       
    //    AudioPlayback.PlayOneShot(MenuAudioManager.Instance.mainMenuRefs.menuButtonSelectEvent, null);
    //}
    public void OnSelectQuitGame()
    {
        Application.Quit();
    }

    public void OnSelectBackToMainMenu()
    {
        FmodRouting.StopMasterBus(); //Sam on destory for audio stoppping not calling all stops before menu loads, just focefully stop master bus with fade 
        SceneManager.LoadScene(0);
    }

    public void OnSelectBack() 
    {
        AudioPlayback.PlayOneShot(MenuAudioManager.Instance.mainMenuRefs.menuButtonSelectEvent, null);
    }

    public void OnSelectAudioSetttings()
    {
        FadeInAnimation();
        AudioPlayback.PlayOneShot(MenuAudioManager.Instance.mainMenuRefs.menuButtonSelectEvent, null);
    }

    public void OnSelectExitAudioSettings()
    {
        FadeOutAnimation();
        AudioPlayback.PlayOneShot(MenuAudioManager.Instance.mainMenuRefs.menuButtonSelectEvent, null);
    }


    private void FadeInAnimation()
    {

        audioSettingsCanvas.alpha = 0f;
        audioSettingsRect.transform.localPosition = new Vector3(-1000f, 0f, 0f);
        audioSettingsRect.DOAnchorPos(new Vector2(1500f, -600f), fadetime, false).SetEase(Ease.OutElastic);
        audioSettingsCanvas.DOFade(1, fadetime);

    }

    private void FadeOutAnimation()
    {
        audioSettingsCanvas.alpha = 1f;
        audioSettingsRect.transform.localPosition = new Vector3(500f, 0f, 0f);
        audioSettingsRect.DOAnchorPos(new Vector2(2500f  , -600f), 1, false).SetEase(Ease.InFlash);
        audioSettingsCanvas.DOFade(0, fadetime);
    }
}

#endregion