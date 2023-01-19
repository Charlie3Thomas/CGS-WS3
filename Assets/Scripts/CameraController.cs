using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineFreeLook vcam;
    public Transform target;  //the target to rotate around
    public float zoomSpeed = 1f;  //the speed of zoom
    public float zoomMin = 20f;  //minimum zoom level
    public float zoomMax = 70f;  //maximum zoom level

    private float zoom;
    private float zoomVelocity;
    private float smoothDampSpeed = 0.5f;
    private Vector3 velocity;

    void Start()
    {
        zoom = vcam.m_Lens.FieldOfView;
    }

    void LateUpdate()
    {
        if (!target)
            return;

        if (ComputerController.Instance == null)
            return;

        if (ComputerController.Instance.touchingScreen)
        {
            vcam.m_XAxis.m_InputAxisName = "Mouse X";
            vcam.m_YAxis.m_InputAxisName = "Mouse Y";
        }
        else
        {
            vcam.m_XAxis.m_InputAxisValue = 0;
            vcam.m_YAxis.m_InputAxisValue = 0;
            vcam.m_XAxis.m_InputAxisName = "";
            vcam.m_YAxis.m_InputAxisName = "";
        }

        //zoom in and out
        if(ComputerController.Instance.onScreen)
            zoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

        zoom = Mathf.Clamp(zoom, zoomMin, zoomMax);

        target.position = Vector3.SmoothDamp(target.position, ComputerController.Instance.newPos, ref velocity, smoothDampSpeed);
        vcam.m_Lens.FieldOfView = Mathf.SmoothDamp(vcam.m_Lens.FieldOfView, zoom, ref zoomVelocity, 0.3f);
    }
}
