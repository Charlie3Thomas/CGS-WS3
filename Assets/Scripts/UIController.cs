using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    [SerializeField]
    private Animator notepadAnim;
    [SerializeField]
    private Animator pCard1Anim;
    [SerializeField]
    private Animator pCard2Anim;
    [SerializeField]
    private Animator pCard3Anim;
    [SerializeField]
    private Animator pCard4Anim;
    [SerializeField]
    private Animator pCard5Anim;
    [SerializeField]
    private Animator pCard6Anim;
    [SerializeField]
    private Animator pCard7Anim;
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
        if (!cam)
            return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if(Input.GetMouseButtonDown(1) && hit.transform.name == "Screen")
                onScreen = true;

            // Points to objects on the render texture screen
            if (Input.GetMouseButtonDown(0) && hit.transform.name == "Screen")
            {
                var localPoint = hit.textureCoord;
                Ray camRay = Camera.main.ScreenPointToRay(new Vector2(localPoint.x * Camera.main.pixelWidth, localPoint.y * Camera.main.pixelHeight));
                RaycastHit camHit;
                if (Physics.Raycast(camRay, out camHit))
                {
                    Debug.Log(camHit.collider.gameObject.name);
                }
            }

            // Notepad hover
            if(notepadAnim != null)
                notepadAnim.SetBool("IsOver", hit.transform.name == "Notepad");

            // Policy Card 2 hover
            if (pCard1Anim != null)
                pCard1Anim.SetBool("IsOver", hit.transform.name == "Policy_Card_1");

            // Policy Card 2 hover
            if (pCard2Anim != null)
                pCard2Anim.SetBool("IsOver", hit.transform.name == "Policy_Card_2");

            // Policy Card 3 hover
            if (pCard3Anim != null)
                pCard3Anim.SetBool("IsOver", hit.transform.name == "Policy_Card_3");

            // Policy Card 4 hover
            if (pCard4Anim != null)
                pCard4Anim.SetBool("IsOver", hit.transform.name == "Policy_Card_4");

            // Policy Card 5 hover
            if (pCard5Anim != null)
                pCard5Anim.SetBool("IsOver", hit.transform.name == "Policy_Card_5");

            // Policy Card 6 hover
            if (pCard6Anim != null)
                pCard6Anim.SetBool("IsOver", hit.transform.name == "Policy_Card_6");

            // Policy Card 7 hover
            if (pCard7Anim != null)
                pCard7Anim.SetBool("IsOver", hit.transform.name == "Policy_Card_7");
        }

        if (Input.GetMouseButtonUp(1))
        {
            onScreen = false;
        }
    }
}
