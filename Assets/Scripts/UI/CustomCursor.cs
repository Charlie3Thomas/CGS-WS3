using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomCursor : MonoBehaviour
{
    public static CustomCursor Instance;

    public Texture2D mainCursor;
    public Texture2D clickedCursor;
    public Texture2D settingsCusrsor;
    public Texture2D resourceCursor;
    public Texture2D knobCursor;
    public Texture2D horizontalSliderCursor;

    private PlayerControls controls;

    private void Start()
    {
        controls.Cursor.Interaction.started += _ => StartedClick();
        controls.Cursor.Interaction.performed += _ => EndedClick();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("CustomCursor.Awake: Cannot have multiple instances of CustomCursor!");
            Destroy(this);
        }
        
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

    public void OnHoverOverResourceSelector()
    {
        ChangeCursor(resourceCursor);
    }
    public void OnHoverOverKnobSelector()
    {
        ChangeCursor(knobCursor);
    }
    public void SetDefaultCursor()
    {
        ChangeCursor(mainCursor);
    }
    public void OnHoverOverHorizontalSlider()
    {
        ChangeCursor(horizontalSliderCursor);
    }
    private void ChangeCursor(Texture2D cursorType)
    {
        Cursor.SetCursor(cursorType, Vector2.zero, CursorMode.Auto);
    }
}
