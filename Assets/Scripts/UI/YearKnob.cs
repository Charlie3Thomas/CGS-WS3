using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YearKnob : MonoBehaviour
{
    public void YearUp()
    {
        YearData._INSTANCE.YearUp();
    }

    public void YearDown()
    {
        YearData._INSTANCE.YearDown();
    }
}
