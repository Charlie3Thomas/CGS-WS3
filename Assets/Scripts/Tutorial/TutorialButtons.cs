using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialButtons : MonoBehaviour
{
    private TutorialManager tutorialManager;


    public void Start()
    {
        tutorialManager = FindObjectOfType<TutorialManager>();
    }


    // When the user clicks play tutorial, The tutorial Ui will appear and the game will be explained
    public void EnableTutorial()
    {
        Debug.Log("Tutorial Starting");
    }

    // When the user clicks skip, the normal game will just load
    public void DisableTutorial()
    {
        tutorialManager.gameState = TutorialManager.GameState.InGame;
    }
}
