using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum ComputerState
{
    MAIN_COMPUTER,
    TECH_TREE_SCREEN,
    JOURNAL
}

// NOTE: ALL AUDIO WAS MOVED TO THEIR RESPECTIVE ANIM EVENT SCRIPTS
public class ComputerController : MonoBehaviour
{
    public static ComputerController Instance;

    private Transform lookAt;
    private Camera screenCam;
    private ComputerState computerState = ComputerState.MAIN_COMPUTER;

    [HideInInspector]
    public GameObject[] policyCards = new GameObject[7];
    [HideInInspector]
    public List<PointSelector> pointSelectors;

    // Anims
    private Animator[] pCardAnims = new Animator[7];
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

    // UI
    public UnityEngine.UI.Button panUpButton;
    public UnityEngine.UI.Button panDownButton;
    public UnityEngine.UI.Button panBackFromUpButton;
    public UnityEngine.UI.Button panBackFromDownButton;
    private bool panning = false;
    private Vector3 defaultLook = new Vector3(0f, -0.5f, 0f);
    private Vector3 lookDown = new Vector3(0f, -12f, 0f);
    private Vector3 lookUp = new Vector3(0f, 12f, 0f);

    // Year slider
    private GameObject yearSlider;
    private float currentX;
    private bool yearSliding = false;
    private float minYearSlider = 0f;
    private float maxYearSlider = 1.98f;

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

            if (Input.GetMouseButtonDown(1) && onScreen && !yearSliding)
            {
                touchingScreen = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            if (Input.GetMouseButtonUp(1) && !yearSliding)
            {
                touchingScreen = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            HandleAnims(hit);

            if (Input.GetMouseButtonDown(0) && hit.transform.CompareTag("YearSlider"))
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                yearSliding = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                yearSliding = false;
            }

            if(yearSliding)
            {
                float remappedValue = Remap(YearData._INSTANCE.current_year, YearData._INSTANCE.earliest_year, YearData._INSTANCE.latest_year, minYearSlider, maxYearSlider);
                float newRemappedValue = remappedValue + Input.GetAxis("Mouse X") * 0.5f;
                YearData._INSTANCE.current_year = (int)Remap(newRemappedValue, minYearSlider, maxYearSlider, YearData._INSTANCE.earliest_year, YearData._INSTANCE.latest_year);

                if (YearData._INSTANCE.current_year % 5 != 0)
                {
                    YearData._INSTANCE.current_year = YearData._INSTANCE.current_year - (YearData._INSTANCE.current_year % 5);
                    AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.sliderEvent, null);
                }

                if (YearData._INSTANCE.current_year < YearData._INSTANCE.earliest_year)
                    YearData._INSTANCE.current_year = YearData._INSTANCE.earliest_year;

                if (YearData._INSTANCE.current_year > YearData._INSTANCE.latest_year)
                    YearData._INSTANCE.current_year = YearData._INSTANCE.latest_year;

                newRemappedValue = Remap(YearData._INSTANCE.current_year, YearData._INSTANCE.earliest_year, YearData._INSTANCE.latest_year, minYearSlider, maxYearSlider);
                yearSlider.transform.localPosition = new Vector3(newRemappedValue, yearSlider.transform.localPosition.y, yearSlider.transform.localPosition.z);
            }

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
        lookAt = GameObject.FindGameObjectWithTag("LookTarget").transform;
        computerState = ComputerState.MAIN_COMPUTER;
        lookAt.localPosition = defaultLook;
        panUpButton.gameObject.SetActive(true);
        panDownButton.gameObject.SetActive(true);
        panBackFromUpButton.gameObject.SetActive(false);
        panBackFromDownButton.gameObject.SetActive(false);
        cam = GetComponent<Camera>();
        newPos = Vector3.zero;
        yearKnobAnim = GameObject.FindGameObjectWithTag("YearKnob").GetComponent<Animator>();
        yearText = GameObject.FindGameObjectWithTag("YearCounter").GetComponent<TMP_Text>();
        notepadText = GameObject.FindGameObjectWithTag("Notepad").transform.GetChild(0).GetComponent<TMP_Text>();
        pointSelectors = new List<PointSelector>(FindObjectsOfType<PointSelector>());
        yearSlider = GameObject.FindGameObjectWithTag("YearSlider");

