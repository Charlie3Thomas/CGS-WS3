using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// NOTE: ALL AUDIO WAS MOVED TO THEIR RESPECTIVE ANIM EVENT SCRIPTS
public class ComputerController : MonoBehaviour
{
    public static ComputerController Instance;

    private Camera screenCam;

    [HideInInspector]
    public GameObject[] policyCards = new GameObject[7];
    private List<PointSelector> pointSelectors;

    // Anims
    private Animator[] pCardAnims = new Animator[7];
    private Animator notepadAnim;
    private Animator yearKnobAnim;
    private Animator buttonAnim;
    private Animator pointsSelectorAnim;

    // Texts
    [HideInInspector]
    public TMP_Text notepadText;
    [HideInInspector]
    public TMP_Text yearText;
    [HideInInspector]
    public TMP_Text[] pCardTexts = new TMP_Text[7];

    private Camera cam;

    [HideInInspector]
    public Vector3 newPos;

    [HideInInspector]
    public bool touchingScreen = false;
    [HideInInspector]
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
        Setup();
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

    void Setup()
    {
        screenCam = GameObject.FindGameObjectWithTag("ScreenCamera").GetComponent<Camera>();
        cam = GetComponent<Camera>();
        newPos = Vector3.zero;
        notepadAnim = GameObject.FindGameObjectWithTag("Notepad").GetComponent<Animator>();
        yearKnobAnim = GameObject.FindGameObjectWithTag("YearKnob").GetComponent<Animator>();
        yearText = GameObject.FindGameObjectWithTag("YearCounter").GetComponent<TMP_Text>();
        notepadText = GameObject.FindGameObjectWithTag("Notepad").transform.GetChild(0).GetComponent<TMP_Text>();
        pointSelectors = new List<PointSelector>(FindObjectsOfType<PointSelector>());

        policyCards = GameObject.FindGameObjectsWithTag("PolicyCard");
        for (int i = 0; i < policyCards.Length; i++)
        {
            pCardTexts[i] = policyCards[i].transform.GetChild(0).GetComponent<TMP_Text>();
            pCardAnims[i] = policyCards[i].GetComponent<Animator>();
        }
    }

    private void HandleAnims(RaycastHit hit)
    {
        if (Input.GetMouseButtonDown(0) && hit.transform.CompareTag("Button"))
        {
            buttonAnim = hit.transform.GetComponent<Animator>();

            if (buttonAnim != null)
                buttonAnim.SetTrigger("Press");
        }

        if(Input.GetMouseButtonDown(0) && hit.transform.CompareTag("PointsSelector"))
        {
            pointsSelectorAnim = hit.transform.GetComponent<Animator>();

            if (pointsSelectorAnim != null)
                pointsSelectorAnim.SetTrigger("PointsUp");
        }

        if (Input.GetMouseButtonDown(1) && hit.transform.CompareTag("PointsSelector"))
        {
            pointsSelectorAnim = hit.transform.GetComponent<Animator>();

            if (pointsSelectorAnim != null)
                pointsSelectorAnim.SetTrigger("PointsDown");
        }

        // Year knob up/down
        if (yearKnobAnim != null)
        {
            yearKnobAnim.SetBool("YearDownHold", Input.GetMouseButton(1) && hit.transform.CompareTag("YearKnob"));
            yearKnobAnim.SetBool("YearUpHold", Input.GetMouseButton(0) && hit.transform.CompareTag("YearKnob"));
        }

        // Notepad hover
        if (notepadAnim != null)
            notepadAnim.SetBool("IsOver", hit.transform.CompareTag("Notepad"));

        // Policy cards hover
        for (int i = 0; i < 7; i++)
        {
            if (pCardAnims[i] != null)
                pCardAnims[i].SetBool("IsOver", hit.transform.name == policyCards[i].name);
        }
    }

    public void CheckPoints(PointSelector excluded)
    {
        float totalPoints = 0;
        foreach (var pointSelector in pointSelectors)
        {
            totalPoints += pointSelector.pointValue;
        }
        if (totalPoints > 5)
        {
            PointSelector highestPointSelector = FindHighestPointSelector(excluded);
            highestPointSelector.RemovePoints(1);
        }
    }

    private PointSelector FindHighestPointSelector(PointSelector excluded)
    {
        PointSelector highestPointSelector = pointSelectors[0];
        for (int i = 0; i < pointSelectors.Count; i++)
        {
            if ((pointSelectors[i].pointValue > highestPointSelector.pointValue) && pointSelectors[i] != excluded)
            {
                highestPointSelector = pointSelectors[i];
            }
        }
        return highestPointSelector;
    }
}