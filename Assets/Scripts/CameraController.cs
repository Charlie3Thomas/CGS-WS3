using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera cam;
    public Transform target;  //the target to rotate around
    public float rotationSpeed = 1f;  //the speed of rotation
    public float zoomSpeed = 1f;  //the speed of zoom
    public float zoomMin = 20f;  //minimum zoom level
    public float zoomMax = 70f;  //maximum zoom level
    public float distance = 40f;  //distance from target
    public float xSpeed = 250.0f;
    public float ySpeed = 120.0f;
    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;
    public float dragCoefficient = 0.1f; //determines how fast the camera slows down when the mouse is released

    private float x = 0.0f;
    private float y = 0.0f;
    private float zoom;
    private float zoomVelocity;
    private float xVelocity;
    private float yVelocity;
    private bool dragging;

    void Start()
    {
        cam = Camera.main;
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
        zoom = cam.fieldOfView;
    }

    void LateUpdate()
    {
        if (target)
        {
            if (UIController.Instance.onScreen)
            {
                dragging = true;
                x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
                y = ClampAngle(y, yMinLimit, yMaxLimit);
            }
            else
            {
                if (dragging)
                {
                    xVelocity = Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                    yVelocity = -Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
                }
                else
                {
                    xVelocity = Mathf.Lerp(xVelocity, 0, dragCoefficient);
                    yVelocity = Mathf.Lerp(yVelocity, 0, dragCoefficient);
                }
                x += xVelocity;
                y += yVelocity;
                y = ClampAngle(y, yMinLimit, yMaxLimit);
                dragging = false;
            }

            //zoom in and out
            zoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            zoom = Mathf.Clamp(zoom, zoomMin, zoomMax);

            Quaternion rotation = Quaternion.Euler(y, x, 0);
            Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;
            transform.rotation = rotation;
            transform.position = position;
            cam.fieldOfView = Mathf.SmoothDamp(cam.fieldOfView, zoom, ref zoomVelocity, 0.3f);
        }
    }

    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}
