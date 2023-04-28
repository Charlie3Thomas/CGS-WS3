using System;
using UnityEngine;

public class Seed : MonoBehaviour
{
    public string gameSeed = "";
    public bool forceDefaultSeed = false;
    const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
    private int currentSeed = 0;
    private int seedLength = 8;

    private void Awake()
    {
        string seed = "";

        if (forceDefaultSeed)
        {
            seed = "Default";
        }
        else
        {
            for (int i = 0; i < seedLength; i++)
            {
                seed += chars[UnityEngine.Random.Range(0, chars.Length)];
            }
        }

        SetSeed(seed);
    }

    public void SetSeed(string _seed)
    {
        // May be needed at some point idk
        gameSeed = _seed;
        currentSeed = gameSeed.GetHashCode();
        UnityEngine.Random.InitState(currentSeed);
    }

    public string GetSeed()
    {
        return gameSeed;
    }
}
