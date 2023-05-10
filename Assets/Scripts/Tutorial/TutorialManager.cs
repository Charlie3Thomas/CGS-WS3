using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{

    // State of the game
    public enum GameState
    {
        InGame,
        InTutorial,
    }

    public GameState gameState;


    [SerializeField] private GameObject TutorialUI;
    [SerializeField] private GameObject InGameUI;

    public void Start()
    {
        // Assign the gameObject
        TutorialUI = GameObject.Find("TutorialUI");
        InGameUI = GameObject.Find("GameUI");

        // Set the tutorial text to be displayed by default
        gameState = GameState.InTutorial;   
    }

    public void Update()
    {
        if (gameState == GameState.InTutorial)
        {
            TutorialUI.SetActive(true);
            InGameUI.SetActive(false);
        }
        else if (gameState == GameState.InGame)
        {
            TutorialUI.SetActive(false);
            InGameUI.SetActive(true);
        }
    }

}
