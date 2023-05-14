using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

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
        tutorialManager.InteractableUI.SetActive(true);
        tutorialManager.TutorialUI.SetActive(false);
        TutorialManager.gameState = TutorialManager.GameState.Interactions;
        
    }

    // When the user clicks skip, the normal game will just load
    public void DisableTutorial(int Index)
    {
        DOTween.Clear();
        SceneManager.LoadScene(Index);
    }
}
