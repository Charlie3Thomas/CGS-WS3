using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    private Camera cam;

    public bool onScreen = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        //DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if(Input.GetMouseButtonDown(1) && hit.transform.name == "Screen")
            {
                onScreen = true;
            }

            if(Input.GetMouseButtonDown(0) && hit.transform.name == "Screen")
            {
                var localPoint = hit.textureCoord;
                Ray camRay = Camera.main.ScreenPointToRay(new Vector2(localPoint.x * Camera.main.pixelWidth, localPoint.y * Camera.main.pixelHeight));
                RaycastHit camHit;
                if (Physics.Raycast(camRay, out camHit))
                {
                    Debug.Log(camHit.collider.gameObject.name);
                }
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            onScreen = false;
        }
    }
}
