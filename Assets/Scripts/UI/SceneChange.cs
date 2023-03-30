using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
  

    public void OnSelectStartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OnSelectLoadGame()
    {

    }

    public void OnSelectSettings()
    {

    }
    public void OnSelectQuitGame()
    {
        Application.Quit();
    }


}
