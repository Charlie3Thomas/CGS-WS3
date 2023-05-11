using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class EscapeMenu : MonoBehaviour
{
    private PlayerControls playerControls;
    private InputAction escMenu;

    
    

    public float fadetime = 1f;

    [SerializeField] private GameObject escapeMenuUI;
    [SerializeField] private GameObject AudioSettingsUI;
    [SerializeField] private bool isVisible;
    [SerializeField] private bool isAudioVisible=false;
    [SerializeField] private GameObject backgroundBlocker;

    private void Awake()
    {
        playerControls = new PlayerControls();

    }

    private void OnEnable()
    {
        escMenu = playerControls.Game.Menu;
        escMenu.Enable();

        escMenu.performed += VisibilityCheck;
    }

    private void OnDisable()
    {
        escMenu.Disable();
        
    }

    void VisibilityCheck(InputAction.CallbackContext context)
    {
        isVisible = !isVisible;

        if(isVisible)
        {
            ActivateMenu();
        }
        else
        {
            DeactivateMenu();
        }
    }

    public void ActivateMenu()
    {
        Time.timeScale = 0;
        //AUdio Pause calls need to be implemented here
        escapeMenuUI.SetActive(true);

        backgroundBlocker.SetActive(true);

        
    }

    public void DeactivateMenu()
    {
        Time.timeScale = 1;
        backgroundBlocker.SetActive(false);

        if(isAudioVisible)
        {
            AudioSettingsUI.gameObject.SetActive(false);
        }

        //AUdio Pause calls need to be implemented here
        escapeMenuUI.SetActive(false);
        isVisible = false;
    }

    public void ActivateAudioSettings()
    {
        escapeMenuUI.SetActive(false);
        isVisible = true;
        backgroundBlocker.SetActive(true);
        Time.timeScale = 0;

        AudioSettingsUI.gameObject.SetActive(true);
        isAudioVisible = true;

    }

    public void DeactivateAudioSettings()
    {
        AudioSettingsUI.gameObject.SetActive(false);
        backgroundBlocker.SetActive(true);
        Time.timeScale = 1;
        isAudioVisible = false;

        escapeMenuUI.SetActive(true);
        isVisible = true;

    }

    public void OnSelectQuitGame()
    {
        Application.Quit();
    }

    public void OnSelectBackToMainMenu()
    {
        FmodRouting.StopMasterBus(); //Sam on destory for audio stoppping not calling all stops before menu loads, just focefully stop master bus with fade 
        //StartCoroutine(LoadGameTransition(0));
        SceneManager.LoadScene(0);

    }

}
