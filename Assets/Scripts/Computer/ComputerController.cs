using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using CT;
using CT.Lookup;
using CT.Data;
using CT.Enumerations;

public enum ComputerState
{
    MAIN_COMPUTER,
    TECH_TREE_SCREEN,
    JOURNAL
}

// NOTE: ALL AUDIO WAS MOVED TO THEIR RESPECTIVE ANIM EVENT SCRIPTS
public class ComputerController : MonoBehaviour
{

    #region Member Variables
    public static ComputerController Instance;

    private Transform lookAt;
    private Camera screenCam;
    private Camera techCam;
    private CinemachineVirtualCamera vCam;
    private ComputerState computerState = ComputerState.MAIN_COMPUTER;
    [HideInInspector]
    public Material mat_awareness;

    [HideInInspector]
    public GameObject[] policyCards = new GameObject[7];
    [HideInInspector]
    public GameObject[] myPolicyCards = new GameObject[3];
    [HideInInspector]
    public List<PointSelector> pointSelectors;
    [HideInInspector]
    public GameObject graphGO;
    [HideInInspector]
    public GameObject sliderRect;
    [HideInInspector]
    public WindowGraph graph;
    [HideInInspector]
    public GameObject staticScreenEffect;
    [HideInInspector]
    public GameObject screen;

    #region Animations
    // Anims
    [HideInInspector]
    public Animator[] pCardAnims = new Animator[7];
    [HideInInspector]
    public Animator[] myPCardAnims = new Animator[3];
    private Animator yearKnobAnim;
    private Animator buttonAnim;
    private Animator pointsSelectorAnim;

    [HideInInspector]
    public GameObject notepad;
    [HideInInspector]
    public GameObject journal;
    #endregion

    #region Texts
    // Texts
    [HideInInspector]
    public TMP_Text disasterNameText;
    [HideInInspector]
    public TMP_Text disasterYearText;
    [HideInInspector]
    public TMP_Text disasterMagnitudeText;
    [HideInInspector]
    public TMP_Text disasterDeathTollText;
    [HideInInspector]
    public TMP_Text safetyText;
    [HideInInspector]
    public TMP_Text yearText;
    [HideInInspector]
    public TMP_Text foodText;
    [HideInInspector]
    public TMP_Text rpText;
    [HideInInspector]
    public TMP_Text currencyText;
    [HideInInspector]
    public TMP_Text populationText;
    [HideInInspector]
    public TMP_Text[] pCardTexts = new TMP_Text[7];
    [HideInInspector]
    public TMP_Text[] myPCardTexts = new TMP_Text[3];
    #endregion

    #region Interactables
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
    [HideInInspector]
    public bool showGraph = false;
    [HideInInspector]
    public bool canSwitch = true;

    // Year slider
    private GameObject yearSlider;
    private float currentX;
    private bool yearSliding = false;
    private float minYearSlider = 0f;
    private float maxYearSlider = 1.98f;
    public List<CTTurnData> turns = new List<CTTurnData>();
    #endregion

    #region UI
    // UI
    private GameObject panUpButton;
    private GameObject panDownButton;
    private GameObject panBackFromUpButton;
    private GameObject panBackFromDownButton;
    private bool panning = false;
    private Vector3 defaultLook = new Vector3(0f, -0.5f, 0f);
    private Vector3 shiftPos;
    private Vector3 lookDown = new Vector3(0f, -12f, 0f);
    private Vector3 lookUp = new Vector3(0f, 12f, 0f);
    [HideInInspector]
    public float totalPointsLimit = 10f;
    [HideInInspector]
    public int desiredYear = 1900;
    private Color desiredEqualCurrentColour = new Color(0f, 0.74f, 0.69f, 255f) * 5.5f;
    private Color desiredNotEqualCurrentColour = new Color(0.04f, 0.74f, 0f, 255f) * 5.5f;
    #endregion

    #region Inputs
    // inputs
    private Vector2 mousePos;
    private Vector2 mouseDelta;
    private Vector2 scroll;
    private bool isInteractingHeld;
    private bool isInteractingPressed;
    private bool isInteractingReleased;
    private bool isSelectingHeld;
    private bool isSelectingPressed;
    private bool isSelectingReleased;
    private bool isShifting;
    private bool isPausing;
    #endregion
    #endregion

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

