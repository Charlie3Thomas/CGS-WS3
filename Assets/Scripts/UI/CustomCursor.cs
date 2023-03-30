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


    private void Awake()
    {
        ChangeCursor(mainCursor);

        // Limit the cursor to game screen
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void ChangeCursor(Texture2D cursorType)
    {

        Cursor.SetCursor(cursorType, Vector2.zero, CursorMode.Auto);

    }
}
