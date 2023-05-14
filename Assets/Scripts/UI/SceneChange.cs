using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.InputSystem;

public class SceneChange : MonoBehaviour
{
    public Animator transition;

    // This script consists of button press events for changing scenes, enabling disabling canvas objects and to trigger Audio events based on it

    // All button functionality to be added to this script and removed from the Inspector, once all UI elements are finalized.

    public RectTransform audioSettingsRect;
    public CanvasGroup audioSettingsCanvas;

    public float fadetime = 1f;

    private PlayerControls playerControls;
    private InputAction escMenu;


    [SerializeField] private GameObject escapeMenuUI;
    [SerializeField] private GameObject AudioSettingsUI;
    [SerializeField] private bool isVisible;
    [SerializeField] private bool isAudioVisible =false;
    [SerializeField] private GameObject backgroundBlocker;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }
    #region ENABLE, DISABLE AND VISIBILITY CHECK
    private void OnEnable()
    {
        escMenu = playerControls.Game.Menu;
        escMenu.Enable();
        escMenu.performed += VisibilityCheck;
        DataSaveLoadManager.OnSaveSuccess += DeactivateMenu;
    }

    private void OnDisable()
    {
        escMenu.Disable();

    }

    void VisibilityCheck(InputAction.CallbackContext context)
    {
        isVisible = !isVisible;

        if (isVisible)
        {
            ActivateMenu();
        }
        else
        {
            DeactivateMenu();
        }
    }
    #endregion

    #region TAB MENU 
    public void ActivateMenu()
    {
        isVisible = true;
        //Time.timeScale = 0;
        //AUdio Pause calls need to be implemented here
        escapeMenuUI.SetActive(true);
        if (isAudioVisible == false)
        {
            backgroundBlocker.SetActive(true);
        }
        if (isAudioVisible)
        {
            OnSelectExitAudioSettings();
        }
        //Sam: quick testing fix at end... warning text would be over screen if opening menu, so j check the sigleton is not null then set the text to false. Quick fix
        if (DisasterSeqenceManager.Instance != null)
        {
            DisasterSeqenceManager.Instance.warningObject.SetActive(false);
            DisasterSeqenceManager.Instance.resolvedObject.SetActive(false);
        }


    }

    public void DeactivateMenu()
    {
        
        //Time.timeScale = 1;
        if (isAudioVisible == false)
        {
            backgroundBlocker.SetActive(false);
        }
        
        if (isAudioVisible)
        {
            OnSelectExitAudioSettings();
        }

        //AUdio Pause calls need to be implemented here
        escapeMenuUI.SetActive(false);
        isVisible = false;
    }

    #endregion

    //public void ActivateAudioSettings()
    //{
    //    escapeMenuUI.SetActive(false);
    //    isVisible = true;
    //    backgroundBlocker.SetActive(true);
    //    //Time.timeScale = 0;

    //    AudioSettingsUI.gameObject.SetActive(true);
    //    isAudioVisible = true;

    //}

    //public void DeactivateAudioSettings()
    //{
    //    AudioSettingsUI.gameObject.SetActive(false);
    //    backgroundBlocker.SetActive(true);
    //    //Time.timeScale = 1;
    //    isAudioVisible = false;

    //    escapeMenuUI.SetActive(true);
    //    isVisible = true;

    //}



    //public void OnSelectBackToMainMenu()
    //{
    //    FmodRouting.StopMasterBus(); //Sam on destory for audio stoppping not calling all stops before menu loads, just focefully stop master bus with fade 
    //    //StartCoroutine(LoadGameTransition(0));
    //    DOTween.Clear(true);
    //    SceneManager.LoadScene(0);

    //}

    public void OnSelectShowScoreCard()
    {

        FmodRouting.StopMasterBus();
        //Time.timeScale = 1;
        DOTween.Clear(true);

        SceneManager.LoadScene(3);
    }

    #region MAIN MENU BUTTONS AND AUDIO EVENT TRIGGERS
    public void BootComputer()
    {
        AudioPlayback.PlayOneShot(MenuAudioManager.Instance.mainMenuRefs.menuButtonSelectEvent, null);
        LoadMainGame();
        DOTween.Clear();
    }

    private void LoadMainGame()
    {
        Debug.Log("game seed :" + CTSeed.gameSeed);
        //StartCoroutine(LoadGameTransition(1));
        CTSeed.ChangeSeed();

        Debug.Log("game seed after change :" + CTSeed.gameSeed);
        DOTween.Clear(true);
        SceneManager.LoadScene(1);
    }


    public void AdjustSpeakers()
    {
        FadeInAnimation();

        if(MenuAudioManager.Instance != false)
        {
           AudioPlayback.PlayOneShot(MenuAudioManager.Instance.mainMenuRefs.menuButtonSelectEvent, null);
        }
        if(isVisible)
        {
            DeactivateMenu();
        }
        backgroundBlocker.SetActive(true);
        isAudioVisible = true;
    }
    public void RestartComputer()
    {
        FmodRouting.StopMasterBus(); //Sam on destory for audio stoppping not calling all stops before menu loads, just focefully stop master bus with fade 
        //StartCoroutine(LoadGameTransition(0));
        DOTween.Clear(true);
        SceneManager.LoadScene(0);
    }

    public void UnplugCable()
    {
        Application.Quit();
    }

    //public void OnSelectAudioSettingsInGame()
    //{
    //    FadeInAnimationInGame();
    //   // AudioPlayback.PlayOneShot(MenuAudioManager.Instance.mainMenuRefs.menuButtonSelectEvent, null);
    //}


    //IEnumerator LoadGameTransition(int sceneindex)

    //{
    //    transition.SetTrigger("Start");
    //    AudioPlayback.PlayOneShot(MenuAudioManager.Instance.mainMenuRefs.menuButtonSelectEvent, null);
    //    MenuAudioManager.Instance.ReleaseMenuMusic();
    //    yield return new WaitForSeconds(0);

    //    SceneManager.LoadScene(sceneindex);
    //}

    // Load/Save game feature removed 

    //public void OnSelectLoadGame()
    //{
    //    AudioPlayback.PlayOneShot(MenuAudioManager.Instance.mainMenuRefs.menuButtonSelectEvent, null);
    //}

    //public void OnSelectSettings()
    //{

    //    AudioPlayback.PlayOneShot(MenuAudioManager.Instance.mainMenuRefs.menuButtonSelectEvent, null);
    //}

    public void OnSelectExitAudioSettings()
    {
        isAudioVisible = false;
        if (isVisible ==false)
        {
            backgroundBlocker.SetActive(false);
        }
        FadeOutAnimation();
        if(isVisible==false)
        {
            ActivateMenu();
        }
        
        AudioPlayback.PlayOneShot(MenuAudioManager.Instance.mainMenuRefs.menuButtonSelectEvent, null);
    }

    //public void OnSelectExitAudioSettingsInGame()
    //{
    //    FadeOutAnimationInGame();
    //    //AudioPlayback.PlayOneShot(MenuAudioManager.Instance.mainMenuRefs.menuButtonSelectEvent, null);
    //}

    private void FadeInAnimation()
    {

        audioSettingsCanvas.alpha = 0f;
        audioSettingsRect.transform.localPosition = new Vector3(-1000f, 0f, 0f);
        audioSettingsRect.DOAnchorPos(new Vector2(1000f, -500f), fadetime, false).SetEase(Ease.OutElastic);
        audioSettingsCanvas.DOFade(1, fadetime);

    }
    //private void FadeInAnimationInGame()
    //{

    //    audioSettingsCanvas.alpha = 0f;
    //    audioSettingsRect.transform.localPosition = new Vector3(-1000f, 0f, 0f);
    //    audioSettingsRect.DOAnchorPos(new Vector2(1000f, -500f), fadetime, false).SetEase(Ease.OutElastic);
    //    audioSettingsCanvas.DOFade(1, fadetime);

    //}

    private void FadeOutAnimation()
    {
        audioSettingsCanvas.alpha = 1f;
        audioSettingsRect.transform.localPosition = new Vector3(0f, -0f, 0f);
        audioSettingsRect.DOAnchorPos(new Vector2(2200f, -500f), fadetime, false).SetEase(Ease.InFlash);
        audioSettingsCanvas.DOFade(0, fadetime);
    }
    #endregion

    #region 



    #endregion

}