        Ray ray = cam.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            PressButton(hit);
            switch (computerState)
            {
                case ComputerState.MAIN_COMPUTER:
                    YearSlider(hit);
                    MapScreen(hit);
                    PointSelectors(hit);
                    YearKnob(hit);
                    PolicyCards(hit);
                    Focus();

                    break;
                case ComputerState.TECH_TREE_SCREEN:
                    TechScreen(hit);

                    break;
                case ComputerState.JOURNAL:
                    // Specific journal stuff

                    break;
            }
        }
        else
        {
            ResetScreenInteractions();
        }

        ResetMouseButtons();
    }

    private void PressButton(RaycastHit _hit)
    {
        if (isInteractingPressed && _hit.transform.CompareTag("Button"))
        {
            buttonAnim = _hit.transform.GetComponent<Animator>();

            if (buttonAnim != null)
                buttonAnim.SetTrigger("Press");
        }
    }

    private void TechScreen(RaycastHit _hit)
    {
        // Specific tech tree stuff
        onTech = _hit.transform.name == "TechTreeScreen";

        InteractWithScreen(_hit, onTech, techCam);

        if (isSelectingPressed && onTech && !yearSliding)
        {
            touchingTechScreen = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (isSelectingReleased && !yearSliding)
        {
            touchingTechScreen = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void MapScreen(RaycastHit _hit)
    {
        onScreen = _hit.transform.gameObject == screen;
        InteractWithScreen(_hit, onScreen, screenCam);

        if (isSelectingPressed && onScreen && !yearSliding)
        {
            touchingScreen = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (isSelectingReleased && !yearSliding)
        {
            touchingScreen = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void YearSlider(RaycastHit _hit)
    {
        // Moving the year slider
        if (yearSliding)
        {
            float remappedValue = RAUtility.Remap(desiredYear, DataSheet.STARTING_YEAR, DataSheet.END_YEAR, minYearSlider, maxYearSlider);
            float newRemappedValue = remappedValue + mouseDelta.x * 0.025f;
            desiredYear = (int)RAUtility.Remap(newRemappedValue, minYearSlider, maxYearSlider, DataSheet.STARTING_YEAR, DataSheet.END_YEAR);

            if (desiredYear % 5 != 0)
            {
                desiredYear = desiredYear - (desiredYear % 5);
                AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.sliderEvent, null);
            }

            if (desiredYear < DataSheet.STARTING_YEAR)
                desiredYear = (int)DataSheet.STARTING_YEAR;

            if (desiredYear > DataSheet.END_YEAR)
                desiredYear = (int)DataSheet.END_YEAR;

            newRemappedValue = RAUtility.Remap(desiredYear, DataSheet.STARTING_YEAR, DataSheet.END_YEAR, minYearSlider, maxYearSlider);
            yearSlider.transform.localPosition = new Vector3(newRemappedValue, yearSlider.transform.localPosition.y, yearSlider.transform.localPosition.z);
            UpdateSlider();
        }

        if (isInteractingPressed && _hit.transform.CompareTag("YearSlider"))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            yearSliding = true;
        }

        if (isInteractingReleased)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            //Debug.Log("RELEASE");
            yearSliding = false;
        }
    }

    private void PointSelectors(RaycastHit _hit)
    {
        if (isInteractingPressed && _hit.transform.CompareTag("PointsSelector"))
        {
            pointsSelectorAnim = _hit.transform.GetComponent<Animator>();


            if (pointsSelectorAnim != null)
                AudioPlayback.PlayOneShotWithParameters<string>(AudioManager.Instance.uiEvents.pipEvent, null, ("PipDirection", "Up"));
            pointsSelectorAnim.SetTrigger("PointsUp");

            journal.GetComponent<Journal>().UpdateFactionProductionText();
        }

        if (isSelectingPressed && _hit.transform.CompareTag("PointsSelector"))
        {
            pointsSelectorAnim = _hit.transform.GetComponent<Animator>();


            if (pointsSelectorAnim != null)
                AudioPlayback.PlayOneShotWithParameters<string>(AudioManager.Instance.uiEvents.pipEvent, null, ("PipDirection", "Down"));
            pointsSelectorAnim.SetTrigger("PointsDown");

            journal.GetComponent<Journal>().UpdateFactionProductionText();
        }
    }

    public IEnumerator SwitchMainScreen()
    {
        // Plot graph with necessary values when showing graph
        RefreshGraph();
        AudioPlayback.PlayOneShot(AudioManager.Instance.uiEvents.staticGraphShowEvent, null);
        showGraph = !showGraph;
        canSwitch = false;
        staticScreenEffect.SetActive(true);
        yield return new WaitForSeconds(1f);
        screen.SetActive(!showGraph);
        graphGO.SetActive(showGraph);
        staticScreenEffect.SetActive(false);
        canSwitch = true;

        if (showGraph)
        {
            // Set Colours
            currencyText.color = DataSheet.WORKER_COLOUR;
            rpText.color = DataSheet.SCIENTIST_COLOUR;
            foodText.color = DataSheet.FARMER_COLOUR;
            pointSelectors[0].pipMat.SetInt("_isGraph", 1);
            pointSelectors[1].pipMat.SetInt("_isGraph", 1);
            pointSelectors[2].pipMat.SetInt("_isGraph", 1);
            pointSelectors[3].pipMat.SetInt("_isGraph", 1);
        }
        else
        {
            // Reset Colours
            currencyText.color = DataSheet.DEFAULT_COLOR;
            rpText.color = DataSheet.DEFAULT_COLOR;
            foodText.color = DataSheet.DEFAULT_COLOR;
            pointSelectors[0].pipMat.SetInt("_isGraph", 0);
            pointSelectors[1].pipMat.SetInt("_isGraph", 0);
            pointSelectors[2].pipMat.SetInt("_isGraph", 0);
            pointSelectors[3].pipMat.SetInt("_isGraph", 0);

            // Reset Pips

            // Scientist
            pointSelectors[0].pipMat.SetFloat("_FillAmount", GameManager._INSTANCE.GetFactionDistribution().y * 10);
            // Plan
            pointSelectors[1].pipMat.SetFloat("_FillAmount", GameManager._INSTANCE.GetFactionDistribution().w * 10);
            // Farmer
            pointSelectors[3].pipMat.SetFloat("_FillAmount", GameManager._INSTANCE.GetFactionDistribution().z * 10);
            // Worker
            pointSelectors[2].pipMat.SetFloat("_FillAmount", GameManager._INSTANCE.GetFactionDistribution().x * 10);

            // Reset Counters
            GameManager._INSTANCE.UpdateResourceCounters();
        }

        yield return null;
    }

    private void YearKnob(RaycastHit _hit)
    {
        // Year knob up/down
        if (yearKnobAnim != null)
        {
            yearKnobAnim.SetBool("YearDownHold", isSelectingHeld && _hit.transform.CompareTag("YearKnob"));
            yearKnobAnim.SetBool("YearUpHold", isInteractingHeld && _hit.transform.CompareTag("YearKnob"));
        }
    }

    private void PolicyCards(RaycastHit _hit)
    {
        // Select policy card
        if (isInteractingPressed && _hit.transform.CompareTag("PolicyCard"))
        {
            //PolicyManager.instance.ReplacePolicyCard(_hit.transform.GetComponent<CTPolicyContainer>().GetCurrentPolicy().ID);
            CTPolicyContainer test = _hit.transform.GetComponent<CTPolicyContainer>();
            PolicyManager.instance.aboutToBePurchasedCard = test.GetCurrentPolicy();
            PolicyManager.instance.current_policy_container.SetPolicyContainer(_hit.transform.GetComponent<CTPolicyContainer>());
            PolicyManager.instance.PolicySelect(test.GetCurrentPolicy());
            journal.GetComponent<Journal>().UpdateFactionProductionText();
            //PolicyManager.instance.SelectPolicy(test.GetCurrentPolicy().ID);
            //PolicyManager.instance.ShowAllCurrentPoliciesAtTurn();
        }

        // Policy cards hover
        for (int i = 0; i < policyCards.Length; i++)
        {
            if (pCardAnims[i] != null && policyCards[i] != null)
                pCardAnims[i].SetBool("Right", _hit.transform.name == policyCards[i].name);
        }

        // View current policy card selected
        if (isInteractingPressed && _hit.transform.CompareTag("MyPolicyCard"))
        {
            CTPolicyContainer test = _hit.transform.GetComponent<CTPolicyContainer>();
            PolicyManager.instance.current_policy_container.SetPolicyContainer(_hit.transform.GetComponent<CTPolicyContainer>());
            PolicyManager.instance.ShowPolicyViewScreen(test.GetCurrentPolicy());
        }

        for (int i = 0; i < myPolicyCards.Length; i++)
        {
            if (myPCardAnims[i] != null && myPolicyCards[i] != null)
                myPCardAnims[i].SetBool("Left", _hit.transform.name == myPolicyCards[i].name);
        }
    }

    private void Focus()
    {
        if (isShifting && !panning)
        {
            vCam.Follow = lookAt;
            shiftPos = new Vector3(shiftPos.x + (mouseDelta.x * 0.05f), shiftPos.y + (mouseDelta.y * 0.05f), 0.0f);
            lookAt.localPosition = shiftPos;
        }
        else
        {
            shiftPos = defaultLook;
            lookAt.localPosition = defaultLook;
        }
    }

    public virtual void ShowMessage(string _dataToShow)
    {

        UIHoverManager.OnMouseHover(_dataToShow, Input.mousePosition);
    }

    protected IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(0.1f);

        ShowMessage(dataToShow);
    }

    private bool hitTech = false;
    private string dataToShow = "";
    private void InteractWithScreen(RaycastHit hit, bool screen, Camera cam)
    {
        if (hitTech)
        {
            UIHoverManager.OnLoseFocus();
            hitTech = false;
        }
        if (screen)
        {
            var localPoint = hit.textureCoord;
            Ray camRay = cam.ScreenPointToRay(new Vector2(localPoint.x * cam.pixelWidth, localPoint.y * cam.pixelHeight));
            RaycastHit camHit;
            if (Physics.Raycast(camRay, out camHit))
            {
                if(cam == techCam)
                {
                    if (camHit.transform.CompareTag("TechNode"))
                    {
                        if(!hitTech)
                        {
                            dataToShow = camHit.transform.GetComponent<TechNode>().GetDescription();
                            ShowMessage(dataToShow);
                            hitTech = true;
                        }
                        if (isInteractingPressed)
                        {
                            camHit.transform.GetComponent<TechNode>().Unlock();
                            journal.GetComponent<Journal>().UpdateFactionProductionText();
                        }
                    }
                }

                if (cam == screenCam)
                {
                    if(isInteractingPressed)
                        newPos = camHit.transform.position;
                }
            }
        }
    }

    private void ResetScreenInteractions()
    {
        onScreen = false;
        touchingScreen = false;
        onTech = false;
        touchingTechScreen = false;
    }

    // Call whenever you want to set up everything including references (could be used to reset)
    void Setup()
    {
        // Camera Functionality
        cam = GetComponent<Camera>();
        screenCam = GameObject.FindGameObjectWithTag("ScreenCamera").GetComponent<Camera>();
        techCam = GameObject.FindGameObjectWithTag("TechCamera").GetComponent<Camera>();
        vCam = GameObject.Find("ComputerVirtualCamera").GetComponent<CinemachineVirtualCamera>();
        lookAt = GameObject.FindGameObjectWithTag("LookTarget").transform;
        shiftPos = defaultLook;
        lookAt.localPosition = defaultLook;

        // Set References to Objects
        panUpButton = GameObject.Find("PanUpButton");
        panDownButton = GameObject.Find("PanDownButton");
        panBackFromUpButton = GameObject.Find("PanBackButtonFromUp");
        panBackFromDownButton = GameObject.Find("PanBackButtonFromDown");
        staticScreenEffect = GameObject.Find("StaticEffectScreen");
        screen = GameObject.FindGameObjectWithTag("Screen");
        notepad = GameObject.FindGameObjectWithTag("Notepad");
        journal = GameObject.FindGameObjectWithTag("Journal");
        graphGO = GameObject.FindGameObjectWithTag("Graph");
        sliderRect = GameObject.Find("SliderRect");
        graph = graphGO.transform.GetChild(0).GetComponent<WindowGraph>();
        yearKnobAnim = GameObject.FindGameObjectWithTag("YearKnob").GetComponent<Animator>();
        mat_awareness = GameObject.Find("Liquid").GetComponent<Renderer>().material;

        // Texts
        yearText = GameObject.FindGameObjectWithTag("YearCounter").GetComponent<TMP_Text>();
        foodText = GameObject.FindGameObjectWithTag("FoodCounter").GetComponent<TMP_Text>();
        rpText = GameObject.FindGameObjectWithTag("RPCounter").GetComponent<TMP_Text>();
        currencyText = GameObject.FindGameObjectWithTag("CurrencyCounter").GetComponent<TMP_Text>();
        populationText = GameObject.FindGameObjectWithTag("PopCounter").GetComponent<TMP_Text>();
        disasterNameText = notepad.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
        disasterYearText = notepad.transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>();
        disasterMagnitudeText = notepad.transform.GetChild(1).GetChild(2).GetComponent<TMP_Text>();
        disasterDeathTollText = notepad.transform.GetChild(1).GetChild(3).GetComponent<TMP_Text>();
        safetyText = notepad.transform.GetChild(1).GetChild(4).GetComponent<TMP_Text>();
        yearSlider = GameObject.FindGameObjectWithTag("YearSlider");
        yearText.color = desiredEqualCurrentColour;

        // Policy Cards Setup
        policyCards = GameObject.FindGameObjectsWithTag("PolicyCard");
        myPolicyCards = GameObject.FindGameObjectsWithTag("MyPolicyCard");
        for (int i = 0; i < policyCards.Length; i++)
        {
            pCardTexts[i] = policyCards[i].transform.GetChild(0).GetComponent<TMP_Text>();
            pCardAnims[i] = policyCards[i].GetComponent<Animator>();
        }
        for (int i = 0; i < myPolicyCards.Length; i++)
        {
            myPCardTexts[i] = myPolicyCards[i].transform.GetChild(0).GetComponent<TMP_Text>();
            myPCardAnims[i] = myPolicyCards[i].GetComponent<Animator>();
        }

        // Set States
        computerState = ComputerState.MAIN_COMPUTER;
        notepad.SetActive(true);
        journal.SetActive(false);
        panUpButton.SetActive(true);
        panDownButton.SetActive(true);
        panBackFromUpButton.SetActive(false);
        panBackFromDownButton.SetActive(false);
        staticScreenEffect.SetActive(false);
        screen.SetActive(true);
        graphGO.SetActive(false);

        // Set Values
        //desiredYear = YearData._INSTANCE.current_year;
        showGraph = false;

        // Misc
        pointSelectors = new List<PointSelector>(FindObjectsOfType<PointSelector>());
        newPos = Vector3.zero;

        UpdateSlider();

    }

    public void RefreshGraph()
    {
        RAUtility.Vector4List timeLineResources = GameManager._INSTANCE.GetResourcesAcrossYears();
        graph.UpdateAndShowGraphs(timeLineResources.x, timeLineResources.y, timeLineResources.z, timeLineResources.w);
    }

    public void UpdateSlider()
    {
        float rectOffset = 148.545f;
        float sliderPosValue = RAUtility.Remap(desiredYear, DataSheet.STARTING_YEAR, DataSheet.END_YEAR, minYearSlider, maxYearSlider);
        float sliderRectPosValue = RAUtility.Remap(sliderPosValue, minYearSlider, maxYearSlider, 18, 283);
        yearSlider.transform.localPosition = new Vector3(sliderPosValue, yearSlider.transform.localPosition.y, yearSlider.transform.localPosition.z);
        sliderRect.transform.localPosition = new Vector3(sliderRectPosValue - rectOffset, sliderRect.transform.localPosition.y, sliderRect.transform.localPosition.z);

        yearText.text = desiredYear.ToString();

        if (GameManager._INSTANCE == null)
            return;

        if ((desiredYear - DataSheet.STARTING_YEAR) / 5 == GameManager._INSTANCE.GetTurn().turn)
            yearText.color = desiredEqualCurrentColour;
        else
            yearText.color = desiredNotEqualCurrentColour;


        // Updates counters & pips depending on the desired year in graph mode
        ChangeCountersToGraphMode();
    }

    private void ChangeCountersToGraphMode()
    {
        if (!showGraph)
            return;

        int lookupTurn = (int)((desiredYear - DataSheet.STARTING_YEAR) / 5);
        currencyText.text = turns[lookupTurn].Money.ToString();
        rpText.text = turns[lookupTurn].Science.ToString();
        foodText.text = turns[lookupTurn].Food.ToString();
        populationText.text = turns[lookupTurn].Population.ToString();

        //populationText.color = Color.white;

        //Debug.Log(turns[lookupTurn].turn);
        // Sci
        pointSelectors[0].pipMat.SetFloat("_FillAmount", GameManager._INSTANCE.GetFactionDistribtion(CTFaction.Scientist, turns[lookupTurn]) * 10);
        pointSelectors[0].pipMat.SetInt("_isGraph", 1);
        // Plan
        pointSelectors[1].pipMat.SetFloat("_FillAmount", GameManager._INSTANCE.GetFactionDistribtion(CTFaction.Planner, turns[lookupTurn]) * 10);
        pointSelectors[1].pipMat.SetInt("_isGraph", 1);
        // Farmer
        pointSelectors[3].pipMat.SetFloat("_FillAmount", GameManager._INSTANCE.GetFactionDistribtion(CTFaction.Farmer, turns[lookupTurn]) * 10);
        pointSelectors[3].pipMat.SetInt("_isGraph", 1);
        // Worker
        pointSelectors[2].pipMat.SetFloat("_FillAmount", GameManager._INSTANCE.GetFactionDistribtion(CTFaction.Worker, turns[lookupTurn]) * 10);
        pointSelectors[2].pipMat.SetInt("_isGraph", 1);
    }

    public void CheckPoints(PointSelector excluded)
    {
        float totalPoints = 0;
        foreach (var pointSelector in pointSelectors)
        {
            totalPoints += pointSelector.pointValue;
        }
        if (totalPoints > totalPointsLimit)
        {
            PointSelector highestPointSelector = FindHighestPointSelector(excluded);
            highestPointSelector.RemovePoints(1);
        }
    }

    private PointSelector FindHighestPointSelector(PointSelector excluded)
    {
        PointSelector highestPointSelector = null;
        for (int i = 0; i < pointSelectors.Count; i++)
        {
            if (pointSelectors[i] == excluded)
            {
                continue;
            }
            if (highestPointSelector == null || pointSelectors[i].pointValue > highestPointSelector.pointValue)
            {
                highestPointSelector = pointSelectors[i];
            }
        }
        return highestPointSelector;
    }

    public void Pan(int value)
    {
        vCam.Follow = null;
        lookAt.localPosition = defaultLook;
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
        SnapshotHandler.instance.StartCameraPanSnapShot();
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
        SnapshotHandler.instance.StartCameraPanSnapShot();
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
        SnapshotHandler.instance.StopSnapShot(SnapshotHandler.instance.panSnapShotinstance);
        yield return new WaitForSeconds(1.0f);
        panning = false;
        panUpButton.SetActive(true);
        panDownButton.SetActive(true);
        yield return null;
    }

    private void ResetMouseButtons()
    {
        isInteractingPressed = false;
        isInteractingReleased = false;
        isSelectingPressed = false;
        isSelectingReleased = false;
    }

    private void CursorPosInput(InputAction.CallbackContext context)
    {
        mousePos = context.ReadValue<Vector2>();
    }

    private void AimInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    private void ScrollInput(InputAction.CallbackContext context)
    {
        scroll = context.ReadValue<Vector2>();
    }

    private void InteractInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isInteractingPressed = true;
            isInteractingHeld = true;
        }
        else if (context.canceled)
        {
            isInteractingReleased = true;
            isInteractingHeld = false;
        }
    }

    private void ShiftInput(InputAction.CallbackContext context)
    {
        if (context.performed && computerState == ComputerState.MAIN_COMPUTER)
        {
            vCam.m_Lens.FieldOfView = 20.0f;
            Cursor.lockState = CursorLockMode.Locked;
            isShifting = true;
        }
        else if (context.canceled)
        {
            vCam.m_Lens.FieldOfView = 60.0f;
            Cursor.lockState = CursorLockMode.None;
            isShifting = false;
        }
    }

    private void SelectInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isSelectingPressed = true;
            isSelectingHeld = true;
        }
        else if (context.canceled)
        {
            isSelectingReleased = true;
            isSelectingHeld = false;
        }
    }

    private void PauseInput(InputAction.CallbackContext context)
    {
        if (context.performed)
            isPausing = true;
        else if (context.canceled)
            isPausing = false;
    }

    private void SubscribeInputs()
    {
        InputManager.onCursorPos += CursorPosInput;
        InputManager.onShift += ShiftInput;
        InputManager.onAim += AimInput;
        InputManager.onScroll += ScrollInput;
        InputManager.onInteract += InteractInput;
        InputManager.onSelect += SelectInput;
        InputManager.onPause += PauseInput;
    }

    private void UnsubscribeInputs()
    {
        InputManager.onCursorPos -= CursorPosInput;
        InputManager.onShift -= ShiftInput;
        InputManager.onAim -= AimInput;
        InputManager.onScroll -= ScrollInput;
        InputManager.onInteract -= InteractInput;
        InputManager.onSelect -= SelectInput;
        InputManager.onPause -= PauseInput;
    }

    private void OnEnable()
    {
        SubscribeInputs();
    }

    private void OnDisable()
    {
        UnsubscribeInputs();
    }

    private void OnDestroy()
    {
        UnsubscribeInputs();
    }
}