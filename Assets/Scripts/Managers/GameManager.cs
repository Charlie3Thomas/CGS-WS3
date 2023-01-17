using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    void Start()
    {
        Invoke("StartGame", 1.0f);
    }

    void Update()
    {
        
    }

    void StartGame()
    {
        StartTurn();
    }

    void StartTurn()
    {
        Debug.Log("Allocate your resources");
        PolicyManager.instance.NewPolicySet();
    }
}
