using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EscapeMenu : MonoBehaviour
{
    private PlayerControls playerControls;
    private InputAction escMenu;

    

    [SerializeField] private GameObject escapeMenuUI;
    [SerializeField] private bool isVisible;
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
        //AUdio Pause calls need to be implemented here
        escapeMenuUI.SetActive(false);
        isVisible = false;
    }
}
