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
        PanningUPFromScreen,
        PanningDownFromScreen,
        TechTree,
        KeyBoard,
    }

    public static GameState gameState;


    // UI menues
    [SerializeField] public GameObject TutorialUI;
    [SerializeField] public GameObject InteractableUI;
    [SerializeField] public GameObject InGameUI;
    [SerializeField] public GameObject TechTreeUI;
    [SerializeField] public static GameObject KeyBoardUI;


    // Interaction TextObjects
    public static GameObject PipsUIText;
    public static GameObject AwarenessUIText;
    public static GameObject TimeSliderUIText;
    public static GameObject PolicyUIText;
    public static GameObject YearSliderUIText;
    public static GameObject TimePipUIText;
    public static GameObject ConfirmChangeUIText;
    public static GameObject GraphUIText;
    public static GameObject TechTreeUIText;


    public static GameObject RedButtonText;
    public static GameObject BreakDownText;
    public static GameObject TypeButtonText;

    // GamePlay Elements
    public static GameObject PanUpFromScreenObject;
    public static GameObject PanDownFromUpTechTreeObject;
    public static GameObject PanDownFromScreenObject;
    public static GameObject NextButtonObject;

    // Game Buttons
    public static GameObject PipButton;
    public static GameObject AwarenessButton;
    public static GameObject TimeSliderButton;
    public static GameObject PolicyButton;
    public static GameObject YearSliderButton;
    public static GameObject TimePipButton;
    public static GameObject ConfirmChangeButton;
    public static GameObject GraphButton;
    public static GameObject TechTreeButton;

    public static GameObject RedButton;
    public static GameObject BreakDown;
    public static GameObject TypeButton;
    public static GameObject FinishTutorialButton;




    public static int Index = 0;
    public static int KeyboardIndex = 0;
    public void Start()
    {
        // Assign the gameObject
        InitVariables();

        // Set the tutorial text to be displayed by default
        gameState = GameState.InTutorialMenu;   
        InteractableUI.SetActive(false);
        Index = 1;
        KeyboardIndex = 1;
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

        else if (gameState == GameState.Interactions)
        {
            switch (Index)
            {
                case 1:
                    PipButton.SetActive(true);
                    AwarenessButton.SetActive(false);
                    TimeSliderButton.SetActive(false);
                    PolicyButton.SetActive(false);
                    YearSliderButton.SetActive(false);
                    TimePipButton.SetActive(false);
                    ConfirmChangeButton.SetActive(false);
                    GraphButton.SetActive(false);
                    break;
                case 2:
                    PipButton.SetActive(false);
                    AwarenessButton.SetActive(true);
                    TimeSliderButton.SetActive(false);
                    PolicyButton.SetActive(false);
                    YearSliderButton.SetActive(false);
                    TimePipButton.SetActive(false);
                    ConfirmChangeButton.SetActive(false);
                    GraphButton.SetActive(false);
                    break;
                case 3:
                    PipButton.SetActive(false);
                    AwarenessButton.SetActive(false);
                    TimeSliderButton.SetActive(true);
                    PolicyButton.SetActive(false);
                    YearSliderButton.SetActive(false);
                    TimePipButton.SetActive(false);
                    ConfirmChangeButton.SetActive(false);
                    GraphButton.SetActive(false);
                    break;
                case 4:
                    PipButton.SetActive(false);
                    AwarenessButton.SetActive(false);
                    TimeSliderButton.SetActive(false);
                    PolicyButton.SetActive(true);
                    YearSliderButton.SetActive(false);
                    TimePipButton.SetActive(false);
                    ConfirmChangeButton.SetActive(false);
                    GraphButton.SetActive(false);
                    break;
                case 5:
                    PipButton.SetActive(false);
                    AwarenessButton.SetActive(false);
                    TimeSliderButton.SetActive(false);
                    PolicyButton.SetActive(false);
                    YearSliderButton.SetActive(true);
                    TimePipButton.SetActive(false);
                    ConfirmChangeButton.SetActive(false);
                    GraphButton.SetActive(false);
                    break;
                case 6:
                    PipButton.SetActive(false);
                    AwarenessButton.SetActive(false);
                    TimeSliderButton.SetActive(false);
                    PolicyButton.SetActive(false);
                    YearSliderButton.SetActive(false);
                    TimePipButton.SetActive(true);
                    ConfirmChangeButton.SetActive(false);
                    GraphButton.SetActive(false);
                    break;
                case 7:
                    PipButton.SetActive(false);
                    AwarenessButton.SetActive(false);
                    TimeSliderButton.SetActive(false);
                    PolicyButton.SetActive(false);
                    YearSliderButton.SetActive(false);
                    TimePipButton.SetActive(false);
                    ConfirmChangeButton.SetActive(true);
                    GraphButton.SetActive(false);
                    break;
                case 8:
                    PipButton.SetActive(false);
                    AwarenessButton.SetActive(false);
                    TimeSliderButton.SetActive(false);
                    PolicyButton.SetActive(false);
                    YearSliderButton.SetActive(false);
                    TimePipButton.SetActive(false);
                    ConfirmChangeButton.SetActive(false);
                    GraphButton.SetActive(true);
                    break;
                case 9:
                    NextButtonObject.SetActive(true);
                    PipButton.SetActive(false);
                    AwarenessButton.SetActive(false);
                    TimeSliderButton.SetActive(false);
                    PolicyButton.SetActive(false);
                    YearSliderButton.SetActive(false);
                    TimePipButton.SetActive(false);
                    ConfirmChangeButton.SetActive(false);
                    GraphButton.SetActive(false);
                    break;

            }

        }
        
        else if (gameState == GameState.PanningUPFromScreen)
        {
            PipsUIText.SetActive(false);
            AwarenessUIText.SetActive(false);
            TimeSliderUIText.SetActive(false);
            PolicyUIText.SetActive(false);
            YearSliderUIText.SetActive(false);
            TimePipUIText.SetActive(false);
            ConfirmChangeUIText.SetActive(false);
            GraphUIText.SetActive(false);

            NextButtonObject.SetActive(false);
            PanUpFromScreenObject.SetActive(true);
        }

        else if (gameState == GameState.TechTree)
        {
            TechTreeUI.SetActive(true);
            //TechTreeButton.SetActive(true);
        }
        else if (gameState == GameState.PanningDownFromScreen)
        {
            TechTreeUI.SetActive(false);
            PanDownFromScreenObject.SetActive(true);
            //TechTreeButton.SetActive(true);
        }
        else if (gameState == GameState.KeyBoard)
        {
            switch (KeyboardIndex)
            {
                case 1:
                    RedButton.SetActive(true);
                    TypeButton.SetActive(false);
                    BreakDown.SetActive(false);
                    break;
                case 2:
                    RedButton.SetActive(false);
                    TypeButton.SetActive(true);
                    BreakDown.SetActive(false);
                    break;
                case 3:
                    RedButton.SetActive(false);
                    TypeButton.SetActive(false);
                    BreakDown.SetActive(true);
                    break;
                case 4:
                    RedButton.SetActive(false);
                    TypeButton.SetActive(false);
                    BreakDown.SetActive(false);
                    FinishTutorialButton.SetActive(true);
                    break;


            }

        }
    }

    public void InitVariables()
    {

        TutorialUI = GameObject.Find("TutorialOptionUI");
        InGameUI = GameObject.Find("GameUI");
        InteractableUI = GameObject.Find("TutorialInteractables");
        TechTreeUI = GameObject.Find("TechTreeUI");
        KeyBoardUI = GameObject.Find("KeyBoardUI");

        // Text Elements
        PipsUIText =   GameObject.Find("PipsUI");
        AwarenessUIText =   GameObject.Find("AwarenessUI");
        TimeSliderUIText =   GameObject.Find("TimeSliderUI");
        PolicyUIText =   GameObject.Find("PolicyCardsUI");
        YearSliderUIText =   GameObject.Find("YearSliderUI");
        TimePipUIText =   GameObject.Find("TimePipUI");
        ConfirmChangeUIText =   GameObject.Find("ConfirmChangeUI");
        GraphUIText =   GameObject.Find("GraphUI");
        TechTreeUIText = GameObject.Find("TechTreeUIText");

        RedButtonText = GameObject.Find("RedButtonText");
        BreakDownText = GameObject.Find("TypeingText");
        TypeButtonText = GameObject.Find("TypeScreenText");


        PanUpFromScreenObject = GameObject.Find("TutorialUpPanning");
        PanDownFromUpTechTreeObject = GameObject.Find("PanBackButtonFromUp (1)");
        PanDownFromScreenObject = GameObject.Find("PanDownButton (1)");
        NextButtonObject = GameObject.Find("NextPartOfTutorial");

        RedButton = GameObject.Find("RedButton");
        BreakDown = GameObject.Find("TypeingButton");
        TypeButton = GameObject.Find("BreakDownButton");

        // Buttons
        PipButton = GameObject.Find("TutorialButton");
        AwarenessButton = GameObject.Find("TutorialButton (1)");
        TimeSliderButton = GameObject.Find("TutorialButton (2)");
        PolicyButton = GameObject.Find("TutorialButton (3)");
        YearSliderButton = GameObject.Find("TutorialButton (4)");
        TimePipButton = GameObject.Find("TutorialButton (5)");
        ConfirmChangeButton = GameObject.Find("TutorialButton (6)");
        GraphButton = GameObject.Find("TutorialButton (7)");

        TechTreeButton = GameObject.Find("TutorialButton (8)");

        FinishTutorialButton = GameObject.Find("FinishButton");

        PipsUIText.SetActive(false);
        AwarenessUIText.SetActive(false);
        TimeSliderUIText.SetActive(false);
        PolicyUIText.SetActive(false);
        YearSliderUIText.SetActive(false);
        TimePipUIText.SetActive(false);
        ConfirmChangeUIText.SetActive(false);
        GraphUIText.SetActive(false);
        TechTreeUIText.SetActive(false);
        KeyBoardUI.SetActive(false);

        RedButtonText.SetActive(false);
        BreakDownText.SetActive(false);
        TypeButtonText.SetActive(false);

        PanUpFromScreenObject.SetActive(false);
        PanDownFromUpTechTreeObject.SetActive(false);
        PanDownFromScreenObject.SetActive(false);
        NextButtonObject.SetActive(false);

        PipButton.SetActive(false);
        AwarenessButton.SetActive(false);
        TimeSliderButton.SetActive(false);
        PolicyButton.SetActive(false);
        YearSliderButton.SetActive(false);
        TimePipButton.SetActive(false);
        ConfirmChangeButton.SetActive(false);
        GraphButton.SetActive(false);


        RedButton.SetActive(false);
        BreakDown.SetActive(false);
        TypeButton.SetActive(false);

        TechTreeButton.SetActive(false);

        FinishTutorialButton.SetActive(false);
    }      
}
