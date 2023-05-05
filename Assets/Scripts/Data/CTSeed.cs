using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CTSeed
{
    //private static long gameSeed = System.DateTime.Now.Ticks;
    public static long gameSeed = 42069;

    /// <summary>
    /// .Next/.NextDouble(RANGE)
    /// </summary>
    public static System.Random RandFromSeed(uint _turn, string _label)
    {
        int local_seed = ($"{gameSeed}{_turn}{_label}").GetHashCode();

        return new System.Random(local_seed);
    }
}
