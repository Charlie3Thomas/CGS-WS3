using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;
    public Transform target;  //the target to rotate around
    public float zoomSpeed = 1f;  //the speed of zoom
    public float zoomMin = 20f;  //minimum zoom level
    public float zoomMax = 70f;  //maximum zoom level

    private float zoom;
    private float zoomVelocity;
    private float smoothDampSpeed = 0.5f;
    private Vector3 velocity;
    private float smoothDampRot = 0.5f;
    private Vector3 rotVel;

    // inputs
    private Vector2 mouseDelta;
    private Vector2 scroll;
    private bool isInteracting;
    private bool isSelecting;

    void Start()
    {
        zoom = vcam.m_Lens.FieldOfView;
    }

    void LateUpdate()
    {
        if (vcam == null)
            return;

        if (!target)
            return;

        if (ComputerController.Instance == null)
            return;

        if (!ComputerController.Instance.touchingScreen)
        {
            mouseDelta.x = 0;
            mouseDelta.y = 0;
        }

        var angles = new Vector3(target.transform.localEulerAngles.x + (mouseDelta.y * 7f), target.transform.localEulerAngles.y + (mouseDelta.x * 3f), 0);
        angles.x = Mathf.Clamp(angles.x, 10, 80);
        target.transform.localEulerAngles = Vector3.SmoothDamp(target.transform.localEulerAngles, angles, ref rotVel, smoothDampRot);

        //zoom in and out
        if (ComputerController.Instance.onScreen)
            zoom -= scroll.y * zoomSpeed;

        zoom = Mathf.Clamp(zoom, zoomMin, zoomMax);

        target.position = Vector3.SmoothDamp(target.position, ComputerController.Instance.newPos, ref velocity, smoothDampSpeed);
        vcam.m_Lens.FieldOfView = Mathf.SmoothDamp(vcam.m_Lens.FieldOfView, zoom, ref zoomVelocity, 0.3f);
    }

    private void AimInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    private void ScrollInput(InputAction.CallbackContext context)
    {
        scroll = context.ReadValue<Vector2>();
    }

    private void InteractInput(InputAction.CallbackContext context)
    {
        if (context.performed)
            isInteracting = true;
        else if (context.canceled)
            isInteracting = false;
    }

    private void SelectInput(InputAction.CallbackContext context)
    {
        if (context.performed)
            isSelecting = true;
        else if (context.canceled)
            isSelecting = false;
    }

    private void SubscribeInputs()
    {
        InputManager.onAim += AimInput;
        InputManager.onScroll += ScrollInput;
        InputManager.onInteract += InteractInput;
        InputManager.onSelect += SelectInput;
    }

    private void UnsubscribeInputs()
    {
                InputManager.onAim += AimInput;
        InputManager.onScroll -= ScrollInput;
        InputManager.onInteract -= InteractInput;
        InputManager.onSelect -= SelectInput;
    }

    private void OnEnable()
    {
        SubscribeInputs();
    }

    private void OnDisable()
    {
        UnsubscribeInputs();
    }

    private void OnDestroy()
    {
        UnsubscribeInputs();
    }
}
