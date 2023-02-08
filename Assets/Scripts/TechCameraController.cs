using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TechCameraController : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;
    public Transform background;
    public Transform lookAt;
    private float cameraSpeed = 100f;
    private float cameraBounds = 1000f;
    private float mouseX;
    private float mouseY;
    public float zoomSpeed = 1f;
    public float zoomMax = 50f;
    public float zoomMin = 100f;

    private float zoom;
    private float zoomVelocity;
    private float smoothDampSpeed = 0.5f;
    private Vector3 velocity;

    void Start()
    {
        zoom = vcam.m_Lens.OrthographicSize;
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

        float targetX = background.localPosition.x + (mouseX * cameraSpeed);
        float targetZ = background.localPosition.z + (mouseY * cameraSpeed);
        targetX = Mathf.Clamp(targetX, -cameraBounds, cameraBounds);
        targetZ = Mathf.Clamp(targetZ, -cameraBounds, cameraBounds);
        Vector3 target = new Vector3(targetX, 0, targetZ);

        lookAt.localPosition = Vector3.SmoothDamp(lookAt.localPosition, target, ref velocity, smoothDampSpeed);
        vcam.m_Lens.OrthographicSize = Mathf.SmoothDamp(vcam.m_Lens.OrthographicSize, zoom, ref zoomVelocity, 0.3f);
    }
}