        policyCards = GameObject.FindGameObjectsWithTag("PolicyCard");
        for (int i = 0; i < policyCards.Length; i++)
        {
            pCardTexts[i] = policyCards[i].transform.GetChild(0).GetComponent<TMP_Text>();
            pCardAnims[i] = policyCards[i].GetComponent<Animator>();
        }

        UpdateSlider();
    }

    private float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public void UpdateSlider()
    {
        float remappedValue = Remap(YearData._INSTANCE.current_year, YearData._INSTANCE.earliest_year, YearData._INSTANCE.latest_year, minYearSlider, maxYearSlider);
        yearSlider.transform.localPosition = new Vector3(remappedValue, yearSlider.transform.localPosition.y, yearSlider.transform.localPosition.z);
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
            AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.pipEvent, null); //Will create a different beep sound in for removing points, and have param to trigger from one event, that is why this is here atm

            if (pointsSelectorAnim != null)
                pointsSelectorAnim.SetTrigger("PointsUp");
        }

        if (Input.GetMouseButtonDown(1) && hit.transform.CompareTag("PointsSelector"))
        {
            pointsSelectorAnim = hit.transform.GetComponent<Animator>();
            AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.pipEvent, null); //Will create a different beep sound in for removing points, and have param to trigger from one event, that is why this is here atm

            if (pointsSelectorAnim != null)
                pointsSelectorAnim.SetTrigger("PointsDown");
        }

        // Year knob up/down
        if (yearKnobAnim != null)
        {
            yearKnobAnim.SetBool("YearDownHold", Input.GetMouseButton(1) && hit.transform.CompareTag("YearKnob"));
            yearKnobAnim.SetBool("YearUpHold", Input.GetMouseButton(0) && hit.transform.CompareTag("YearKnob"));
        }

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

    public void Pan(int value)
    {
        switch((ComputerState)value)
        {
            case ComputerState.MAIN_COMPUTER:
                StartCoroutine(PanBack());
                break;
            case ComputerState.TECH_TREE_SCREEN:
                StartCoroutine(PanUp());
                break;
            case ComputerState.JOURNAL:
                StartCoroutine(PanDown());
                break;
            default:
                StartCoroutine(PanBack());
                break;
        }
    }

    private IEnumerator PanUp()
    {
        panning = true;
        panUpButton.gameObject.SetActive(false);
        panDownButton.gameObject.SetActive(false);
        computerState = ComputerState.TECH_TREE_SCREEN;
        lookAt.localPosition = lookUp;
        yield return new WaitForSeconds(1.0f);
        panning = false;
        panBackFromUpButton.gameObject.SetActive(true);
        yield return null;
    }

    private IEnumerator PanDown()
    {
        panning = true;
        panUpButton.gameObject.SetActive(false);
        panDownButton.gameObject.SetActive(false);
        computerState = ComputerState.JOURNAL;
        lookAt.localPosition = lookDown;
        yield return new WaitForSeconds(1.0f);
        panning = false;
        panBackFromDownButton.gameObject.SetActive(true);
        yield return null;
    }

    private IEnumerator PanBack()
    {
        panning = true;
        panBackFromDownButton.gameObject.SetActive(false);
        panBackFromUpButton.gameObject.SetActive(false);
        computerState = ComputerState.MAIN_COMPUTER;
        lookAt.localPosition = defaultLook;
        yield return new WaitForSeconds(1.0f);
        panning = false;
        panUpButton.gameObject.SetActive(true);
        panDownButton.gameObject.SetActive(true);
        yield return null;
    }
}