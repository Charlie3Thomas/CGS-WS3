using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomCursor : MonoBehaviour
{
    public Texture2D mainCursor;
    public Texture2D clickedCursor;
    public Texture2D settingsCusrsor;

    private PlayerControls controls;

    private void Start()
    {
        controls.Game.Interact.started += _ => StartedClick();
        controls.Game.Interact.performed += _ => EndedClick();
    }

    private void Awake()
    {
        controls = new PlayerControls();

        ChangeCursor(mainCursor); 

        // Limit the cursor to game screen
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void StartedClick()
    {
        ChangeCursor(clickedCursor);
    }

    private void EndedClick()
    {
        ChangeCursor(mainCursor);
    }


    private void ChangeCursor(Texture2D cursorType)
    {

        Cursor.SetCursor(cursorType, Vector2.zero, CursorMode.Auto);

    }
}
