using CT.Lookup;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Disaster
{
    public Disaster() { }

    public CTDisasters type;
    public int year;
    public float intensity;
    public int turn;
}
