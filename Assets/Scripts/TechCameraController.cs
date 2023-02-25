using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class TechCameraController : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;
    public Transform background;
    public Transform lookAt;
    private float cameraSpeed = 5f;
    private float cameraBoundsX = 600f;
    private float cameraBoundsY = 150f;
    private float mouseX;
    private float mouseY;
    private float zoomSpeed = 100f;
    private float zoomMax = 100f;
    private float zoomMin = 50f;
    private float targetX = 0f;
    private float targetZ = 0f;

    private float zoom;
    private float zoomVelocity;
    private float smoothDampSpeed = 0.5f;
    private Vector3 velocity;

    // inputs
    private Vector2 mouseDelta;
    private Vector2 scroll;
    private bool isInteracting;
    private bool isSelecting;

    void Start()
    {
        zoom = vcam.m_Lens.OrthographicSize;
        targetX = background.localPosition.x;
        targetZ = background.localPosition.z;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (vcam == null)
            return;

        if (!lookAt)
            return;

        if (ComputerController.Instance == null)
            return;

        if (ComputerController.Instance.touchingTechScreen)
        {
            mouseX = mouseDelta.x;
            mouseY = mouseDelta.y;
        }
        else
        {
            mouseX = 0f;
            mouseY = 0f;
        }

        if (ComputerController.Instance.onTech)
            zoom -= scroll.y * zoomSpeed;

        zoom = Mathf.Clamp(zoom, zoomMin, zoomMax);

        targetX = targetX + (mouseX * cameraSpeed);
        targetZ = targetZ + (mouseY * cameraSpeed);
        targetX = Mathf.Clamp(targetX, -cameraBoundsX, cameraBoundsX);
        targetZ = Mathf.Clamp(targetZ, -cameraBoundsY, cameraBoundsY);
        Vector3 target = new Vector3(targetX, 0, targetZ);

        lookAt.localPosition = target;
        vcam.m_Lens.OrthographicSize = Mathf.SmoothDamp(vcam.m_Lens.OrthographicSize, zoom, ref zoomVelocity, 0.3f);
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
        InputManager.onAim -= AimInput;
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
