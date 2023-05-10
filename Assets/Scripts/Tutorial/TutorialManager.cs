using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{

    // State of the game
    public enum GameState
    {
        InGame,
        InTutorialMenu,
        Interactions,
        Panning,
    }

    public GameState gameState;


    // UI menues
    [SerializeField] public GameObject TutorialUI;
    [SerializeField] public GameObject InteractableUI;
    [SerializeField] public GameObject InGameUI;

    // Interaction TextObjects
    public static GameObject PipsUIText;
    public static GameObject AwarenessUIText;
    public static GameObject TimeSliderUIText;
    public static GameObject PolicyUIText;
    public static GameObject YearSliderUIText;
    public static GameObject TimePipUIText;
    public static GameObject ConfirmChangeUIText;
    public static GameObject GraphUIText;

    // GamePlay Elements
    public static GameObject PanUpFromScreenObject;
    public static GameObject NextButtonObject;
    
    public List<GameObject> InteractionButtons = new List<GameObject>();

    public void Start()
    {
        // Assign the gameObject
        InitVariables();

        // Set the tutorial text to be displayed by default
        gameState = GameState.InTutorialMenu;   
        InteractableUI.SetActive(false);
    }

    public void Update()
    {
        if (gameState == GameState.InTutorialMenu)
        {
            TutorialUI.SetActive(true);
            InGameUI.SetActive(false);
        }
        else if (gameState == GameState.InGame)
        {
            TutorialUI.SetActive(false);
            InGameUI.SetActive(true);
        }

        else if (gameState == GameState.Panning)
        {
            
        }
    }

    public void InitVariables()
    {
        TutorialUI = GameObject.Find("TutorialOptionUI");
        InGameUI = GameObject.Find("GameUI");
        InteractableUI = GameObject.Find("TutorialInteractables");

        PipsUIText =   GameObject.Find("PipsUI");
        AwarenessUIText =   GameObject.Find("AwarenessUI");
        TimeSliderUIText =   GameObject.Find("TimeSliderUI");
        PolicyUIText =   GameObject.Find("PolicyCardsUI");
        YearSliderUIText =   GameObject.Find("YearSliderUI");
        TimePipUIText =   GameObject.Find("TimePipUI");
        ConfirmChangeUIText =   GameObject.Find("ConfirmChangeUI");
        GraphUIText =   GameObject.Find("GraphUI");


        PanUpFromScreenObject = GameObject.Find("PanUpButton");
        NextButtonObject = GameObject.Find("NextPartOfTutorial");

        PipsUIText.SetActive(false);
        AwarenessUIText.SetActive(false);
        TimeSliderUIText.SetActive(false);
        PolicyUIText.SetActive(false);
        YearSliderUIText.SetActive(false);
        TimePipUIText.SetActive(false);
        ConfirmChangeUIText.SetActive(false);
        GraphUIText.SetActive(false);

        PanUpFromScreenObject.SetActive(false);
        NextButtonObject.SetActive(false);
    }      
}
