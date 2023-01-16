using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

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
            Ray camRay = Camera.main.ScreenPointToRay(new Vector2(localPoint.x * Camera.main.pixelWidth, localPoint.y * Camera.main.pixelHeight));
            RaycastHit camHit;
            if (Physics.Raycast(camRay, out camHit))
            {
                newPos = camHit.transform.position;
            }
        }
    }

    private void HandleAnims(RaycastHit hit)
    {
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

        if (Input.GetMouseButtonDown(0) && hit.transform.name == "Tech_tree_button")
        {
            if (techButtonAnim != null)
                techButtonAnim.SetTrigger("Press");
        }

        if (Input.GetMouseButtonDown(0) && hit.transform.name == "Button")
        {
            if (buttonAnim != null)
                buttonAnim.SetTrigger("Press");
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (yearKnobBAnim != null)
                yearKnobBAnim.SetBool("YearDownHold", false);

            if (yearKnobFAnim != null)
                yearKnobFAnim.SetBool("YearUpHold", false);
        }

        // Notepad hover
        if (notepadAnim != null)
            notepadAnim.SetBool("IsOver", hit.transform.name == "Notepad");

        // Policy cards hover
        for (int i = 1; i <= 7; i++)
        {
            if (pCardAnim[i - 1] != null)
                pCardAnim[i - 1].SetBool("IsOver", hit.transform.name == "Policy_Card_" + i);
        }
    }
}