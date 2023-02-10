using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    private Camera techCam;
    private ComputerState computerState = ComputerState.MAIN_COMPUTER;

    [HideInInspector]
    public GameObject[] policyCards = new GameObject[7];
    [HideInInspector]
    public List<PointSelector> pointSelectors;
    private Policy currentPolicy;

    // Anims
    [HideInInspector]
    public Animator[] pCardAnims = new Animator[7];
    private Animator yearKnobAnim;
    private Animator buttonAnim;
    private Animator pointsSelectorAnim;

    [HideInInspector]
    public GameObject notepad;
    [HideInInspector]
    public GameObject journal;

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
    public bool touchingTechScreen = false;
    [HideInInspector]
    public bool onScreen = false;
    [HideInInspector]
    public bool onTech = false;

    // UI
    private GameObject panUpButton;
    private GameObject panDownButton;
    private GameObject panBackFromUpButton;
    private GameObject panBackFromDownButton;
    private bool panning = false;
    private Vector3 defaultLook = new Vector3(0f, -0.5f, 0f);
    private Vector3 lookDown = new Vector3(0f, -12f, 0f);
    private Vector3 lookUp = new Vector3(0f, 12f, 0f);
    [HideInInspector]
    public int desiredYear = 1900;
    private Color desiredEqualCurrentColour = new Color(0f, 0.74f, 0.69f, 255f) * 5.5f;
    private Color desiredNotEqualCurrentColour = new Color(0.04f, 0.74f, 0f, 255f) * 5.5f;

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

        yearText.text = desiredYear.ToString();
        if (desiredYear == YearData._INSTANCE.current_year)
            yearText.color = desiredEqualCurrentColour;
        else
            yearText.color = desiredNotEqualCurrentColour;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0) && hit.transform.CompareTag("Button"))
            {
                buttonAnim = hit.transform.GetComponent<Animator>();

                if (buttonAnim != null)
                    buttonAnim.SetTrigger("Press");
            }

            switch (computerState)
            {
                case ComputerState.MAIN_COMPUTER:
                    if (yearSliding)
                    {
                        float remappedValue = Remap(desiredYear, YearData._INSTANCE.earliest_year, YearData._INSTANCE.latest_year, minYearSlider, maxYearSlider);
                        float newRemappedValue = remappedValue + Input.GetAxis("Mouse X") * 0.5f;
                        desiredYear = (int)Remap(newRemappedValue, minYearSlider, maxYearSlider, YearData._INSTANCE.earliest_year, YearData._INSTANCE.latest_year);

                        if (desiredYear % 5 != 0)
                        {
                            desiredYear = desiredYear - (desiredYear % 5);
                            AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.sliderEvent, null);
                        }

                        if (desiredYear < YearData._INSTANCE.earliest_year)
                            desiredYear = YearData._INSTANCE.earliest_year;

                        if (desiredYear > YearData._INSTANCE.latest_year)
                            desiredYear = YearData._INSTANCE.latest_year;

                        newRemappedValue = Remap(desiredYear, YearData._INSTANCE.earliest_year, YearData._INSTANCE.latest_year, minYearSlider, maxYearSlider);
                        yearSlider.transform.localPosition = new Vector3(newRemappedValue, yearSlider.transform.localPosition.y, yearSlider.transform.localPosition.z);
                    }

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

                    onScreen = hit.transform.name == "Screen";

                    InteractWithScreen(hit, onScreen, screenCam);

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

                    if (Input.GetMouseButtonDown(0) && hit.transform.CompareTag("PointsSelector"))
                    {
                        pointsSelectorAnim = hit.transform.GetComponent<Animator>();
                       

                        if (pointsSelectorAnim != null)
                            AudioPlayback.PlayOneShotWithParameters<string>(AudioManager.Instance.uiEvents.pipEvent, null, ("PipDirection", "Up"));
                            pointsSelectorAnim.SetTrigger("PointsUp");
                    }

                    if (Input.GetMouseButtonDown(1) && hit.transform.CompareTag("PointsSelector"))
                    {
                        pointsSelectorAnim = hit.transform.GetComponent<Animator>();
                        

                        if (pointsSelectorAnim != null)
                            AudioPlayback.PlayOneShotWithParameters<string>(AudioManager.Instance.uiEvents.pipEvent, null, ("PipDirection", "Down"));
                            pointsSelectorAnim.SetTrigger("PointsDown");
                    }

                    // Year knob up/down
                    if (yearKnobAnim != null)
                    {
                        yearKnobAnim.SetBool("YearDownHold", Input.GetMouseButton(1) && hit.transform.CompareTag("YearKnob"));
                        yearKnobAnim.SetBool("YearUpHold", Input.GetMouseButton(0) && hit.transform.CompareTag("YearKnob"));
                    }

                    // Select policy card
                    if (Input.GetMouseButtonDown(0) && hit.transform.CompareTag("PolicyCard") && currentPolicy == null)
                    {
                        currentPolicy = hit.transform.GetComponent<PolicyCard>().policy;
                        PolicyManager.instance.finalChoices.Remove(currentPolicy.finalChoice);
                        PolicyManager.instance.policyList.Remove(currentPolicy);
                        Destroy(hit.transform.gameObject);
                        PolicyManager.instance.ReplacePolicyCard();
                    }

                    // Policy cards hover
                    for (int i = 0; i < 7; i++)
                    {
                        if (pCardAnims[i] != null && policyCards[i] != null)
                            pCardAnims[i].SetBool("IsOver", hit.transform.name == policyCards[i].name);
                    }

                    break;
                case ComputerState.TECH_TREE_SCREEN:
                    // Specific tech tree stuff
                    onTech = hit.transform.name == "TechTreeScreen";

                    InteractWithScreen(hit, onTech, techCam);

                    if (Input.GetMouseButtonDown(1) && onTech && !yearSliding)
                    {
                        touchingTechScreen = true;
                        Cursor.lockState = CursorLockMode.Locked;
                        Cursor.visible = false;
                    }

                    if (Input.GetMouseButtonUp(1) && !yearSliding)
                    {
                        touchingTechScreen = false;
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                    }
                    break;
                case ComputerState.JOURNAL:
                    // Specific journal stuff
                    break;
            }
        }
        else
        {
            onScreen = false;
            touchingScreen = false;
            onTech = false;
            touchingTechScreen = false;
        }
    }

    private void InteractWithScreen(RaycastHit hit, bool screen, Camera cam)
    {
        if (Input.GetMouseButtonDown(0) && screen)
        {
            var localPoint = hit.textureCoord;
            Ray camRay = cam.ScreenPointToRay(new Vector2(localPoint.x * cam.pixelWidth, localPoint.y * cam.pixelHeight));
            RaycastHit camHit;
            if (Physics.Raycast(camRay, out camHit))
            {
                if(cam == techCam)
                {
                    if (camHit.transform.CompareTag("TechNode"))
                        camHit.transform.GetComponent<TechNode>().Unlock();
                }

                if (cam == screenCam)
                    newPos = camHit.transform.position;
            }
        }
    }

    // Call whenever you want to set up everything including references (could be used to reset)
    void Setup()
    {
        screenCam = GameObject.FindGameObjectWithTag("ScreenCamera").GetComponent<Camera>();
        techCam = GameObject.FindGameObjectWithTag("TechCamera").GetComponent<Camera>();
        lookAt = GameObject.FindGameObjectWithTag("LookTarget").transform;
        computerState = ComputerState.MAIN_COMPUTER;
        lookAt.localPosition = defaultLook;
        panUpButton = GameObject.Find("PanUpButton");
        panDownButton = GameObject.Find("PanDownButton");
        panBackFromUpButton = GameObject.Find("PanBackButtonFromUp");
        panBackFromDownButton = GameObject.Find("PanBackButtonFromDown");
        Debug.Log(panBackFromUpButton.name);
        panUpButton.SetActive(true);
        panDownButton.SetActive(true);
        panBackFromUpButton.SetActive(false);
        panBackFromDownButton.SetActive(false);
        cam = GetComponent<Camera>();
        newPos = Vector3.zero;
        yearKnobAnim = GameObject.FindGameObjectWithTag("YearKnob").GetComponent<Animator>();
        desiredYear = YearData._INSTANCE.current_year;
        yearText = GameObject.FindGameObjectWithTag("YearCounter").GetComponent<TMP_Text>();
        notepad = GameObject.FindGameObjectWithTag("Notepad");
        journal = GameObject.FindGameObjectWithTag("Journal");
        notepadText = notepad.transform.GetChild(0).GetComponent<TMP_Text>();
        notepad.SetActive(true);
        journal.SetActive(false);
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
        float remappedValue = Remap(desiredYear, YearData._INSTANCE.earliest_year, YearData._INSTANCE.latest_year, minYearSlider, maxYearSlider);
        yearSlider.transform.localPosition = new Vector3(remappedValue, yearSlider.transform.localPosition.y, yearSlider.transform.localPosition.z);
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
        panUpButton.SetActive(false);
        panDownButton.SetActive(false);
        computerState = ComputerState.TECH_TREE_SCREEN;
        lookAt.localPosition = lookUp;
        yield return new WaitForSeconds(1.0f);
        panning = false;
        panBackFromUpButton.SetActive(true);
        yield return null;
    }

    private IEnumerator PanDown()
    {
        panning = true;
        panUpButton.SetActive(false);
        panDownButton.SetActive(false);
        computerState = ComputerState.JOURNAL;
        lookAt.localPosition = lookDown;
        yield return new WaitForSeconds(1.0f);
        panning = false;
        panBackFromDownButton.SetActive(true);
        yield return null;
    }

    private IEnumerator PanBack()
    {
        panning = true;
        panBackFromDownButton.SetActive(false);
        panBackFromUpButton.SetActive(false);
        computerState = ComputerState.MAIN_COMPUTER;
        lookAt.localPosition = defaultLook;
        yield return new WaitForSeconds(1.0f);
        panning = false;
        panUpButton.SetActive(true);
        panDownButton.SetActive(true);
        yield return null;
    }
}