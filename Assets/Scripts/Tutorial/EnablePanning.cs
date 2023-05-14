using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnablePanning : MonoBehaviour
{
    public void disablePanningObject(GameObject panningUI)
    {
        panningUI.SetActive(false);
        StartCoroutine(screenToTech(1f));
    }

    public void PanDownFromUP(GameObject panningUI)
    {
        panningUI.SetActive(false);
        TutorialManager.gameState = TutorialManager.GameState.PanningDownFromScreen;
    }

    public void PanDownFromScreen(GameObject panningUI)
    {
        panningUI.SetActive(false);
        TutorialManager.gameState = TutorialManager.GameState.KeyBoard;
        StartCoroutine(screenToKeyboard(1f));
    }

    IEnumerator screenToTech(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        TutorialManager.gameState = TutorialManager.GameState.TechTree;
        TutorialManager.TechTreeButton.SetActive(true);
    }
    IEnumerator screenToKeyboard(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        TutorialManager.gameState = TutorialManager.GameState.KeyBoard;
        TutorialManager.KeyBoardUI.SetActive(true);
    }
}

