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
    public bool fadeIn = false;
    public bool fadeOut = false;

    private PlayerControls playerControls;
    private InputAction escMenu;

    // INSPECTED LINKED OBJECTS

    [SerializeField] private GameObject escapeMenuUI;
    [SerializeField] private GameObject AudioSettingsUI;
    [SerializeField] private bool isVisible;
    [SerializeField] private bool isAudioVisible = false;
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
        escapeMenuUI.SetActive(true);
        BackgrounToggle();

        if (isAudioVisible)
        {
            FadeOutAnimation();
            isAudioVisible = false;
            BackgrounToggle();
        }

        //Sam: quick testing fix at end... warning text would be over screen if opening menu, so j check the sigleton is not null then set the text to false. Quick fix
        if (DisasterSeqenceManager.Instance != null)
        {
            DisasterSeqenceManager.Instance.warningObject.SetActive(false);
            DisasterSeqenceManager.Instance.resolvedObject.SetActive(false);
        }
        AudioPlayback.PlayOneShot(MenuAudioManager.Instance.mainMenuRefs.menuButtonSelectEvent, null);

        isVisible = true;

    }
    private void BackgrounToggle()
    {
        if(isVisible || isAudioVisible)
        {
            backgroundBlocker.SetActive(true);
        }
        else
        {
            backgroundBlocker.SetActive(false);
        }
    }

    public void DeactivateMenu()
    {

        //Time.timeScale = 1;
        BackgrounToggle();
        escapeMenuUI.SetActive(false);

        if (isAudioVisible &&isVisible)
        {
            OnSelectExitAudioSettings();
        }

        //AUdio Pause calls need to be implemented here
        
        isVisible = false;

        AudioPlayback.PlayOneShot(MenuAudioManager.Instance.mainMenuRefs.menuButtonSelectEvent, null);
    }

    #endregion


    #region MAIN MENU BUTTONS AND AUDIO EVENT TRIGGERS
    public void BootComputer()
    {
       
        LoadMainGame();
       
    }

    private void LoadMainGame()
    {
        Debug.Log("game seed :" + CTSeed.gameSeed);
        //StartCoroutine(LoadGameTransition(1));
        CTSeed.ChangeSeed();

        Debug.Log("game seed after change :" + CTSeed.gameSeed);
        DOTween.Clear(true);
        SceneManager.LoadScene(1);

        AudioPlayback.PlayOneShot(MenuAudioManager.Instance.mainMenuRefs.menuButtonSelectEvent, null);
    }


    public void AdjustSpeakers()
    {
        isAudioVisible = true;
        FadeInAnimation();
        BackgrounToggle();
        if(isVisible)
        {
            escapeMenuUI.SetActive(false);
            isVisible = !isVisible;
        }
        AudioPlayback.PlayOneShot(MenuAudioManager.Instance.mainMenuRefs.menuButtonSelectEvent, null);

        
     
        

    }
    public void RestartComputer()
    {
        FmodRouting.StopMasterBus(); //Sam on destory for audio stoppping not calling all stops before menu loads, just focefully stop master bus with fade 
        //StartCoroutine(LoadGameTransition(0));
        DOTween.Clear(true);
        SceneManager.LoadScene(0);

        AudioPlayback.PlayOneShot(MenuAudioManager.Instance.mainMenuRefs.menuButtonSelectEvent, null);
    }

    public void UnplugCable()
    {
        Application.Quit();
    }

    //NESTED MENU FUNCTIONS

    public void OnSelectExitAudioSettings()
    {
        

        FadeOutAnimation();
        if (SceneManager.GetActiveScene().buildIndex != 0 && !isVisible)
            {
                ActivateMenu();
            }

      
        isAudioVisible = false;
        BackgrounToggle();
        AudioPlayback.PlayOneShot(MenuAudioManager.Instance.mainMenuRefs.menuButtonSelectEvent, null);
    }


    public void OnSelectShowScoreCard()
    {

        FmodRouting.StopMasterBus();
        //Time.timeScale = 1;
        DOTween.Clear(true);

        SceneManager.LoadScene(3);

        AudioPlayback.PlayOneShot(MenuAudioManager.Instance.mainMenuRefs.menuButtonSelectEvent, null);
    }

    #region UI ANIMATION 
    private void FadeInAnimation()
    {

        audioSettingsCanvas.alpha = 0f;
        audioSettingsRect.transform.localPosition = new Vector3(-1000f, 0f, 0f);
        audioSettingsRect.DOAnchorPos(new Vector2(1000f, -500f), fadetime, false).SetEase(Ease.OutElastic);
        audioSettingsCanvas.DOFade(1, fadetime);

    }


    private void FadeOutAnimation()
    {
        audioSettingsCanvas.alpha = 1f;
        audioSettingsRect.transform.localPosition = new Vector3(0f, -0f, 0f);
        audioSettingsRect.DOAnchorPos(new Vector2(2200f, -500f), fadetime, false).SetEase(Ease.InFlash);
        audioSettingsCanvas.DOFade(0, fadetime);
    }
    #endregion

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
}

#endregion
