using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public static event Action<InputAction.CallbackContext> onInteract;
    public static event Action<InputAction.CallbackContext> onShift;
    public static event Action<InputAction.CallbackContext> onScroll;
    public static event Action<InputAction.CallbackContext> onSelect;
    public static event Action<InputAction.CallbackContext> onCursorPos;
    public static event Action<InputAction.CallbackContext> onAim;
    public static event Action<InputAction.CallbackContext> onPause;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        onInteract?.Invoke(context);
    }

    public void OnShift(InputAction.CallbackContext context)
    {
        onShift?.Invoke(context);
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
        onSelect?.Invoke(context);
    }

    public void OnCursorPosition(InputAction.CallbackContext context)
    {
        onCursorPos?.Invoke(context);
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        onAim?.Invoke(context);
    }

    public void OnScroll(InputAction.CallbackContext context)
    {
        onScroll?.Invoke(context);
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        onPause?.Invoke(context);
    }
}

