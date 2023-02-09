using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TechCameraController : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;
    public Transform background;
    public Transform lookAt;
    private float cameraSpeed = 25f;
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
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
        }

        if (ComputerController.Instance.onTech)
            zoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

        zoom = Mathf.Clamp(zoom, zoomMin, zoomMax);

        targetX = targetX + (mouseX * cameraSpeed);
        targetZ = targetZ + (mouseY * cameraSpeed);
        targetX = Mathf.Clamp(targetX, -cameraBoundsX, cameraBoundsX);
        targetZ = Mathf.Clamp(targetZ, -cameraBoundsY, cameraBoundsY);
        Vector3 target = new Vector3(targetX, 0, targetZ);

        lookAt.localPosition = target;
        vcam.m_Lens.OrthographicSize = Mathf.SmoothDamp(vcam.m_Lens.OrthographicSize, zoom, ref zoomVelocity, 0.3f);
    }
}
