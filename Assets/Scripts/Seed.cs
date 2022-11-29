using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    public bool forceDefaultSeed = false;
    const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
    private string gameSeed = "";
    private int currentSeed = 0;
    private int seedLength = 8;

    private void Awake()
    {
        if (forceDefaultSeed)
        {
            gameSeed = "Default";
        }
        else
        {
            for(int i = 0; i < seedLength; i++)
            {
                gameSeed += chars[Random.Range(0, chars.Length)];
            }
        }

        currentSeed = gameSeed.GetHashCode();
        Random.InitState(currentSeed);
    }
}
