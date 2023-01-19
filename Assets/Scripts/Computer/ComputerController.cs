using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComputerController : MonoBehaviour
{
    public static ComputerController Instance;

    public Camera screenCam;

    [Header("Anims")]
    [SerializeField]
    private Animator notepadAnim;
    [SerializeField]
    private Animator buttonAnim;
    [SerializeField]
    private Animator techButtonAnim;
    [SerializeField]
    private Animator[] pCardAnim = new Animator[7];
    [SerializeField]
    private Animator yearKnobBAnim;
    [SerializeField]
    private Animator yearKnobFAnim;
    [SerializeField]
    private Animator pointsSelectorAnim;





    [Header("Texts")]
    public TMP_Text notepadText;
    public TMP_Text yearText;
    public TMP_Text[] pCardText = new TMP_Text[7];

    private Camera cam;
    [HideInInspector]
    public Vector3 newPos;

    [HideInInspector]
    public bool touchingScreen = false;

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
    }

    void Start()
    {
        screenCam = GameObject.FindGameObjectWithTag("ScreenCamera").GetComponent<Camera>();
        cam = GetComponent<Camera>();
        newPos = Vector3.zero;
    }

    void Update()
    {
        if (!cam)
            return;

        yearText.text = YearData._INSTANCE.current_year.ToString();

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            onScreen = hit.transform.name == "Screen";

            InteractWithScreen(hit);

            if (Input.GetMouseButtonDown(1) && onScreen)
            {
                touchingScreen = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            if (Input.GetMouseButtonUp(1))
            {
                touchingScreen = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            HandleAnims(hit);

        }
        else
        {
            onScreen = false;
            touchingScreen = false;
        }
    }

    private void InteractWithScreen(RaycastHit hit)
    {
        if (Input.GetMouseButtonDown(0) && onScreen)
        {
            var localPoint = hit.textureCoord;
            Ray camRay = screenCam.ScreenPointToRay(new Vector2(localPoint.x * screenCam.pixelWidth, localPoint.y * screenCam.pixelHeight));
            RaycastHit camHit;
            if (Physics.Raycast(camRay, out camHit))
            {
                newPos = camHit.transform.position;
            }
        }
    }

    private void HandleAnims(RaycastHit hit)
    {
        // TODO: Add tags instead of searching through names
        if (Input.GetMouseButtonDown(0) && hit.transform.name == "Year_selection_backwards")
        {
            if (yearKnobBAnim != null)
                yearKnobBAnim.SetBool("YearDownHold", true);
        }

        if (Input.GetMouseButtonDown(0) && hit.transform.name == "Year_selection_forwards")
        {
            if (yearKnobFAnim != null)
                yearKnobFAnim.SetBool("YearUpHold", true);
        }

        if (Input.GetMouseButtonDown(0) && hit.transform.CompareTag("Button"))
        {
            if (techButtonAnim != null)
                techButtonAnim.SetTrigger("Press");
                AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.buttonPressEvent, null);
        }

        if (Input.GetMouseButtonDown(0) && hit.transform.CompareTag("Button"))
        {
            if (buttonAnim != null)
                buttonAnim.SetTrigger("Press");
                AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.buttonPressEvent, null);
        }

        if(Input.GetMouseButtonDown(0) && (hit.transform.name == "Point_selector_planning" || hit.transform.name == "Point_selector_Science" || hit.transform.name == "Point_selector_Food" ||
            hit.transform.name == "Point_selector_workers"))
        {
            pointsSelectorAnim = hit.transform.GetComponent<Animator>();

            if (pointsSelectorAnim != null)
                pointsSelectorAnim.SetTrigger("PointsUp");
        }

        if (Input.GetMouseButtonDown(1) && (hit.transform.name == "Point_selector_planning" || hit.transform.name == "Point_selector_Science" || hit.transform.name == "Point_selector_Food" ||
            hit.transform.name == "Point_selector_workers"))
        {
            pointsSelectorAnim = hit.transform.GetComponent<Animator>();

            if (pointsSelectorAnim != null)
                pointsSelectorAnim.SetTrigger("PointsDown");
        }

        if (Input.GetMouseButtonUp(0))
        {
            //Sam edit: added braces to if statements as it was causing dial event to trigger on button press when statements had no braces
            if (yearKnobBAnim != null)
            {
                yearKnobBAnim.SetBool("YearDownHold", false);
            }

            if (yearKnobFAnim != null)
            {
                yearKnobFAnim.SetBool("YearUpHold", false);
            }
        }

        // Notepad hover
        if (notepadAnim != null)
        {
            notepadAnim.SetBool("IsOver", hit.transform.name == "Notepad");
        } 
        // Policy cards hover
        for (int i = 1; i <= 7; i++)
        {
            if (pCardAnim[i - 1] != null)
                pCardAnim[i - 1].SetBool("IsOver", hit.transform.name == "Policy_Card_" + i);
        }
    }
}